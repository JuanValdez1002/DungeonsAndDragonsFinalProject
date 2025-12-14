using System.Collections;
using System.Collections.Generic;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
  

    // --- Represents one cell (or tile) in the dungeon grid ---
    public class Cell
    {
        public bool visited = false;   // True if this cell has already been explored/used
        public bool[] status = new bool[4]; // Represents walls or passages in 4 directions (Up, Down, Right, Left)
    }

    // --- Defines rules for spawning rooms ---
    [System.Serializable]
    public class Rule
    {
        public GameObject room;              // Prefab of the room to spawn
        public Vector2Int minPosition;       // Minimum coordinates this room type can appear in
        public Vector2Int maxPosition;       // Maximum coordinates this room type can appear in
        public bool obligatory;              // Whether this room must appear in its region

        // Checks if a room can or must spawn at coordinates (x, y)
        public int ProbabilityOfSpawning(int x, int y)
        {
            // Return values:
            // 0 → Cannot spawn
            // 1 → Can spawn
            // 2 → Must spawn

            // If the coordinates are within the room's allowed region
            if (x >= minPosition.x && x <= maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
            {
                // If obligatory, must spawn → return 2
                // Otherwise, can spawn → return 1
                return obligatory ? 2 : 1;
            }

            // Outside the region → cannot spawn
            return 0;
        }
    }

    // ---  Variables set in the Unity Inspector ---
    public Vector2Int size;          // Size of the dungeon grid (X = width, Y = height)
    public int startPos = 0;         // Starting cell index (usually 0)
    public Rule[] rooms;             // Array of room types and their spawning rules
    public Vector2 offset;           // Distance between rooms (spacing on the X/Z plane)
    public GameObject player;        // Reference to the player object

    // --- Internal dungeon data ---
    List<Cell> board; // Stores all cells of the dungeon grid

    // --- Unity Start() method ---
    void Start()
    {
        // Start the entire maze/dungeon generation process
        MazeGenerator();
    }

    // ---  Generates the actual dungeon based on the maze data ---
    void GenerateDungeon()
    {
        // Loop through all coordinates of the grid
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                // Get the cell at (i, j)
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];

                // Only create a room if the cell has been visited (part of the maze)
                if (currentCell.visited)
                {
                    int randomRoom = -1;
                    List<int> availableRooms = new List<int>();

                    // --- Check which rooms can spawn here ---
                    for (int k = 0; k < rooms.Length; k++)
                    {
                        int p = rooms[k].ProbabilityOfSpawning(i, j);

                        // If this room *must* spawn here
                        if (p == 2)
                        {
                            randomRoom = k;
                            break;
                        }
                        // If this room *can* spawn here, add it to the options
                        else if (p == 1)
                        {
                            availableRooms.Add(k);
                        }
                    }

                    // --- Choose which room to spawn ---
                    if (randomRoom == -1)
                    {
                        if (availableRooms.Count > 0)
                        {
                            // Pick a random available room
                            randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                        }
                        else
                        {
                            // Default to the first room type if none fit
                            randomRoom = 0;
                        }
                    }

                    // --- Instantiate (spawn) the room ---
                    var newRoom = Instantiate(
                        rooms[randomRoom].room, 
                        new Vector3(i * offset.x, 0, -j * offset.y), // Position it in world space
                        Quaternion.identity, 
                        transform // Make it a child of the DungeonGenerator object
                    ).GetComponent<RoomBehavior>();

                    // Update room walls based on maze data (which sides are open)
                    newRoom.UpdateRoom(currentCell.status);

                    // Rename room for easier debugging (e.g. "Room 2-3")
                    newRoom.name += " " + i + "-" + j;
                }
            }
        }
    }

    // --- Generates the maze layout using Depth-First Search (DFS) ---
    void MazeGenerator()
    {
        board = new List<Cell>();

        // Initialize grid with empty cells
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos; // Start from the given starting cell
        Stack<int> path = new Stack<int>(); // Stack for backtracking
        int k = 0; // Safety counter to avoid infinite loops

        // --- Main maze generation loop ---
        while (k < 1000)
        {
            k++;
            board[currentCell].visited = true; // Mark cell as visited

            // Stop if we’ve reached the last cell
            if (currentCell == board.Count - 1)
            {
                break;
            }

            // Get all unvisited neighbors
            List<int> neighbors = CheckNeighbors(currentCell);

            // --- If there are no available neighbors ---
            if (neighbors.Count == 0)
            {
                // If the path stack is empty, we’re done
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    // Otherwise, backtrack
                    currentCell = path.Pop();
                }
            }
            else
            {
                // Choose a random unvisited neighbor
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                // Connect the two cells (remove walls between them)
                if (newCell > currentCell)
                {
                    // Moving down or right
                    if (newCell - 1 == currentCell)
                    {
                        // Right connection
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        // Down connection
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    // Moving up or left
                    if (newCell + 1 == currentCell)
                    {
                        // Left connection
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        // Up connection
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }

        // After maze generation, build the actual rooms
        GenerateDungeon();

        // --- Spawn player safely ---
        if (player != null)
        {
            int startX = startPos % size.x;
            int startY = startPos / size.x;

            // Calculate spawn position centered within the first room
            Vector3 spawnPos = new Vector3(
                startX * offset.x + offset.x / 2f,
                2f, // small height offset so player isn't stuck in floor
                -startY * offset.y - offset.y / 2f
            );

            // Check for collisions at spawn point
            Collider[] hitColliders = Physics.OverlapSphere(spawnPos, 0.5f);

            // If spawn point is inside a wall or object, move up slightly
            if (hitColliders.Length > 0)
            {
                spawnPos += Vector3.up * 2f;
            }

            // Move player to the spawn position
            player.transform.position = spawnPos;
        }
    }

    // --- Checks which neighboring cells can be visited next ---
    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        // Check Up neighbor
        if (cell - size.x >= 0 && !board[(cell - size.x)].visited)
        {
            neighbors.Add((cell - size.x));
        }

        // Check Down neighbor
        if (cell + size.x < board.Count && !board[(cell + size.x)].visited)
        {
            neighbors.Add((cell + size.x));
        }

        // Check Right neighbor
        if ((cell + 1) % size.x != 0 && !board[(cell + 1)].visited)
        {
            neighbors.Add((cell + 1));
        }

        // Check Left neighbor
        if (cell % size.x != 0 && !board[(cell - 1)].visited)
        {
            neighbors.Add((cell - 1));
        }

        return neighbors;
    }
}
