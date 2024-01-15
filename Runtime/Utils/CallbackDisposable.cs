using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class CallbackDisposable : IDisposable
    {
        private readonly Action _callback;
        private bool _disposed;
    
        public CallbackDisposable(Action callback)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
        }
    
        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;
            _callback.Invoke();
        }
    }
}
