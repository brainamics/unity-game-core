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
        public Vector3 FromPosition;
        public Vector3 ToPosition;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var transform = (RectTransform)GetTargetTransform(behaviour);
            var fromPosition = AutoFromPosition ? transform.position : FromPosition;
            return RunTimedLoop(lerp => transform.anchoredPosition = Vector3.LerpUnclamped(fromPosition, ToPosition, lerp));
        }
    }
}