#if DOTWEEN
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Brainamics.Core
{
    public static class DOTweenExtensions
    {
        public static T WhenDone<T>(this T t, TweenCallback action) where T : Tween
        {
            if (t.IsComplete())
            {
                action.Invoke();
                return t;
            }

            t.onComplete += action;
            return t;
        }
    }
}
#endif