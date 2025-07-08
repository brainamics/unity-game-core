using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Flags]
public enum PrimaryDirection
{
    None = 0,
    Left = 1,
    Up = 2,
    Right = 4,
    Down = 8,
}

public static class PrimaryDirectionUtils
{
    public static readonly IReadOnlyList<PrimaryDirection> PrimaryDirections = new[] {
        PrimaryDirection.Left, PrimaryDirection.Up, PrimaryDirection.Right, PrimaryDirection.Down
    };
    public static readonly IReadOnlyList<PrimaryDirection> DiagonalDirections = new[] {
        PrimaryDirection.Left | PrimaryDirection.Up,
        PrimaryDirection.Right | PrimaryDirection.Up,
        PrimaryDirection.Left | PrimaryDirection.Down,
        PrimaryDirection.Right | PrimaryDirection.Down,
    };

    public static IEnumerable<PrimaryDirection> EnumerateDirections(bool primary = true, bool diagonal = true)
    {
        var enumerable = Enumerable.Empty<PrimaryDirection>();
        if (primary)
            enumerable = enumerable.Concat(PrimaryDirections);
        if (diagonal)
            enumerable = enumerable.Concat(DiagonalDirections);
        return enumerable;
    }

    public static PrimaryDirection RandomDirection => PrimaryDirections[Random.Range(0, PrimaryDirections.Count)];

    public static bool IsHorizontal(this PrimaryDirection dir)
    {
        return dir is PrimaryDirection.Left or PrimaryDirection.Right;
    }

    public static bool IsVertical(this PrimaryDirection dir)
    {
        return dir is PrimaryDirection.Up or PrimaryDirection.Down;
    }

    public static bool IsPrimary(this PrimaryDirection direction)
    {
        return PrimaryDirections.Contains(direction);
    }

    public static bool IsDiagonal(this PrimaryDirection direction)
    {
        return DiagonalDirections.Contains(direction);
    }

    public static IEnumerable<PrimaryDirection> ValidateAndDeconstruct(this PrimaryDirection direction)
    {
        if (direction.IsPrimary())
        {
            yield return direction;
            yield break;
        }

        if (direction.IsDiagonal())
        {
            var (horz, vert) = direction.DeconstructDiagonal();
            yield return horz;
            yield return vert;
            yield break;
        }

        throw new System.InvalidOperationException($"Invalid direction: {direction}");
    }

    public static (PrimaryDirection horz, PrimaryDirection vert) DeconstructDiagonal(this PrimaryDirection direction)
    {
        var horz = PrimaryDirection.None;
        var vert = PrimaryDirection.None;
        foreach (var flag in direction.GetFlags())
            if (flag.IsHorizontal())
                horz = flag;
            else if (flag.IsVertical())
                vert = flag;
        return (horz, vert);
    }

    public static PrimaryDirection GetOppositeDirection(this PrimaryDirection dir)
    {
        return dir switch
        {
            PrimaryDirection.Up => PrimaryDirection.Down,
            PrimaryDirection.Down => PrimaryDirection.Up,
            PrimaryDirection.Left => PrimaryDirection.Right,
            PrimaryDirection.Right => PrimaryDirection.Left,
            PrimaryDirection.Left | PrimaryDirection.Up => PrimaryDirection.Right | PrimaryDirection.Down,
            PrimaryDirection.Left | PrimaryDirection.Down => PrimaryDirection.Right | PrimaryDirection.Up,
            PrimaryDirection.Right | PrimaryDirection.Down => PrimaryDirection.Left | PrimaryDirection.Up,
            PrimaryDirection.Right | PrimaryDirection.Up => PrimaryDirection.Left | PrimaryDirection.Down,
            _ => throw new System.ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    public static PrimaryDirection Rotate90Clockwise(this PrimaryDirection dir, int times = 1)
    {
        times %= 4;
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

    public static PrimaryDirection Rotate90CounterClockwise(this PrimaryDirection dir, int times = 1)
    {
        times %= 4;
        for (; times > 0; times--)
        {
            dir = dir switch
            {
                PrimaryDirection.Up => PrimaryDirection.Left,
                PrimaryDirection.Right => PrimaryDirection.Up,
                PrimaryDirection.Down => PrimaryDirection.Right,
                PrimaryDirection.Left => PrimaryDirection.Down,
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

    public static (float angle, Vector3 delta) GetAngleOnPlane(Vector3 fromPoint, Vector3 toPoint, Vector3 referenceDirection, Vector3 up)
    {
        var plane = new Plane(up, fromPoint);
        toPoint = plane.ClosestPointOnPlane(toPoint);
        var shiftingDelta = toPoint - fromPoint;
        var angle = Vector3.SignedAngle(referenceDirection, shiftingDelta, up);
        return (angle, shiftingDelta);
    }

    public static PrimaryDirection DirectionFromAngle(float angle, bool clockwise)
    {
        if (!clockwise)
            angle = 360 - angle;
        if (angle < 0)
            angle += 360;
        angle %= 360;

        return angle switch
        {
            < 45 => PrimaryDirection.Right,
            < 135 => PrimaryDirection.Up,
            < 225 => PrimaryDirection.Left,
            < 315 => PrimaryDirection.Down,
            _ => PrimaryDirection.Right
        };
    }
}
