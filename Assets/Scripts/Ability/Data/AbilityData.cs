using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData",
menuName = "Scriptable Objects/Ability/Ability Data", order = 1)]
public class AbilityData : ScriptableObject
{
    [Header("UI Values")]
    public string AbilityName;
    [NaughtyAttributes.ShowAssetPreview]
    public Sprite UIIcon;

    [Header("Type")]
    public Relics.RelicSkillTypes Type;
    public AbilityInstanceBase AbilityScriptRef;
    
    [Header("Basic Values")]
    public int ProjectileCount = 1;
    //public float ProjectileSpawnAngle = 0f;
    //public float BulletSpawnWait = 0f;
    public float BulletLifetime = 3f;
    public float BulletDamage = 1f;
    public float BulletSpeed = 10f;
    public float BulletMaxTravelRange = 10f;
    public int BulletMaxBounce = 0;
    public int BulletMinBounce = 0;

    [Header("Cooldown")]
    [Tooltip("Cooldown time")]
    public float CooldownTime = 1f;
    //start with cd or not
    [Tooltip("Some abilities might want to start with cd instead of being instantly available")]
    public float InitialCooldownTime = 0f;

    [Header("Prefab")]
    public GameObject SpawnObject;

    public AbilityInstanceBase CreateAbilityInstance()
    {
        return new AbilityInstanceBase(this);
    }
}