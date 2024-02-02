using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    public sealed class AnimationClipTimeSettings
    {
        [Min(0)]
        public float Delay = 0f;

        [Min(0)]
        public float Duration = 1f;

        public bool UnscaledTime;
        public bool Loop;
        public bool Reverse;

        public AnimationClipEasing Ease = AnimationClipEasing.Linear;
        public AnimationCurve Curve = AnimationCurve.Linear(0, 0, 1, 1);
    }
}
