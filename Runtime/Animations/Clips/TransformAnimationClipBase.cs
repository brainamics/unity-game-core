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

        protected Transform GetTargetTransform(MonoBehaviour behaviour)
        {
            return _targetTransform == null ? behaviour.transform : _targetTransform;
        }
    }
}
