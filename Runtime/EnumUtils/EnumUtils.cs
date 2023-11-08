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
    }
}