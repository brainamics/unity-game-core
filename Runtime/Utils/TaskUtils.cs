using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Brainamics.Core
{
    public static class TaskUtils
    {
        public static async void RunInBackground(this Task task)
        {
            try
            {
                await task;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
                throw;
            }
        }
    }
}