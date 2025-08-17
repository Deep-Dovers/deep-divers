using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnContext
{
    public Vector2Int m_GridPosition;        // Where in dungeon grid
    public RoomType m_PreviousRoomType;      // Type of room we came from
    public RoomType m_NextRoomType;          // Type of room we want to place
    public int m_PathIndex;                  // Index in main path
    public int m_TotalPathLength;            // Total length of path
    public int m_RemainingConnectors;        // How many connectors are still required
    public DungeonSettingsSO m_Settings;     // Settings reference
}