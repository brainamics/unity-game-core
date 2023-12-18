using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public struct SerializableQuaternion
    {
        public SerializableQuaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public SerializableQuaternion(Quaternion value)
        {
            X = value.x;
            Y = value.y;
            Z = value.z;
            W = value.w;
        }

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public float W { get; set; }

        public static implicit operator Quaternion(SerializableQuaternion value)
        {
            return new(value.X, value.Y, value.Z, value.W);
        }

        public static implicit operator SerializableQuaternion(Quaternion value)
        {
            return new SerializableQuaternion(value);
        }
    }
}
