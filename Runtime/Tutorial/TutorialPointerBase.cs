using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public abstract class TutorialPointerBase : MonoBehaviour, ITutorialPointer
    {
        public abstract SpaceCoordinates Coordinates { get; }

        public abstract bool IsVisible { get; }

        public abstract bool IsClickDown { get; }

        public abstract TutorialPointAtRequest ActiveRequest { get; }

        public abstract UnityEvent<ITutorialPointer, TutorialPointAtRequest> OnPoint { get; }

        public abstract UnityEvent<ITutorialPointer> OnClickStateChanged { get; }

        public abstract void PointAt(TutorialPointAtRequest request);

        public abstract void SetClickState(bool down);

        protected void StartOmniCoroutine(ref Coroutine coroutine, IEnumerator actions, Action clearCoroutineRef)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            var finished = false;
            coroutine = StartCoroutine(Run());
            if (finished)
                coroutine = null;

            IEnumerator Run()
            {
                while (actions.MoveNext())
                    yield return actions;
                clearCoroutineRef?.Invoke();
                finished = true;
            }
        }

        protected void StopCoroutine(ref Coroutine coroutine)
        {
            if (coroutine == null)
                return;
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
