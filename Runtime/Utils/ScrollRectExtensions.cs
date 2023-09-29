using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Brainamics.Core
{
    public static class ScrollRectExtensions
    {
        /// <remarks>
        /// Usage:
        /// `scrollRect.content.localPosition = scrollRect.GetSnapToPositionToBringChildIntoView(child);`
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetSnapToPositionToBringChildIntoView(this ScrollRect scrollRect, RectTransform child)
        {
            Canvas.ForceUpdateCanvases();
            Vector2 viewportLocalPosition = scrollRect.viewport.localPosition;
            Vector2 childLocalPosition = child.localPosition;
            var result = new Vector2(
                0 - (viewportLocalPosition.x + childLocalPosition.x),
                0 - (viewportLocalPosition.y + childLocalPosition.y)
            );
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SnapTo(this ScrollRect scrollRect, RectTransform child)
        {
            scrollRect.content.localPosition = scrollRect.GetSnapToPositionToBringChildIntoView(child);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SnapTo(this ScrollRect scrollRect, RectTransform contentPanel, RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            contentPanel.anchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
        }
    }
}
