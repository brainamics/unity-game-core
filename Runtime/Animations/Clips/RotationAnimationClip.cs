using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Transform/Rotation")]
    public class RotationAnimationClip : TransformAnimationClipBase
    {
        public bool LocalTransform = true;
        public bool AutoFromRotation;
        public Vector3 FromRotation;
        public Vector3 ToRotation = Vector3.one;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var transform = GetTargetTransform(behaviour);
            var fromRotation = AutoFromRotation ? GetRotation(transform) : FromRotation;
            return RunTimedLoop(lerp =>
            {
                var rot = Vector3.LerpUnclamped(fromRotation, ToRotation, lerp);
                SetRotation(transform, rot);
            });
        }

        private Vector3 GetRotation(Transform transform)
        {
            return LocalTransform ? transform.localEulerAngles : transform.eulerAngles;
        }

        private void SetRotation(Transform transform, Vector3 rot)
        {
            if (LocalTransform)
                transform.localEulerAngles = rot;
            else
                transform.eulerAngles = rot;
        }
    }
}
