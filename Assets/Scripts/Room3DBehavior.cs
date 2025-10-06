using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room3DBehavior : MonoBehaviour
{
    // --- 🔹 Assign these in the Inspector for each room prefab ---
    [Header("Room Structure")]
    public GameObject[] walls; // 0 = Up, 1 = Down, 2 = Right, 3 = Left
    public GameObject[] doors; // Matches the same order as walls

    [Header("Vertical Connections")]
    public GameObject stairsUpPrefab;   // Prefab for stairs going up (connects to room above)
    public GameObject stairsDownPrefab; // Prefab for stairs going down (connects to room below)

    [Header("Optional Hallway Anchors (for better visual alignment)")]
    public Transform[] hallwayAnchors; // Optional points where hallways connect
                                       // 0 = Up, 1 = Down, 2 = Right, 3 = Left

    // For debugging / testing
    [HideInInspector] public bool[] testStatus;

    // --- 🔹 Updates the visual layout of the room ---
    public void UpdateRoom(bool[] status)
    {
        // Status array: [0]=Up, [1]=Down, [2]=Right, [3]=Left, [4]=Above, [5]=Below
        testStatus = status;

        // Handle horizontal openings (doors/walls)
        for (int i = 0; i < 4; i++)
        {
            if (doors != null && i < doors.Length && doors[i] != null)
            {
                // Activate the door if passage exists in that direction
                doors[i].SetActive(status[i]);
            }

            if (walls != null && i < walls.Length && walls[i] != null)
            {
                // Hide the wall if there’s a passage (door), otherwise show wall
                walls[i].SetActive(!status[i]);
            }
        }

        // Handle vertical connections (stairs)
        HandleStairs(status[4], status[5]);
    }

    // --- 🔹 Creates stairs for up/down connections ---
    private void HandleStairs(bool hasAbove, bool hasBelow)
    {
        // If there’s a room above → spawn upward stairs
        if (hasAbove && stairsUpPrefab != null)
        {
            GameObject stairsUp = Instantiate(stairsUpPrefab, transform);
            stairsUp.transform.localPosition = Vector3.zero + new Vector3(0, 0, 0);
            stairsUp.name = "Stairs_Up";
        }

        // If there’s a room below → spawn downward stairs
        if (hasBelow && stairsDownPrefab != null)
        {
            GameObject stairsDown = Instantiate(stairsDownPrefab, transform);
            stairsDown.transform.localPosition = Vector3.zero + new Vector3(0, 0, 0);
            stairsDown.name = "Stairs_Down";
        }
    }

    // --- 🔹 (Optional) Used for visual debugging ---
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (testStatus != null)
        {
            // Visualize which directions are open
            Vector3 pos = transform.position;

            if (testStatus[0]) Gizmos.DrawLine(pos, pos + Vector3.forward * 3);  // Up
            if (testStatus[1]) Gizmos.DrawLine(pos, pos + Vector3.back * 3);     // Down
            if (testStatus[2]) Gizmos.DrawLine(pos, pos + Vector3.right * 3);    // Right
            if (testStatus[3]) Gizmos.DrawLine(pos, pos + Vector3.left * 3);     // Left
            if (testStatus[4]) Gizmos.DrawLine(pos, pos + Vector3.up * 3);       // Above
            if (testStatus[5]) Gizmos.DrawLine(pos, pos + Vector3.down * 3);     // Below
        }
    }
}
