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

        public static float Get(Vector2 range, AnimationCurve curve, int steps, OddsRandomizer<float> randomizer)
        {
            if (steps <= 0)
                throw new System.ArgumentOutOfRangeException(nameof(steps));
            if (curve.keys.Length == 0)
                return range.x;
            var minKey = curve.keys[0];
            var maxKey = curve.keys[^1];

            // 1. Get the overall integral (sum)
            var integral = MathUtils.IntegrateCurve(curve, minKey.time, maxKey.time, steps);

            // 2. Iterate through samples and create an odds randomizer out of it
            var stepSize = (maxKey.time - minKey.time) / steps;
            randomizer.Clear();
            for (var i = 0; i < steps; i++)
            {
                var time = minKey.time + (i * stepSize) + stepSize / 2f;
                var rawOdds = curve.Evaluate(time);
                var relativeOdds = rawOdds / integral;
                randomizer.AddOdds(time, relativeOdds);
            }

            // 3. Execute the randomizer
            var randomVal = randomizer.Randomize();
            return MathUtils.Remap(randomVal, minKey.time, maxKey.time, range.x, range.y);
        }

        public static float Get(Vector2 range, AnimationCurve curve, int steps)
        {
            var randomizer = new OddsRandomizer<float>();
            return Get(range, curve, steps, randomizer);
        }

        public static Vector3 Get(Vector3 min, Vector3 max)
        {
            return new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z));
        }

        public static Vector2 Get(Vector2 min, Vector2 max)
        {
            return new Vector2(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y));
        }

        public static float Get(Vector2 range)
        {
            return Random.Range(range.x, range.y);
        }
    }
}
