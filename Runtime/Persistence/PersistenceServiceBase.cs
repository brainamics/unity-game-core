using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Brainamics.Core
{
    // [CreateAssetMenu("Game/Services/Persistence/Persistence Service")]
    public abstract class PersistenceServiceBase<TState> : ScriptableObject, IPersistenceService<TState>
    {
        public string LoadingScene;

        protected TState _state;
        protected ScenePersistenceManagerBase<TState> _activeScenePersistenceManager;
        private bool _operating;
        private UnityEvent _onGameSaved = new();
        private UnityEvent _onGameLoaded = new();

        [SerializeField]
        private PersistenceProviderBase<TState> _persistenceProvider;

        [SerializeField]
        private List<ScriptableObject> _scriptableObjects = new();

        [SerializeField]
        private bool _reloadActiveScene;

        [SerializeField]
        private bool _enableLogging;

        public UnityEvent OnGameSaved => _onGameSaved;

        public UnityEvent OnGameLoaded => _onGameLoaded;

        public TState LastState => _state;

        public abstract DateTime? LastSaveTime { get; }

        public IEnumerable<ScriptableObject> ScriptableObjects => _scriptableObjects;
        
        protected virtual bool ReloadIfStateUnavailableDuringSceneAwake => false;

        public virtual void SetActiveScenePersistenceManager(ScenePersistenceManagerBase<TState> manager)
        {
            if (_state == null)
            {
                LoadGameInBackground();
                return;
            }
            _activeScenePersistenceManager = manager;
        }

        public async Task NewGameAsync(IProgress<float> progress)
        {
            _state = NewState();
            Log("creating a new game.");
            await _persistenceProvider.SaveStateAsync(_state);
            await LoadGameState(_state, progress);
        }

        public void NewGameInBackground()
        {
            _ = NewGameAsync(null);
        }

        public Task SaveGameAsync()
        {
            return StartSaveLoadOperation(async () =>
            {
                if (!SaveGameState())
                    return;
                await _persistenceProvider.SaveStateAsync(_state);
                _onGameSaved.Invoke();
            });
        }

        public void SaveGameInBackground()
        {
            SaveGameAsync().RunInBackground();
        }

        public void SaveGame()
        {
            StartSaveLoadOperation(() =>
            {
                if (!SaveGameState())
                    return;
                _persistenceProvider.SaveStateAsync(_state).GetAwaiter().GetResult();
                _onGameSaved.Invoke();
            });
        }

        public Task LoadGameAsync(IProgress<float> progress)
        {
            return StartSaveLoadOperation(async () =>
            {
                _state = await _persistenceProvider.LoadStateAsync();
                return LoadGameState(_state, progress);
            });
        }

        public void LoadGameInBackground()
        {
            LoadGameAsync(null).RunInBackground();
        }

        protected abstract TState NewState();

        protected abstract Task LoadGameState(TState state, IProgress<float> progress);

        protected abstract void UpdateStateBeforeSave(TState state);

        public void LoadActiveSceneState()
        {
            if (_state == null && ReloadIfStateUnavailableDuringSceneAwake)
            {
                LoadGameInBackground();
                return;
            }
            StartSaveLoadOperation(() =>
            {
                if (_activeScenePersistenceManager == null)
                    throw new InvalidOperationException("No active scene persistence manager is set.");
                LoadSceneState(_activeScenePersistenceManager.PersistableObjects);
            });
        }

        private void LoadSceneState(IEnumerable<IPersistentState<TState>> statefulObjects)
        {
            if (_state == null)
                return;
            foreach (var obj in EnumeratePersistableObjects(statefulObjects))
            {
                obj.LoadState(_state);
            }
        }

        private bool SaveGameState()
        {
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.name == LoadingScene)
            {
                Debug.LogWarning("Blocked save of the loading scene.");
                return false;
            }

            TState state;
            IEnumerable<IPersistentState<TState>> persistableObjects;
            if (_activeScenePersistenceManager == null)
            {
                Debug.LogWarning("Updating the previous state; because the active scene persistence manager is missing.");
                state = _state;
                persistableObjects = EnumeratePersistableObjects(Enumerable.Empty<IPersistentState<TState>>());
            }
            else
            {
                state = NewState();
                persistableObjects = EnumeratePersistableObjects(_activeScenePersistenceManager.PersistableObjects);
            }
            UpdateStateBeforeSave(state);
            foreach (var obj in persistableObjects)
            {
                obj.SaveState(state);
            }
            _state = state;
            Log($"saving game: {state}");
            return true;
        }

        private void Log(object o)
        {
            if (_enableLogging)
                Debug.Log($"[Persistence] {o}");
        }

        private void LogWarning(object o)
        {
            Debug.LogWarning($"[Persistence] {o}");
        }

        private void LogError(object o)
        {
            Debug.LogError($"[Persistence] {o}");
        }

        private IEnumerable<IPersistentState<TState>> EnumeratePersistableObjects(IEnumerable<IPersistentState<TState>> statefulObjects)
        {
            return ScriptableObjects.OfType<IPersistentState<TState>>().Concat(statefulObjects);
        }

        private T StartSaveLoadOperation<T>(Func<T> callback, T @default = default)
        {
            if (_operating)
            {
                LogError("Preventing the operation, another save/load operation is in progress.");
                return @default;
            }

            _operating = true;
            try
            {
                return callback();
            }
            finally
            {
                _operating = false;
            }
        }

        private void StartSaveLoadOperation(Action callback)
        {
            StartSaveLoadOperation(() =>
            {
                callback();
                return false;
            });
        }

        private async Task<T> StartSaveLoadOperationAsync<T>(Func<Task<T>> callback, T @default = default)
        {
            if (_operating)
            {
                LogError("Preventing the operation, another save/load operation is in progress.");
                return @default;
            }

            _operating = true;
            try
            {
                return await callback();
            }
            finally
            {
                _operating = false;
            }
        }

        private async Task StartSaveLoadOperationAsync(Func<Task> callback)
        {
            await StartSaveLoadOperationAsync(async () =>
            {
                await callback();
                return false;
            });
        }
    }
}
