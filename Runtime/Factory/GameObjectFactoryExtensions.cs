using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class GameObjectFactoryExtensions
    {
        public static T Create<T>(this IGameObjectFactory factory)
        {
            var gameObject = factory.Create();
            if (!gameObject.TryGetComponent<T>(out var component))
                throw new System.InvalidOperationException($"Could not find component '{typeof(T)}' on the created object.");
            return component;
        }
    }
}