using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public interface IDropZone : IBehaviour
    {
        UnityEvent<IDropZone> OnDraggingEnter { get; }

        UnityEvent<IDropZone> OnDraggingExit { get; }

        UnityEvent<IDropZone> OnDraggingOver { get; }

        UnityEvent<IDropZone, IDraggable> OnDrop { get; }

        IDisposable RegisterAcceptHandler(DraggingZoneAcceptDelegate handler);

        void NotifyEnter(DraggingContext context);

        void NotifyExit(DraggingContext context);

        void NotifyDrop(DraggingContext context);

        void UpdateDrag(DraggingContext context);

        bool Accepts(IDraggable draggable, DraggingContext context);
    }
}