using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class ScaleAnimator : AnimatorEffectBase
    {
        private Coroutine _coroutine;

        [SerializeField]
        private bool _autoInitialScale;

        [SerializeField]
        private bool _useMiddleScale = true;
        
        [SerializeField]
        private bool _useInitialScaleAsTarget = false;

        [SerializeField]
        private float _middleScaleDuration = 0.1f;

        [SerializeField]
        private Vector3 _initialScale;

        [SerializeField]
        private Vector3 _middleScale = new(1.1f, 1.1f, 1.1f);

        [SerializeField]
        private Vector3 _targetScale = Vector3.one;

        public override void Trigger()
        {
            if (_coroutine != null)
                return;
            var initialScale = _autoInitialScale ? transform.localScale : _initialScale;
            if (initialScale == _targetScale)
                return;
            _coroutine = StartCoroutine(PlayAnimation(initialScale));
        }

        private IEnumerator PlayAnimation(Vector3 initialScale)
        {
            var targetScale = _useInitialScaleAsTarget ? initialScale : _targetScale;
        
            if (_useMiddleScale)
            {
                var bounceDuration = _middleScaleDuration / 2;

                var e = ScaleTo(initialScale, _middleScale, bounceDuration);
                while (e.MoveNext())
                    yield return e.Current;

                e = ScaleTo(_middleScale, targetScale, bounceDuration);
                while (e.MoveNext())
                    yield return e.Current;
            } else {
                var e = ScaleTo(initialScale, targetScale, _duration);
                while (e.MoveNext())
                    yield return e.Current;
            }
            _coroutine = null;
        }

        private IEnumerator ScaleTo(Vector3 initialScale, Vector3 targetScale, float duration)
        {
            var start = Time.time;
            var scaleDiff = targetScale - initialScale;
            while (true)
            {
                var passedTime = Time.time - start;
                if (passedTime >= duration)
                    break;
                transform.localScale = initialScale + scaleDiff * (passedTime / duration);
                yield return null;
            }
            transform.localScale = targetScale;
        }
    }
}
