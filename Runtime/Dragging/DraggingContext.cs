using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Brainamics.Core
{
    public sealed class DraggingContext
    {
        public IList<RaycastResult> RaycastResults { get; } = new List<RaycastResult>();

        public IList<RaycastHit> RaycastHits { get; } = new List<RaycastHit>();

        public IDraggable Draggable { get; set; }

        public GameObject DraggableObject { get; private set; }

        public bool PointerDragging { get; private set; }

        public Vector2 PointerPosition { get; set; }

        public void UpdateEventData(Vector2 pointerPosition)
        {
            PointerPosition = pointerPosition;
        }

        public void Reset()
        {
            Draggable = null;
            DraggableObject = null;
            PointerDragging = false;
            RaycastResults.Clear();
        }
    }
}