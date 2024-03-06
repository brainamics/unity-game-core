using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public readonly struct SpaceBounds
    {
        public readonly CoordinateMode Mode;
        public readonly Bounds Bounds;
        public readonly Rect Rect;

        public SpaceBounds(CoordinateMode mode, Bounds bounds)
        {
            Mode = mode;
            Bounds = bounds;
            var min = Bounds.min;
            var max = Bounds.max;
            Rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        public SpaceBounds(CoordinateMode mode, Rect rect)
        {
            Mode = mode;
            Rect = rect;
            var min = rect.min;
            var max = rect.max;
            Bounds = default;
            Bounds.SetMinMax(min, max);
        }

        public override string ToString()
        {
            return $"Bounds[{Mode}] {(Mode == CoordinateMode.Screen ? Rect : Bounds)}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Mode, Bounds);
        }

        public override bool Equals(object obj)
        {
            if (obj is SpaceBounds b)
                return Mode == b.Mode && Bounds == b.Bounds && Rect == b.Rect;
            return false;
        }

        public static bool operator ==(SpaceBounds a, SpaceBounds b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(SpaceBounds a, SpaceBounds b)
        {
            return !a.Equals(b);
        }
    }

}
