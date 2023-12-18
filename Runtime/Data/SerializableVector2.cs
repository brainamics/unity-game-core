using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public struct SerializableVector2
    {
        public SerializableVector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public SerializableVector2(Vector2 value)
        {
            X = value.x;
            Y = value.y;
        }

        public float X { get; set; }

        public float Y { get; set; }

        public static implicit operator Vector2(SerializableVector2 value)
        {
            return new(value.X, value.Y);
        }

        public static implicit operator SerializableVector2(Vector2 value)
        {
            return new SerializableVector2(value);
        }
    }
}
