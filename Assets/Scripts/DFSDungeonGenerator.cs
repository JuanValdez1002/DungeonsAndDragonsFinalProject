using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;      // For NavMeshSurface
using UnityEngine.AI;          // For NavMesh.SamplePosition

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
            // 0 = cannot spawn
            // 1 = can spawn
            // 2 = must spawn
            if (x >= minPosition.x && x <= maxPosition.x &&
                y >= minPosition.y && y <= maxPosition.y)
            {
                return obligatory ? 2 : 1;
            }
            return 0;
        }
    }

    // === PUBLIC SETTINGS ===
    [Header("Dungeon Grid")]
    public Vector2Int size;
    public int startPos = 0;
    public Rule[] rooms;
    public Vector2 offset;

    [Header("Player Spawn (optional)")]
    public GameObject player;

    [Header("External NavMesh Reference (REQUIRED)")]
    public NavMeshSurface navSurface;   // Assigned from NavMeshManager in inspector

    [Header("Enemies")]
    public GameObject enemyPrefab;      // Skeleton enemy prefab
    public int enemiesPerRoom = 1;      // How many per room (can be 0, 1, 2, etc.)

    // === PRIVATE STATE ===
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

    // ──────────────────────────────────────────────
    // Generates the dungeon AFTER DFS finishes
    // ──────────────────────────────────────────────
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

                // Select which room prefab to spawn
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
                    this.transform // rooms are children of generator (safe)
                );

                RoomBehaviour_DFSDG newRoom = roomGO.GetComponent<RoomBehaviour_DFSDG>();
                newRoom.UpdateRoom(currentCell.status);
                roomGO.name = $"Room {i}-{j}";

                // Collect enemy spawn point if this room has one
                if (newRoom.enemySpawnPoint != null)
                {
                    enemySpawnPoints.Add(newRoom.enemySpawnPoint);
                }
            }
        }

        // Build NavMesh (ONLY if assigned)
        StartCoroutine(DelayedNavmeshBuild());
    }

    IEnumerator DelayedNavmeshBuild()
    {
        if (navSurface == null)
            yield break;

        // Wait for all rooms to spawn and transforms to settle
        yield return new WaitForSeconds(0.25f);

        Debug.Log("Building runtime NavMesh...");
        navSurface.BuildNavMesh();
        Debug.Log("NavMesh build complete.");

        // After NavMesh exists, spawn enemies so their NavMeshAgents are valid
        SpawnEnemies();
    }

    // ──────────────────────────────────────────────
    // DFS MAZE GENERATION ALGORITHM
    // ──────────────────────────────────────────────
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

    // Creates openings between maze cells
    void CreatePassage(int current, int next)
    {
        int x = current % size.x;
        int y = current / size.x;
        int nx = next % size.x;
        int ny = next / size.x;

        if (nx == x && ny == y + 1) // down
        {
            board[current].status[1] = true;
            board[next].status[0] = true;
        }
        else if (nx == x && ny == y - 1) // up
        {
            board[current].status[0] = true;
            board[next].status[1] = true;
        }
        else if (nx == x + 1 && ny == y) // right
        {
            board[current].status[2] = true;
            board[next].status[3] = true;
        }
        else if (nx == x - 1 && ny == y) // left
        {
            board[current].status[3] = true;
            board[next].status[2] = true;
        }
    }

    // Finds unvisited neighbors
    List<int> CheckNeighbors(int cell)
    {
        List<int> result = new List<int>();
        int x = cell % size.x;
        int y = cell / size.x;

        // Up
        if (y > 0 && !board[cell - size.x].visited)
            result.Add(cell - size.x);

        // Down
        if (y < size.y - 1 && !board[cell + size.x].visited)
            result.Add(cell + size.x);

        // Right
        if (x < size.x - 1 && !board[cell + 1].visited)
            result.Add(cell + 1);

        // Left
        if (x > 0 && !board[cell - 1].visited)
            result.Add(cell - 1);

        return result;
    }

    // ──────────────────────────────────────────────
    // ENEMY SPAWNING AFTER NAVMESH BUILD
    // ──────────────────────────────────────────────
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
                // Ensure the spawn point is on the NavMesh
                if (NavMesh.SamplePosition(spawnPoint.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    Instantiate(enemyPrefab, hit.position, spawnPoint.rotation);
                }
                else
                {
                    Debug.LogWarning($"Enemy spawn point '{spawnPoint.name}' is not on NavMesh, skipping.");
                }
            }
        }
    }
}
