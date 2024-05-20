using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : AbilityInstanceBase
{
    public BasicAttack() { }
    public BasicAttack(AbilityData data) : base(data)
    {
    }
    public static BasicAttack CreateInstance(AbilityData data) 
    { 
        return new BasicAttack(data); 
    }
}
