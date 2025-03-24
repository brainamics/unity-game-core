using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Brainamics.Core
{
    [DefaultExecutionOrder(-1000)]
    public class GameObjectPool : MonoBehaviour, IGameObjectPool
    {
        private ObjectPool<GameObject> _pool;

        [Tooltip("Whether or not to add the PooledObject component to the pooled objects.")]
        public bool AddPoolReferenceComponent;

        [SerializeField]
        private int _capacity = 50;

        [SerializeField]
        private GameObject _prefab;

        [SerializeField]
        private Transform _parent;

        public int Capacity
        {
            get => _pool.Capacity;
            set => _pool.Capacity = _capacity = value;
        }

        public GameObject Prefab
        {
            get => _prefab;
            set => _prefab = value;
        }

        public Transform Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public void Reserve(int count)
            => _pool.Reserve(count);

        public void TrimExcess()
            => _pool.TrimExcess();

        public GameObject Rent()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                var obj = CreateNew();
                Undo.RegisterCreatedObjectUndo(obj, "Create pool object");
                return obj;
            }
#endif
            return _pool.Rent();
        }

        public void Return(GameObject obj)
        {
            if (!obj)
            {
                Debug.LogWarning("Prevented returning null (or destroyed) object into the pool.");
                return;
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                Undo.DestroyObjectImmediate(obj);
                return;
            }
#endif
            if (Capacity > 0)
                obj.transform.SetParent(_parent, false);
            _pool.Return(obj);
        }

        protected virtual GameObject CreateNew()
        {
            var obj = Instantiate(_prefab, _parent);

            if (AddPoolReferenceComponent && Application.isPlaying)
            {
                var pooledObj = obj.AddComponent<PooledObject>();
                pooledObj.Pool = this;
            }
            return obj;
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
            foreach (var recyclable in obj.GetComponents<IRecyclable>())
                recyclable.Recycle();
            obj.SetActive(true);
        }

        private void Awake()
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

        private void OnDestroy()
        {
            _pool.Dispose();
        }
    }
}
