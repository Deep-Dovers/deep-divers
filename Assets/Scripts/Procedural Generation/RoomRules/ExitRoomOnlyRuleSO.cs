using UnityEngine;

[CreateAssetMenu(fileName = "ExitRoomOnlyRule", menuName = "Dungeon/Spawn Rules/ExitRoomOnly")]
public class ExitRoomOnlyRuleSO : RoomSpawnRuleSO
{
    public override bool CanSpawn(RoomNode fromRoom, DungeonGeneratorContext context, Direction incomingDirection)
    {
        // This room can only spawn if we are placing the exit.
        // The exit is always the *last* room in the main path.
        //
        // The generator knows the intended dungeon length
        // (stored in context.m_DungeonLength).
        //
        // So the exit room is valid only when we're exactly
        // dungeonLength - 1 steps away from the start.

        int currentDepth = context.m_Rooms.Count;
        return currentDepth == context.m_DungeonLength - 1;
    }
}