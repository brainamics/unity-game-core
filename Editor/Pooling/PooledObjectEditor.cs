using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PooledObject))]
public class PooledObjectEditor : Editor
{
    private PooledObject Target => (PooledObject)target;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Pool");
        if (Target.Pool == null)
        {
            GUILayout.Label("(Unset)");
        }
        else
        {
            if (GUILayout.Button(Target.Pool.ToString()))
                Selection.activeObject = GameObjectUtils.GetGameObject(Target.Pool);
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("In Pool");
        GUILayout.Label(Target.InPool ? "Yes" : "No");
        GUILayout.EndHorizontal();
    }
}
