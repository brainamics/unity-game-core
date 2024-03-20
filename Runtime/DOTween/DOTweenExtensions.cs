#if DOTWEEN
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Brainamics.Core
{
    public static class DOTweenExtensions
    {
        public static bool IsKilled<T>(this T t) where T : Tween
        {
            return !t.IsActive() && !t.IsComplete();
        }

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

        public static T WhenKilled<T>(this T t, TweenCallback action) where T : Tween
        {
            if (t.IsKilled())
            {
                action.Invoke();
                return t;
            }

            t.onKill += action;
            return t;
        }
    }
}
#endif