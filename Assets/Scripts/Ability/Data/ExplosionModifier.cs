using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityModifier",
menuName = "Scriptable Objects/Ability/ExplosionModifier Data", order = 51)]
public class ExplosionModifier : AbilityModifierBase
{
    [HorizontalLine]
    [Header("Explosion Specific Variables")]
    public Vector2 Scale = Vector2.one;
    public GameObject ExplosionPrefab;

    public override void ApplyModifier(in AbilityInstanceBase modTo)
    {
        modTo.EOnBulletImpact.AddListener(OnBulletImpact);
    }

    private void OnBulletImpact(Vector3 spawnPos)
    {
        var go = Instantiate(ExplosionPrefab, spawnPos, Quaternion.identity);
        go.transform.localScale = Scale;

        Destroy(go, 0.9f);
    }
}
