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
        private readonly Dictionary<object, PipelineAction> _actionsById = new();
        private int _queueIndex = int.MinValue;

        public bool AnyOngoingFeedbacks => _feedbacks.Count > 0;

        public int FeedbacksCount => _feedbacks.Count;

        public int InQueueActionsCount => _actionQueue.Count;

#if UNITY_EDITOR
        public IEnumerable<object> Feedbacks => _feedbacks;
#endif

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
                    a.InvokeAction();
                }
                catch (System.Exception exception)
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
            _actionsById.Clear();
        }

        public void CancelAction(object id)
        {
            if (!_actionsById.TryGetValue(id, out var action))
                return;
            CancelAction(action);
        }

        public void CancelAction(PipelineAction action)
        {
            if (action == null)
                throw new System.ArgumentNullException(nameof(action));
            action.Cancel();
            _actionsById.Remove(action.Id);
        }

        public PipelineAction EnqueueAction(PipelineAction action, int priority = 0, bool replace = false)
        {
            var duplicateId = action.Id != null && _actionsById.TryGetValue(action.Id, out var prevAction) && !prevAction.IsCanceled;
            if (duplicateId)
            {
                if (replace)
                {
                    CancelAction(action);
                }
                else
                {
                    return null;
                }
            }

            var pipelinePriority = CreatePriority(priority);
            if (action.Id != null)
                _actionsById[action.Id] = action;
            _actionQueue.Enqueue(action, pipelinePriority);
            return action;
        }

        public PipelineAction EnqueueAction(object id, System.Action action, int priority = 0, bool replace = false)
            => EnqueueAction(new PipelineAction(id, action), priority, replace: replace);

        public PipelineAction EnqueueAction(System.Action action, int priority = 0)
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

        public System.IDisposable RegisterFeedbackHandle()
        {
            var handle = new FeedbackObject();
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

            if (action.Id != null)
                _actionsById.Remove(action.Id);
            action.InvokeAction();
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
