using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    [AnimationClip(MenuName = "Invoke")]
    public class InvokeAnimationClip : AnimationClipBase
    {
        public UnityEvent OnInvoke;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            OnInvoke.Invoke();
            yield break;
        }
    }
}
