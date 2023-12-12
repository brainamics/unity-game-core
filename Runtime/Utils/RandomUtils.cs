using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class RandomUtils
    {
        public static T Select<T>(IReadOnlyList<T> list)
        {
            return list.Count switch
            {
                0 => default,
                1 => list[0],
                _ => list[Random.Range(0, list.Count)],
            };
        }
    }
}