using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Relics
{
    [CreateAssetMenu(fileName = "Scriptable_RelicBase",
    menuName = "Scriptable Objects/Relic/Basic Relic Data", order = 0)]
    public class Scriptable_RelicBase : ScriptableObject
    {
        [Header("Display Stuff")]
        public RelicTypes Type;
        public string DisplayName;
        [Tooltip("Aka, +1 or +2 etc after the display name on 'upgraded' versions of relics")]
        public string UpgradeLevelAppend = "+{num}";
        [Multiline]
        public string DefaultDescription;
        public Image DefaultIcon;

        [Header("Spawn")]
        [Range(0f, 100f), Tooltip("0 to 100%")]
        public float DropRate = 10f;
        public RelicAvailableIn SpawnCondition;

        [Header("Stats (Per Level)")]
        public List<RelicStatPerLevel> StatPerLevel = new();

        public RelicStatPerLevel FindBy(int level)
        {
            return StatPerLevel.Find(x => x.Level == level);
        }
    }
}