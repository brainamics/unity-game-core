using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public readonly struct PipelineAction
    {
        public readonly object Id;
        public readonly Action Action;

        public PipelineAction(object key, Action action)
        {
            Id = key;
            Action = action;
        }

        public PipelineAction(Action action)
            : this(null, action)
        { }
    }
}
