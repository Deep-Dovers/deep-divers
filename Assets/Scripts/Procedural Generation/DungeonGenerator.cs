using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private DungeonSettingsSO m_Settings;
    [SerializeField] private List<RoomDefinitionSO> m_RoomDefinitions;

    private System.Random m_Rng;
    private List<RoomNode> m_RoomNodes;
    private HashSet<Vector2Int> m_OccupiedCells; // track used grid cells

    public void Start()
    {
        GenerateDungeon();
    }
    public void GenerateDungeon(int seed = 0)
    {
        m_Rng = new System.Random();
        m_RoomNodes = new List<RoomNode>();
        m_OccupiedCells = new HashSet<Vector2Int>();

        // Clamp dungeon length within [m_MinDungeonLength, m_MaxRooms]
        int maxRooms = Mathf.Max(m_Settings.m_MinRooms, m_Settings.m_MaxRooms);
        int dungeonLength = Mathf.Clamp(
            m_Rng.Next(m_Settings.m_MinDungeonLength, maxRooms + 1),
            m_Settings.m_MinDungeonLength,
            maxRooms
        );

        // ---- Start Room (no required connector) ----
        RoomDefinitionSO startDef = GetRoomDefinitionOfType(RoomType.Start, null);
        Vector2Int startPos = Vector2Int.zero;
        RoomNode startNode = new RoomNode(startPos, startDef);
        m_RoomNodes.Add(startNode);
        m_OccupiedCells.Add(startPos);

        // ---- Main Path (shortest path by stepping 1 cell per room) ----
        RoomNode current = startNode;
        for (int i = 0; i < dungeonLength - 1; i++)
        {
            bool isExit = (i == dungeonLength - 2);

            // pick a direction that is (a) available by connectors and (b) not occupied
            Direction dir = GetRandomOpenDirection(current);
            if (dir == Direction.None) break; // no more moves possible

            Vector2Int step = dir.ToVector2Int();
            Vector2Int nextPos = current.m_GridPosition + step;
            if (m_OccupiedCells.Contains(nextPos))
                break; // prevent overlap/backtrack

            // choose a room with the required incoming connector (back to parent)
            RoomType nextType = isExit ? RoomType.Exit : RoomType.Normal;
            RoomDefinitionSO nextDef = GetRoomDefinitionOfType(nextType, dir.Opposite());
            if (nextDef == null)
                break;

            RoomNode next = new RoomNode(nextPos, nextDef);
            current.Connect(next, dir);
            m_RoomNodes.Add(next);
            m_OccupiedCells.Add(nextPos);

            current = next;
        }

        // ---- Branching (use remaining room budget) ----
        int availableBranches = maxRooms - m_RoomNodes.Count;
        if (availableBranches > 0)
        {
            Queue<RoomNode> frontier = new Queue<RoomNode>(m_RoomNodes);
            while (availableBranches > 0 && frontier.Count > 0)
            {
                RoomNode baseNode = frontier.Dequeue();

                // Don't branch from Exit
                if (baseNode.m_Definition.m_RoomType == RoomType.Exit)
                    continue;

                // Collect free, non-occupied directions
                List<Direction> freeDirs = baseNode.GetAvailableDirections();
                // Remove directions that would hit occupied cells
                freeDirs.RemoveAll(d => m_OccupiedCells.Contains(baseNode.m_GridPosition + d.ToVector2Int()));
                if (freeDirs.Count == 0) continue;

                // We'll iterate through shuffled directions and use designer branch probabilities
                ShuffleInPlace(freeDirs);

                int spawnedHere = 0;
                foreach (var dir in freeDirs)
                {
                    if (availableBranches <= 0) break;

                    float chance = GetBranchChance(baseNode.m_Definition, spawnedHere);
                    if (chance <= 0f) break;

                    if (m_Rng.NextDouble() <= chance)
                    {
                        Vector2Int pos = baseNode.m_GridPosition + dir.ToVector2Int();
                        if (m_OccupiedCells.Contains(pos)) continue;

                        RoomDefinitionSO def = GetRoomDefinitionOfType(RoomType.Normal, dir.Opposite());
                        if (def == null) continue;

                        RoomNode node = new RoomNode(pos, def);
                        baseNode.Connect(node, dir);

                        m_RoomNodes.Add(node);
                        m_OccupiedCells.Add(pos);
                        frontier.Enqueue(node);

                        availableBranches--;
                        spawnedHere++;
                    }
                }
            }
        }

        // ---- Instantiate on XY plane (Z = 0), using per-room tile size and global grid size ----
        foreach (RoomNode node in m_RoomNodes)
        {
            Vector2Int roomTile = node.m_Definition.m_TileSize; // per-room tile size
            Vector2 gridSize = m_Settings.m_GridCellSize;        // global spacing multiplier

            Vector3 worldPos = new Vector3(
                node.m_GridPosition.x * roomTile.x * gridSize.x, // X from grid X
                node.m_GridPosition.y * roomTile.y * gridSize.y, // Y from grid Y
                0f                                               // Z fixed to 0 (XY plane)
            );

            Instantiate(node.m_Definition.m_Prefab, worldPos, Quaternion.identity, transform);
        }
    }

    // --- Helpers ---

    private RoomDefinitionSO GetRoomDefinitionOfType(RoomType type, Direction? requiredConnector)
    {
        List<RoomDefinitionSO> candidates;
        if (requiredConnector.HasValue)
        {
            candidates = m_RoomDefinitions.FindAll(r =>
                r.m_RoomType == type &&
                r.m_Connectors != null &&
                r.m_Connectors.Contains(requiredConnector.Value)
            );
        }
        else
        {
            candidates = m_RoomDefinitions.FindAll(r => r.m_RoomType == type);
        }

        if (candidates == null || candidates.Count == 0)
        {
            Debug.LogError($"No RoomDefinition of type {type} that matches connector {requiredConnector}");
            return null;
        }

        return candidates[m_Rng.Next(candidates.Count)];
    }

    private Direction GetRandomOpenDirection(RoomNode node)
    {
        List<Direction> dirs = node.GetAvailableDirections();
        // prune directions that target occupied cells
        dirs.RemoveAll(d => m_OccupiedCells.Contains(node.m_GridPosition + d.ToVector2Int()));

        if (dirs.Count == 0) return Direction.None;
        return dirs[m_Rng.Next(dirs.Count)];
    }

    private float GetBranchChance(RoomDefinitionSO def, int spawnedHere)
    {
        // respect the room's connector capacity: one connector is already used by the parent link (entry)
        int maxConnectors = (def.m_Connectors != null) ? def.m_Connectors.Count : 0;
        int remaining = maxConnectors - 1 - spawnedHere; // -1 for the entry back to parent
        if (remaining <= 0) return 0f;

        if (spawnedHere < m_Settings.m_BranchProbabilities.Length)
            return m_Settings.m_BranchProbabilities[spawnedHere];

        return 0f;
    }

    private void ShuffleInPlace<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = m_Rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
