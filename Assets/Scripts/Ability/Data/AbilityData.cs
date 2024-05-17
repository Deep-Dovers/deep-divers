using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData",
menuName = "Scriptable Objects/Ability/Ability Data", order = 1)]
public class AbilityData : ScriptableObject
{
    [Header("UI Values")]
    public string AbilityName;
    [NaughtyAttributes.ShowAssetPreview]
    public Sprite UIIcon;
    
    [Header("Basic Values")]
    public int ProjectileCount = 1;
    public float BulletLifetime = 3f;
    public float BulletDamage = 1f;
    public float BulletSpeed = 10f;
    public float BulletMaxTravelRange = 10f;
    public int BulletMaxBounce = 0;
    public int BulletMinBounce = 0;
    [Tooltip("Cooldown time")]
    public float CooldownTime = 1f;
    //start with cd or not
    [Tooltip("Some abilities might want to start with cd instead of being instantly available")]
    public float InitialCooldownTime = 0f;

    [Header("Prefab")]
    public GameObject SpawnObject;
}