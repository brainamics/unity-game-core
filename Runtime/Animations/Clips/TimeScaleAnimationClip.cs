using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [AnimationClip(MenuName = "Time/Time Scale")]
    [System.Serializable]
    public class TimeScaleAnimationClip : AnimationClipBase
    {
        public bool AutoFrom;
        public float FromScale = 1;
        public float ToScale;

        public TimeScaleAnimationClip()
        {
            TimeSettings.UnscaledTime = true;
        }

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var fromScale = AutoFrom ? UnityEngine.Time.timeScale : FromScale;
            return RunTimedLoop(lerp => UnityEngine.Time.timeScale = Mathf.Lerp(fromScale, ToScale, lerp));
        }
    }
}
