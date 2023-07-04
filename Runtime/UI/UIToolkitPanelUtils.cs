using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Brainamics.Core
{
    public static class UIToolkitPanelUtils
    {
        public static Vector2 PanelToScreen(this IPanel panel, Vector2 point)
        {
            var convResult = RuntimePanelUtils.ScreenToPanel(panel, new Vector2(Screen.width, Screen.height));
            var mul = new Vector2(Screen.width / convResult.x, Screen.height / convResult.y);
            return new Vector2(point.x * mul.x, Screen.height - point.y * mul.y);
        }
    }
}
