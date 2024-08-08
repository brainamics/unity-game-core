using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public sealed class GameplayPipelineContext
    {
        public bool? IsSuccess { get; private set; }

        public bool IsCanceled { get; private set; }

        public GameplayPipelineContext()
        {
            Reset();
        }

        /// <summary>
        /// Prepares the context for the next call on the pipeline.
        /// </summary>
        public void PrepareForCall()
        {
            IsSuccess = null;
        }

        public void SetResult(bool success)
        {
            if (IsSuccess == success)
                return;
            if (IsSuccess.HasValue)
                throw new System.InvalidOperationException("Result already set.");
            IsSuccess = success;
        }

        /// <summary>
        /// Sets the result to success and cancels the pipeline.
        /// </summary>
        public void Conclude()
        {
            SetResult(true);
            Cancel();
        }

        /// <summary>
        /// Sets the result to failure and cancels the pipeline.
        /// </summary>
        public void Fail()
        {
            SetResult(false);
            Cancel();
        }

        public void Cancel()
        {
            IsCanceled = true;
        }

        public void Reset()
        {
            IsCanceled = false;
            IsSuccess = null;
        }
    }
}