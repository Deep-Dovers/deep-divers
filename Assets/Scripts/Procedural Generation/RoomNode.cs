using UnityEngine;
using System.Collections.Generic;

public class RoomNode
{
    public Vector2Int m_Position;
    public RoomDefinitionSO m_Definition;
    public Dictionary<Direction, RoomNode> m_Connections = new();

    public RoomNode(Vector2Int pos, RoomDefinitionSO def)
    {
        m_Position = pos;
        m_Definition = def;
    }

    public bool HasConnection(Direction dir) => m_Connections.ContainsKey(dir);
}