using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Brainamics.Core
{
    public static class StyleExtensions
    {
        public static void SetBorderWidth(this IStyle style, float value)
        {
            style.borderLeftWidth = value;
            style.borderRightWidth = value;
            style.borderTopWidth = value;
            style.borderBottomWidth = value;
        }

        public static void SetBorderColor(this IStyle style, Color value)
        {
            style.borderLeftColor = value;
            style.borderRightColor = value;
            style.borderTopColor = value;
            style.borderBottomColor = value;
        }
    }
}