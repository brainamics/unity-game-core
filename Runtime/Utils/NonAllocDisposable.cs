using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public readonly struct NonAllocDisposable : IDisposable
    {
        private readonly Action _callback;

        public NonAllocDisposable(Action callback)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }

        public void Dispose()
        {
            _callback.Invoke();
        }
    }
}
