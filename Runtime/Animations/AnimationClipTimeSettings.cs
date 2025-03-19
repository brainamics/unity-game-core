using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    public sealed class AnimationClipTimeSettings
    {
        [Min(0)]
        [SerializeField]
        [FormerlySerializedAs("Delay")]
        private float _delay;

        public Vector2 RandomDelayRange = Vector2.zero;

        [Min(0)]
        public float Duration = 1f;

        public bool Immediate;
        public bool UnscaledTime;
        public bool Loop;
        public bool Reverse;

        public AnimationClipEasing Ease = AnimationClipEasing.Linear;
        public AnimationCurve Curve = AnimationCurve.Linear(0, 0, 1, 1);

        public float Delay
        {
            get
            {
                if (RandomDelayRange.y <= 0)
                    return _delay;
                return _delay + RandomUtils.Get(RandomDelayRange);
            }
        }
    }
}
