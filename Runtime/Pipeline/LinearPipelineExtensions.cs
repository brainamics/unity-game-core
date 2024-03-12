using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class LinearPipelineExtensions
    {
        public static Coroutine StartCoroutine(this LinearPipeline pipeline, MonoBehaviour host, IEnumerator routine)
        {
            var feedbackHandle = pipeline.RegisterFeedbackHandle();
            return host.StartCoroutine(ExecuteCo(routine, feedbackHandle));

            static IEnumerator ExecuteCo(IEnumerator routine, IDisposable feedbackHandle)
            {
                try
                {
                    while (routine.MoveNext())
                        yield return routine.Current;
                }
                finally
                {
                    feedbackHandle.Dispose();
                }
            }
        }
    }
}
