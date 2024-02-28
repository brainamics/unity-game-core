using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Brainamics.Core
{
    [DefaultExecutionOrder(-9)]
    public class DraggableGraphicRaycaster : DraggableRaycasterBase
    {
        public enum FeedbackModes
        {
            Pointer,
            TransformPosition,
            FeedbackPoints,
        }

        private readonly List<RaycastResult> _raycastResults = new();

        [Header("Elements")]
        public Canvas Canvas;
        public RectTransform RectTransform;
        public GraphicRaycaster Raycaster;
        public EventSystem EventSystem;

        [Header("Feedback")]
        public FeedbackModes FeedbackMode = FeedbackModes.Pointer;
        public Transform[] FeedbackPoints;

        protected override void Awake()
        {
            base.Awake();

            if (Canvas == null)
                Canvas = GetComponentInParent<Canvas>(true);
            if (Raycaster == null)
                Raycaster = Canvas.GetComponent<GraphicRaycaster>();
            if (EventSystem == null)
                EventSystem = EventSystem.current;
            if (RectTransform == null)
                RectTransform = GetComponent<RectTransform>();
            if (RectTransform == null && FeedbackMode == FeedbackModes.TransformPosition)
                throw new System.InvalidOperationException("Could not find a RectTransform.");
        }

        protected override void UpdateDrag(Ray ray, DraggingContext context)
        {
            switch (FeedbackMode)
            {
                case FeedbackModes.Pointer:
                    DoRaycast(context, RaycastingCamera.WorldToScreenPoint(ray.origin));
                    break;

                case FeedbackModes.TransformPosition:
                    DoRaycast(context, RectTransform.position);
                    break;

                case FeedbackModes.FeedbackPoints:
                    foreach (var point in FeedbackPoints)
                        if (DoRaycast(context, point.position))
                            break;
                    break;

                default:
                    throw new System.NotImplementedException();
            }
        }

        private bool DoRaycast(DraggingContext context, Vector3 pos)
        {
            var eventData = new PointerEventData(EventSystem)
            {
                position = pos,
            };

            _raycastResults.Clear();
            Raycaster.Raycast(eventData, _raycastResults);
            context.RaycastResults.Clear();
            context.RaycastResults.AddRange(_raycastResults);
            return _raycastResults.Count > 0;
        }
    }
}