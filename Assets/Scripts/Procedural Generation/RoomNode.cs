using System.Collections.Generic;
using UnityEngine;

public class RoomNode
{
    public Vector2Int m_GridPosition;
    public RoomDefinitionSO m_Definition;
    private Dictionary<Direction, RoomNode> m_Connections;

    public RoomNode(Vector2Int gridPos, RoomDefinitionSO def)
    {
        m_GridPosition = gridPos;
        m_Definition = def;
        m_Connections = new Dictionary<Direction, RoomNode>();
    }

    public int ConnectionCount => m_Connections.Count;

    public bool HasConnection(Direction dir)
    {
        return m_Connections.ContainsKey(dir);
    }

    public void Connect(RoomNode other, Direction dir)
    {
        if (HasConnection(dir)) return;

        m_Connections[dir] = other;
        other.m_Connections[dir.Opposite()] = this;
    }

    public List<Direction> GetAvailableDirections()
    {
        List<Direction> openDirs = new List<Direction>();

        foreach (Direction d in m_Definition.m_Connectors)
        {
            if (!HasConnection(d))
                openDirs.Add(d);
        }

        return openDirs;
    }
}
