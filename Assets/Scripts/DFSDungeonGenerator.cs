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

        // NEW: room limiting and boss identification
        public bool isBoss = false;      // Only one rule should have this checked
        public int maxSpawns = -1;       // -1 = unlimited

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

    // NEW: sparsity control (limits how many cells become rooms)
    [Header("Dungeon Sparsity Control")]
    public int maxRooms = -1; // -1 = unlimited (fills all reachable cells)

    private List<Cell> board;

    // NEW: spawn point + room pairing (keeps room context)
    private class SpawnPointInfo
    {
        public Transform spawnPoint;
        public RoomBehaviour_DFSDG room;
    }

    private List<SpawnPointInfo> enemySpawnPoints = new List<SpawnPointInfo>();

    // NEW: room spawn tracking and boss placement
    private Dictionary<int, int> roomSpawnCounts = new Dictionary<int, int>();
    private int bossRuleIndex = -1;
    private int bossCellIndex = -1;

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
        roomSpawnCounts.Clear();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                int cellIndex = i + j * size.x;
                Cell currentCell = board[cellIndex];

                if (!currentCell.visited)
                    continue;

                int roomIndex = -1;
                List<int> valid = new List<int>();

                // NEW: force boss room at farthest cell
                if (cellIndex == bossCellIndex && bossRuleIndex != -1)
                {
                    roomIndex = bossRuleIndex;
                }
                else
                {
                    for (int k = 0; k < rooms.Length; k++)
                    {
                        if (rooms[k].isBoss)
                            continue;

                        // NEW: enforce max spawn limit
                        bool underLimit =
                            rooms[k].maxSpawns < 0 ||
                            !roomSpawnCounts.ContainsKey(k) ||
                            roomSpawnCounts[k] < rooms[k].maxSpawns;

                        if (!underLimit)
                            continue;

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
                }

                // NEW: increment spawn count
                if (!roomSpawnCounts.ContainsKey(roomIndex))
                    roomSpawnCounts[roomIndex] = 0;
                roomSpawnCounts[roomIndex]++;

                GameObject roomGO = Instantiate(
                    rooms[roomIndex].room,
                    new Vector3(i * offset.x, 0, -j * offset.y),
                    Quaternion.identity,
                    this.transform
                );

                RoomBehaviour_DFSDG rb = roomGO.GetComponent<RoomBehaviour_DFSDG>();
                rb.UpdateRoom(currentCell.status);

                roomGO.name = $"Room {i}-{j}";

                // UPDATED: store spawn point with room reference
                if (rb.enemySpawnPoints != null && rb.enemySpawnPoints.Length > 0)
                {
                    foreach (Transform sp in rb.enemySpawnPoints)
                    {
                        if (sp != null)
                        {
                            enemySpawnPoints.Add(new SpawnPointInfo
                            {
                                spawnPoint = sp,
                                room = rb
                            });
                        }
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

        // NEW: track how many unique cells have been visited (sparsity)
        int visitedCount = 0;

        // NEW: clamp maxRooms if set
        int targetRooms = maxRooms;
        if (targetRooms < 0)
            targetRooms = size.x * size.y;
        else
            targetRooms = Mathf.Clamp(targetRooms, 1, size.x * size.y);

        while (failsafe < 10000)
        {
            failsafe++;

            // NEW: only count the first time we visit a cell
            if (!board[currentCell].visited)
            {
                board[currentCell].visited = true;
                visitedCount++;

                // NEW: stop early to recreate sparse/organic layouts
                if (visitedCount >= targetRooms)
                    break;
            }

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

        // NEW: determine boss rule and farthest cell AFTER maze is built
        bossRuleIndex = FindBossRuleIndex();
        bossCellIndex = FindFarthestCellFromStart();

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
        if (enemySpawnPoints.Count == 0)
        {
            Debug.Log("DFSDungeonGenerator: No enemySpawnPoints found in rooms.");
            return;
        }

        foreach (var info in enemySpawnPoints)
        {
            Transform spawnPoint = info.spawnPoint;
            RoomBehaviour_DFSDG room = info.room;

            bool hasCustomEnemies =
                room != null &&
                room.enemiesToSpawn != null &&
                room.enemiesToSpawn.Length > 0;

            if (hasCustomEnemies)
            {
                foreach (var e in room.enemiesToSpawn)
                {
                    if (e == null || e.enemyPrefab == null || e.count <= 0)
                        continue;

                    for (int c = 0; c < e.count; c++)
                    {
                        if (NavMesh.SamplePosition(spawnPoint.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                        {
                            GameObject enemyGO = Instantiate(e.enemyPrefab, hit.position, spawnPoint.rotation);

                            AIEnemySimple ai = enemyGO.GetComponent<AIEnemySimple>();
                            if (ai != null)
                            {
                                ai.patrolPoints = room.patrolPoints;
                            }
                        }
                    }
                }
            }
            else
            {
                // Fallback to original behavior (does not break existing setups)
                if (enemyPrefab == null)
                    continue;

                for (int i = 0; i < enemiesPerRoom; i++)
                {
                    if (NavMesh.SamplePosition(spawnPoint.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                    {
                        GameObject enemyGO = Instantiate(enemyPrefab, hit.position, spawnPoint.rotation);

                        AIEnemySimple ai = enemyGO.GetComponent<AIEnemySimple>();
                        if (ai != null)
                        {
                            ai.patrolPoints = room.patrolPoints;
                        }
                    }
                }
            }
        }
    }

    // NEW: locate which rule is the boss
    int FindBossRuleIndex()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            if (rooms[i] != null && rooms[i].isBoss)
                return i;
        }
        return -1;
    }

    // NEW: BFS to find farthest reachable cell from startPos
    int FindFarthestCellFromStart()
    {
        int start = Mathf.Clamp(startPos, 0, board.Count - 1);

        // NEW: if start isn't part of the dungeon due to low maxRooms, find nearest visited cell
        if (!board[start].visited)
        {
            int nearest = -1;
            float best = float.PositiveInfinity;

            Vector2Int startXY = new Vector2Int(start % size.x, start / size.x);

            for (int i = 0; i < board.Count; i++)
            {
                if (!board[i].visited)
                    continue;

                Vector2Int xy = new Vector2Int(i % size.x, i / size.x);
                float d = Vector2Int.Distance(startXY, xy);
                if (d < best)
                {
                    best = d;
                    nearest = i;
                }
            }

            if (nearest == -1)
                return -1;

            start = nearest;
        }

        Queue<int> q = new Queue<int>();
        int[] dist = new int[board.Count];
        for (int i = 0; i < dist.Length; i++)
            dist[i] = -1;

        dist[start] = 0;
        q.Enqueue(start);

        int farthest = start;

        while (q.Count > 0)
        {
            int cur = q.Dequeue();
            if (dist[cur] > dist[farthest])
                farthest = cur;

            int x = cur % size.x;
            int y = cur / size.x;

            if (board[cur].status[0] && y > 0) TryVisit(cur - size.x, cur, dist, q);
            if (board[cur].status[1] && y < size.y - 1) TryVisit(cur + size.x, cur, dist, q);
            if (board[cur].status[2] && x < size.x - 1) TryVisit(cur + 1, cur, dist, q);
            if (board[cur].status[3] && x > 0) TryVisit(cur - 1, cur, dist, q);
        }

        return farthest;
    }

    void TryVisit(int next, int cur, int[] dist, Queue<int> q)
    {
        if (board[next].visited && dist[next] == -1)
        {
            dist[next] = dist[cur] + 1;
            q.Enqueue(next);
        }
    }
}
