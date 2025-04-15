using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Animator/Set State")]
    public class SetAnimatorStateAnimation : AnimationClipBase
    {
        public Animator Animator;
        public string StateName;

        [Min(0)]
        public int Layer;

        [Range(0, 1)]
        public float NormalizedTime;

        [Min(0)]
        public float TransitionDuration;

        public bool WaitForCurrentState;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            if (!Animator)
                yield break;

            if (WaitForCurrentState)
            {
                while (true)
                {
                    var stateInfo = Animator.GetCurrentAnimatorStateInfo(Layer);
                    if (stateInfo.normalizedTime >= 1f)
                        break;
                    yield return null;
                }
            }
        
            if (TransitionDuration > 0)
            {
                Animator.CrossFade(StateName, TransitionDuration, Layer, NormalizedTime);
                yield break;
            }

            Animator.Play(StateName, Layer, NormalizedTime);
        }
    }
}
