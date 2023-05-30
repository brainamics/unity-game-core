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

        private void Start()
        {
            if (_playOnAwake)
                Trigger();
        }

        public abstract void Trigger();
    }
}