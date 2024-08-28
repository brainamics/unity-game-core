using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class ObjectPool<T> : IDisposable
    {
        private readonly Queue<T> _pool = new();
        private int _capacity = 50;

        public ObjectPool()
        {
            DestroyHandler = DefaultHandleDestroy;
        }

        public ObjectPool(int capacity) : this()
        {
            Capacity = capacity;
        }

        public static readonly ObjectPool<T> SharedInstance = new();

        public int Capacity
        {
            get => _capacity;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _capacity = value;
            }
        }

        public Action<T> SleepHandler { get; set; } = _ => { };

        public Action<T> WakeHandler { get; set; } = _ => { };

        public Action<T> DestroyHandler { get; set; }

        public Func<T> Factory { get; set; } = () => Activator.CreateInstance<T>();

        public void TrimExcess()
        {
            while (_pool.Count > Capacity)
            {
                var o = _pool.Dequeue();
                DestroyHandler(o);
            }
        }

        public void Reserve(int count)
        {
            count = Math.Min(count, _capacity);
            while (_pool.Count < count)
            {
                var o = Factory();
                Return(o);
            }
        }

        public T Rent()
        {
            T obj = default;
            while (true)
            {
                if (!_pool.TryDequeue(out obj))
                {
                    obj = Factory();
                    break;
                }
                if (obj is GameObject o && o == null)
                    continue;
                break;
            }
            if (obj == null)
                return obj;
            WakeHandler.Invoke(obj);
            return obj;
        }

        public void Return(T obj)
        {
            if (_pool.Count >= Capacity || Capacity == 0)
            {
                DestroyHandler.Invoke(obj);
                return;
            }

            SleepHandler.Invoke(obj);
            _pool.Enqueue(obj);
        }

        public void Dispose()
        {
            while (_pool.TryDequeue(out var obj))
                DestroyHandler.Invoke(obj);
        }

        private void DefaultHandleDestroy(T obj)
        {
            if (obj is IDisposable disposable)
                disposable.Dispose();
        }
    }
}
