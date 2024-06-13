using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for all abilities
/// </summary>
public class AbilityInstanceBase
{
    // UI Values
    public string AbilityName = "Lorem Ipsum My Foot";

    // Type
    public Relics.RelicSkillTypes Type;

    // VALUES THAT CAN BE MODIFIED BY MODIFIERS
    public AbilityData AbilityData { get; private set; }
    //declare here so that it can be modified by modifiers
    //m_data will contain the base data
    public int ProjectileCount { get; protected set; } = 1;
    public float BulletLifetime { get; protected set; } = 3f;
    public float BulletDamage { get; protected set; } = 1f;
    public float BulletSpeed { get; protected set; } = 10f;
    public float BulletMaxTravelRange { get; protected set; } = 10f;
    public int BulletMaxBounce { get; protected set; } = 0;
    public int BulletMinBounce { get; protected set; } = 0;
    public float CooldownTime { get; protected set; } = 0f;
    //start with cd or not
    public float OnGetCooldownTime { get; protected set; } = 0f;
    //================================================================
    public float CurrentCooldown { get; protected set; } = 0f;

    // BASIC, UNMODIFIED VALUES!!!!
    public int BaseProjectileCount { get; protected set; } = 1;
    public float BaseBulletLifetime { get; protected set; } = 3f;
    public float BaseBulletDamage { get; protected set; } = 1f;
    public float BaseBulletSpeed { get; protected set; } = 10f;
    public float BaseBulletMaxTravelRange { get; protected set; } = 10f;
    public int BaseBulletMaxBounce { get; protected set; } = 0;
    public int BaseBulletMinBounce { get; protected set; } = 0;
    public float BaseCooldownTime { get; protected set; } = 0f;
    //================================================================

    private Vector3 m_myPos = Vector3.zero;
    private Vector2 m_abilityDir = Vector3.zero;

    public UnityEvent<float> EOnAbilityCooldownUpdate { get; protected set; } = new();
    public UnityEvent<bool> EOnAbilityTriggered { get; protected set; } = new();

    //bullet spawn stuff
    public UnityEvent EOnBulletSpawn { get; protected set; } = new();
    public System.Action<Vector3> EOnBulletImpact;

    #region Constructor/Destructor
    public AbilityInstanceBase()
    {
        AbilityData = null;

        CurrentCooldown = 0f;
    }

    public AbilityInstanceBase(AbilityData data)
    {
        AbilityData = data;
        AbilityName = data ? data.AbilityName : GetType().Name;

        ProjectileCount = data.ProjectileCount;
        BulletLifetime = data.BulletLifetime;
        BulletDamage = data.BulletDamage;
        BulletSpeed = data.BulletSpeed;
        BulletMaxTravelRange = data.BulletMaxTravelRange;
        BulletMaxBounce = data.BulletMaxBounce;
        BulletMinBounce = data.BulletMinBounce;
        CooldownTime = data.CooldownTime;
        OnGetCooldownTime = data.InitialCooldownTime;

        CurrentCooldown = CooldownTime;
    }
    ~AbilityInstanceBase()
    {
        EOnAbilityCooldownUpdate.RemoveAllListeners();
        EOnAbilityTriggered.RemoveAllListeners();
        EOnBulletSpawn.RemoveAllListeners();

        for (int i = EOnBulletImpact.GetInvocationList().Length - 1; i >= 0; i--)
        {
            EOnBulletImpact -= EOnBulletImpact.GetInvocationList()[i] as
                System.Action<Vector3>;
        }
    }
    #endregion

    public virtual void Execute(Vector3 myPos, Vector3 dir, ref List<AbilityModifierBase> Modifiers)
    {
        m_myPos = myPos;
        m_abilityDir = dir;

        ApplyModifiers(ref Modifiers);
        EOnAbilityTriggered?.Invoke(true);
        SpawnBullets();

        //clean up
        EOnBulletSpawn.RemoveAllListeners();

        for (int i = EOnBulletImpact.GetInvocationList().Length - 1; i >= 0; i--)
        {
            EOnBulletImpact -= EOnBulletImpact.GetInvocationList()[i] as
                System.Action<Vector3>;
        }
    }

    public virtual void SpawnBullets()
    {
        GameObject toSpawn = AbilityData ? AbilityData.SpawnObject : null;

        if (!toSpawn)
        {
            Debug.Log($"{(AbilityData ? AbilityData.AbilityName : GetType().Name)} Missing bullet prefab");
            return;
        }

        for (int i = 0; i < ProjectileCount; i++)
        {
            ProjectileBase p = GameObject.Instantiate(toSpawn, m_myPos, Quaternion.identity).GetComponent<ProjectileBase>();

            p.SetTravelDirection(m_abilityDir);
            p.Setup(BulletDamage, BulletSpeed, BulletLifetime, BulletMaxTravelRange);

            p.ApplyImpactModifiers(EOnBulletImpact);
        }
    }

    public virtual void ApplyModifiers(ref List<AbilityModifierBase> Modifiers)
    {
        foreach (var mod in Modifiers)
        {
            mod.ApplyModifier(this);
        }
    }

    public void SetCooldown(float cooldown)
    {
        CurrentCooldown = cooldown;
        EOnAbilityCooldownUpdate?.Invoke(CurrentCooldown / CooldownTime);
    }

    public void RegisterUI(UIHudSkill skill)
    {
        skill.SetAbility(this);
    }
}
