using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

namespace Brainamics.Core
{
    public abstract class LevelEditorWindowBase<TLevel> : EditorWindow
        where TLevel : Behaviour
    {
        private bool _selectionFoldout = true;
        private bool _viewFoldout = true;

        public virtual string EditorSceneName => "Editor Scene";

        public virtual string EditorScenePath => $"Assets/_Game/Scenes/{EditorSceneName}.unity";

        public virtual string LevelsAssetPath => $"Assets/_Game/Config/Levels";

        protected abstract PrefabLevelLoader LevelHost { get; }

        protected abstract GameObject LevelBasePrefab { get; }

        private bool HasUnsavedChanges => PrefabUtility.HasPrefabInstanceAnyOverrides(LevelHost.LoadedLevel, false);

        [MenuItem("Tools/Level Editor")]
        public static void OpenWindow()
        {
            var window = GetWindow<LevelEditorWindowBase<TLevel>>("Level Editor");
            window.Show();
        }

        protected virtual void OnEnable()
        {
            Selection.selectionChanged += Repaint;
        }

        protected virtual void OnDisable()
        {
            Selection.selectionChanged -= Repaint;
        }

        protected virtual void OnGUI()
        {
            var margin = 10f;
            var marginRect = new Rect(margin, margin, position.width - 2 * margin, position.height - 2 * margin);
            GUILayout.BeginArea(marginRect);

            if (!IsOnEditorScene())
            {
                GUILayout.Box("Not on the editor scene.");
                if (!string.IsNullOrEmpty(EditorSceneName) && GUILayout.Button("Open Editor"))
                    EditorSceneManager.OpenScene(EditorScenePath);
                GUILayout.EndArea();
                return;
            }

            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));
            if (GUILayout.Button("Create Level"))
            {
                CreateLevel();
            }
            if (GUILayout.Button("Open Level"))
            {
                OpenLevel();
            }
            if (LevelHost.IsLevelLoaded && GUILayout.Button("Unload Level"))
            {
                UnloadLevel();
            }
            EditorGUILayout.EndHorizontal();

            RenderLevelTools();

            GUILayout.EndArea();
        }

        protected abstract bool IsOnEditorScene();

        protected abstract void InitializeLevelPrefab(GameObject root);

        protected virtual void CreateLevel()
        {
            var outputPath = EditorUtility.SaveFilePanelInProject("Save Level Prefab", null, "prefab", null, LevelsAssetPath);
            if (string.IsNullOrEmpty(outputPath))
                return;
            var root = (GameObject)PrefabUtility.InstantiatePrefab(LevelBasePrefab);
            InitializeLevelPrefab(root);
            var newLevelPrefab = PrefabUtility.SaveAsPrefabAssetAndConnect(root, outputPath, InteractionMode.UserAction);
            DestroyImmediate(root);

            if (EditorUtility.DisplayDialog("New Level", "Open the new level?", "Open Level", "No"))
                OpenLevel(newLevelPrefab);
        }

        protected virtual void OpenLevel()
        {
            var path = EditorUtility.OpenFilePanel("Open Level Prefab", LevelsAssetPath, "prefab");
            if (string.IsNullOrEmpty(path))
                return;
            path = EditorUtils.GetRelativeAssetPath(path);
            var prefab = AssetDatabase.LoadAssetAtPath<TLevel>(path);
            OpenLevel(prefab.gameObject);
        }

        protected virtual void OpenLevel(GameObject prefab)
        {
            UnloadLevel();
            if (LevelHost.IsLevelLoaded)
                return;
            LevelHost.LoadLevel(prefab);
        }

        protected virtual void UnloadLevel()
        {
            var host = LevelHost;
            if (!host.IsLevelLoaded)
                return;
            if (HasUnsavedChanges &&
                !EditorUtility.DisplayDialog("Unload level", "Are you sure you wanna lose the changes made on the prefab?", "Yes", "No"))
                return;
            LevelHost.UnloadLevel();
        }

        protected virtual void SaveLevel()
        {
            var instance = LevelHost.LoadedLevel;
            PrefabUtility.ApplyPrefabInstance(instance, InteractionMode.UserAction);
        }

        protected virtual void RenderLevelEditor()
        {
        }

        private void RenderLevelTools()
        {
            var host = LevelHost;
            if (!host.IsLevelLoaded)
                return;
            var level = host.LoadedLevel;

            EditorGUILayout.Space();
            GUILayout.Label($"Editing: {level.name}", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth(300));
            if (GUILayout.Button("Save Level" + (HasUnsavedChanges ? '*' : null)))
                SaveLevel();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            RenderLevelEditor();
        }
    }
}
