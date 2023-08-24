using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public struct TutorialPointAtRequest
    {
        public static readonly TutorialPointAtRequest Invisible = default;

        public static readonly TutorialPointAtRequest InvisibleImmediate = new() {
            Visible = false,
            Immediate = true
        };

        public bool Visible;
        public SpaceCoordinates TargetPosition;
        public bool Immediate;
        public float TransitionDuration;
        public AnimationCurve TransitionCurve;
    }
}
