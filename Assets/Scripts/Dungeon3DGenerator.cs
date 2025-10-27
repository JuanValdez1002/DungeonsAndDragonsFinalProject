using System.Collections.Generic;
using UnityEngine;

public class Dungeon3DGenerator : MonoBehaviour
{
    // ------------------------------------------------------------
    // Represents one cell in the 3D grid
    // ------------------------------------------------------------
    public class Cell
    {
        public bool visited = false;          // Has this cell been visited during maze generation
        public bool[] status = new bool[6];   // 0=N, 1=S, 2=E, 3=W, 4=Up, 5=Down (open passages)
    }

    // ------------------------------------------------------------
    // Editor settings for dungeon size, spacing, and prefabs
    // ------------------------------------------------------------
    [Header("Dungeon Settings")]
    public Vector3Int size = new Vector3Int(5, 5, 2); // X=left/right, Y=forward/back, Z=vertical levels
    public Vector3 offset = new Vector3(70, 70, 70);  // World offset between rooms
    [Range(0.1f, 1f)] public float density = 0.7f;    // Optional density factor for variation

    [Header("Room Prefabs")]
    public GameObject[] normalRooms;
    public GameObject bossRoomPrefab;
    public GameObject treasureRoomPrefab;

    [Header("Player")]
    public GameObject player;

    // ------------------------------------------------------------
    // Internal state variables
    // ------------------------------------------------------------
    public List<Cell> board;                  // The generated 3D grid of cells
    private List<int> visitedOrder = new List<int>();
    private int startIndex = 0;               // Start cell index for maze generation
    private GameObject startRoomGO;           // Reference to the room where the player starts

    // Singleton instance for easy global access (used by enemies)
    public static Dungeon3DGenerator Instance { get; private set; }

    // ------------------------------------------------------------
    // Shared pathfinding cache for enemies (Dijkstra paths)
    // ------------------------------------------------------------
    private Dictionary<(int, int), List<int>> cachedPaths = new Dictionary<(int, int), List<int>>();
    private float lastCacheClearTime;
    public float cacheLifetime = 2f; // How long cached paths last in seconds

    // ------------------------------------------------------------
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MazeGenerator();   // Step 1: Generate the maze connections
        SpawnPlayer();     // Step 2: Place the player in the starting room
    }

    // ------------------------------------------------------------
    // Step 1: Depth-first search (DFS) to generate the maze structure
    // ------------------------------------------------------------
    void MazeGenerator()
    {
        // Create empty grid of cells
        board = new List<Cell>();
        for (int z = 0; z < size.z; z++)
            for (int y = 0; y < size.y; y++)
                for (int x = 0; x < size.x; x++)
                    board.Add(new Cell());

        int current = startIndex;
        Stack<int> stack = new Stack<int>();
        int uniqueVisited = 0;
        int safety = 0;

        // DFS to connect cells and carve passages
        while (stack.Count > 0 || uniqueVisited < board.Count)
        {
            if (!board[current].visited)
            {
                board[current].visited = true;
                visitedOrder.Add(current);
                uniqueVisited++;
            }

            List<int> neighbors = UnvisitedNeighbors(current);

            if (neighbors.Count == 0)
            {
                if (stack.Count == 0) break;
                current = stack.Pop();
            }
            else
            {
                int next = neighbors[Random.Range(0, neighbors.Count)];
                stack.Push(current);
                ConnectCells(current, next);
                current = next;
            }

            if (safety++ > 50000) break; // Fail-safe for infinite loops
        }

        // Ensure start cell connects to at least one neighbor
        List<int> startNeighbors = GetVisitedNeighbors(startIndex);
        if (startNeighbors.Count == 0)
        {
            List<int> allNeighbors = GetAllNeighborIndices(startIndex);
            if (allNeighbors.Count > 0)
            {
                int pick = allNeighbors[Random.Range(0, allNeighbors.Count)];
                ConnectCells(startIndex, pick);
                board[pick].visited = true;
                visitedOrder.Add(pick);
            }
        }

        GenerateDungeon(); // Instantiate the actual room prefabs
    }

    // ------------------------------------------------------------
    // Returns a list of unvisited neighbor indices for a given cell
    // ------------------------------------------------------------
    List<int> UnvisitedNeighbors(int idx)
    {
        List<int> n = new List<int>();

        int x = idx % size.x;
        int y = (idx / size.x) % size.y;
        int z = idx / (size.x * size.y);

        if (y > 0) { int s = idx - size.x; if (!board[s].visited) n.Add(s); }
        if (y < size.y - 1) { int nIdx = idx + size.x; if (!board[nIdx].visited) n.Add(nIdx); }
        if (x > 0) { int w = idx - 1; if (!board[w].visited) n.Add(w); }
        if (x < size.x - 1) { int e = idx + 1; if (!board[e].visited) n.Add(e); }
        if (z > 0) { int d = idx - size.x * size.y; if (!board[d].visited) n.Add(d); }
        if (z < size.z - 1) { int u = idx + size.x * size.y; if (!board[u].visited) n.Add(u); }

        return n;
    }

    // ------------------------------------------------------------
    // Connect two adjacent cells by opening the correct walls
    // ------------------------------------------------------------
    void ConnectCells(int a, int b)
    {
        int diff = b - a;

        if (diff == 1) { board[a].status[2] = true; board[b].status[3] = true; }
        else if (diff == -1) { board[a].status[3] = true; board[b].status[2] = true; }
        else if (diff == size.x) { board[a].status[0] = true; board[b].status[1] = true; }
        else if (diff == -size.x) { board[a].status[1] = true; board[b].status[0] = true; }
        else if (diff == size.x * size.y) { board[a].status[4] = true; board[b].status[5] = true; }
        else if (diff == -size.x * size.y) { board[a].status[5] = true; board[b].status[4] = true; }
    }

    // ------------------------------------------------------------
    // Step 2: Instantiate the dungeon from the generated data
    // ------------------------------------------------------------
    void GenerateDungeon()
    {
        if (visitedOrder.Count == 0) return;

        int bossIndex = FindFarthestCell(startIndex);
        List<int> candidates = new List<int>(visitedOrder);
        candidates.Remove(startIndex);
        candidates.Remove(bossIndex);

        int treasure1 = -1, treasure2 = -1;
        if (candidates.Count > 0)
        {
            treasure1 = candidates[Random.Range(0, candidates.Count)];
            candidates.Remove(treasure1);
        }
        if (candidates.Count > 0)
        {
            treasure2 = candidates[Random.Range(0, candidates.Count)];
        }

        // Instantiate rooms for each valid visited cell
        for (int z = 0; z < size.z; z++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    int idx = Index(x, y, z);
                    if (!board[idx].visited) continue;

                    bool hasDoor = false;
                    foreach (bool s in board[idx].status)
                        if (s) { hasDoor = true; break; }
                    if (!hasDoor) continue;

                    GameObject prefab;
                    if (idx == bossIndex && bossRoomPrefab != null)
                        prefab = bossRoomPrefab;
                    else if ((idx == treasure1 || idx == treasure2) && treasureRoomPrefab != null)
                        prefab = treasureRoomPrefab;
                    else
                        prefab = (normalRooms != null && normalRooms.Length > 0)
                            ? normalRooms[Random.Range(0, normalRooms.Length)]
                            : null;

                    if (prefab == null) continue;

                    Vector3 pos = new Vector3(x * offset.x, z * offset.y, -y * offset.z);
                    GameObject room = Instantiate(prefab, pos, Quaternion.identity, transform);
                    room.name = prefab.name + $"_{x}-{y}-{z}";

                    bool[] open = ComputeValidatedDoors(idx);
                    var rb = room.GetComponent<Room3DBehavior>();
                    if (rb != null) rb.UpdateRoom(open);

                    if (idx == startIndex)
                        startRoomGO = room;
                }
            }
        }

        Debug.Log("Dungeon generated successfully with guaranteed connections and safe doors.");
    }

    // ------------------------------------------------------------
    // Validates which doors in a room should remain open
    // ------------------------------------------------------------
    bool[] ComputeValidatedDoors(int idx)
    {
        bool[] open = new bool[4]; // 0=N,1=S,2=E,3=W

        for (int dir = 0; dir < 4; dir++)
        {
            if (!board[idx].status[dir])
            {
                open[dir] = false;
                continue;
            }

            int nx, ny, nz;
            ToCoord(idx, out nx, out ny, out nz);
            int nIdx = NeighborIndex(nx, ny, nz, dir);

            if (nIdx >= 0 && nIdx < board.Count)
            {
                if (board[nIdx].visited && board[nIdx].status[Opposite(dir)])
                {
                    int nnx, nny, nnz;
                    ToCoord(nIdx, out nnx, out nny, out nnz);
                    bool inBounds =
                        nnx >= 0 && nnx < size.x &&
                        nny >= 0 && nny < size.y &&
                        nnz >= 0 && nnz < size.z;

                    if (inBounds)
                    {
                        open[dir] = true;
                        continue;
                    }
                }
            }

            open[dir] = false;
        }

        return open;
    }

    // ------------------------------------------------------------
    // Helper methods for indexing and coordinate conversions
    // ------------------------------------------------------------
    List<int> GetVisitedNeighbors(int idx)
    {
        List<int> list = new List<int>();
        int x = idx % size.x;
        int y = (idx / size.x) % size.y;
        int z = idx / (size.x * size.y);

        if (y > 0 && board[Index(x, y - 1, z)].visited) list.Add(Index(x, y - 1, z));
        if (y < size.y - 1 && board[Index(x, y + 1, z)].visited) list.Add(Index(x, y + 1, z));
        if (x > 0 && board[Index(x - 1, y, z)].visited) list.Add(Index(x - 1, y, z));
        if (x < size.x - 1 && board[Index(x + 1, y, z)].visited) list.Add(Index(x + 1, y, z));
        if (z > 0 && board[Index(x, y, z - 1)].visited) list.Add(Index(x, y, z - 1));
        if (z < size.z - 1 && board[Index(x, y, z + 1)].visited) list.Add(Index(x, y, z + 1));

        return list;
    }

    List<int> GetAllNeighborIndices(int idx)
    {
        List<int> list = new List<int>();
        int x = idx % size.x;
        int y = (idx / size.x) % size.y;
        int z = idx / (size.x * size.y);

        if (y < size.y - 1) list.Add(Index(x, y + 1, z));
        if (y > 0) list.Add(Index(x, y - 1, z));
        if (x < size.x - 1) list.Add(Index(x + 1, y, z));
        if (x > 0) list.Add(Index(x - 1, y, z));
        if (z < size.z - 1) list.Add(Index(x, y, z + 1));
        if (z > 0) list.Add(Index(x, y, z - 1));

        return list;
    }

    int Opposite(int dir)
    {
        switch (dir)
        {
            case 0: return 1;
            case 1: return 0;
            case 2: return 3;
            case 3: return 2;
            default: return dir;
        }
    }

    int NeighborIndex(int x, int y, int z, int dir)
    {
        switch (dir)
        {
            case 0: if (y < size.y - 1) return Index(x, y + 1, z); break; // N
            case 1: if (y > 0) return Index(x, y - 1, z); break;         // S
            case 2: if (x < size.x - 1) return Index(x + 1, y, z); break; // E
            case 3: if (x > 0) return Index(x - 1, y, z); break;         // W
        }
        return -1;
    }

    int Index(int x, int y, int z) => x + y * size.x + z * size.x * size.y;

    void ToCoord(int idx, out int x, out int y, out int z)
    {
        x = idx % size.x;
        y = (idx / size.x) % size.y;
        z = idx / (size.x * size.y);
    }

    // ------------------------------------------------------------
    // Finds the farthest cell from the start (used for boss room)
    // ------------------------------------------------------------
    int FindFarthestCell(int start)
    {
        Queue<int> q = new Queue<int>();
        Dictionary<int, int> dist = new Dictionary<int, int>();
        q.Enqueue(start);
        dist[start] = 0;
        int farthest = start;

        while (q.Count > 0)
        {
            int cur = q.Dequeue();
            foreach (int n in ConnectedNeighbors(cur))
            {
                if (!dist.ContainsKey(n))
                {
                    dist[n] = dist[cur] + 1;
                    q.Enqueue(n);
                    if (dist[n] > dist[farthest]) farthest = n;
                }
            }
        }

        return farthest;
    }

    // Returns all directly connected neighbors for a given cell
    public List<int> ConnectedNeighbors(int idx)
    {
        List<int> result = new List<int>();
        bool[] s = board[idx].status;

        int x, y, z;
        ToCoord(idx, out x, out y, out z);

        if (s[0] && y < size.y - 1) result.Add(Index(x, y + 1, z));
        if (s[1] && y > 0) result.Add(Index(x, y - 1, z));
        if (s[2] && x < size.x - 1) result.Add(Index(x + 1, y, z));
        if (s[3] && x > 0) result.Add(Index(x - 1, y, z));
        if (s[4] && z < size.z - 1) result.Add(Index(x, y, z + 1));
        if (s[5] && z > 0) result.Add(Index(x, y, z - 1));

        return result;
    }

    // ------------------------------------------------------------
    // Converts a cell index to its corresponding world position
    // ------------------------------------------------------------
    public Vector3 IndexToWorld(int idx)
    {
        int x = idx % size.x;
        int y = (idx / size.x) % size.y;
        int z = idx / (size.x * size.y);
        return new Vector3(x * offset.x, z * offset.y, -y * offset.z);
    }

    // ------------------------------------------------------------
    // Shared Dijkstra path cache for enemies
    // ------------------------------------------------------------
    public List<int> GetPath(int startIndex, int targetIndex)
    {
        if (board == null || startIndex < 0 || targetIndex < 0)
            return new List<int>();

        if (Time.time - lastCacheClearTime > cacheLifetime)
        {
            cachedPaths.Clear();
            lastCacheClearTime = Time.time;
        }

        if (cachedPaths.TryGetValue((startIndex, targetIndex), out List<int> cached))
            return new List<int>(cached);

        List<int> path = ComputeDijkstra(startIndex, targetIndex);
        cachedPaths[(startIndex, targetIndex)] = path;
        return path;
    }

    private List<int> ComputeDijkstra(int start, int target)
    {
        var dist = new Dictionary<int, float>();
        var prev = new Dictionary<int, int>();
        var unvisited = new List<int>();

        for (int i = 0; i < board.Count; i++)
        {
            dist[i] = Mathf.Infinity;
            prev[i] = -1;
            unvisited.Add(i);
        }

        dist[start] = 0;

        while (unvisited.Count > 0)
        {
            unvisited.Sort((a, b) => dist[a].CompareTo(dist[b]));
            int current = unvisited[0];
            unvisited.RemoveAt(0);

            if (current == target)
                break;

            foreach (int neighbor in ConnectedNeighbors(current))
            {
                float alt = dist[current] + Vector3.Distance(IndexToWorld(current), IndexToWorld(neighbor));
                if (alt < dist[neighbor])
                {
                    dist[neighbor] = alt;
                    prev[neighbor] = current;
                }
            }
        }

        List<int> path = new List<int>();
        int temp = target;
        while (temp != -1)
        {
            path.Insert(0, temp);
            temp = prev[temp];
        }

        return path;
    }

    // ------------------------------------------------------------
    // Step 7: Spawns the player at the start room
    // ------------------------------------------------------------
    void SpawnPlayer()
    {
        if (player == null || startRoomGO == null) return;
        player.transform.position = startRoomGO.transform.position + Vector3.up * 2f;
        Debug.Log($"Player spawned in start room: {startRoomGO.name}");
    }
}
