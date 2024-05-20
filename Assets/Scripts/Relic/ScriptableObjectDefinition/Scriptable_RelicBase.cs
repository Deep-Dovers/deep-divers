using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Relics
{
    public class Scriptable_RelicBase : ScriptableObject
    {
        [Header("Relic Settings")]
        public RelicTypes Type;
        //[ShowIf("Type", RelicTypes.Active)]
        [HideInInspector]
        public RelicSkillTypes SkillSubType;
        [ShowIf("Type", RelicTypes.Active)]
        public AbilityData Ability; //placeholder

        [ShowIf("Type", RelicTypes.Passive)]
        public AbilityModifierBase AttackBoost;
        
        public RelicRarity BaseRarity;
        //------------------------------------------------
        [HorizontalLine]
        [Header("Display Stuff")]
        public string DisplayName;
        [ResizableTextArea]
        public string DefaultDescription = "Lorem ipsum dolor hodor";
        [ShowAssetPreview]
        public Sprite DefaultIcon;

        [Header("Stats (Per Level)")]
        public List<RelicStatPerRarity> StatPerLevel = new();

        public RelicStatPerRarity FindBy(RelicRarity rarity)
        {
            return StatPerLevel.Find(x => x.Rarity == rarity);
        }

        public const string RarityRareAppend = "+";
        public const string RarityEpicAppend = "++";
        public const string RarityLegendaryAppend = "EX";

        public string GetName(RelicRarity rarity)
        {
            switch (rarity)
            {
                case RelicRarity.Rare:
                    return DisplayName + RarityRareAppend;
                case RelicRarity.Epic:
                    return DisplayName + RarityEpicAppend;
                case RelicRarity.Legendary:
                    return DisplayName + RarityLegendaryAppend;
                default:
                    return DisplayName;
            }
        }

        public string GetName(int index)
        {
            return GetName(StatPerLevel[index].Rarity);
        }

        public virtual void ApplyToPlayer(GameObject player)
        {
            var aList = player.GetComponent<AbilityList>();

            if (!aList)
                return;

            if(Type == RelicTypes.Active)
                aList.Equip(Ability);
        }

        public virtual void RemoveFromPlayer(GameObject player) 
        {
            var aList = player.GetComponent<AbilityList>();

            if (!aList)
                return;

            if (Type == RelicTypes.Active)
                aList.UnEquip(Ability);
        }
    }
}