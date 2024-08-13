using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public readonly struct SpaceCoordinates
    {
        public static readonly SpaceCoordinates Invalid = new(Vector3.zero, CoordinateMode.None);

        public readonly Vector3 Position;
        public readonly CoordinateMode Mode;

        public SpaceCoordinates(Vector3 position, CoordinateMode mode)
        {
            Position = position;
            Mode = mode;
        }

        public bool IsInvalid => Mode == CoordinateMode.None;

        public static SpaceCoordinates operator +(SpaceCoordinates coordinates, Vector3 position)
        {
            return coordinates.WithPosition(coordinates.Position + position);
        }

        public static SpaceCoordinates operator -(SpaceCoordinates coordinates, Vector3 position)
        {
            return coordinates.WithPosition(coordinates.Position - position);
        }

        public static bool operator ==(SpaceCoordinates a, SpaceCoordinates b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(SpaceCoordinates a, SpaceCoordinates b)
        {
            return !a.Equals(b);
        }

        public static SpaceCoordinates World(Vector3 position)
            => new(position, CoordinateMode.World);

        public static SpaceCoordinates Screen(Vector2 position)
            => new(position, CoordinateMode.Screen);

        public static SpaceCoordinates Viewport(Vector2 position)
            => new(position, CoordinateMode.Viewport);

        public static SpaceCoordinates FromObject(object o)
            => FromObject(o, null);

        public static SpaceCoordinates ScreenCenter => Screen(new Vector2(UnityEngine.Screen.width / 2, UnityEngine.Screen.height / 2));

        public static SpaceCoordinates CameraCenter(Camera camera) => Screen(camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)));

        public static SpaceCoordinates FromObject(object o, Func<RectTransform, Vector3> getRectTransformPos)
        {
            getRectTransformPos ??= GetScreenCenterOfRectTransform;
            switch (o)
            {
                case Behaviour b:
                    return FromObject(b.transform);

                case GameObject go:
                    return FromObject(go.transform);

                case RectTransform rectTransform:
                    var canvas = rectTransform.GetComponentInParent<Canvas>();
                    if (canvas == null)
                        throw new System.InvalidOperationException("Could not find the parent canvas for the RectTransform.");

                    return Screen(getRectTransformPos(rectTransform));

                case Transform transform:
                    return World(transform.position);

                default:
                    throw new NotImplementedException();
            }
        }

        private static Vector3 GetScreenCenterOfRectTransform(RectTransform rectTransform)
        {
            return rectTransform.position;
        }

        public SpaceCoordinates WithPosition(Vector3 position)
            => new(position, Mode);

        public SpaceCoordinates Translate(Vector3 vector)
            => WithPosition(Position + vector);

        public override bool Equals(object obj)
        {
            if (obj is SpaceCoordinates coord)
                return coord.Position == Position && coord.Mode == Mode;
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Mode, Position);
        }

        public override string ToString()
        {
            return $"{Position}@{Mode}";
        }
    }
}
