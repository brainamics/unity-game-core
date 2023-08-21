using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    [DefaultExecutionOrder(-1000)]
    public class TransformChangeNotifier : MonoBehaviour
    {
        private bool _transformChangedThisFrame;
        private bool _performedResetLastFrame = false;
        private Coroutine _resetCoroutine;

        public bool FireOnStart = true;

        public UnityEvent<Transform> OnTransformChanged = new();

        public bool TransformChangedThisFrame => _transformChangedThisFrame || transform.hasChanged;

        public static TransformChangeNotifier For(GameObject obj, bool fireOnStart = false)
        {
            if (obj.TryGetComponent<TransformChangeNotifier>(out var notifier))
                return notifier;
            notifier = obj.AddComponent<TransformChangeNotifier>();
            notifier.FireOnStart = fireOnStart;
            if (!fireOnStart)
                obj.transform.hasChanged = false;
            return notifier;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TransformChangeNotifier For(Behaviour behaviour, bool fireOnStart = false)
        {
            return For(behaviour.gameObject, fireOnStart);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TransformChangeNotifier For(Transform transform, bool fireOnStart = false)
        {
            return For(transform.gameObject, fireOnStart);
        }

        private void OnEnable()
        {
            EnsureResetCoroutineInitialized(FireOnStart);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _resetCoroutine = null;
        }

        private void Update()
        {
            if (!_performedResetLastFrame)
                EnsureResetCoroutineInitialized(false);
            _performedResetLastFrame = false;
            if (transform.hasChanged)
                _transformChangedThisFrame = true;

            if (!_transformChangedThisFrame)
                return;

            OnTransformChanged.Invoke(transform);
        }

        private void EnsureResetCoroutineInitialized(bool fireEvent)
        {
            if (_performedResetLastFrame)
                return;

            if (_resetCoroutine != null)
                StopCoroutine(_resetCoroutine);
            _performedResetLastFrame = true;
            _resetCoroutine = StartCoroutine(ResetAfterEachFrame());

            if (fireEvent)
            {
                OnTransformChanged.Invoke(transform);
            }
        }

        private IEnumerator ResetAfterEachFrame()
        {
            var waitFrameEnd = new WaitForEndOfFrame();

            while (true)
            {
                if (_transformChangedThisFrame)
                {
                    transform.hasChanged = false;
                    _transformChangedThisFrame = false;
                }
                _performedResetLastFrame = true;
                yield return waitFrameEnd;
            }
        }
    }
}
