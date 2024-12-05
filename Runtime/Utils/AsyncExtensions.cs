using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Brainamics.Core
{
    public static class AsyncExtensions
    {
        public static Task AsTask(this AsyncOperation asyncOperation)
        {
            if (asyncOperation == null)
                return Task.CompletedTask;
            var taskSource = new TaskCompletionSource<bool>();
            asyncOperation.completed += AsyncOperation_completed;
            if (asyncOperation.isDone)
                return Task.CompletedTask;
            return taskSource.Task;

            void AsyncOperation_completed(AsyncOperation operation)
            {
                asyncOperation.completed -= AsyncOperation_completed;
                taskSource.SetResult(true);
            }
        }

        public static IDisposable Bind(this GameplayPipelineBase pipeline, AsyncCounterFlag flag)
        {
            flag.OnValueChanged += OnFlagValueChanged;
            return new CallbackDisposable(() => flag.OnValueChanged -= OnFlagValueChanged);

            void OnFlagValueChanged(AsyncCounterFlag flag)
            {
                if (!flag.IsSet)
                    pipeline.Schedule();
            }
        }

        public static IDisposable Bind(this GameplayPipelineBase pipeline, AsyncFlag flag)
        {
            flag.OnValueChanged += OnFlagValueChanged;
            return new CallbackDisposable(() => flag.OnValueChanged -= OnFlagValueChanged);

            void OnFlagValueChanged(AsyncFlag flag)
            {
                if (!flag.IsSet)
                    pipeline.Schedule();
            }
        }
    }
}