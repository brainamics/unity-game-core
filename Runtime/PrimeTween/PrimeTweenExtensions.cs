using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PRIMETWEEN_INSTALLED
using PrimeTween;
#endif

#if PRIMETWEEN_INSTALLED
public static class PrimeTweenExtensions
{
    public static void RegisterFeedback(this LinearPipeline pipeline, Tween tween)
    {
        if (!tween.isAlive)
            return;

        var handle = pipeline.RegisterFeedbackHandle();
        tween.OnComplete(handle.Dispose);
    }
}
#endif
