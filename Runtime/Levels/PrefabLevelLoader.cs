using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    [ExecuteAlways]
    public class PrefabLevelLoader : MonoBehaviour
    {
        [SerializeField]
        private Transform _host;

        [Header("Events")]
        public UnityEvent OnLevelChanged;
        public UnityEvent OnLevelLoaded;
        public UnityEvent OnLevelUnloaded;

        public GameObject LevelPrefab { get; private set; }

        public GameObject LoadedLevel { get; private set; }

        public void LoadLevel(GameObject levelPrefab)
        {
            if (levelPrefab == LevelPrefab)
                return;

            UnloadLevel();
            LoadedLevel = Instantiate(LevelPrefab, _host);
            OnLevelLoaded.Invoke();
        }

        public void UnloadLevel()
        {
            if (LoadedLevel == null)
                return;

            Destroy(LoadedLevel);
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
    }
}
