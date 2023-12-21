using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Brainamics.Core
{
    [CustomEditor(typeof(PersistentId))]
    public class PersistentIdEditor : Editor
    {
        private PersistentId Target => (PersistentId)target;

        public override void OnInspectorGUI()
        {
            if (!Target.IsApplicable)
            {
                EditorGUILayout.HelpBox("Persistent ID is not applicable in prefab mode.", MessageType.Info);
                return;
            }

            base.OnInspectorGUI();

            if (Target.IsValid)
                return;

            if (string.IsNullOrEmpty(Target.Id))
            {
                EditorGUILayout.HelpBox("No ID is specified.", MessageType.Error);
                if (GUILayout.Button("Generate"))
                    Target.GenerateNewId();
                return;
            }

            if (!Target.enabled)
            {
                EditorGUILayout.HelpBox("The component is disabled and cannot be verified.", MessageType.Warning);
                return;
            }

            EditorGUILayout.HelpBox("Duplicate persistent ID detected.", MessageType.Error);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh"))
            {
                Target.enabled = false;
                Target.enabled = true;
            }
            if (GUILayout.Button("Generate"))
                Target.GenerateNewId();
            GUILayout.EndHorizontal();
        }
    }
}