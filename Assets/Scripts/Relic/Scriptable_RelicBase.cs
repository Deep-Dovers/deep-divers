using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Relics
{
    [CreateAssetMenu(fileName = "Scriptable_RelicBase",
    menuName = "Scriptable Objects/Relic/Basic Relic Data", order = 0)]
    public class Scriptable_RelicBase : ScriptableObject
    {
        [Header("Relic Settings")]
        public RelicTypes Type;
        [ShowIf("Type", RelicTypes.Skill)]
        public RelicSkillTypes SkillSubType;
        [ShowIf("Type", RelicTypes.Skill)]
        public string Ability; //placeholder

        [ShowIf("Type", RelicTypes.Support)]
        public List<Scriptable_RelicBase> Relics = new();
        
        public RelicRarity Rarity;
        //------------------------------------------------
        [HorizontalLine]
        [Header("Display Stuff")]
        public string DisplayName;
        [Tooltip("Aka, +1 or +2 etc after the display name on 'upgraded' versions of relics")]
        public string UpgradeLevelAppend = "+{num}";
        [Multiline]
        public string DefaultDescription = "Lorem ipsum dolor hodor";
        public Image DefaultIcon;

        [Header("Spawn")]
        public RelicSpawn SpawnSettings;

        [Header("Stats (Per Level)")]
        public List<RelicStatPerLevel> StatPerLevel = new();

        public RelicStatPerLevel FindBy(int level)
        {
            return StatPerLevel.Find(x => x.Level == level);
        }

        /// <summary>
        /// Converts the drop rate from 0-100% to 0f-1f
        /// </summary>
        /// <returns>Drop rate between 0f-1f</returns>
        public float GetNormalizedDropRate()
        {
            return SpawnSettings.DropRate / 100f;
        }
    }
}