using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brainamics.Core
{
    public class ScenePersistenceManager<TState> : MonoBehaviour
    {
        private readonly List<IPersistentState<TState>> _persistableObjects = new();
        private PersistenceServiceBase<TState> _persistenceService;

        public IEnumerable<IPersistentState<TState>> PersistableObjects => _persistableObjects;

        private void Awake()
        {
            var serviceLocator = GetComponent<ServiceReference>().ServiceLocator;
            _persistenceService = serviceLocator.PersistenceService;

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
            this.RunOnNextFrame(() =>
            {
                _persistenceService.LoadActiveSceneState();
            });
        }
    }
}