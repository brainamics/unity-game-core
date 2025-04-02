using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Brainamics.Core
{
    public static class VectorExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithX(this Vector3 vector, float x)
            => new(x, vector.y, vector.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithY(this Vector3 vector, float y)
            => new(vector.x, y, vector.z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithZ(this Vector3 vector, float z)
            => new(vector.x, vector.y, z);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 WithX(this Vector2 vector, float x)
            => new(x, vector.y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 WithY(this Vector2 vector, float y)
            => new(vector.x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Lerp(Vector3 a, Vector3 b, Vector3 t)
        {
            return new Vector3(
                Mathf.Lerp(a.x, b.x, t.x),
                Mathf.Lerp(a.y, b.y, t.y),
                Mathf.Lerp(a.z, b.z, t.z)
            );
        }
    }
}
