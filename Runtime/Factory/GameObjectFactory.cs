using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class GameObjectFactory : GameObjectFactoryBase
    {
        [SerializeField]
        private Transform _parent;

        [SerializeField]
        private GameObject _prefab;

        public override GameObject Create()
        {
            return Instantiate(_prefab, _parent);
        }
    }
}