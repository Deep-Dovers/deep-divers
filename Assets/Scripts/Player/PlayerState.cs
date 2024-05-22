using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerState : NetworkBehaviour
{
    public float MaxHealth = 100f;
    public float CurrentHealth = 100f;
    public float HealthPercent => (CurrentHealth / MaxHealth);

    //arg = percentage
    public UnityEvent<float> EOnHealthChanged = new();

    public override void OnDestroy()
    {
        EOnHealthChanged.RemoveAllListeners();
    }

    public void SetHealth(float health)
    {
        CurrentHealth = health;
        EOnHealthChanged?.Invoke(CurrentHealth / MaxHealth);
    }

    /// <summary>
    /// Use this to decrease as well, just put in negative number
    /// </summary>
    /// <param name="toAdd">Number to add to current health capped to max</param>
    public void AddHealth(float toAdd)
    {
        SetHealth(CurrentHealth + toAdd);
    }
}
