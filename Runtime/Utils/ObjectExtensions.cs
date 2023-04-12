using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brainamics.Core
{
    public static class ObjectExtensions
    {
        public static IEnumerable<Transform> FindDescendants(this Transform transform, Func<Transform, bool> predicate)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (predicate(child))
                    yield return child;
                foreach (var ch in child.FindDescendants(predicate))
                    yield return ch;
            }
        }

        public static IEnumerable<Transform> FindDescendantsWithTag(this Transform transform, string tag)
        {
            return transform.FindDescendants(t => t.gameObject.CompareTag(tag));
        }

        public static Transform FindDescendantWithTag(this Transform transform, string tag)
        {
            return transform.FindDescendantsWithTag(tag).FirstOrDefault();
        }
    }
}