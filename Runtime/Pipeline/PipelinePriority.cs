using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public readonly struct PipelinePriority : IComparable<PipelinePriority>
    {
        public readonly int Priority;
        public readonly int QueueOrder;

        public PipelinePriority(int priority, int queueOrder)
        {
            Priority = priority;
            QueueOrder = queueOrder;
        }

        public int CompareTo(PipelinePriority other)
        {
            var priorityResult = Priority.CompareTo(other.Priority);
            if (priorityResult != 0)
                return priorityResult;
            return QueueOrder.CompareTo(other.QueueOrder);
        }
    }
}
