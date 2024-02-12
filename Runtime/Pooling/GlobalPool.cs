using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class GlobalPool<T>
        where T : class
    {
        private static GlobalPool<T> _instance;
        private readonly Stack<T> _pool = new();

        public GlobalPool()
        {
            Factory = CreateDefault;
        }

        public static GlobalPool<T> Instance
        {
            get
            {
                _instance ??= new GlobalPool<T>();
                return _instance;
            }
        }

        public Func<T> Factory { get; set; }

        public int Capacity { get; set; } = 50;

        public T Rent()
        {
            if (_pool.TryPop(out var result))
                return result;
            result = Factory();
            return result;
        }

        public void Return(T obj)
        {
            if (_pool.Count >= Capacity)
            {
                Destroy(obj);
                return;
            }

            _pool.Push(obj);
        }

        private T CreateDefault()
        {
            return Activator.CreateInstance<T>();
        }

        private void Destroy(T _)
        {
            // nothing to do, GC will take care of it
        }
    }
}
