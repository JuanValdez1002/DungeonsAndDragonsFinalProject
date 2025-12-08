using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour_DFSDG : MonoBehaviour
{ 
    [Header("Enemy spawn point for this room")]
    public Transform[] enemySpawnPoints;
    public GameObject[] walls;
    public GameObject[] doors;

    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }

}