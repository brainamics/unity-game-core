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
        private readonly List<RaycastResult> _raycastResults = new();

        public Canvas Canvas;
        public RectTransform RectTransform;
        public GraphicRaycaster Raycaster;
        public EventSystem EventSystem;
        public bool UseDraggableCenter;

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
            if (RectTransform == null && UseDraggableCenter)
                throw new System.InvalidOperationException("Could not find a RectTransform.");
        }

        protected override void UpdateDrag(Ray ray, DraggingContext context)
        {
            Vector2 pos;

            if (UseDraggableCenter)
                pos = RectTransform.position;
            else
                pos = RaycastingCamera.WorldToScreenPoint(ray.origin);

            var eventData = new PointerEventData(EventSystem)
            {
                position = pos,
            };

            _raycastResults.Clear();
            Raycaster.Raycast(eventData, _raycastResults);
            context.RaycastResults.Clear();
            context.RaycastResults.AddRange(_raycastResults);
        }
    }
}