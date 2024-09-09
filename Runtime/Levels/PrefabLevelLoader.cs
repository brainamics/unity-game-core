using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Brainamics.Core
{
    [ExecuteAlways]
    [DefaultExecutionOrder(-50)]
    public class PrefabLevelLoader : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        private GameObject _levelPrefab;
    
        [SerializeField]
        private Transform _host;

        public bool AllowLevelReloading = true;

        [Header("Events")]
        public UnityEvent OnLevelChanged;
        public UnityEvent OnLevelLoaded;
        public UnityEvent OnLevelUnloaded;

        public GameObject LevelPrefab
        {
            get => _levelPrefab;
            protected set => _levelPrefab = value;
        }

        public GameObject LoadedLevel { get; private set; }

        public bool IsLevelLoaded => LoadedLevel != null;

        public void LoadLevel(GameObject levelPrefab)
            => LoadLevel(levelPrefab, AllowLevelReloading);

        public void LoadLevel(GameObject levelPrefab, bool allowLevelReloading)
        {
            if (levelPrefab == LevelPrefab && !allowLevelReloading)
                return;
            LoadLevelInternal(levelPrefab, allowLevelReloading);
        }

        public virtual void UnloadLevel()
        {
            if (LoadedLevel == null)
                return;

#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                DestroyImmediate(LoadedLevel);
            }
            else
            {
                EditorSceneManager.MarkSceneDirty(LoadedLevel.scene);
                DestroyImmediate(LoadedLevel);
            }
#else
            DestroyImmediate(LoadedLevel);
#endif
            LoadedLevel = null;
            LevelPrefab = null;
            OnLevelUnloaded.Invoke();
        }

        protected virtual void Awake()
        {
        }

        protected virtual void OnEnable()
        {
            DetectActiveLevel();
        }

        protected virtual void OnDisable()
        {
            if (Application.isPlaying)
                UnloadLevel();
        }

        protected virtual void DetectActiveLevel()
        {
            if (_host.childCount == 0)
            {
                LoadedLevel = null;
                return;
            }

            if (_host.childCount > 1)
                throw new System.InvalidOperationException($"The level loader cannot detect the current level if the host transform has more than a single child ({_host.childCount}).");

            LoadedLevel = _host.GetChild(0).gameObject;
        }

        protected virtual void LoadLevelInternal(GameObject levelPrefab, bool allowLevelReloading)
        {
            UnloadLevel();
            LevelPrefab = levelPrefab;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                LoadedLevel = (GameObject)PrefabUtility.InstantiatePrefab(LevelPrefab, _host);
                EditorSceneManager.MarkSceneDirty(LoadedLevel.scene);
            }
            else
#endif
            {
                LoadedLevel = Instantiate(LevelPrefab, _host);
            }
            OnLevelLoaded.Invoke();
        }
    }
}
