using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class RandomUtils
    {
        public static bool Bool()
        {
            return Random.value >= 0.5;
        }
        
        public static T Select<T>(IReadOnlyList<T> list)
        {
            return list.Count switch
            {
                0 => default,
                1 => list[0],
                _ => list[Random.Range(0, list.Count)],
            };
        }

        public static float Get(Vector2 range, AnimationCurve curve, int samples = 1000)
        {
            return Get(range.x, range.y, curve, samples);
        }

        public static float Get(float min, float max, AnimationCurve curve, int samples = 1000)
        {
            // 1. Sample the curve to build a cumulative distribution function (CDF)
            var cdf = new float[samples];
            var totalArea = 0f;
            var stepSize = (max - min) / (samples - 1);

            for (var i = 0; i < samples; i++)
            {
                var x = min + stepSize * i;
                var y = curve.Evaluate(x);
                totalArea += y;
                cdf[i] = totalArea;
            }

            // 2. Normalize the CDF
            for (var i = 0; i < samples; i++)
            {
                cdf[i] /= totalArea;
            }

            // 3. Use a random value to select a position in the CDF
            var randomValue = Random.value;
            for (var i = 0; i < samples; i++)
            {
                if (randomValue <= cdf[i])
                {
                    // Return the corresponding x value
                    return min + stepSize * i;
                }
            }

            // In case of an error or edge case, return the max value
            return max;
        }
    }
}
