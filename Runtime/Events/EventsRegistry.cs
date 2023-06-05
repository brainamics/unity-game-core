using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class EventsRegistry : IEventsRegistry
    {
        private readonly Dictionary<Type, List<Delegate>> _listeners = new();
        private readonly Action<object> _globalListeners = new();

        public IReadOnlyList<Delegate> GetListeners(Type type)
            => _listeners.TryGetValue(type, out var list) ? list : Array.Empty<Delegate>();

        public void Invoke<T>(T eventArgs)
        {
            var listeners = GetListeners(typeof(T));
            foreach (var listener in listeners)
            {
                listener.DynamicInvoke(eventArgs);
            }
            _globalListeners.Invoke(eventArgs);
        }

        public void RegisterGlobal(Action<object> handler)
        {
            _globalListeners.Add(handler);
        }

        public void UnregisterGlobal(Action<object> handler)
        {
            _globalListeners.Remove(handler);
        }

        public void Register<T>(Action<T> handler)
        {
            var listeners = ObtainListeners(typeof(T));
            listeners.Add(handler);
        }

        public void Unregister<T>(Action<T> handler)
        {
            var listeners = ObtainListeners(typeof(T));
            listeners.Remove(handler);
        }

        private List<Delegate> ObtainListeners(Type type)
        {
            if (_listeners.TryGetValue(type, out var list))
                return list;
            return _listeners[type] = new List<Delegate>();
        }
    }
}
