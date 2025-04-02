using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;

namespace Brainamics.Core
{
    public static class AxisUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetIndex(this Axis axis)
        {
            return axis switch
            {
                Axis.X => 0,
                Axis.Y => 1,
                Axis.Z => 2,
                _ => throw new System.ArgumentOutOfRangeException(nameof(axis), "Invalid axis value."),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Axis GetAxis(int index)
        {
            return index switch
            {
                0 => Axis.X,
                1 => Axis.Y,
                2 => Axis.Z,
                _ => throw new System.ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and 2.")
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetAxisValue(this Vector3Int point, Axis axis)
        {
            return axis switch
            {
                Axis.X => point.x,
                Axis.Y => point.y,
                Axis.Z => point.z,
                _ => throw new System.ArgumentOutOfRangeException(nameof(point), "Invalid axis value.")
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetAxisValue(this Vector3 point, Axis axis)
        {
            return axis switch
            {
                Axis.X => point.x,
                Axis.Y => point.y,
                Axis.Z => point.z,
                _ => throw new System.ArgumentOutOfRangeException(nameof(point), "Invalid axis value.")
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3Int SetAxisValue(this Vector3Int point, Axis axis, int value)
        {
            return axis switch
            {
                Axis.X => new Vector3Int(value, point.y, point.z),
                Axis.Y => new Vector3Int(point.x, value, point.z),
                Axis.Z => new Vector3Int(point.x, point.y, value),
                _ => throw new System.ArgumentOutOfRangeException(nameof(point), "Invalid axis value.")
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 SetAxisValue(this Vector3 point, Axis axis, float value)
        {
            return axis switch
            {
                Axis.X => new Vector3(value, point.y, point.z),
                Axis.Y => new Vector3(point.x, value, point.z),
                Axis.Z => new Vector3(point.x, point.y, value),
                _ => throw new System.ArgumentOutOfRangeException(nameof(point), "Invalid axis value.")
            };
        }
    }
}