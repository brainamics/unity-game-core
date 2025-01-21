using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Transform/Size Delta")]
    public class SizeDeltaAnimationClip : TransformAnimationClipBase
    {
        public bool AutoFromSize;
        public bool AutoToSize;
        public Vector2 FromSize;
        public Vector2 ToSize;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var transform = (RectTransform)GetTargetTransform(behaviour);
            var fromSize = AutoFromSize ? transform.anchoredPosition : FromSize;
            var toSize = AutoToSize ? transform.anchoredPosition : ToSize;
            return RunTimedLoop(lerp =>
                transform.sizeDelta = Vector2.LerpUnclamped(fromSize, toSize, lerp));
        }
    }
}