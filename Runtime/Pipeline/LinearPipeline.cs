using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Brainamics.Core
{
    public class LinearPipeline
    {
        private readonly PriorityQueue<PipelineAction, PipelinePriority> _actionQueue = new();
        private readonly HashSet<object> _feedbacks = new();
        private readonly HashSet<System.Action> _processors = new();
        private int _queueIndex = int.MinValue;

        public bool AnyOngoingFeedbacks => _feedbacks.Count > 0;

        private sealed class FeedbackObject
        {
#if UNITY_EDITOR
            public System.Diagnostics.StackTrace Trace { get; } = new();
#endif
        }

        public void ExecuteActionsAndClear()
        {
            while (_actionQueue.TryDequeue(out var a, out _))
            {
                try
                {
                    a.Action.Invoke();
                }
                catch (Exception exception)
                {
                    Debug.LogError(exception);
                }
            }
            Clear();
        }

        public void Clear()
        {
            _actionQueue.Clear();
            _feedbacks.Clear();
        }

        public void EnqueueAction(PipelineAction action, int priority = 0)
        {
            // TODO check ID for duplicates
            var pipelinePriority = CreatePriority(priority);
            _actionQueue.Enqueue(action, pipelinePriority);
        }

        public void EnqueueAction(object id, System.Action action, int priority = 0)
            => EnqueueAction(new PipelineAction(id, action), priority);

        public void EnqueueAction(System.Action action, int priority = 0)
            => EnqueueAction(new PipelineAction(action), priority);

        public void RegisterProcessor(System.Action processor)
            => _processors.Add(processor);

        public void UnregisterProcessor(System.Action processor)
            => _processors.Remove(processor);

        public void RegisterFeedback(Task task)
        {
            RegisterFeedbackHandle(task);
            task.ContinueWith(UnregisterFeedbackHandle);
        }

        public IDisposable RegisterFeedbackHandle()
        {
            var handle = new object();
            RegisterFeedbackHandle(handle);
            return new CallbackDisposable(() => UnregisterFeedbackHandle(handle));
        }

        public void RegisterFeedbackHandle(object feedback)
        {
            if (!_feedbacks.Add(feedback))
                throw new System.InvalidOperationException("Could not register feedback; it's already registered.");
        }

        public bool UnregisterFeedbackHandle(object feedback)
        {
            return _feedbacks.Remove(feedback);
        }

        public bool TriggerProcess()
        {
            foreach (var processor in _processors)
                processor.Invoke();

            var processed = false;

            while (true)
            {
                if (AnyOngoingFeedbacks)
                    return processed;

                var result = ExecuteAction();
                if (!result)
                    return processed;
                processed = true;
            }
        }

        public bool ExecuteAction()
        {
            if (!_actionQueue.TryDequeue(out var action, out _))
                return false;

            action.Action.Invoke();
            return true;
        }

        private PipelinePriority CreatePriority(int priority = 0)
        {
            unchecked
            {
                return new PipelinePriority(priority, _queueIndex++);
            }
        }
    }
}
