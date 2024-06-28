using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomScriptable",
menuName = "Scriptable Objects/RoomList Data", order = 1)]
public class Scriptable_RoomList : ScriptableObject
{
    [Header("Pregen Settings")]
    [Tooltip("This is what the parent game object will be called")]
    public string SetName = "SetXX";
    [MinMaxSlider(2, 40)]
    public Vector2Int LevelRoomGenLimit = new(7, 15);

    [Header("Content List")]
    [Space]
    [Header("Entry Room Presets")]
    public List<GameObject> EntryRooms;
    [Header("Exit Room Presets")]
    public List<GameObject> ExitRooms;

    [HorizontalLine]
    [Header("Other Rooms")]
    [Header("Progression Room Presets")]
    public List<GameObject> ProgRooms;
    [Tooltip("MinMax range to unlock the exit room")]
    [MinMaxSlider(1, 38)]
    public Vector2Int MinProgRoomClear = new(1, 1);
    public bool UseRandomBetweenMinMax = false;

    [Header("Gamble Room Presets")]
    public List<GameObject> GambleRooms;

    [Header("Misc Room Presets")]
    public bool UseMiscRoom = false;
    public List<GameObject> MiscRooms;

    [Button]
    void CreateLevel()
    {
        int count = Random.Range(LevelRoomGenLimit.x, LevelRoomGenLimit.y + 1);
        Debug.Log("heewowooo new rooms be " + count);

        GameObject entryRmPrefab = GetRandomRoom(ref EntryRooms);
        GameObject exitRmPrefab = GetRandomRoom(ref ExitRooms);

        Transform parent = new GameObject().GetComponent<Transform>();
        parent.name = SetName;

        //spawn entry
        Transform entry = Instantiate(entryRmPrefab, parent)?.transform;
    }

    [Button]
    void ClearLevel()
    {
        GameObject parent = GameObject.Find(SetName);

        if(parent)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                Destroy(parent);
            else
                DestroyImmediate(parent);
#else
            Destroy(parent);
#endif

            if(parent == null)
                Debug.Log("Success! The level has been deleted.");

            return;
        }

        Debug.Log("Failure! The level could not be deleted.");
    }

    public List<GameObject> GetRoomList(ROOM_TYPE roomType)
    {
        switch (roomType)
        {
            case ROOM_TYPE.ENTRY:
                break;
            case ROOM_TYPE.COMBAT:
                break;
            case ROOM_TYPE.GAMBLE:
                break;
            case ROOM_TYPE.MISC:
                break;
            case ROOM_TYPE.EXIT:
                break;
            default:
                break;
        }

        return null;
    }

    GameObject GetRandomRoom(ref List<GameObject> roomList)
    {
        return roomList[Random.Range(0, roomList.Count)];
    }
}