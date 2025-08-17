using UnityEngine;

public enum Direction { Up, Down, Left, Right }

public static class DirectionExtensions
{
    public static Direction Opposite(this Direction dir)
    {
        return dir switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            _ => dir
        };
    }

    public static Vector2Int ToVector2Int(this Direction dir)
    {
        return dir switch
        {
            Direction.Up => new Vector2Int(0, 1),
            Direction.Down => new Vector2Int(0, -1),
            Direction.Left => new Vector2Int(-1, 0),
            Direction.Right => new Vector2Int(1, 0),
            _ => Vector2Int.zero
        };
    }
}