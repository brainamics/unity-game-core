using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public enum AnimationClipEasing
    {
        Linear,
        InSine,
        OutSine,
        InOutSine,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InElastic,
        OutElastic,
        InOutElastic,
        InBack,
        OutBack,
        InOutBack,
        InBounce,
        OutBounce,
        InOutBounce,
        Custom
    }

    public static class AnimationClipEasingExtensions
    {
        public static System.Func<float, float> GetEasingFunction(this AnimationClipEasing easing, AnimationCurve curve = null)
        {
            switch (easing)
            {
                case AnimationClipEasing.Linear: return Linear;
                case AnimationClipEasing.InSine: return InSine;
                case AnimationClipEasing.OutSine: return OutSine;
                case AnimationClipEasing.InOutSine: return InOutSine;
                case AnimationClipEasing.InQuad: return InQuad;
                case AnimationClipEasing.OutQuad: return OutQuad;
                case AnimationClipEasing.InOutQuad: return InOutQuad;
                case AnimationClipEasing.InCubic: return InCubic;
                case AnimationClipEasing.OutCubic: return OutCubic;
                case AnimationClipEasing.InOutCubic: return InOutCubic;
                case AnimationClipEasing.InQuart: return InQuart;
                case AnimationClipEasing.OutQuart: return OutQuart;
                case AnimationClipEasing.InOutQuart: return InOutQuart;
                case AnimationClipEasing.InQuint: return InQuint;
                case AnimationClipEasing.OutQuint: return OutQuint;
                case AnimationClipEasing.InOutQuint: return InOutQuint;
                case AnimationClipEasing.InExpo: return InExpo;
                case AnimationClipEasing.OutExpo: return OutExpo;
                case AnimationClipEasing.InOutExpo: return InOutExpo;
                case AnimationClipEasing.InCirc: return InCirc;
                case AnimationClipEasing.OutCirc: return OutCirc;
                case AnimationClipEasing.InOutCirc: return InOutCirc;
                case AnimationClipEasing.InElastic: return InElastic;
                case AnimationClipEasing.OutElastic: return OutElastic;
                case AnimationClipEasing.InOutElastic: return InOutElastic;
                case AnimationClipEasing.InBack: return InBack;
                case AnimationClipEasing.OutBack: return OutBack;
                case AnimationClipEasing.InOutBack: return InOutBack;
                case AnimationClipEasing.InBounce: return InBounce;
                case AnimationClipEasing.OutBounce: return OutBounce;
                case AnimationClipEasing.InOutBounce: return InOutBounce;
                case AnimationClipEasing.Custom:
                    if (curve == null)
                        throw new System.InvalidOperationException("An AnimationCurve is required for the Custom easing.");
                    return MapCurve(curve);
                default: throw new System.NotImplementedException($"Easing not implemented: {easing}");
            }
        }

        public static Func<float, float> MapCurve(AnimationCurve curve)
        {
            if (curve.keys.Length == 0)
                return curve.Evaluate;
            var maxTime = curve.keys[^1].time;
            return v => curve.Evaluate(v * maxTime);
        }

        public static float Linear(float t) => t;

        public static float InSine(float t) => 1 - Mathf.Cos((t * Mathf.PI) / 2);

        public static float OutSine(float t) => Mathf.Sin((t * Mathf.PI) / 2);

        public static float InOutSine(float t) => -(Mathf.Cos(Mathf.PI * t) - 1) / 2;

        public static float InQuad(float t) => t * t;

        public static float OutQuad(float t) => t * (2 - t);

        public static float InOutQuad(float t) => t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;

        public static float InCubic(float t) => t * t * t;

        public static float OutCubic(float t) => (--t) * t * t + 1;

        public static float InOutCubic(float t) => t < 0.5f ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;

        public static float InQuart(float t) => t * t * t * t;

        public static float OutQuart(float t) => 1 - (--t) * t * t * t;

        public static float InOutQuart(float t) => t < 0.5f ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t;

        public static float InQuint(float t) => t * t * t * t * t;

        public static float OutQuint(float t) => 1 + (--t) * t * t * t * t;

        public static float InOutQuint(float t) => t < 0.5f ? 16 * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t;

        public static float InExpo(float t) => t == 0 ? 0 : Mathf.Pow(2, 10 * (t - 1));

        public static float OutExpo(float t) => t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t);

        public static float InOutExpo(float t) => t == 0 ? 0 : t == 1 ? 1 : t < 0.5f ? Mathf.Pow(2, 20 * t - 10) / 2 : (2 - Mathf.Pow(2, -20 * t + 10)) / 2;

        public static float InCirc(float t) => 1 - Mathf.Sqrt(1 - t * t);

        public static float OutCirc(float t) => Mathf.Sqrt(1 - (--t) * t);

        public static float InOutCirc(float t) => t < 0.5f ? (1 - Mathf.Sqrt(1 - 2 * t * t)) / 2 : (Mathf.Sqrt(1 - 2 * (--t) * t) + 1) / 2;

        public static float InElastic(float t) => t == 0 ? 0 : t == 1 ? 1 : -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((t * 10 - 10.75f) * ((2 * Mathf.PI) / 3));

        public static float OutElastic(float t) => t == 0 ? 0 : t == 1 ? 1 : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * ((2 * Mathf.PI) / 3)) + 1;

        public static float InOutElastic(float t) => t == 0 ? 0 : t == 1 ? 1 : t < 0.5f ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * ((2 * Mathf.PI) / 4.5f))) / 2 : (Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * ((2 * Mathf.PI) / 4.5f))) / 2 + 1;

        public static float InBack(float t) => 2.70158f * t * t * t - 1.70158f * t * t;

        public static float OutBack(float t) => 1 + 2.70158f * (--t) * t * t + 1.70158f * t * t;

        public static float InOutBack(float t) => t < 0.5f ? (Mathf.Pow(2 * t, 2) * ((2.5949095f + 1) * 2 * t - 2.5949095f)) / 2 : (Mathf.Pow(2 * t - 2, 2) * ((2.5949095f + 1) * (t * 2 - 2) + 2.5949095f) + 2) / 2;

        public static float InBounce(float t) => 1 - OutBounce(1 - t);

        public static float OutBounce(float t)
        {
            if (t < (1 / 2.75f))
            {
                return 7.5625f * t * t;
            }
            else if (t < (2 / 2.75f))
            {
                return 7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f;
            }
            else if (t < (2.5 / 2.75))
            {
                return 7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f;
            }
            else
            {
                return 7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f;
            }
        }

        public static float InOutBounce(float t) => t < 0.5f ? (1 - OutBounce(1 - 2 * t)) / 2 : (1 + OutBounce(2 * t - 1)) / 2;
    }
}
