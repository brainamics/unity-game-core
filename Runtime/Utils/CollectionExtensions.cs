using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brainamics.Core
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
        
        public static void PushRange<T>(this Stack<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Push(item);
            }
        }

        public static void CopyFrom(this Stack<T> stack, Stack<T> source)
        {
            stack.Clear();
            stack.PushRange(source.Reverse());
        }

        public static T AnyItem<T>(this HashSet<T> set, T defaultValue = default)
        {
            var enumerator = set.GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current : defaultValue;
        }
    }
}
