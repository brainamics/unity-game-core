using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Brainamics.Core
{
    public static class GameObjectMenu
    {
        [UnityEditor.MenuItem(EditorConsts.GameObjectMenu + "Animation Group", false, 10)]
        public static void CreateAnimationGroup()
        {
            var go = new GameObject("Feedback");
            go.AddComponent<AnimationGroup>();
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeGameObject = go;
        }
    }
}