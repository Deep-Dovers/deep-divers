using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityModifier",
menuName = "Scriptable Objects/Ability/AbilityModifier Data", order = 50)]
public class AbilityModifierBase : ScriptableObject
{
    public enum EModifyType
    {
        Add,                //adds to value
        Subtract,           //subtracts from main value
        Multiply,           //what it says
        Set,                //sets value directly
        Event               //non-trivial stuff that aren't value changes, eg explosion on impact
    }

    public enum EModifies
    {
        None = 0,
        BulletCount,        //spawn count
        BulletRange,        //travel range
        BulletDamage,
        BulletSpeed,
        BulletBounce,       //how many times can it bounce before it dies
        BulletImpact,       //what happens on impact
        BulletSpawn,        //what happens on spawn
    }

    public EModifyType ModifyType = EModifyType.Add;
    public EModifies Modifies = EModifies.BulletCount;

    [Header("UI")]
    public string AbilityName = "Lorem Ipsum My Foot";
    [ShowAssetPreview]
    public Sprite Icon;

    public void ApplyModifier(in AbilityInstanceBase modTo)
    {

    }
}