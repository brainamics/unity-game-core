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

        /// <summary>
        /// Gets whether or not the persistence service is busy either loading or saving the game.
        /// </summary>
        bool IsSavingOrLoading { get; }

        void SetActiveScenePersistenceManager(ScenePersistenceManagerBase<TState> persistenceManager);

        void ClearActiveScenePersistenceManager(ScenePersistenceManagerBase<TState> persistenceManager);

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
