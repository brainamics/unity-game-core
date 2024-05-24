using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Brainamics.Core
{
    public static class EditorUtils
    {
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
