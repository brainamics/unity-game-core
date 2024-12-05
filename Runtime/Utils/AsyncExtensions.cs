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

        public static void Bind(this GameplayPipelineBase pipeline, AsyncCounterFlag flag)
        {
            flag.OnValueChanged += flag =>
            {
                if (!flag.IsSet)
                    pipeline.Schedule();
            };
        }

        public static void Bind(this GameplayPipelineBase pipeline, AsyncFlag flag)
        {
            flag.OnValueChanged += flag =>
            {
                if (!flag.IsSet)
                    pipeline.Schedule();
            };
        }
    }
}