using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brainamics.Core
{
    public class IntArrayMap<TValue> : IReadOnlyDictionary<int, TValue>
    {
        private readonly List<TValue> _map = new();

        public int Count => _map.Count;

        public IEnumerable<int> Keys => Enumerable.Range(0, _map.Count);

        public IEnumerable<TValue> Values => Keys.Select(k => this[k]);

        public TValue this[int index]
        {
            get => index < _map.Count ? _map[index] : default;
            set
            {
                while (index >= _map.Count)
                    _map.Add(default);
                _map[index] = value;
            }
        }

        public void Reset(TValue @default = default)
        {
            for (var i = 0; i < _map.Count; i++)
                _map[i] = @default;
        }

        public bool ContainsKey(int key)
        {
            return key >= 0 && key < _map.Count;
        }

        public bool TryGetValue(int key, out TValue value)
        {
            if (!ContainsKey(key))
            {
                value = default;
                return false;
            }
            value = this[key];
            return true;
        }

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            foreach (var key in Keys)
                yield return new KeyValuePair<int, TValue>(key, this[key]);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
