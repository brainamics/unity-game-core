using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [AnimationClip(MenuName = "Canvas/Canvas Group")]
    [System.Serializable]
    public class CanvasGroupAnimationClip : AnimationClipBase
    {
        public CanvasGroup Group;
        public bool AutoFromAlpha;
        public float FromAlpha;
        public float ToAlpha = 1f;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var group = Group == null ? behaviour.GetComponent<CanvasGroup>() : Group;
            if (group == null)
                throw new System.InvalidOperationException("Could not find the target canvas group.");
            var fromAlpha = AutoFromAlpha ? group.alpha : FromAlpha;
            return RunTimedLoop(lerp => group.alpha = Mathf.Lerp(fromAlpha, ToAlpha, lerp));
        }
    }
}
