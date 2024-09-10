using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

namespace Brainamics.Core
{
    public abstract class LevelEditorWindowBase<TLevel> : EditorWindow
        where TLevel : Behaviour
    {
        private Vector2 _scrollPosition;

        protected virtual int FooterExtraHeight => 0;

        public virtual string EditorSceneName => "Editor Scene";

        public virtual string EditorScenePath => $"Assets/_Game/Scenes/{EditorSceneName}.unity";

        public virtual string LevelsAssetPath => $"Assets/_Game/Config/Levels";

        protected abstract PrefabLevelLoader LevelHost { get; }

        protected abstract GameObject LevelBasePrefab { get; }

        protected virtual bool RenderUnloadButton => true;

        private bool HasUnsavedChanges => PrefabUtility.HasPrefabInstanceAnyOverrides(LevelHost.LoadedLevel, false);

        protected virtual void OnEnable()
        {
            Selection.selectionChanged += Repaint;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        protected virtual void OnDisable()
        {
            Selection.selectionChanged -= Repaint;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        protected virtual void OnPlayModeStateChanged(PlayModeStateChange stateChange)
        {
            if (stateChange == PlayModeStateChange.ExitingEditMode)
            {
                SaveAndPlayLevel();
            }
        }

        protected virtual void OnGUI()
        {
            if (!IsOnEditorScene())
            {
                RenderNotOnEditorScene();
                return;
            }

            RenderLevelButtons();
            RenderLevelTools();
        }

        private void RenderNotOnEditorScene()
        {
            GUILayout.Box("Not on the editor scene.");
            if (!string.IsNullOrEmpty(EditorSceneName) && GUILayout.Button("Open Editor"))
            {
                EditorSceneManager.OpenScene(EditorScenePath);
            }
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
            {
                OpenLevel(newLevelPrefab);
            }
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
            titleContent = new GUIContent($"Level Editor ({prefab.name})");
        }

        protected virtual void UnloadLevel()
        {
            var host = LevelHost;
            if (!host.IsLevelLoaded)
                return;

            if (HasUnsavedChanges && !EditorUtility.DisplayDialog("Unload level", "Are you sure you wanna lose the changes made on the prefab?", "Yes", "No"))
                return;

            LevelHost.UnloadLevel();
            titleContent = new GUIContent("Level Editor");
        }

        protected virtual void SaveLevel()
        {
            var instance = LevelHost.LoadedLevel;
            PrefabUtility.ApplyPrefabInstance(instance, InteractionMode.UserAction);
            PrefabUtility.RevertPrefabInstance(instance, InteractionMode.UserAction);
            EditorSceneManager.SaveOpenScenes();
        }

        protected virtual void SaveAndPlayLevel()
        {
            Debug.LogWarning("This button is not implemented yet :P");
        }

        protected virtual void RenderLevelEditor()
        {
        }

        protected virtual void RenderLevelEditorFooter()
        {
        }

        private void RenderLevelTools()
        {
            var host = LevelHost;
            if (!host.IsLevelLoaded)
                return;

            RenderConfigButtons();
            EditorGUILayout.Space(3);

            RenderScrollableLevelEditor();
            GUILayout.FlexibleSpace();

            RenderFooter();
        }

        private void RenderLevelButtons()
        {
            EditorGUILayout.BeginHorizontal();
            var buttonWidth = (position.width - 10) / 3;

            if (GUILayout.Button(new GUIContent(" New Level", EditorGUIUtility.IconContent("d_CreateAddNew").image), GUILayout.Width(buttonWidth)))
            {
                CreateLevel();
            }

            if (GUILayout.Button(new GUIContent(" Open Level", EditorGUIUtility.IconContent("Folder Icon").image), GUILayout.Width(buttonWidth), GUILayout.Height(20)))
            {
                OpenLevel();
            }

            if (RenderUnloadButton && LevelHost.IsLevelLoaded && GUILayout.Button(new GUIContent(" Unload Level"), GUILayout.Width(buttonWidth), GUILayout.Height(20)))
            {
                UnloadLevel();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void RenderConfigButtons()
        {
            EditorGUILayout.BeginHorizontal();
            float buttonWidth = (position.width - 10) / 2;

            if (GUILayout.Button(new GUIContent(" Level Config", EditorGUIUtility.IconContent("_Popup").image), GUILayout.Width(buttonWidth)))
            {
                Selection.activeObject = LevelHost.LevelPrefab;
                EditorGUIUtility.PingObject(LevelHost.LevelPrefab);
            }

            if (GUILayout.Button(new GUIContent(" Global Config", EditorGUIUtility.IconContent("_Popup").image), GUILayout.Width(buttonWidth)))
            {
                SelectGlobalConfig();
            }

            EditorGUILayout.EndHorizontal();
        }

        protected virtual void SelectGlobalConfig()
        {
            // var levelsConfig = DesignerManager.Instance.LevelService.Config;
            // Selection.activeObject = levelsConfig;
            // EditorGUIUtility.PingObject(levelsConfig);
            Debug.LogWarning("This button is not implemented yet :P");
        }

        private void RenderScrollableLevelEditor()
        {
            EditorGUILayout.BeginVertical();
            float scrollViewHeight = position.height - 85 - FooterExtraHeight;
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(scrollViewHeight));

            RenderLevelEditor();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void RenderFooter()
        {
            RenderLevelEditorFooter();

            if (Application.isPlaying)
                return;

            EditorGUILayout.BeginHorizontal();
            float buttonWidth = (position.width - 10) / 2;

            if (GUILayout.Button(new GUIContent(" Save Level" + (HasUnsavedChanges ? " *" : ""), EditorGUIUtility.IconContent("SaveAs").image), GUILayout.Width(buttonWidth), GUILayout.Height(30)))
            {
                SaveLevel();
            }

            if (HasUnsavedChanges)
            {
                GUI.enabled = false;
            }

            if (GUILayout.Button(new GUIContent(" Play Level", EditorGUIUtility.IconContent("PlayButton").image), GUILayout.Width(buttonWidth), GUILayout.Height(30)))
            {
                SaveAndPlayLevel();
            }

            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
        }
    }
}
