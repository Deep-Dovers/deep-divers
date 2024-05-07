using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    const int MAX_WALKER = 3;
    RandomWalker[] _walker = new RandomWalker[MAX_WALKER];
     
    Room[,] _rooms;

    List<Vector2> _takenPosition = new List<Vector2>();

    [SerializeField]
    private int gridSizeX = 9;
    [SerializeField]
    private int gridSizeY = 8;

    [SerializeField]
    private int maxNumberOfRoom = 15;
    
    
    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();   
    }
    void GenerateLevel()
    {
        CreateRooms();
        SetRoomDoors();
    }
    void CreateRooms()
    {
        _rooms = new Room[gridSizeX, gridSizeY];
        CreateStartingRoom();
        //spawn the first walker to walk first
        SpawnRandomWalker(Vector2.zero);
        while(_takenPosition.Count <= maxNumberOfRoom)
        {
            foreach (RandomWalker walker in _walker)
            {
                if (walker == null) continue;
                walker.UpdateWalker();
                Vector2 prev = walker.GetCurrPosition();
                if (walker.CheckOutOfBoundGrid(gridSizeX, gridSizeY))
                {
                    //kill this stupid walker for going out of bound
                    walker.SetWalkerPosition(prev);
                    continue;
                }
                Vector2 currPos = walker.GetCurrPosition();
                if (!_takenPosition.Contains(currPos))
                    _takenPosition.Add(currPos);
            }
        }

       
        //Debug Print all available position taken
        foreach (var position in _takenPosition)
        {
            Debug.Log(position);
        }

	}
    void CreateStartingRoom()
    {
        //Starting the room at the center
        int x = Mathf.FloorToInt(gridSizeX * 0.5f);
        int y = Mathf.FloorToInt(gridSizeY * 0.5f);

        _rooms[x, y] = new Room(Vector2.zero, ROOM_TYPE.NORMAL);
        _takenPosition.Add(Vector2.zero);
    }
    void SetRoomDoors()
    {

    }
    void SpawnRandomWalker(Vector2 currPos)
    {
        for (int i = 0; i < MAX_WALKER; i++)
        {
            if (_walker[i] == null)
            {
                _walker[i] = new RandomWalker(currPos);
                break;
            }
        }
    }
}
