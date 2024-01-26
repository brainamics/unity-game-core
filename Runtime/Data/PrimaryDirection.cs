using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrimaryDirection
{
    None,
    Left,
    Up,
    Right,
    Down,
}

public static class PrimaryDirectionUtils
{
    public static readonly IReadOnlyList<PrimaryDirection> Directions = new[] { PrimaryDirection.Left, PrimaryDirection.Up, PrimaryDirection.Right, PrimaryDirection.Down };

    public static PrimaryDirection RandomDirection => Directions[Random.Range(0, Directions.Count)];

    public static PrimaryDirection Rotate90Clockwise(this PrimaryDirection dir, int times = 1)
    {
        for (; times > 0; times--)
        {
            dir = dir switch
            {
                PrimaryDirection.Up => PrimaryDirection.Right,
                PrimaryDirection.Right => PrimaryDirection.Down,
                PrimaryDirection.Down => PrimaryDirection.Left,
                PrimaryDirection.Left => PrimaryDirection.Up,
                _ => throw new System.NotImplementedException($"Primary direction not implemented: {dir}"),
            };
        }
        return dir;
    }

    public static PrimaryDirection Rotate180(this PrimaryDirection dir)
        => dir.Rotate90Clockwise(2);

    public static PrimaryDirection GetSwipeDirection(Vector3 start, Vector3 end)
    {
        var angle = GetSwipeAngle(start, end);
        return DirectionFromAngle(angle);
    }

    public static float GetSwipeAngle(Vector3 start, Vector3 end)
    {
        var diff = end - start;

        var pivotVector = new Vector3(1f, 0f, 0f);
        var angle = Vector3.Angle(pivotVector, diff);
        if (diff.z < 0)
            angle = -angle;
        return angle;
    }

    private static PrimaryDirection DirectionFromAngle(float angle)
    {
        if (angle >= 360f)
        {
            angle -= Mathf.Floor(angle / 360f) * 360f;
        }
        while (angle < 0f)
            angle += 360f;
    
        return angle switch
        {
            (>= 0f and < 45f) or (>= 315f) => PrimaryDirection.Right,
            >= 45f and < 135f => PrimaryDirection.Up,
            >= 135f and < 225f => PrimaryDirection.Left,
            >= 225f and < 315f => PrimaryDirection.Down,
            _ => PrimaryDirection.None,
        };
    }
}
