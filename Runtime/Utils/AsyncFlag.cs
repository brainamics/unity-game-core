#if UNITASK
using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Brainamics.Core
{
    public sealed class AsyncFlag
    {
        private bool _value;

        public event System.Action<AsyncFlag> OnValueChanged;

        public bool IsSet
        {
            get => _value;
            set
            {
                if (_value == value)
                    return;
                _value = value;
                OnValueChanged?.Invoke(this);
            }
        }

        public static UniTask WaitForAll(IEnumerable<AsyncFlag> flags, bool value)
        {
            Func<AsyncFlag, UniTask> taskSelector;
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

            void HandleValueChanged(AsyncFlag self)
            {
                OnValueChanged -= HandleValueChanged;
                taskSource.TrySetResult();
            }
        }
    }
}
#endif