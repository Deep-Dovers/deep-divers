using Relics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PenetrationRelic",
menuName = "Scriptable Objects/Relic/Penetration Relic", order = 2)]
public class Scriptable_PenetrationRelic : Scriptable_RelicBase
{
    public override void ApplyToPlayer(GameObject player)
    {
        Debug.Log("penetration");

        //apply modifier here
    }

    public override void RemoveFromPlayer(GameObject player) { }
}