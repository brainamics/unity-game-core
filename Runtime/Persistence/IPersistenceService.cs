using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public interface IPersistenceService<TState>
    {
        UnityEvent OnGameSaved { get; }

        UnityEvent OnGameLoaded { get; }

        DateTime? LastSaveTime { get; }

        IEnumerable<ScriptableObject> ScriptableObjects { get; }

        TState LastState { get; }

        void SetActiveScenePersistenceManager(ScenePersistenceManagerBase<TState> persistenceManager);

        void LoadActiveSceneState();

        Task LoadGameAsync(IProgress<float> progress);

        void LoadGameInBackground(IProgress<float> progress);

        Task NewGameAsync(IProgress<float> progress);

        void NewGameInBackground();

        void SaveGame();

        Task SaveGameAsync();

        void SaveGameInBackground();
    }
}
