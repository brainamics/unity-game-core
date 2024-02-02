using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Brainamics.Core
{
    public static class EditorExtensions
    {
        public static void DrawAllProperties(this SerializedProperty property, bool enterChildren = true)
        {
            if (property == null)
            {
                EditorGUILayout.HelpBox("SerializedProperty is null", MessageType.Error);
                return;
            }

            EditorGUI.indentLevel++;

            var currentProperty = property.Copy();
            var startDepth = currentProperty.depth;

            while (currentProperty.NextVisible(enterChildren) && currentProperty.depth > startDepth)
            {
                enterChildren = false;
                if (currentProperty.name == "m_Script")
                {
                    continue;
                }
                EditorGUILayout.PropertyField(currentProperty, true);
            }

            EditorGUI.indentLevel--;
        }
    }
}