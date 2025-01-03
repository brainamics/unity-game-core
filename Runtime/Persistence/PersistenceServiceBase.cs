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
    // [CreateAssetMenu(menuName = "Game/Services/Persistence/Persistence Service")]
    public abstract class PersistenceServiceBase<TState> : PersistenceServiceBase<TState, TState> { }

    public abstract class PersistenceServiceBase<TState, TMinimalState> : ScriptableObject, IPersistenceService<TState>
        where TState : TMinimalState
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
        private bool _autoLoadPersistenceManager;

        [Tooltip("Whether to ignore errors when loading scene objects.")]
        public bool SceneObjectsLoadResilience = false;

        public UnityEvent OnGameSaved => _onGameSaved;

        public UnityEvent OnGameLoaded => _onGameLoaded;

        public TState LastState => _state;

        public bool IsSavingOrLoading => _operating;

        public abstract DateTime? LastSaveTime { get; }

        public virtual int LatestIntVersion => 0;

        public IEnumerable<ScriptableObject> ScriptableObjects => _scriptableObjects;

        protected virtual bool ReloadIfStateUnavailableDuringSceneAwake => false;

        public virtual int? GetIntVersion(TMinimalState state) => null;

        public virtual bool SetIntVersion(TMinimalState state, int version) => false;

        public virtual void SetActiveScenePersistenceManager(ScenePersistenceManagerBase<TState> manager)
        {
            if (_state == null)
            {
                LoadGameInBackground(null);
                return;
            }
            _activeScenePersistenceManager = manager;
        }

        public virtual void ClearActiveScenePersistenceManager(ScenePersistenceManagerBase<TState> manager)
        {
            if (_activeScenePersistenceManager == manager)
                _activeScenePersistenceManager = null;
        }

        public async Task NewGameAsync(IProgress<float> progress, TState state)
        {
            UpdateStateBeforeSave(state);
            await _persistenceProvider.SaveStateAsync(state);
            await LoadGameState(state, progress);
        }

        public async Task NewGameAsync(IProgress<float> progress)
        {
            _state = NewState();
            Log("creating a new game.");
            await NewGameAsync(progress, _state);
        }

        public void NewGameInBackground()
        {
            _ = NewGameAsync(null);
        }

        public Task SaveGameAsync()
        {
            return StartSaveLoadOperationAsync(async () =>
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
            return StartSaveLoadOperationAsync(async () =>
            {
                _state = await MigrateAndLoadStateFromProviderAsync();
                Log($"loading game: {_state}");
                await LoadGameState(_state, progress);
                if (_activeScenePersistenceManager && _autoLoadPersistenceManager)
                    LoadSceneState(_activeScenePersistenceManager.PersistableObjects);
            });
        }

        public void LoadGameInBackground(IProgress<float> progress)
        {
            LoadGameAsync(progress).RunInBackground();
        }
        
        public void LoadActiveSceneState()
        {
            if (_state == null && ReloadIfStateUnavailableDuringSceneAwake)
            {
                LoadGameInBackground(null);
                return;
            }
            StartSaveLoadOperation(() =>
            {
                if (_activeScenePersistenceManager == null)
                    throw new InvalidOperationException("No active scene persistence manager is set.");
                LoadSceneState(_activeScenePersistenceManager.PersistableObjects);
            });
        }

        protected abstract TState NewState();

        protected abstract Task LoadGameState(TState state, IProgress<float> progress);

        protected abstract void UpdateStateBeforeSave(TState state);

        protected virtual void Log(object o)
        {
            Debug.Log($"[Persistence] {o}");
        }

        protected virtual void LogWarning(object o)
        {
            Debug.LogWarning($"[Persistence] {o}");
        }

        protected virtual void LogError(object o)
        {
            Debug.LogError($"[Persistence] {o}");
        }

        protected virtual Task<TState> MigrateAsync(int fromVersion, int toVersion, IPersistenceProvider<TState> persistenceProvider)
        {
            throw new NotSupportedException($"Migrating save file from version {fromVersion} to {toVersion} is not supported.");
        }
        
        protected virtual bool CanSave(TState state)
        {
            return true;
        }

        private async Task<TState> MigrateAndLoadStateFromProviderAsync()
        {
            // check if versioning is supported
            var latestVersion = LatestIntVersion;
            if (latestVersion < 1)
                return await _persistenceProvider.LoadStateAsync();

            // load the minimal state first
            var minimalState = await _persistenceProvider.LoadStateAsync<TMinimalState>();
            var saveVersion = minimalState == null ? null : GetIntVersion(minimalState);
            if (saveVersion == null)
                return await _persistenceProvider.LoadStateAsync();

            // test if migration is necessary
            if (saveVersion.Value < latestVersion)
                return await MigrateAsync(saveVersion.Value, latestVersion, _persistenceProvider);

            return await _persistenceProvider.LoadStateAsync();
        }

        private void LoadSceneState(IEnumerable<IPersistentState<TState>> statefulObjects)
        {
            _state ??= NewState();
            foreach (var obj in EnumeratePersistableObjects(statefulObjects))
            {
                try
                {
                    obj.LoadState(_state);
                }
                catch (Exception exception)
                {
                    if (!SceneObjectsLoadResilience)
                        throw;
                    Debug.LogError($"Error while loading scene object state '{obj}'; reporting and ignoring.");
                    Debug.LogError(exception);
                }
            }
        }

        private bool SaveGameState()
        {
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.name == LoadingScene)
            {
                LogWarning("Blocked save of the loading scene.");
                return false;
            }

            TState state;
            IEnumerable<IPersistentState<TState>> persistableObjects;
            if (_activeScenePersistenceManager == null)
            {
                LogWarning("Updating the previous state; because the active scene persistence manager is missing.");
                state = _state ?? NewState();
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
            if (!CanSave(state))
            {
                LogWarning("Blocked save of the game state because of CanSave returning false.");
                return false;
            }
            _state = state;
            Log($"saving game: {state}");
            return true;
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
