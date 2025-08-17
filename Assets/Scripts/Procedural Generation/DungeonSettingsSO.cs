using UnityEngine;

[CreateAssetMenu(fileName = "DungeonSettings",menuName = "Dungeon/Dungeon Settings")]
public class DungeonSettingsSO : ScriptableObject
{
    public int m_GridWidth = 20;
    public int m_GridHeight = 20;
    public int m_MinRooms = 7;
    public int m_MaxRooms = 12;

    [Header("Dungeon Path Length")]
    public int m_MinDungeonLength = 5;
    public int m_MaxDungeonLength = 12;

    [Header("Branching Settings")]
    [Tooltip("Probabilities for adding extra branches. Index = branch count (0=first branch, 1=second, etc).")]
    public float[] m_BranchProbabilities = new float[] { 0.5f, 0.25f, 0.1f };

    [Header("Room Size (in tiles)")]
    public int m_RoomTileWidth = 40;
    public int m_RoomTileHeight = 40;

    [Header("Tilemap Grid Settings")]
    [Tooltip("World-space size for one room grid step. If each designer room prefab is authored on a fixed Tilemap size, set this to that room's world width/height.")]
    public Vector2 m_GridCellSize = new Vector2(1.28f, 1.28f);
}