using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public sealed class ValueWrapper<T>
    {
        private readonly IEqualityComparer<T> _comparer;
        private T _value;

        public ValueWrapper(T value = default, IEqualityComparer<T> comparer = null)
        {
            _value = value;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (_comparer.Equals(_value, value))
                    return;

                _value = value;
            }
        }

        public ref T GetRef()
        {
            return ref _value;
        }
    }
}