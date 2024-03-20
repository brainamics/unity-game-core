#if DOTWEEN
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Brainamics.Core
{
    public static class LinearPipelineDOTweenExtensions
    {
        public static void RegisterFeedback(this LinearPipeline pipeline, Tween tween)
        {
            if (tween.IsComplete())
                return;
            pipeline.RegisterFeedbackHandle(tween);
            tween.WhenDone(() =>
            {
                pipeline.UnregisterFeedbackHandle(tween);
            });
            tween.WhenKilled(() =>
            {
                pipeline.UnregisterFeedbackHandle(tween);
            });
        }
    }
}
#endif