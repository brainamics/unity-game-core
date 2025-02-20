using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Brainamics.Core
{
    public static class EnumUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TElement[] CreateArrayForEnum<TElement, TEnum>()
            where TEnum : struct
        {
            return new TElement[Enum.GetValues(typeof(TEnum)).Length];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetValues<T>()
            where T : struct
        {
            return Enum.GetValues(typeof(T)).OfType<T>();
        }

        public static IEnumerable<T> GetFlags<T>(this T @enum)
            where T : struct
        {
            var rem = Convert.ToInt64(@enum);
            for (var i = 0; rem > 0; i++)
            {
                var bit = (rem & 1) > 0;
                rem >>= 1;
                if (!bit)
                    continue;
                yield return (T)Enum.ToObject(typeof(T), 1 << i);
                // yield return (T)Convert.ChangeType(1 << i, typeof(T));
            }
        }
    }
}
