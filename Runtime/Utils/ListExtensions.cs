using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class ListExtensions
    {
        public static IEnumerable<T> Yield<T>(this T val)
        {
            yield return val;
        }
        
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                list.Add(item);
            }
        }

        public static void Move<T>(this IList<T> list, int oldIndex, int newIndex)
        {
            var item = list[oldIndex];

            list.RemoveAt(oldIndex);

            if (newIndex > oldIndex)
                newIndex--;

            list.Insert(newIndex, item);
        }

        public static void Move<T>(this IList<T> list, T item, int newIndex)
        {
            if (item != null)
            {
                var oldIndex = list.IndexOf(item);
                if (oldIndex > -1)
                {
                    list.RemoveAt(oldIndex);

                    if (newIndex > oldIndex)
                        newIndex--;

                    list.Insert(newIndex, item);
                }
            }

        }

        public static void Push<T>(this IList<T> list, T item)
        {
            list.Add(item);
        }

        public static bool TryPop<T>(this IList<T> list, out T result)
        {
            if (list.Count == 0)
            {
                result = default;
                return false;
            }

            result = list[^1];
            list.RemoveAt(list.Count - 1);
            return true;
        }

        public static T Pop<T>(this List<T> list)
        {
            if (!TryPop(list, out var result))
                throw new System.InvalidOperationException("The list is empty.");
            return result;
        }
    }
}
