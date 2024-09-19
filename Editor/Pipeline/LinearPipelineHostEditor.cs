using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Brainamics.Core
{
    [CustomEditor(typeof(LinearPipelineHost))]
    public class LinearPipelineHostEditor : EditorBase<LinearPipelineHost>
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            RenderRuntimeTools();
        }

        private void RenderRuntimeTools()
        {
            if (!Application.isPlaying)
                return;

            GUILayout.Space(10);
            GUILayout.Label("Runtime Tools", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Active Feedbacks:");
            GUILayout.Label(Target.Pipeline.FeedbacksCount.ToString());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Enqueued Actions:");
            GUILayout.Label(Target.Pipeline.InQueueActionsCount.ToString());
            EditorGUILayout.EndHorizontal();
        }
    }
}
