using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : AbilityInstanceBase
{
    public BasicAttack() { }
    public BasicAttack(AbilityData data) : base(data)
    {
    }

    public override void Execute()
    {
        base.Execute();
    }

    public override void SpawnBullets()
    {
        base.SpawnBullets();

        for (int i = 0; i < ProjectileCount; i++)
        {
            //temp
            GameObject b = new GameObject();
        }
    }
}
