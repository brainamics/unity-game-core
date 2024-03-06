using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Brainamics.Core
{
    public class DropZone : MonoBehaviour, IDropZone
    {
        private readonly List<DraggingZoneAcceptDelegate> _acceptHandlers = new();
        private DraggingContext _activeContext;

        [Header("Properties")]
        public bool AcceptByDefault;

        [Header("Feedbacks")]
        public AnimationGroup EnterFeedback;
        public AnimationGroup ExitFeedback;

        [Header("Events")]
        [SerializeField]
        private UnityEvent<IDropZone> _onDraggingEnter;

        [SerializeField]
        private UnityEvent<IDropZone> _onDraggingExit;

        [SerializeField]
        private UnityEvent<IDropZone> _onDraggingOver;

        [SerializeField]
        private UnityEvent<IDropZone, IDraggable> _onDrop;

        public bool IsDraggingOver => _activeContext != null;

        public UnityEvent<IDropZone> OnDraggingEnter => _onDraggingEnter;

        public UnityEvent<IDropZone> OnDraggingExit => _onDraggingExit;

        public UnityEvent<IDropZone> OnDraggingOver => _onDraggingOver;

        public UnityEvent<IDropZone, IDraggable> OnDrop => _onDrop;

        public DraggingContext Context => _activeContext;

        public void NotifyEnter(DraggingContext context)
        {
            if (_activeContext != null)
                return;

            _activeContext = context;
            if (ExitFeedback != null)
                ExitFeedback.Kill();
            if (EnterFeedback != null)
                EnterFeedback.Play();
            OnDraggingEnter.Invoke(this);
        }

        public void NotifyExit(DraggingContext context)
        {
            if (!Equals(_activeContext, context))
                return;

            _activeContext = null;
            if (EnterFeedback != null)
                EnterFeedback.Kill();
            if (ExitFeedback != null)
                ExitFeedback.Play();
            OnDraggingExit.Invoke(this);
        }

        public void NotifyDrop(DraggingContext context)
        {
            if (!Equals(_activeContext, context))
                throw new System.InvalidOperationException("Cannot drop into a drop zone without notifying enter first.");

            OnDrop.Invoke(this, Context.Draggable);
        }

        public void UpdateDrag(DraggingContext context)
        {
            if (!Equals(_activeContext, context))
                return;

            OnDraggingOver.Invoke(this);
        }

        public System.IDisposable RegisterAcceptHandler(DraggingZoneAcceptDelegate handler)
        {
            _acceptHandlers.Add(handler);
            return new CallbackDisposable(() => _acceptHandlers.Remove(handler));
        }

        public bool Accepts(IDraggable draggable, DraggingContext context)
        {
            if (!isActiveAndEnabled)
                return false;
            if (_acceptHandlers.Count == 0)
                return AcceptByDefault;

            foreach (var handler in _acceptHandlers)
                if (!handler(context))
                    return false;
            return true;
        }

        // Just to make sure component can be disabled
        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }
    }
}