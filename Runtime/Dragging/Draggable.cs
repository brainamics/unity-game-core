using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Brainamics.Core
{
    [DefaultExecutionOrder(-10)]
    public class Draggable : MonoBehaviour, IDraggable, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        private readonly DraggingContext _context = new();
        private readonly List<DraggingUpdateDelegate> _handlers = new();
        private IDropZone _zone;

        [Header("Interactions")]
        public bool InteractWithZones = true;

        [Header("Events")]
        [SerializeField]
        public UnityEvent<IDraggable> _onDraggingStart;

        [SerializeField]
        public UnityEvent<IDraggable> _onDraggingEnd;

        [SerializeField]
        public UnityEvent<IDraggable> _onDragging;

        [SerializeField]
        public UnityEvent<IDraggable, IDropZone> _onDrop;

        public bool IsDragging { get; private set; }

        public Func<IReadOnlyList<IDropZone>, IDropZone> ZoneSelector { get; set; }

        public DraggingContext Context => _context;

        public UnityEvent<IDraggable> OnDraggingStart => _onDraggingStart;

        public UnityEvent<IDraggable> OnDraggingEnd => _onDraggingEnd;

        public UnityEvent<IDraggable> OnDragging => _onDragging;

        public UnityEvent<IDraggable, IDropZone> OnDrop => _onDrop;

        public Draggable()
        {
            ZoneSelector = SelectZoneDefault;
        }

        public IDisposable RegisterDragHandler(DraggingUpdateDelegate handler)
        {
            _handlers.Add(handler);
            return new CallbackDisposable(() => _handlers.Remove(handler));
        }

        public void StartDragging(Vector2 position)
        {
            if (IsDragging)
                throw new InvalidOperationException("The draggable is already being dragged.");
            IsDragging = true;
            _context.UpdateEventData(position);
            OnDraggingStart.Invoke(this);
        }

        public void UpdateDragging(Vector2 position)
        {
            if (!IsDragging)
                throw new InvalidOperationException("The dragging must be started before updating.");
            _context.UpdateEventData(position);
            foreach (var handler in _handlers)
                handler.Invoke(_context);

            UpdateZone();
            OnDragging.Invoke(this);
        }

        public void EndDraggging(Vector2 position)
        {
            if (!IsDragging)
                return;
            IsDragging = false;
            _context.UpdateEventData(position);
            if (_zone != null && _zone.Accepts(this, Context))
            {
                OnDrop.Invoke(this, _zone);
                _zone.NotifyDrop(Context);
            }
            OnDraggingEnd.Invoke(this);
            SetCurrentZone(null);

            _context.Reset();
        }

        protected virtual IDropZone SelectZoneDefault(IEnumerable<IDropZone> zones)
        {
            return zones.FirstOrDefault();
        }

        private void Awake()
        {
            _context.SetDraggable(this, gameObject);
        }

        private void OnEnable()
        {
        }

        private void SetCurrentZone(IDropZone zone)
        {
            if (Equals(zone, _zone))
                return;

            if (_zone != null)
            {
                _zone.NotifyExit(Context);
                _zone = null;
            }

            _zone = zone;
            if (zone == null)
                return;
            zone.NotifyEnter(Context);
        }

        private void UpdateZone()
        {
            if (!InteractWithZones)
                return;

            var zones = Context.RaycastResults
                .Select(result => result.gameObject.GetComponentInParent<IDropZone>())
                .Where(zone => zone != null)
                .Where(zone => zone.Accepts(this, Context))
                .ToArray();
            var zone = ZoneSelector.Invoke(zones);
            SetCurrentZone(zone);

            zone?.UpdateDrag(Context);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            StartDragging(eventData.position);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            EndDraggging(eventData.position);
        }

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            if (!IsDragging)
                return;
            UpdateDragging(eventData.position);
            EndDraggging(eventData.position);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            UpdateDragging(eventData.position);
        }
    }
}