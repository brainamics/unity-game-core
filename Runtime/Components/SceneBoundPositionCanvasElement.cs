using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    /// <summary>
    /// Defines a MonoBehaviour for any canvas-based object that needs to be displayed at a world position.
    /// </summary>
    public class SceneBoundPositionCanvasElement : MonoBehaviour
    {
        private Transform _lastBoundObject;

        public Vector3 Position;
        public Transform BoundObject;
        public Vector3 TranslateOnCanvas = Vector3.zero;

        [SerializeField]
        [Tooltip("Optional camera; falls back to the main camera")]
        private Camera _camera;

        private TransformChangeNotifier _cameraTransformNotifier;

        private void OnEnable()
        {
            if (_camera == null)
                _camera = Camera.main;
            _cameraTransformNotifier = TransformChangeNotifier.For(_camera);
            _cameraTransformNotifier.OnTransformChanged.AddListener(HandleTransformMoved);
            var notifier = TransformChangeNotifier.For(BoundObject);
            notifier.OnTransformChanged.AddListener(HandleTransformMoved);
            Reposition();
        }

        private void OnDisable()
        {
            if (_cameraTransformNotifier != null)
                _cameraTransformNotifier.OnTransformChanged.RemoveListener(HandleTransformMoved);
        }

        private void Update()
        {
            UpdateBoundObject();
        }

        private void UpdateBoundObject()
        {
            if (_lastBoundObject == BoundObject)
                return;
            UnbindFromBoundObject(_lastBoundObject);
            _lastBoundObject = BoundObject;
            BindToBoundObject(BoundObject);
        }

        private void BindToBoundObject(Transform bo)
        {
            if (bo == null)
                return;
            var notifier = TransformChangeNotifier.For(bo);
            notifier.OnTransformChanged.AddListener(HandleTransformMoved);
        }

        private void UnbindFromBoundObject(Transform bo)
        {
            var notifier = TransformChangeNotifier.For(bo);
            notifier.OnTransformChanged.RemoveListener(HandleTransformMoved);
        }

        private void HandleTransformMoved(Transform _)
        {
            Reposition();
        }

        private void Reposition()
        {
            if (BoundObject == null)
            {
                transform.position = _camera.WorldToScreenPoint(Position) + TranslateOnCanvas;
                return;
            }

            if (BoundObject.TryGetComponent<RectTransform>(out var rectTransform))
            {
                var position = rectTransform.position + Position;
                transform.position = position + TranslateOnCanvas;
                return;
            }

            transform.position = _camera.WorldToScreenPoint(BoundObject.position) + TranslateOnCanvas;
        }
    }
}
