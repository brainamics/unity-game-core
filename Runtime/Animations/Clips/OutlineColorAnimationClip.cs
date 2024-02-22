using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Canvas/Outline Color")]
    public class OutlineColorAnimationClip : AnimationClipBase
    {
        public Outline Target;

        public bool AutoFrom;
        public Color From, To;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var from = AutoFrom ? Target.effectColor : From;
            return RunTimedLoop(lerp =>
            {
                Target.effectColor = Color.LerpUnclamped(from, To, lerp);
            });
        }
    }
}