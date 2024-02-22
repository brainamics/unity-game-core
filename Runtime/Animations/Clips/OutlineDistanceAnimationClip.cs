using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Canvas/Outline Distance")]
    public class OutlineDistanceAnimationClip : AnimationClipBase
    {
        public Outline Target;

        public bool AutoFrom;
        public Vector2 From, To;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var from = AutoFrom ? Target.effectDistance : From;
            return RunTimedLoop(lerp =>
            {
                Target.effectDistance = Vector2.LerpUnclamped(from, To, lerp);
            });
        }
    }
}