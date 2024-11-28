#if UNITASK
using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Brainamics.Core
{
    public class AsyncCounterFlag
    {
        private int _counter;

        public event System.Action<AsyncCounterFlag> OnValueChanged;

        public bool IsSet => _counter > 0;

        public int Counter
        {
            get => _counter;
            set
            {
                if (_counter == value)
                    return;
                var wasSet = IsSet;
                _counter = value;
                if (wasSet != IsSet)
                    OnValueChanged?.Invoke(this);
            }
        }

        public static UniTask WaitForAll(IEnumerable<AsyncCounterFlag> flags, bool value)
        {
            Func<AsyncCounterFlag, UniTask> taskSelector;
            if (value)
                taskSelector = flag => flag.WaitForAsync(true);
            else
                taskSelector = flag => flag.WaitForAsync(false);
            return UniTask.WhenAll(flags.Select(taskSelector));
        }

        public async UniTask WaitForAsync(bool value)
        {
            while (true)
            {
                if (IsSet == value)
                    return;

                await WaitForNextValue();
            }
        }

        private UniTask WaitForNextValue()
        {
            var taskSource = AutoResetUniTaskCompletionSource.Create();
            OnValueChanged += HandleValueChanged;
            return taskSource.Task;

            void HandleValueChanged(AsyncCounterFlag self)
            {
                OnValueChanged -= HandleValueChanged;
                taskSource.TrySetResult();
            }
        }
    }
}
#endif