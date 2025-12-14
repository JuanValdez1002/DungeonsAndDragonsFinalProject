using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;      
using UnityEngine.AI;          

public class DFSDungeonGenerator : MonoBehaviour
{

    [System.Serializable]
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4]; // [Up, Down, Right, Left]
    }

    [System.Serializable]
    public class Rule
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;
        public bool obligatory;

        public int ProbabilityOfSpawning(int x, int y)
        {
            if (x >= minPosition.x && x <= maxPosition.x &&
                y >= minPosition.y && y <= maxPosition.y)
            {
                return obligatory ? 2 : 1;
            }
            return 0;
        }
    }

    public Vector2Int size;
    public int startPos = 0;
    public Rule[] rooms;
    public Vector2 offset;

    public GameObject player;

    public NavMeshSurface navSurface;

    public GameObject enemyPrefab;
    public int enemiesPerRoom = 1;

    private List<Cell> board;
    private List<Transform> enemySpawnPoints = new List<Transform>();

    void Start()
    {
        if (navSurface == null)
        {
            Debug.LogWarning("DFSDungeonGenerator: No NavMeshSurface was assigned. NavMesh will NOT build.");
        }

        MazeGenerator();
    }

    void GenerateDungeon()
    {
        enemySpawnPoints.Clear();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[(i + j * size.x)];

                if (!currentCell.visited)
                    continue;

                int roomIndex = -1;
                List<int> valid = new List<int>();

                for (int k = 0; k < rooms.Length; k++)
                {
                    int p = rooms[k].ProbabilityOfSpawning(i, j);

                    if (p == 2)
                    {
                        roomIndex = k;
                        break;
                    }
                    else if (p == 1)
                    {
                        valid.Add(k);
                    }
                }

                if (roomIndex == -1)
                {
                    roomIndex = valid.Count > 0 ? valid[Random.Range(0, valid.Count)] : 0;
                }

                GameObject roomGO = Instantiate(
                    rooms[roomIndex].room,
                    new Vector3(i * offset.x, 0, -j * offset.y),
                    Quaternion.identity,
                    this.transform
                );

                RoomBehaviour_DFSDG rb = roomGO.GetComponent<RoomBehaviour_DFSDG>();
                rb.UpdateRoom(currentCell.status);

                roomGO.name = $"Room {i}-{j}";

                // -----------------------------
                // MULTIPLE SPAWN POINTS FIX ONLY
                // -----------------------------
                if (rb.enemySpawnPoints != null && rb.enemySpawnPoints.Length > 0)
                {
                    foreach (Transform sp in rb.enemySpawnPoints)
                    {
                        if (sp != null)
                            enemySpawnPoints.Add(sp);
                    }
                }
            }
        }

        StartCoroutine(DelayedNavmeshBuild());
    }

    IEnumerator DelayedNavmeshBuild()
    {
        if (navSurface == null)
            yield break;

        yield return new WaitForSeconds(0.25f);

        Debug.Log("Building runtime NavMesh...");
        navSurface.BuildNavMesh();
        Debug.Log("NavMesh build complete.");

        SpawnEnemies();
    }

    void MazeGenerator()
    {
        board = new List<Cell>();
        for (int i = 0; i < size.x * size.y; i++)
            board.Add(new Cell());

        int currentCell = startPos;
        Stack<int> path = new Stack<int>();
        int failsafe = 0;

        while (failsafe < 10000)
        {
            failsafe++;
            board[currentCell].visited = true;

            if (currentCell == board.Count - 1)
                break;

            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                    break;

                currentCell = path.Pop();
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];
                CreatePassage(currentCell, newCell);
                currentCell = newCell;
            }
        }

        GenerateDungeon();
    }

    void CreatePassage(int current, int next)
    {
        int x = current % size.x;
        int y = current / size.x;
        int nx = next % size.x;
        int ny = next / size.x;

        if (nx == x && ny == y + 1)
        {
            board[current].status[1] = true;
            board[next].status[0] = true;
        }
        else if (nx == x && ny == y - 1)
        {
            board[current].status[0] = true;
            board[next].status[1] = true;
        }
        else if (nx == x + 1 && ny == y)
        {
            board[current].status[2] = true;
            board[next].status[3] = true;
        }
        else if (nx == x - 1 && ny == y)
        {
            board[current].status[3] = true;
            board[next].status[2] = true;
        }
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> result = new List<int>();
        int x = cell % size.x;
        int y = cell / size.x;

        if (y > 0 && !board[cell - size.x].visited)
            result.Add(cell - size.x);

        if (y < size.y - 1 && !board[cell + size.x].visited)
            result.Add(cell + size.x);

        if (x < size.x - 1 && !board[cell + 1].visited)
            result.Add(cell + 1);

        if (x > 0 && !board[cell - 1].visited)
            result.Add(cell - 1);

        return result;
    }

    void SpawnEnemies()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("DFSDungeonGenerator: enemyPrefab not assigned, no enemies spawned.");
            return;
        }

        if (enemySpawnPoints.Count == 0)
        {
            Debug.Log("DFSDungeonGenerator: No enemySpawnPoints found in rooms.");
            return;
        }

        Debug.Log($"Spawning enemies at {enemySpawnPoints.Count} room spawn points.");

        foreach (Transform spawnPoint in enemySpawnPoints)
        {
            for (int i = 0; i < enemiesPerRoom; i++)
            {
               if (NavMesh.SamplePosition(spawnPoint.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    GameObject enemyGO = Instantiate(enemyPrefab, hit.position, spawnPoint.rotation);

                    AIEnemySimple ai = enemyGO.GetComponent<AIEnemySimple>();
                    if (ai != null)
                    {
                        ai.patrolPoints = spawnPoint
                            .GetComponentInParent<RoomBehaviour_DFSDG>()
                            .patrolPoints;
                    }
                }

            }
        }
      

    }
}
