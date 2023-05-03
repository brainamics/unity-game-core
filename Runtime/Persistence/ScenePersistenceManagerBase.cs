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
        
        [SerializeField]
        private bool _loadOnFirstFrame;

        public IEnumerable<IPersistentState<TState>> PersistableObjects => _persistableObjects;

        protected abstract IPersistenceService<TState> GetPersistenceService();

        private void Awake()
        {
            _persistenceService = GetPersistenceService();
            _persistenceService.SetActiveScenePersistenceManager(this);

            _persistableObjects.Clear();
            var scene = gameObject.scene;
            var rootObjects = scene.GetRootGameObjects();
            foreach (var obj in rootObjects)
            {
                var components = obj.GetComponentsInChildren<IPersistentState<TState>>();
                _persistableObjects.AddRange(components);
            }
        }

        private void Start()
        {
            if (_loadOnFirstFrame)
            {
                _persistenceService.LoadActiveSceneState();
                return;
            }
            this.RunOnNextFrame(_persistenceService.LoadActiveSceneState);
        }
    }
}
