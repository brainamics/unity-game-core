using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public struct SerializableVector3
    {        
        public SerializableVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    
        public SerializableVector3(Vector3 value)
        {
            X = value.x;
            Y = value.y;
            Z = value.z;
        }
    
        public float X;
        public float Y;
        public float Z;
    
        public static implicit operator Vector3(SerializableVector3 value)
        {
            return new(value.X, value.Y, value.Z);
        }
    
        public static implicit operator SerializableVector3(Vector3 value)
        {
            return new SerializableVector3(value);
        }
    }
}
