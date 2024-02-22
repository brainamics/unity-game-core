using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Debug/Log")]
    public class LogAnimationClip : AnimationClipBase
    {
        public string Message;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            Debug.Log(Message);
            yield break;
        }
    }
}