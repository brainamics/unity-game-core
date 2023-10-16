using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brainamics.Core
{
    public class EnumMap<TEnum, TValue> : IReadOnlyDictionary<TEnum, TValue>
        where TEnum : struct
    {
        private readonly TValue[] _map;
        private readonly int _lowBound, _upBound;

        public EnumMap()
        {
            var values = Enum.GetValues(typeof(TEnum)).OfType<TEnum>().ToArray();
            _lowBound = values.Select(GetIntValue).Min();
            _upBound = values.Select(GetIntValue).Max();
            _map = new TValue[_upBound - _lowBound + 1];
        }

        public int Count => _map.Length;

        public IEnumerable<TEnum> Keys => Enum.GetValues(typeof(TEnum)).OfType<TEnum>();

        public IEnumerable<TValue> Values => Keys.Select(k => this[k]);

        public TValue this[TEnum index]
        {
            get => _map[GetArrayIndex(index)];
            set => _map[GetArrayIndex(index)] = value;
        }

        private int GetArrayIndex(TEnum value)
        {
            var val = GetIntValue(value);
            return val - _lowBound;
        }

        private static int GetIntValue(TEnum value)
        {
            return (int)(object)value;
        }

        public bool ContainsKey(TEnum key)
        {
            return true;
        }

        public bool TryGetValue(TEnum key, out TValue value)
        {
            value = this[key];
            return true;
        }

        public IEnumerator<KeyValuePair<TEnum, TValue>> GetEnumerator()
        {
            foreach (var key in Keys)
                yield return new KeyValuePair<TEnum, TValue>(key, this[key]);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
