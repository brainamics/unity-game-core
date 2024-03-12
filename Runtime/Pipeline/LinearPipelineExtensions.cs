using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class LinearPipelineExtensions
    {
        public static Coroutine StartCoroutine(this LinearPipeline pipeline, MonoBehaviour host, IEnumerator routine, Action doneCallback = null)
        {
            var feedbackHandle = pipeline.RegisterFeedbackHandle();
            return host.StartCoroutine(ExecuteCo(routine, feedbackHandle, doneCallback));

            static IEnumerator ExecuteCo(IEnumerator routine, IDisposable feedbackHandle, Action doneCallback)
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
                }
            }
        }
    }
}
