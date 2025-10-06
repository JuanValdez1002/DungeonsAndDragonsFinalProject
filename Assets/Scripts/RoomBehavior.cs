using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    public GameObject[] walls; //0 - up 1 -Down 2 - Right 3- left
    public GameObject[] doors;

    public bool[] testStatus;

    //Start is called before the first frame update

    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }

}
