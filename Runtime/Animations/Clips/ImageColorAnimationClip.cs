using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Canvas/Image Color")]
    public class ImageColorAnimationClip : AnimationClipBase
    {
        public Image Target;

        public bool AutoFrom;
        public Color From, To;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var from = AutoFrom ? Target.color : From;
            return RunTimedLoop(lerp =>
            {
                Target.color = Color.LerpUnclamped(from, To, lerp);
            });
        }
    }
}