using UnityEngine;


public enum ROOM_TYPE
{
    ENTRY = 0,
    COMBAT,
    GAUNTLET,
    GAMBLE,
    MISC,
    EXIT
}

public class Room 
{
    public Vector2 m_gridPos;
    public ROOM_TYPE m_roomType;
    //using byte
    public bool m_doorUp, m_doorDown, m_doorLeft, m_doorRight;
    public Room(Vector2Int gridPos, ROOM_TYPE roomType)
	{
		m_gridPos   = gridPos;
		m_roomType  = roomType;
	}
}
