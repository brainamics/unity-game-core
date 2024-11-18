using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Transform/Relative Move")]
    public class RelativeMoveAnimationClip : TransformAnimationClipBase
    {
        public bool LocalSpace = true;

        [Tooltip("If true, the offset is applied to the \"To\" position; otherwise the \"From\" position.")]
        public bool OffsetToTargetPosition = false;

        public Vector3 Offset;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var transform = GetTargetTransform(behaviour);
            Vector3 fromPosition, toPosition;
            fromPosition = toPosition = GetCurrentValue(transform);
            if (OffsetToTargetPosition)
                toPosition += Offset;
            else
                fromPosition += Offset;

            return RunTimedLoop(lerp => SetCurrentValue(transform, Vector3.LerpUnclamped(fromPosition, toPosition, lerp)));
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