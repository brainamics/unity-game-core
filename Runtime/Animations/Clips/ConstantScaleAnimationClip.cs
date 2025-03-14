using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Transform/Constant Scale")]
    public class ConstantScaleAnimationClip : TransformAnimationClipBase
    {
        public AnimationCurve Curve;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var transform = GetTargetTransform(behaviour);
            return RunTimedLoop(lerp =>
            {
                var maxTime = Curve[Curve.length - 1].time;
                lerp = lerp * maxTime;
                var value = Curve.Evaluate(lerp);
                transform.localScale = new Vector3(value, value, value);
            });
        }
    }
}