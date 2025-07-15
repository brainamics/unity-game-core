using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    public abstract class TransformAnimationClipBase : AnimationClipBase
    {
        [SerializeField]
        private Transform _targetTransform;

        protected Transform TryGetTargetTransform(MonoBehaviour behaviour)
        {
            return _targetTransform;
        }

        protected Transform GetTargetTransform(MonoBehaviour behaviour)
        {
            var transform = TryGetTargetTransform(behaviour);
            if (transform == null)
                throw new System.InvalidOperationException("Could not find the target transform.");
            return transform;
        }
    }
}
