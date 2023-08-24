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

        public SpaceCoordinates WithPosition(Vector3 position)
            => new(position, Mode);

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
    }
}
