using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Brainamics.Core
{
    public class CanvasTutorialPointer : TutorialPointerBase
    {
        private TutorialPointAtRequest _request = TutorialPointAtRequest.InvisibleImmediate;
        private bool _visible, _down;
        private Coroutine _visibilityCoroutine, _transformCoroutine;

        [SerializeField]
        private Canvas _canvas;

        public bool RepositionOnUpdate = true;
        public RectTransform Image;
        public CanvasGroup CanvasGroup;
        public Vector3 WorldPreTranslate;
        public Vector3 ScreenPreTranslate;
        public Vector2 ScreenPostTranslate;
        public Vector3 CanvasPostTranslate;
        public Camera Camera;

        [SerializeField]
        private UnityEvent<ITutorialPointer> _onClickStateChanged;

        [SerializeField]
        public UnityEvent<ITutorialPointer, TutorialPointAtRequest> _onPoint;

        [Header("Animations")]
        public float VisibilityTransitionDuration = 0.1f;
        public float DefaultPositionTransitionDuration = 0.2f;
        public AnimationCurve DefaultPositionTransitionAnimation = AnimationCurve.Linear(0, 0, 1, 1);

        public override SpaceCoordinates Coordinates { get; }

        public override bool IsVisible => _visible;

        public override bool IsClickDown => _down;

        public override TutorialPointAtRequest ActiveRequest => _request;

        protected Camera ResolvedCamera => Camera == null ? Camera.main : Camera;

        public override UnityEvent<ITutorialPointer, TutorialPointAtRequest> OnPoint => _onPoint;

        public override UnityEvent<ITutorialPointer> OnClickStateChanged => _onClickStateChanged;

        public override void PointAt(TutorialPointAtRequest request)
            => PointAt(request, true);

        public void PointAt(TutorialPointAtRequest request, bool cancelAnimations, bool invokeEvents = true)
        {
            _request = request;
            PointAt(cancelAnimations, invokeEvents);
        }

        public override void SetClickState(bool down)
        {
            if (_down == down)
                return;
            _down = down;
            SetClickStateInternal();
            OnClickStateChanged.Invoke(this);
        }

        protected virtual void Awake()
        {
            InitializeCanvas();
        }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

        private void Update()
        {
            if (RepositionOnUpdate && _transformCoroutine == null && _request.Visible)
                PointAt(false, false);
        }

        private void PointAt(bool cancelAnimations, bool invokeEvents = true)
        {
            var request = _request;
            if (request.Visible)
            {
                var position = request.TargetPosition.Position;
                if (request.TargetPosition.Mode == CoordinateMode.World)
                    position += WorldPreTranslate;
                if (request.TargetPosition.Mode == CoordinateMode.Screen)
                    position += ScreenPreTranslate;

                var target = request.TargetPosition.WithPosition(position).ToScreen(ResolvedCamera);
                target += ScreenPostTranslate;
                PointAt(target.Position, request.TargetRotation, request, cancelAnimations);
            }
            else
            {
                PointAt(Vector3.zero, Quaternion.identity, request, cancelAnimations);
            }
            if (invokeEvents)
                OnPoint.Invoke(this, request);
        }

        private void InitializeCanvas()
        {
            if (_canvas != null)
                return;
            _canvas = GetComponentInParent<Canvas>();
            // if (_canvas == null)
            //    throw new System.InvalidOperationException($"The canvas pointer should be placed inside a canvas.");
        }

        protected virtual IEnumerator AnimateVisibility(bool visible)
        {
            if (CanvasGroup == null)
            {
                SetVisibilityImmediate(visible);
                yield break;
            }

            if (visible)
                Activate(true);

            if (CanvasGroup.alpha == (visible ? 1 : 0))
                yield break;

            var startTime = Time.time;
            while (true)
            {
                var progress = Mathf.Clamp01((Time.time - startTime) / VisibilityTransitionDuration);
                CanvasGroup.alpha = visible ? progress : 1 - progress;

                if (progress >= 1)
                    break;
                yield return null;
            }
        }

        protected virtual void SetVisibilityImmediate(bool visible)
        {
            Activate(visible);
            if (CanvasGroup != null)
                CanvasGroup.alpha = visible ? 1 : 0;
        }

        protected virtual IEnumerator AnimatePosition(Vector3 position, TutorialPointAtRequest request)
        {
            var startPos = request.StartingPosition ?? GetPosition();

            if (!IsVisible || startPos == position)
            {
                SetPositionImmediate(position, request);
                yield break;
            }

            var duration = request.TransitionDuration > 0 ? request.TransitionDuration : DefaultPositionTransitionDuration;
            var curve = request.TransitionCurve != null ? request.TransitionCurve : DefaultPositionTransitionAnimation;
            var startTime = Time.time;
            var diff = position - startPos;
            while (true)
            {
                var progress = Mathf.Clamp01((Time.time - startTime) / duration);
                position = startPos + diff * curve.Evaluate(progress);
                SetPositionImmediate(position, request);

                if (progress >= 1)
                    break;
                yield return null;
            }
        }

        protected virtual Vector3 GetPosition()
            => ((RectTransform)transform).position;

        protected virtual void SetPositionImmediate(Vector3 position, TutorialPointAtRequest request)
        {
            var rectTransform = (RectTransform)transform;
            rectTransform.position = position;
        }

        protected virtual void Activate(bool activate)
        {
            if (activate)
                _canvas.gameObject.SetActive(true);
            Image.gameObject.SetActive(activate);
        }

        protected virtual void SetClickStateInternal() { }

        private void PointAt(Vector3 screenPosition, Quaternion targetRotation, TutorialPointAtRequest request, bool cancelAnimations)
        {
            InitializeCanvas();
            if (_canvas == null)
            {
                Debug.LogWarning("The CanvasTutorialPointer object is not placed on a canvas.");
                return;
            }
            _request = request;
            var position = screenPosition + CanvasPostTranslate;

            if (request.Visible)
                SetPositionAndRotation(position, targetRotation, request, cancelAnimations);
            SetVisibility(request.Visible, request.Immediate, cancelAnimations);
        }

        private void SetVisibility(bool visible, bool immediate = false, bool cancelAnimations = false)
        {
            if (_visible == visible)
                return;
            _visible = visible;

            if (_canvas == null)
                throw new System.InvalidOperationException("Canvas cannot be NULL.");

            if (visible)
                _canvas.gameObject.SetActive(true);

            if (cancelAnimations)
            {
                StopCoroutine(ref _visibilityCoroutine);
            }

            if (immediate)
            {
                SetVisibilityImmediate(visible);
                return;
            }

            StartOmniCoroutine(ref _visibilityCoroutine, AnimateVisibility(visible), () => _visibilityCoroutine = null);
        }

        private void SetPositionAndRotation(Vector3 position, Quaternion rotation, TutorialPointAtRequest request, bool cancelAnimations = false)
        {
            // TODO rotation

            if (cancelAnimations)
            {
                // StopCoroutine(ref _visibilityCoroutine);
                StopCoroutine(ref _transformCoroutine);
            }

            if (request.Immediate || !IsVisible)
            {
                SetPositionImmediate(position, request);
                return;
            }

            StartOmniCoroutine(ref _transformCoroutine, AnimatePosition(position, request), () => _transformCoroutine = null);
        }
    }
}
