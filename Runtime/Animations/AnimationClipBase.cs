using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    public abstract class AnimationClipBase
    {
        private Coroutine _coroutine;
        private bool _keepPlaying = true;
        private System.Func<float, float> _currentEaseFunc;
        private AnimationClipEasing _currentEasing;

#if UNITY_EDITOR
        [HideInInspector]
        public bool Fold;
#endif

        public AnimationClipTimeSettings TimeSettings;

        public bool IsPlaying => _coroutine != null;

        protected float Time => TimeSettings.UnscaledTime ? UnityEngine.Time.unscaledTime : UnityEngine.Time.time;

        public virtual void Play(MonoBehaviour behaviour)
        {
            behaviour.StartMonoCoroutine(ref _coroutine, PlayCo());

            IEnumerator PlayCo()
            {
                if (TimeSettings.Delay > 0)
                    yield return WaitForSeconds(TimeSettings.Delay);

                _keepPlaying = true;
                do
                {
                    var enumerator = PlayCoroutine(behaviour);
                    while (enumerator.MoveNext())
                        yield return enumerator.Current;
                } while (_keepPlaying && TimeSettings.Loop);

                _coroutine = null;
            }
        }

        public void Stop()
        {
            _keepPlaying = false;
        }

        public void Kill(MonoBehaviour behaviour)
        {
            behaviour.CancelCoroutine(ref _coroutine);
        }

        protected abstract IEnumerator PlayCoroutine(MonoBehaviour behaviour);

        protected IEnumerator RunTimedLoop(System.Action<float> apply)
        {
            var startTime = Time;
            while (true)
            {
                var passedTime = Time - startTime;
                var progress = Mathf.Clamp01(passedTime / TimeSettings.Duration);

                var lerp = progress;
                if (TimeSettings.Reverse)
                    lerp = 1 - lerp;
                lerp = CalculateLerp(lerp);
                apply(lerp);

                if (progress >= 1)
                    break;
                yield return null;
            }
        }

        protected float CalculateLerp(float time)
        {
            var func = ObtainEaseFunc();
            return func(time);
        }

        protected object WaitForSeconds(float seconds)
        {
            if (TimeSettings.UnscaledTime)
                return new WaitForSecondsRealtime(seconds);

            // TODO use TimeScaleManager (or sth similar)
            return new WaitForSeconds(seconds);
        }

        private System.Func<float, float> ObtainEaseFunc(bool forceReload = false)
        {
            if (!forceReload && _currentEaseFunc != null && _currentEasing == TimeSettings.Ease)
                return _currentEaseFunc;

            _currentEaseFunc = TimeSettings.Ease.GetEasingFunction(TimeSettings.Curve);
            _currentEasing = TimeSettings.Ease;
            return _currentEaseFunc;
        }
    }
}
