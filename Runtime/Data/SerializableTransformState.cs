using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public struct SerializableTransformState
    {
        public SerializableTransformState(SerializableVector3 position, SerializableQuaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }

        public SerializableVector3 Position { get; set; }

        public SerializableQuaternion Rotation { get; set; }

        public static SerializableTransformState World(Transform transform)
        {
            return new SerializableTransformState(transform.position, transform.rotation);
        }

        public static SerializableTransformState Local(Transform transform)
        {
            return new SerializableTransformState(transform.localPosition, transform.localRotation);
        }

        public readonly void ApplyWorld(Transform transform)
        {
            transform.position = Position;
            transform.rotation = Rotation;
        }

        public readonly void ApplyLocal(Transform transform)
        {
            transform.localPosition = Position;
            transform.localRotation = Rotation;
        }
    }
}
