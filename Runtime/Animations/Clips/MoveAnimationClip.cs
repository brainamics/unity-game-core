using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Transform/Move")]
    public class MoveAnimationClip : TransformAnimationClipBase
    {
        public bool LocalSpace = true;
        public bool AutoFromPosition;
        public Vector3 FromPosition;
        public Vector3 ToPosition;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var transform = GetTargetTransform(behaviour);
            var fromPosition = AutoFromPosition ? transform.position : FromPosition;
            return RunTimedLoop(lerp => SetCurrentValue(transform, Vector3.LerpUnclamped(fromPosition, ToPosition, lerp)));
        }

        private Vector3 GetCurrentValue(Transform t)
            => LocalSpace ? t.localPosition : t.position;

        private void SetCurrentValue(Transform t, Vector3 value)
        {
            if (LocalSpace)
                t.localPosition = value;
            else
                t.position = value;
        }
    }
}