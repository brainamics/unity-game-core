using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace Brainamics.Core
{
    public static class NullableExtensions
    {
        public static bool NullableEquals<T>(this T? a, T? b)
            where T : struct
        {
            if (a.HasValue != b.HasValue)
                return false;
            if (!a.HasValue)
                return true;
            return Equals(a.Value, b.Value);
        }
    }
}
