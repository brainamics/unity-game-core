using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [RequireComponent(typeof(IDraggable))]
    [DefaultExecutionOrder(1)]
    public class CanvasDraggableMover : MonoBehaviour
    {
        private IDraggable _draggable;
        private Vector3 _positionDiff;

        [Tooltip("Optional target transform - leave empty to use the hosting transform")]
        [SerializeField]
        private Transform _targetTransform;

        public bool SetLastSiblingOnStart = true;

        private void Awake()
        {
            if (!TryGetComponent(out _draggable))
                throw new System.InvalidOperationException("Could not find an IDraggable component on the game object.");
            if (_targetTransform == null)
                _targetTransform = transform;
        }

        private void OnEnable()
        {
            _draggable.OnDraggingStart.AddListener(HandleDraggingStart);
            _draggable.OnDraggingEnd.AddListener(HandleDraggingEnd);
            _draggable.OnDragging.AddListener(HandleDragging);
        }

        private void OnDisable()
        {
            _draggable.OnDraggingStart.RemoveListener(HandleDraggingStart);
            _draggable.OnDraggingEnd.RemoveListener(HandleDraggingEnd);
            _draggable.OnDragging.RemoveListener(HandleDragging);
        }

        private void HandleDraggingStart(IDraggable _)
        {
            _positionDiff = _targetTransform.position - (Vector3)_draggable.Context.PointerPosition;

            if (SetLastSiblingOnStart)
                _targetTransform.SetAsLastSibling();
        }

        private void HandleDraggingEnd(IDraggable _)
        {
        }

        private void HandleDragging(IDraggable _)
        {
            _targetTransform.position = (Vector3)_draggable.Context.PointerPosition + _positionDiff;
        }
    }
}