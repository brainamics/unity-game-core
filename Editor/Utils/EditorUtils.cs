using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Brainamics.Core
{
    public static class EditorUtils
    {
        public static Texture2D ColorTex(Color color)
        {
            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }

        public static Texture2D ColorTex(int width, int height, Color32 col)
        {
            var pix = new Color32[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            var result = new Texture2D(width, height);
            result.SetPixels32(pix);
            result.Apply();

            return result;
        }

        public static void HorizontalLine(float height = 1, Color lineColor = default)
        {
            if (lineColor == default)
                lineColor = new Color(0.5f, 0.5f, 0.5f, 1);
            var rect = EditorGUILayout.GetControlRect(false, height);
            rect.height = height;
            EditorGUI.DrawRect(rect, lineColor);
        }

        public static string GetRelativeAssetPath(string path)
        {
            return path.Replace(Application.dataPath, "Assets");
        }

        public static void RenderWrapped(int count, float cellWidth, float availableWidth, Action<int> render)
        {
            var elementsPerRow = Mathf.Max(1, (int)(availableWidth / cellWidth));
            EditorGUILayout.BeginHorizontal();
            for (var i = 0; i < count; i++)
            {
                if (i % elementsPerRow == 0)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }

                render(i);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
