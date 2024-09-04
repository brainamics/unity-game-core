using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class MathUtils
    {
        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static float RemapTo01(float value, float from, float to)
        {
            return Remap(value, from, to, 0, 1);
        }

        public static float IntegrateCurve(AnimationCurve curve, float startTime, float endTime, int steps)
        {
            return Integrate(curve.Evaluate, startTime, endTime, steps);
        }

        // Integrate function f(x) using the trapezoidal rule between x=x_low..x_high
        public static float Integrate(System.Func<float, float> f, float x_low, float x_high, int N_steps)
        {
            var h = (x_high - x_low) / N_steps;
            var res = (f(x_low) + f(x_high)) / 2;
            for (int i = 1; i < N_steps; i++)
            {
                res += f(x_low + i * h);
            }
            return h * res;
        }

        public static Vector2 OscillationRange(float number, float oscillation)
        {
            oscillation = Mathf.Abs(oscillation);
            return new(number - oscillation, number + oscillation);
        }
    }
}
