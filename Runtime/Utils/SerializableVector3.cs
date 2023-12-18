using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float X { get; set; }

    public float Y { get; set; }

    public float Z { get; set; }

    public static implicit operator Vector3(SerializableVector3 value)
    {
        return new(value.X, value.Y, value.Z);
    }

    public static implicit operator SerializableVector3(Vector3 value)
    {
        return new SerializableVector3(value);
    }
}
