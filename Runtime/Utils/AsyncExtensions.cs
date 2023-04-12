using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Brainamics.Core
{
    internal static class AsyncExtensions
    {
        public static Task AsTask(this AsyncOperation asyncOperation)
        {
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
    }
}