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

        public Transform Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public GameObject Prefab
        {
            get => _prefab;
            set => _prefab = value;
        }

        public override GameObject Create()
        {
            return Instantiate(_prefab, _parent);
        }
    }
}