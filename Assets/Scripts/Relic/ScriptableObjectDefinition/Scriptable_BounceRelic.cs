using Relics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BounceRelic",
menuName = "Scriptable Objects/Relic/Bounce Relic", order = 0)]
public class Scriptable_BounceRelic : Scriptable_RelicBase
{
    public int BounceTime = 1;

    public override void ApplyToPlayer(GameObject player)
    {
        Debug.Log("bouncy boi");

        //apply modifier here
    }

    public override void RemoveFromPlayer(GameObject player) { }
}