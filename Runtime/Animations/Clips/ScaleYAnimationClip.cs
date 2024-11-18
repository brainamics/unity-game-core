using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Transform/Scale Y")]
    public class ScaleYAnimationClip : TransformAnimationClipBase
    {
        public bool AutoFromScale;
        public bool AutoToScale;
        public float FromScale;
        public float ToScale = 1;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var transform = GetTargetTransform(behaviour);
            var fromScale = AutoFromScale ? transform.localScale.y : FromScale;
            var toScale = AutoToScale ? transform.localScale.y : ToScale;
            return RunTimedLoop(lerp =>
            {
                var scale = transform.localScale;
                scale.y = Mathf.LerpUnclamped(fromScale, toScale, lerp);
                transform.localScale = scale;
            });
        }
    }
}