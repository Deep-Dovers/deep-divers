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
        BulletPenetration,  //how many enemies can the bullet hit before it expires
        BulletImpact,       //what happens on impact
        BulletSpawn,        //what happens on spawn
        BulletEndLIfe,      //what happens on end of lifetime WITHOUT impact
    }

    public EModifyType ModifyType = EModifyType.Add;
    public EModifies Modifies = EModifies.BulletCount;

    [Header("UI")]
    public string AbilityName = "Lorem Ipsum My Foot";
    [ShowAssetPreview]
    public Sprite Icon;

    /// <summary>
    /// This is for applying any modification to the ability instance,
    /// i.e adding damage, etc
    /// </summary>
    /// <param name="modTo">the target</param>
    public virtual void ApplyModifier(in AbilityInstanceBase modTo)
    {
        switch (ModifyType)
        {
            case EModifyType.Add:
                break;
            case EModifyType.Subtract:
                break;
            case EModifyType.Multiply:
                break;
            case EModifyType.Set:
                break;
            case EModifyType.Event:
                break;
            default:
                break;
        }
    }
}