using NaughtyAttributes;
using Relics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionRelic",
menuName = "Scriptable Objects/Relic/Explosion Relic", order = 1)]
public class Scriptable_ExplosionRelic : Scriptable_RelicBase
{
    [HorizontalLine]
    [SerializeField]
    private float m_explosionSize = 1f;

    [Header("Spawning/Visuals")]
    private GameObject m_explosionPrefab;

    public override void ApplyToPlayer(GameObject player)
    {
        //apply modifier here
        base.ApplyToPlayer(player);
    }

    public override void RemoveFromPlayer(GameObject player) { }
}