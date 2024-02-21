using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Brainamics.Core
{
    [RequireComponent(typeof(IDraggable))]
    [DefaultExecutionOrder(-9)]
    public abstract class DraggableRaycasterBase : MonoBehaviour
    {
        private IDisposable _dragHandler;

        public Camera RaycastingCamera;

        protected IDraggable Draggable { get; private set; }

        protected virtual void Awake()
        {
            if (RaycastingCamera == null)
                RaycastingCamera = Camera.main;
            if (!TryGetComponent<IDraggable>(out var draggable))
                throw new InvalidOperationException("Could not find the IDraggable component.");
            Draggable = draggable;
        }

        protected virtual void OnEnable()
        {
            _dragHandler = Draggable.RegisterDragHandler(UpdateDrag);
        }

        protected virtual void OnDisable()
        {
            _dragHandler?.Dispose();
            _dragHandler = null;
        }

        protected abstract void UpdateDrag(Ray ray, DraggingContext context);

        private void UpdateDrag(DraggingContext context)
        {
            var ray = RaycastingCamera.ScreenPointToRay(context.PointerPosition);
            UpdateDrag(ray, context);
        }
    }
}