using UnityEngine;

public class Room3DBehavior : MonoBehaviour
{
    // 0 = North, 1 = South, 2 = East, 3 = West
    public GameObject[] walls = new GameObject[4];
    public GameObject[] doors = new GameObject[4];
    public bool logChanges = false;

    private GameObject[] doorPanels = new GameObject[4];

    void Awake()
    {
        // Auto-detect the child door panels inside each wall
        for (int i = 0; i < walls.Length; i++)
        {
            if (walls[i] == null) continue;

            foreach (Transform child in walls[i].transform)
            {
                if (child.name.ToLower().Contains("door panel"))
                {
                    doorPanels[i] = child.gameObject;
                    break;
                }
            }

            if (logChanges)
            {
                Debug.Log($"{name}: Found door panel {doorPanels[i]?.name ?? "(none)"} inside {walls[i].name}");
            }
        }
    }

    public void UpdateRoom(bool[] open)
    {
        if (open == null || open.Length < 4)
        {
            Debug.LogWarning($"{name}: invalid door status array");
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            bool isOpen = open[i];

            // Keep walls always visible
            if (walls[i] != null)
                walls[i].SetActive(true);

            // Show or hide the door mesh
            if (doors[i] != null)
                doors[i].SetActive(isOpen);

            // If a door panel was found, disable it when open
            if (doorPanels[i] != null)
                doorPanels[i].SetActive(!isOpen);

            if (logChanges)
                Debug.Log($"{name}: {(isOpen ? "OPEN" : "CLOSED")} {(Direction)i} | Door={doors[i]?.name} | Panel={doorPanels[i]?.name}");
        }
    }

    private enum Direction { North, South, East, West }
}
