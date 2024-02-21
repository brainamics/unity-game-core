using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class CanvasExtensions
    {
        public static Vector2 CanvasToScreen(this Canvas canvas, Vector3 position)
        {
            return canvas.renderMode switch
            {
                RenderMode.ScreenSpaceOverlay => (Vector2)position,
                RenderMode.ScreenSpaceCamera or RenderMode.WorldSpace => (Vector2)canvas.worldCamera.WorldToScreenPoint(position),
                _ => throw new System.NotImplementedException($"Canvas render mode not implemented: {canvas.renderMode}"),
            };
        }

        public static Vector2 ScreenToCanvas(this Canvas canvas, Vector3 position)
        {
            return canvas.renderMode switch
            {
                RenderMode.ScreenSpaceOverlay => position,
                RenderMode.ScreenSpaceCamera or RenderMode.WorldSpace => canvas.worldCamera.ScreenToWorldPoint(position),
                _ => throw new System.NotImplementedException($"Canvas render mode not implemented: {canvas.renderMode}"),
            };
        }
    }
}
