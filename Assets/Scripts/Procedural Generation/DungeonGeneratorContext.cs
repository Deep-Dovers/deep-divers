using System.Collections.Generic;
using UnityEngine;

public class DungeonGeneratorContext
{
    public DungeonSettingsSO m_Settings;
    public Dictionary<Vector2Int, RoomNode> m_Rooms = new();
        // The chosen main path length (random between 3 and maxRooms)
    public int m_DungeonLength;

    // Track available connectors left for branching
    public int m_AvailableConnectors;

    public DungeonGeneratorContext(DungeonSettingsSO settings)
    {
        m_Settings = settings;
    }
}