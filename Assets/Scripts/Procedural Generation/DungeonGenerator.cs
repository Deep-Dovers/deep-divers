using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private DungeonSettingsSO m_Settings;
    [SerializeField] private List<RoomDefinitionSO> m_RoomDefinitions;

    private DungeonGeneratorContext m_Context;
    private System.Random m_Rng;
    private int m_AvailConnectors;

    public DungeonGeneratorContext Generate(int seed = 0)
    {
        m_Rng = (seed == 0) ? new System.Random() : new System.Random(seed);
        m_Context = new DungeonGeneratorContext(m_Settings);

        // Step 1: Determine main parameters
        int maxRooms = Mathf.Max(m_Settings.m_MinRooms, m_Settings.m_MaxRooms);
        int dungeonLength = m_Rng.Next(m_Settings.m_MinDungeonLength, maxRooms);
        m_AvailConnectors = maxRooms - 1;

        // Step 2: Place Start room
        var startPos = new Vector2Int(m_Settings.m_GridWidth / 2, m_Settings.m_GridHeight / 2);
        var startRoom = PlaceRoom(startPos, RoomType.Start, new List<Direction>());
        
        // Step 3: Generate main path to Exit
        var mainPath = GenerateMainPath(startRoom, dungeonLength);

        // Step 4: Branch generation
        GenerateBranches(mainPath);

        return m_Context;
    }

    private RoomNode PlaceRoom(Vector2Int pos, RoomType type, List<Direction> requiredDirs)
    {
        var candidates = m_RoomDefinitions
            .Where(r => r.m_RoomType == type || type == RoomType.Normal)
            .Where(r => r.m_Connectors.Count == requiredDirs.Count && !requiredDirs.Except(r.m_Connectors).Any())
            .ToList();

        if (candidates.Count == 0)
            Debug.LogError($"No matching prefab for {type} with connectors: {string.Join(",", requiredDirs)}");

        var def = candidates[m_Rng.Next(candidates.Count)];
        var node = new RoomNode(pos, def);
        m_Context.m_Rooms[pos] = node;
        return node;
    }

    private List<RoomNode> GenerateMainPath(RoomNode startRoom, int length)
    {
        var path = new List<RoomNode> { startRoom };
        var current = startRoom;

        for (int i = 0; i < length - 1; i++)
        {
            Direction dir = GetAvailableDirection(current.m_Position);
            var newPos = current.m_Position + dir.ToVector2Int();
            var newNode = PlaceRoom(newPos, (i == length - 2) ? RoomType.Exit : RoomType.Normal,
                new List<Direction> { dir.Opposite() });
            
            current.m_Connections[dir] = newNode;
            newNode.m_Connections[dir.Opposite()] = current;

            path.Add(newNode);
            current = newNode;
            m_AvailConnectors--;
        }

        return path;
    }

    private void GenerateBranches(List<RoomNode> mainPath)
    {
        Queue<RoomNode> frontier = new Queue<RoomNode>(mainPath);

        while (m_AvailConnectors > 0 && frontier.Count > 0)
        {
            var room = frontier.Dequeue();
            if (room.m_Definition.m_RoomType == RoomType.Exit) continue;

            var branchChance = GetBranchChance(room.m_Connections.Count);
            if (m_Rng.NextDouble() < branchChance)
            {
                Direction dir = GetAvailableDirection(room.m_Position);
                if (dir != default)
                {
                    var newPos = room.m_Position + dir.ToVector2Int();
                    var newNode = PlaceRoom(newPos, RoomType.Normal, new List<Direction> { dir.Opposite() });

                    room.m_Connections[dir] = newNode;
                    newNode.m_Connections[dir.Opposite()] = room;

                    frontier.Enqueue(newNode);
                    m_AvailConnectors--;
                }
            }
        }
    }

    private double GetBranchChance(int existingConnections)
    {
        return existingConnections switch
        {
            1 => m_Settings.m_BranchChanceSecond,
            2 => m_Settings.m_BranchChanceThird,
            3 => m_Settings.m_BranchChanceFourth,
            _ => 0
        };
    }

    private Direction GetAvailableDirection(Vector2Int pos)
    {
        var dirs = System.Enum.GetValues(typeof(Direction)).Cast<Direction>()
            .Where(d => !m_Context.m_Rooms.ContainsKey(pos + d.ToVector2Int()))
            .Where(d => InBounds(pos + d.ToVector2Int()))
            .ToList();

        if (dirs.Count == 0) return default;
        return dirs[m_Rng.Next(dirs.Count)];
    }

    private bool InBounds(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < m_Settings.m_GridWidth &&
               pos.y >= 0 && pos.y < m_Settings.m_GridHeight;
    }
}