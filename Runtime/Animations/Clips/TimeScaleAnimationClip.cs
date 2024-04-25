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
            TimeSettings ??= new();
            TimeSettings.UnscaledTime = true;
        }

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var fromScale = AutoFrom ? UnityEngine.Time.timeScale : FromScale;
            return RunTimedLoop(lerp =>
            {
                var scale = ToScale;
                if (fromScale != ToScale)
                {
                    scale = Mathf.Lerp(fromScale, ToScale, lerp);
                    if (float.IsNaN(scale))
                        scale = ToScale;
                }
                UnityEngine.Time.timeScale = scale;
            });
        }
    }
}
