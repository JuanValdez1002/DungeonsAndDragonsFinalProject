using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    [SerializeField] public int dungeonWidth, dungeonLength;
    [SerializeField] public int roomWidthMin, roomLengthMin;
    [SerializeField] public int maxIterations;
    [SerializeField] public int corridorWidth;
    // Start is called before the first frame update
    void Start()
    {
        CreateDungeon();
    }

    private void CreateDungeon()
    {
        DungeonGenerator generator = new DungeonGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateRooms(maxIterations, roomWidthMin, roomLengthMin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}