using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public sealed class PipelineAction
    {
        public readonly object Id;
        public readonly Action Action;
        public event Action OnCanceled;
        public event Action OnComplete;

        public bool IsCanceled { get; private set; }

        public PipelineAction(object key, Action action)
        {
            Id = key;
            Action = action;
        }

        public PipelineAction(Action action)
            : this(null, action)
        { }

        public void InvokeAction()
        {
            if (IsCanceled)
                return;

            Action.Invoke();
            OnComplete?.Invoke();
        }

        public void Cancel()
        {
            if (IsCanceled)
                return;

            IsCanceled = true;
            OnCanceled?.Invoke();
        }
    }
}
