using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public interface IDraggable : IBehaviour
    {
        bool IsDragging { get; }

        IDropZone ActiveZone { get; }

        DraggingContext Context { get; }

        UnityEvent<IDraggable> OnDraggingStart { get; }

        UnityEvent<IDraggable> OnDraggingEnd { get; }

        UnityEvent<IDraggable> OnDragging { get; }

        UnityEvent<IDraggable, IDropZone> OnDrop { get; }

        UnityEvent<IDraggable> OnZoneChanged { get; }

        System.IDisposable RegisterDragHandler(DraggingUpdateDelegate handler);

        void StartDragging(Vector2 pointerPosition);

        void UpdateDragging(Vector2 pointerPosition);

        void EndDraggging(Vector2 pointerPosition);
    }
}
