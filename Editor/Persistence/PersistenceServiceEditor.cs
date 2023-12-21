using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Brainamics.Core
{
    //[CustomEditor(typeof(PersistenceService))]
    //public class PersistenceServiceEditor : Editor
    //{
    //    private PersistenceService Target => target as PersistenceService;

    //    public override void OnInspectorGUI()
    //    {
    //        base.OnInspectorGUI();

    //        if (!Application.isPlaying)
    //            return;

    //        GUILayout.Label("Runtime Tools", EditorStyles.boldLabel);

    //        GUILayout.BeginHorizontal();
    //        if (GUILayout.Button("New"))
    //            Target.NewGameInBackground();
    //        if (GUILayout.Button("Save"))
    //            Target.SaveGameInBackground();
    //        if (GUILayout.Button("Load"))
    //            Target.LoadGameInBackground(null);
    //        GUILayout.EndHorizontal();
    //    }
    //}
}