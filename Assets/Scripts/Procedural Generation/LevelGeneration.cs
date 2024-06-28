using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    const int MAX_WALKER = 3;
    RandomWalker[] _walker = new RandomWalker[MAX_WALKER];
     
    Room[,] _rooms;

    List<Vector2Int> _takenPositionInGrid = new();

    [SerializeField]
    private int gridSizeX = 9;
    [SerializeField]
    private int gridSizeY = 8;

    [SerializeField]
    private int maxNumberOfRoom = 15;

    //public Dictionary<ROOM_TYPE, List<GameObject>> RoomList;

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
        SpawnRandomWalker(Vector2Int.zero);
        while(_takenPositionInGrid.Count <= maxNumberOfRoom)
        {
            foreach (RandomWalker walker in _walker)
            {
                if (walker == null) continue;
                walker.UpdateWalker();
                Vector2Int prev = walker.CurrentPosition;
                if (walker.CheckOutOfBoundGrid(gridSizeX, gridSizeY))
                {
                    //kill this stupid walker for going out of bound
                    walker.SetWalkerPosition(prev);
                    continue;
                }
                Vector2Int currPos = walker.CurrentPosition;
                if (!_takenPositionInGrid.Contains(currPos))
                    _takenPositionInGrid.Add(currPos);
            }
        }

       
        //Debug Print all available position taken
        foreach (var position in _takenPositionInGrid)
        {
            Debug.Log(position);
        }

	}
    void CreateStartingRoom()
    {
        //Starting the room at the center
        int x = Mathf.FloorToInt(gridSizeX * 0.5f);
        int y = Mathf.FloorToInt(gridSizeY * 0.5f);

        _rooms[x, y] = new Room(Vector2Int.zero, ROOM_TYPE.ENTRY);
        _takenPositionInGrid.Add(Vector2Int.zero);
    }
    void SetRoomDoors()
    {

    }
    void SpawnRandomWalker(Vector2Int currPos)
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
