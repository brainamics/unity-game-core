using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if DOTWEEN
using DG.Tweening;
#endif

namespace Brainamics.Core
{
    public static class LinearPipelineExtensions
    {
#if DOTWEEN
        public static void RegisterFeedback(this LinearPipeline pipeline, float duration)
        {
            var handle = pipeline.RegisterFeedbackHandle();
            DOVirtual.DelayedCall(CollectionDuration, handle.Dispose, false);
        }
#endif

        public static Coroutine StartCoroutine(this LinearPipeline pipeline, MonoBehaviour host,
            IEnumerator routine, Action doneCallback = null, ISet<IDisposable> handlesSet = null)
        {
            var feedbackHandle = pipeline.RegisterFeedbackHandle();
            handlesSet?.Add(feedbackHandle);
            return host.StartCoroutine(ExecuteCo(routine, feedbackHandle, doneCallback, handlesSet));

            static IEnumerator ExecuteCo(IEnumerator routine, IDisposable feedbackHandle, Action doneCallback,
                ISet<IDisposable> handlesSet)
            {
                try
                {
                    while (routine.MoveNext())
                        yield return routine.Current;
                }
                finally
                {
                    feedbackHandle.Dispose();
                    doneCallback?.Invoke();
                    handlesSet?.Remove(feedbackHandle);
                }
            }
        }

        public static void OnDisable(this LinearPipeline pipeline, ISet<IDisposable> handlesSet)
        {
            foreach (var handle in handlesSet)
                handle.Dispose();
            handlesSet.Clear();
        }
    }
}
