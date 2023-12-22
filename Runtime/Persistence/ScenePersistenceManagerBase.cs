using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            _persistableObjects.Clear();
            var scene = gameObject.scene;
            var rootObjects = scene.GetRootGameObjects();
            foreach (var obj in rootObjects)
            {
                var components = obj.GetComponentsInChildren<IPersistentState<TState>>(IncludeInactiveObjects);
                _persistableObjects.AddRange(components);
            }
        }

        protected virtual void Awake()
        {
            _persistenceService = GetPersistenceService();
            _persistenceService.SetActiveScenePersistenceManager(this);

            RescanSceneObjects();
        }

        protected virtual void Start()
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

        protected virtual void OnDestroy()
        {
            _persistenceService.ClearActiveScenePersistenceManager(this);
        }
    }
}
