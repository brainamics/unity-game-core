using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Transform/Scale")]
    public class ScaleAnimationClip : TransformAnimationClipBase
    {
        public bool AutoFromScale;
        public Vector3 FromScale;
        public Vector3 ToScale = Vector3.one;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var transform = GetTargetTransform(behaviour);
            var fromScale = AutoFromScale ? transform.localScale : FromScale;
            return RunTimedLoop(lerp =>
            {
                transform.localScale = Vector3.LerpUnclamped(fromScale, ToScale, lerp);
            });
        }
    }
}
