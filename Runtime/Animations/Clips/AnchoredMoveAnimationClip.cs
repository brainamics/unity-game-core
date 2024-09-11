using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Transform/Anchored Move")]
    public class AnchoredMoveAnimationClip : TransformAnimationClipBase
    {
        public bool AutoFromPosition;
        public bool AutoToPosition;
        public Vector2 FromPosition;
        public Vector2 ToPosition;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var transform = (RectTransform)GetTargetTransform(behaviour);
            var fromPosition = AutoFromPosition ? transform.anchoredPosition : FromPosition;
            var toPosition = AutoToPosition ? transform.anchoredPosition : ToPosition;
            return RunTimedLoop(lerp => transform.anchoredPosition = Vector2.LerpUnclamped(fromPosition, toPosition, lerp));
        }
    }
}
