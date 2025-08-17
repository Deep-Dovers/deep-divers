using UnityEngine;

public abstract class RoomSpawnRuleSO : ScriptableObject
{
    public abstract bool CanSpawn(RoomNode fromRoom, DungeonGeneratorContext context, Direction dir);
}