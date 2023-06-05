using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class GameObjectPool : MonoBehaviour, IGameObjectPool
    {
        private readonly ObjectPool<GameObject> _pool;

        [SerializeField]
        private int _capacity = 50;

        [SerializeField]
        private GameObject _prefab;

        public GameObjectPool()
        {
            _pool = new ObjectPool<GameObject>
            {
                Factory = CreateNew,
                DestroyHandler = DestroyObject,
                SleepHandler = PutToSleep,
                WakeHandler = WakeFromSleep,
                Capacity = _capacity,
            };
        }

        public int Capacity
        {
            get => _pool.Capacity;
            set => _pool.Capacity = value;
        }

        public void Reserve(int count)
            => _pool.Reserve(count);

        public void TrimExcess()
            => _pool.TrimExcess();

        public GameObject Rent()
            => _pool.Rent();

        public void Return(GameObject obj)
            => _pool.Return(obj);

        protected virtual GameObject CreateNew()
        {
            return Instantiate(_prefab);
        }

        protected virtual void DestroyObject(GameObject obj)
        {
            Destroy(obj);
        }

        protected virtual void PutToSleep(GameObject obj)
        {
            obj.SetActive(false);
        }

        protected virtual void WakeFromSleep(GameObject obj)
        {
            obj.SetActive(true);
        }

        private void OnDestroy()
        {
            _pool.Dispose();
        }
    }
}
