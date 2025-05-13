using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Animator/Set Bool")]
    public class AnimatorSetBoolAnimationClip : AnimationClipBase
    {
        public Animator Animator;
        public string ParamName;
        public bool Value = true;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            Animator.SetBool(ParamName, Value);

            yield break;
        }
    }
}