using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class PooledObject : MonoBehaviour, IDestroyable, IRecyclable
    {
        private bool _inPool;
        private IGameObjectPool _pool;

        public bool InPool => _inPool;

        public IGameObjectPool Pool
        {
            get => _pool;
            set
            {
                if (_pool == value)
                    return;
                if (_pool != null)
                    throw new System.InvalidOperationException("The pool reference is already set.");
                _pool = value;
            }
        }

        public void Destroy()
        {
            if (_inPool)
                return;
            _inPool = true;
            _pool.Return(gameObject);
        }

        public void Recycle()
        {
            _inPool = false;
        }
    }
}
