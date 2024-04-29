using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Brainamics.Core
{
    [DefaultExecutionOrder(100)]
    public abstract class ScenePersistenceManagerBase<TState> : MonoBehaviour
    {
        private readonly List<IPersistentState<TState>> _persistableObjects = new();
        private IPersistenceService<TState> _persistenceService;

        public bool LoadOnStart = true;
        public bool LoadOnFirstFrame = true;
        public bool IncludeInactiveObjects = false;
        public bool EnumeratePersistablesDynamically = false;
        public bool SaveGameOnStart = false;
        public bool SaveGameOnlyIfMissing = false;
        public bool OrderPersistablesByExecutionOrder = false;

        public float SaveGameDelay;

        public IEnumerable<IPersistentState<TState>> PersistableObjects
        {
            get
            {
                if (EnumeratePersistablesDynamically)
                    RescanSceneObjects();
                return _persistableObjects;
            }
        }

        protected abstract IPersistenceService<TState> GetPersistenceService();

        public void RescanSceneObjects()
        {
            var persistableObjects = Enumerable.Empty<IPersistentState<TState>>();
            _persistableObjects.Clear();
            var scene = gameObject.scene;
            var rootObjects = scene.GetRootGameObjects();
            foreach (var obj in rootObjects)
            {
                var components = obj.GetComponentsInChildren<IPersistentState<TState>>(IncludeInactiveObjects);
                persistableObjects = persistableObjects.Concat(components);
            }
            if (OrderPersistablesByExecutionOrder)
                persistableObjects = persistableObjects
                        .OrderBy(o =>
                        {
                            var t = o.GetType();
                            var order = t.GetCustomAttribute<DefaultExecutionOrder>();
                            return order?.order ?? 0;
                        });
            _persistableObjects.AddRange(persistableObjects);
        }

        protected virtual void Awake()
        {
            _persistenceService = GetPersistenceService();
            _persistenceService.SetActiveScenePersistenceManager(this);

            RescanSceneObjects();
        }

        protected virtual void Start()
        {
            PerformLoadOnStart();
            PerformSaveOnStart();
        }

        protected virtual void OnDestroy()
        {
            _persistenceService.ClearActiveScenePersistenceManager(this);
        }

        protected virtual void PerformLoadOnStart()
        {
            if (!LoadOnStart)
                return;
            if (LoadOnFirstFrame)
            {
                _persistenceService.LoadActiveSceneState();
                return;
            }
            this.RunOnNextFrame(_persistenceService.LoadActiveSceneState);
        }

        protected virtual void PerformSaveOnStart()
        {
            if (SaveGameOnStart)
                this.RunAfterDelay(SaveGameDelay, SaveGameConditionally);
        }

        protected virtual void SaveGameConditionally()
        {
            if (!SaveGameOnStart)
                return;
            if (SaveGameOnlyIfMissing && IsSaved(_persistenceService.LastState))
                return;
            _persistenceService.SaveGameInBackground();
        }

        protected virtual bool IsSaved(TState state)
        {
            return state != null;
        }
    }
}
