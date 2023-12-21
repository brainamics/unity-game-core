using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public abstract class GameObjectFactoryBase : MonoBehaviour, IGameObjectFactory
    {
        public abstract GameObject Create();

        public TComponent Create<TComponent>()
        {
            var obj = Create();
            return obj.GetComponent<TComponent>();
        }
    }
}