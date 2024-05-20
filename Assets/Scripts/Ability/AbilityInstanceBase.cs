using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for all abilities
/// </summary>
public class AbilityInstanceBase
{
    [Header("UI Values")]
    public string AbilityName = "Lorem Ipsum My Foot";

    [Header("Type")]
    public Relics.RelicSkillTypes Type;

    [Header("Modifiers")]
    public List<AbilityModifierBase> Modifiers = new();

    [Header("Basic Values")]
    [SerializeField, NaughtyAttributes.ReadOnly]
    private AbilityData m_data; //base ability data without modifiers
    public AbilityData AbilityData => m_data;
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
    public float InitialCooldownTime { get; protected set; } = 0f;

    public float CurrentCooldown { get; protected set; } = 0f;
    private Vector3 m_myPos = Vector3.zero;
    private Vector2 m_abilityDir = Vector3.zero;

    public UnityEvent<float> EOnAbilityCooldownUpdate { get; protected set; } = new();
    public UnityEvent<bool> EOnAbilityTriggered { get; protected set; } = new();

    public AbilityInstanceBase()
    {
        m_data = null;

        CurrentCooldown = 0f;
    }

    public AbilityInstanceBase(AbilityData data)
    {
        m_data = data;

        AbilityName = data.AbilityName;

        ProjectileCount = data.ProjectileCount;
        BulletLifetime = data.BulletLifetime;
        BulletDamage = data.BulletDamage;
        BulletSpeed = data.BulletSpeed;
        BulletMaxTravelRange = data.BulletMaxTravelRange;
        BulletMaxBounce = data.BulletMaxBounce;
        BulletMinBounce = data.BulletMinBounce;
        CooldownTime = data.CooldownTime;
        InitialCooldownTime = data.InitialCooldownTime;

        CurrentCooldown = CooldownTime;
    }

    public virtual void Execute(Vector3 myPos, Vector3 dir)
    {
        m_myPos = myPos;
        m_abilityDir = dir;

        ApplyModifiers();
        EOnAbilityTriggered?.Invoke(true);
        SpawnBullets();
    }

    public virtual void SpawnBullets()
    {
        GameObject toSpawn = m_data ? m_data.SpawnObject : null;

        for (int i = 0; i < ProjectileCount; i++)
        {
            if(toSpawn)
            {
                ProjectileBase p = GameObject.Instantiate(toSpawn, m_myPos, Quaternion.identity).GetComponent<ProjectileBase>();

                p.SetTravelDirection(m_abilityDir);
                p.Setup(BulletDamage, BulletSpeed, BulletLifetime, BulletMaxTravelRange);
            }
        }
    }

    public virtual void ApplyModifiers()
    {
        foreach(var mod in Modifiers)
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
