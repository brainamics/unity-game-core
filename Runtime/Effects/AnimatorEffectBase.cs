using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public abstract class AnimatorEffectBase : MonoBehaviour
    {
        [SerializeField]
        protected bool _playOnAwake = true;

        [SerializeField]
        protected float _duration = 0.5f;

        protected virtual void Awake() {}

        protected virtual void OnEnable()
        {
            if (_playOnAwake)
                Trigger();
        }

        protected virtual void OnDisable() {}

        public abstract void Trigger();
    }
}
