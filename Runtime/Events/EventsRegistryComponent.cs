using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class EventsRegistryComponent : MonoBehaviour, IEventsRegistry
    {
        private readonly EventsRegistry _registry = new();

        public void Invoke<T>(T eventArgs)
            => _registry.Invoke(eventArgs);

        public void Register<T>(Action<T> handler)
            => _registry.Register(handler);

        public void Unregister<T>(Action<T> handler)
            => _registry.Unregister(handler);

        public void RegisterGlobal(Action<object> handler)
            => _registry.RegisterGlobal(handler);

        public void UnregisterGlobal(Action<object> handler)
            => _registry.UnregisterGlobal(handler);
    }
}
