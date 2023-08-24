using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public struct TutorialPointAtRequest
    {
        public bool Visible;
        public SpaceCoordinates TargetPosition;
        public bool Immediate;
        public float TransitionDuration;
        public AnimationCurve TransitionCurve;
    }
}
