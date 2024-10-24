using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class MonoBehaviourExtensions
    {
        public static void SkipFramesAndRun(this MonoBehaviour monoBehaviour, int frames, Action action)
        {
            monoBehaviour.StartCoroutine(RunCoroutine());

            IEnumerator RunCoroutine()
            {
                for (; frames > 0; frames--)
                    yield return null;
                action();
            }
        }

        public static void RunAfterDelay(this MonoBehaviour b, float delay, Action action)
        {
            b.StartCoroutine(RunAfterDelayCo());

            IEnumerator RunAfterDelayCo()
            {
                yield return new WaitForSeconds(delay);
                action();
            }
        }
        
        public static void RunOnNextFrame(this MonoBehaviour monoBehaviour, Action action)
        {
            monoBehaviour.StartCoroutine(RunNextFrameCoroutine());

            IEnumerator RunNextFrameCoroutine()
            {
                yield return null;
                action();
            }
        }

        public static void RunWhile(this MonoBehaviour b, Action action, Func<bool> test)
        {
            b.StartCoroutine(Co());

            IEnumerator Co()
            {
                while (test())
                {
                    action();
                    yield return null;
                }
            }
        }

        public static void RunWhile(this MonoBehaviour b, float duration, Action action)
        {
            var startTime = Time.time;
            b.RunWhile(action, () => Time.time - startTime >= duration);
        }

        public static void CancelCoroutine(this MonoBehaviour b, ref Coroutine coroutine)
        {
            if (coroutine ==  null)
                return;
            b.StopCoroutine(coroutine);
            coroutine = null;
        }

        public static Coroutine StartMonoCoroutine(this MonoBehaviour b, ref Coroutine coroutine, IEnumerator routine)
        {
            b.CancelCoroutine(ref coroutine);
            return coroutine = b.StartCoroutine(routine);
        }
    }
}
