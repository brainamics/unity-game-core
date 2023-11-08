using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class StringEnum
    {
        private readonly Dictionary<string, int> _nameValueMap = new();
        private readonly List<string> _valueNameMap = new();

        public int Count => _valueNameMap.Count;

        public IEnumerable<string> Names => _valueNameMap;

        public IEnumerable<int> Values => _nameValueMap.Values;

        public int Parse(string key)
        {
            if (_nameValueMap.TryGetValue(key, out var val))
                return val;
            return DefineNew(key);
        }

        public string GetName(int value)
        {
            if (value < 0 || value >= _valueNameMap.Count)
                throw new System.ArgumentOutOfRangeException(nameof(value));

            return _valueNameMap[value];
        }

        private int DefineNew(string name)
        {
            var val = _valueNameMap.Count;
            _nameValueMap.Add(name, val);
            _valueNameMap.Add(name);
            return val;
        }
    }
}
