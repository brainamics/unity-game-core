using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Transform/Scale X")]
    public class ScaleXAnimationClip : TransformAnimationClipBase
    {
        public bool AutoFromScale;
        public float FromScale;
        public float ToScale = 1;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var transform = GetTargetTransform(behaviour);
            var fromScale = AutoFromScale ? transform.localScale.x : FromScale;
            return RunTimedLoop(lerp =>
            {
                var scale = transform.localScale;
                scale.x = Mathf.LerpUnclamped(fromScale, ToScale, lerp);
                transform.localScale = scale;
            });
        }
    }
}