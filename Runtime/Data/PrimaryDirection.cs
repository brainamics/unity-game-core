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
}