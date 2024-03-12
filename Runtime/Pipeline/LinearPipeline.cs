using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Brainamics.Core
{
    public class LinearPipeline
    {
        private readonly Queue<PipelineAction> _actionQueue = new();
        private readonly HashSet<object> _feedbacks = new();
        private readonly HashSet<System.Action> _processors = new();

        public bool AnyOngoingFeedbacks => _feedbacks.Count > 0;

        public void EnqueueAction(PipelineAction action)
        {
            // TODO check ID for duplicates
            _actionQueue.Enqueue(action);
        }

        public void EnqueueAction(object id, System.Action action)
            => EnqueueAction(new PipelineAction(id, action));

        public void RegisterProcessor(System.Action processor)
            => _processors.Add(processor);

        public void UnregisterProcessor(System.Action processor)
            => _processors.Remove(processor);

        public void RegisterFeedback(Task task)
        {
            RegisterFeedbackHandle(task);
            task.ContinueWith(UnregisterFeedbackHandle);
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

            if (AnyOngoingFeedbacks)
                return false;

            return ExecuteAction();
        }

        public bool ExecuteAction()
        {
            if (!_actionQueue.TryDequeue(out var action))
                return false;

            action.Action.Invoke();
            return true;
        }
    }
}
