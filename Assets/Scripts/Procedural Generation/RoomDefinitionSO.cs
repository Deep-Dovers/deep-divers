using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RoomDefinition",menuName = "Dungeon/Room Definition")]
public class RoomDefinitionSO : ScriptableObject
{
    public string m_RoomName;

    public GameObject m_Prefab;
    public RoomType m_RoomType;
        
    public Vector2Int m_TileSize = new Vector2Int(40, 40);
    public List<Direction> m_Connectors; // exact connectors this prefab supports
    public List<RoomSpawnRuleSO> m_SpawnRules;
}