using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Game Object/Activation")]
    public class ObjectActivationAnimationClip : TransformAnimationClipBase
    {
        public bool AutoFrom;
        public bool From;
        public bool To;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var gameObj = GetTargetTransform(behaviour).gameObject;
            var from = AutoFrom ? gameObj.activeSelf : From;
            return RunTimedLoop(t => gameObj.SetActive(Lerp(from, To, t)));
        }

        private bool Lerp(bool a, bool b, float t)
        {
            return t < 0.5f ? a : b;
        }
    }
}
