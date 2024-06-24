using Relics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PenetrationRelic",
menuName = "Scriptable Objects/Relic/Penetration Relic", order = 2)]
public class Scriptable_PenetrationRelic : Scriptable_RelicBase
{
    [Tooltip("This gets added on to the initial 1 hit, so 1 would be +1")]
    public int PenetrationAmt = 1;
    
    public override void ApplyToPlayer(GameObject player)
    {
        //apply modifier here
        base.ApplyToPlayer(player);
    }

    public override void RemoveFromPlayer(GameObject player) { }
}