using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public struct TutorialPointAtRequest
    {
        public static readonly TutorialPointAtRequest Invisible = default;

        public static readonly TutorialPointAtRequest InvisibleImmediate = new()
        {
            Visible = false,
            Immediate = true
        };

        public bool Visible;
        public SpaceCoordinates TargetPosition;
        public bool Immediate;
        public float TransitionDuration;
        public AnimationCurve TransitionCurve;

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(Visible, TargetPosition, Immediate, TransitionDuration, TransitionCurve);
        }

        public override readonly bool Equals(object obj)
        {
            if (obj is TutorialPointAtRequest r)
            {
                return Visible == r.Visible && TargetPosition == r.TargetPosition && Immediate == r.Immediate && TransitionDuration == r.TransitionDuration && TransitionCurve == r.TransitionCurve;
            }

            return false;
        }

        public static bool operator ==(TutorialPointAtRequest a, TutorialPointAtRequest b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(TutorialPointAtRequest a, TutorialPointAtRequest b)
        {
            return !a.Equals(b);
        }
    }
}
