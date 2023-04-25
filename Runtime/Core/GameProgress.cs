using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class GameProgress<T> : IProgress<T>
    {
        public event Action<T> OnProgress;

        public void Report(T value)
        {
            OnProgress?.Invoke(value);
        }
    }
}
