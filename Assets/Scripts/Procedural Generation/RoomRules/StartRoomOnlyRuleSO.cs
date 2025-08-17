using UnityEngine;

[CreateAssetMenu(fileName = "StartRoomOnly",menuName = "Dungeon/Spawn Rules/StartRoomOnly")]
public class StartRoomOnlyRuleSO : RoomSpawnRuleSO
{
    public override bool CanSpawn(RoomSpawnContext context)
    {
        // This room can only spawn if we're placing the Start Room.
        // The generator usually knows if it's placing the first room
        // (e.g. path length = 0, or context says "placing start").
        
        return context.m_PathIndex == 0;
    }
}