using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Game Object/Component Activation")]
    public class ComponentActivationAnimationClip : AnimationClipBase
    {
        public List<Behaviour> Components;
        public bool To;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            return RunTimedLoop(t =>
            {
                foreach (var component in Components)
                {
                    component.enabled = Lerp(component.enabled, To, t);
                }
            });
        }

        private bool Lerp(bool a, bool b, float t)
        {
            return t < 0.5f ? a : b;
        }
    }
}
