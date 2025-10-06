using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon3DGenerator : MonoBehaviour
{
    // --- Represents one cell (room space) in the dungeon grid ---
    public class Cell
    {
        public bool visited = false;          // True if part of the generated dungeon
        public bool[] status = new bool[6];   // 0=Up, 1=Down, 2=Right, 3=Left, 4=Above, 5=Below
    }

    // --- Room spawning rules (optional for future customization) ---
    [System.Serializable]
    public class Rule
    {
        public GameObject room;              // Prefab to spawn
        public Vector3Int minPosition;       // Minimum coordinates
        public Vector3Int maxPosition;       // Maximum coordinates
        public bool obligatory;              // Must spawn in region?

        // Checks if the room can or must spawn here
        public int ProbabilityOfSpawning(int x, int y, int z)
        {
            if (x >= minPosition.x && x <= maxPosition.x &&
                y >= minPosition.y && y <= maxPosition.y &&
                z >= minPosition.z && z <= maxPosition.z)
            {
                return obligatory ? 2 : 1;
            }
            return 0;
        }
    }

    // --- Inspector variables (set in Unity) ---
    public Vector3Int size = new Vector3Int(5, 5, 3);  // Width, Depth, Levels
    public int startPos = 0;                           // Start cell index
    public Rule[] rooms;                               // Room rules (optional)
    public Vector3 offset = new Vector3(12, 6, 12);    // Distance between rooms
    public GameObject[] roomPrefabs;                   // Different room designs
    public GameObject hallwayPrefab;                   // Hallway prefab between rooms
    public GameObject player;                          // Player object

    // --- Internal data ---
    private List<Cell> board;

    void Start()
    {
        MazeGenerator();
    }

    // --- Generates the 3D maze layout ---
    void MazeGenerator()
    {
        board = new List<Cell>();

        // Initialize the entire 3D grid
        for (int z = 0; z < size.z; z++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    board.Add(new Cell());
                }
            }
        }

        int currentCell = startPos;
        Stack<int> path = new Stack<int>();
        int k = 0;

        //  Depth-first search (DFS) generation algorithm
        while (k < 5000)
        {
            k++;
            board[currentCell].visited = true;

            if (currentCell == board.Count - 1) break;

            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0) break;
                currentCell = path.Pop();
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                // Connect rooms in 3D directions
                ConnectCells(currentCell, newCell);
                currentCell = newCell;
            }
        }

        // After maze is complete → build it in the world
        GenerateDungeon();

        // Spawn the player in the start room
        SpawnPlayer();
    }

    // --- Connects two neighboring cells ---
    void ConnectCells(int current, int next)
    {
        int diff = next - current;

        // Movement directions depend on difference in index
        // X axis (left/right)
        if (diff == 1) { board[current].status[2] = true; board[next].status[3] = true; }
        else if (diff == -1) { board[current].status[3] = true; board[next].status[2] = true; }

        // Y axis (up/down in grid)
        else if (diff == size.x) { board[current].status[1] = true; board[next].status[0] = true; }
        else if (diff == -size.x) { board[current].status[0] = true; board[next].status[1] = true; }

        // Z axis (floor connections)
        else if (diff == size.x * size.y) { board[current].status[4] = true; board[next].status[5] = true; }
        else if (diff == -size.x * size.y) { board[current].status[5] = true; board[next].status[4] = true; }
    }

    // --- Builds the dungeon in Unity world space ---
    void GenerateDungeon()
    {
        for (int z = 0; z < size.z; z++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    int index = x + y * size.x + z * size.x * size.y;
                    Cell cell = board[index];

                    if (cell.visited)
                    {
                        // Choose random prefab for variety
                        GameObject chosenRoom = roomPrefabs[Random.Range(0, roomPrefabs.Length)];

                        // World position
                        Vector3 pos = new Vector3(x * offset.x, z * offset.y, -y * offset.z);

                        // Create the room
                        var roomObj = Instantiate(chosenRoom, pos, Quaternion.identity, transform);
                        var roomBehavior = roomObj.GetComponent<RoomBehavior>();
                        roomBehavior.UpdateRoom(cell.status);

                        // --- Spawn hallways between connected rooms ---
                        SpawnHallways(x, y, z, cell);

                        roomObj.name = $"Room {x}-{y}-{z}";
                    }
                }
            }
        }
    }

    // --- Adds hallways where rooms connect ---
    void SpawnHallways(int x, int y, int z, Cell cell)
    {
        Vector3 basePos = new Vector3(x * offset.x, z * offset.y, -y * offset.z);

        // Example: spawn small hallway objects between connected directions
        if (cell.status[2]) Instantiate(hallwayPrefab, basePos + new Vector3(offset.x / 2, 0, 0), Quaternion.identity, transform);   // Right
        if (cell.status[3]) Instantiate(hallwayPrefab, basePos + new Vector3(-offset.x / 2, 0, 0), Quaternion.identity, transform);  // Left
        if (cell.status[0]) Instantiate(hallwayPrefab, basePos + new Vector3(0, 0, offset.z / 2), Quaternion.identity, transform);   // Up
        if (cell.status[1]) Instantiate(hallwayPrefab, basePos + new Vector3(0, 0, -offset.z / 2), Quaternion.identity, transform);  // Down
    }

    // --- Finds available unvisited neighbors (3D) ---
    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        int x = cell % size.x;
        int y = (cell / size.x) % size.y;
        int z = cell / (size.x * size.y);

        // Up (y-1)
        if (y > 0 && !board[cell - size.x].visited) neighbors.Add(cell - size.x);

        // Down (y+1)
        if (y < size.y - 1 && !board[cell + size.x].visited) neighbors.Add(cell + size.x);

        // Left (x-1)
        if (x > 0 && !board[cell - 1].visited) neighbors.Add(cell - 1);

        // Right (x+1)
        if (x < size.x - 1 && !board[cell + 1].visited) neighbors.Add(cell + 1);

        // Above floor (z+1)
        if (z < size.z - 1 && !board[cell + size.x * size.y].visited) neighbors.Add(cell + size.x * size.y);

        // Below floor (z-1)
        if (z > 0 && !board[cell - size.x * size.y].visited) neighbors.Add(cell - size.x * size.y);

        return neighbors;
    }

    // --- Spawns the player safely in the first room ---
    void SpawnPlayer()
    {
        if (player != null)
        {
            int startX = startPos % size.x;
            int startY = (startPos / size.x) % size.y;
            int startZ = startPos / (size.x * size.y);

            Vector3 spawnPos = new Vector3(startX * offset.x + offset.x / 2f, startZ * offset.y + 2f, -startY * offset.z - offset.z / 2f);

            player.transform.position = spawnPos;
        }
    }
}
