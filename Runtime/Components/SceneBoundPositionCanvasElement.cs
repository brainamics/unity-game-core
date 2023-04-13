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
        public Vector3 Position;
        public Transform BoundObject;
        public Vector3 TranslateOnCanvas = Vector3.zero;

        [SerializeField]
        [Tooltip("Optional camera; falls back to the main camera (The camera should have the CameraTransformer component attached)")]
        private Camera _camera;

        private CameraTransformer _cameraTransformer;

        private void Start()
        {
            if (_camera == null)
                _camera = Camera.main;
            if (!_camera.TryGetComponent<CameraTransformer>(out _cameraTransformer))
                return;
            _cameraTransformer.OnPositionChanged.AddListener(Reposition);
            Reposition();
        }

        private void OnDestroy()
        {
            if (_cameraTransformer != null)
                _cameraTransformer.OnPositionChanged.RemoveListener(Reposition);
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