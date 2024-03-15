using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Brainamics.Core
{
    internal static class EditorUtils
    {
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
