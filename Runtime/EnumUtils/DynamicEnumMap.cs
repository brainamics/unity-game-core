using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brainamics.Core
{
    public class DynamicEnumMap<TValue> : IReadOnlyDictionary<string, TValue>
    {
        private readonly StringEnum _stringEnum;
        private readonly TValue[] _map;

        public DynamicEnumMap(StringEnum stringEnum)
        {
            _stringEnum = stringEnum;
            _map = new TValue[stringEnum.Count];
        }

        public int Count => _map.Length;

        public IEnumerable<string> Keys => _stringEnum.Names;

        public IEnumerable<TValue> Values => Keys.Select(k => this[k]);

        public TValue this[string index]
        {
            get => _map[GetArrayIndex(index)];
            set => _map[GetArrayIndex(index)] = value;
        }

        public void Reset(TValue @default = default)
        {
            for (var i = 0; i < _map.Length; i++)
                _map[i] = @default;
        }

        public bool ContainsKey(string key)
        {
            return true;
        }

        public bool TryGetValue(string key, out TValue value)
        {
            value = this[key];
            return true;
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            foreach (var key in Keys)
                yield return new KeyValuePair<string, TValue>(key, this[key]);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int GetArrayIndex(string value)
        {
            return GetIntValue(value);
        }

        private int GetIntValue(string value)
        {
            return _stringEnum.Parse(value);
        }
    }
}
