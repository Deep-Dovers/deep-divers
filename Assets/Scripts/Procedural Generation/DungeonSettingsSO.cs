using UnityEngine;

[CreateAssetMenu(fileName = "DungeonSettings",menuName = "Dungeon/Dungeon Settings")]
public class DungeonSettingsSO : ScriptableObject
{
    public int m_GridWidth = 20;
    public int m_GridHeight = 20;
    public int m_MinRooms = 7;
    public int m_MaxRooms = 12;

    [Range(3, 10)] public int m_MinDungeonLength = 3;
    public float m_BranchChanceSecond = 0.5f;
    public float m_BranchChanceThird = 0.25f;
    public float m_BranchChanceFourth = 0.1f;
}