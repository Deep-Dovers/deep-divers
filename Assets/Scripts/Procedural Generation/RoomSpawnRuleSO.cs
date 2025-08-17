using UnityEngine;

public abstract class RoomSpawnRuleSO : ScriptableObject
{
    public abstract bool CanSpawn(RoomSpawnContext context);
}