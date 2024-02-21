using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Brainamics.Core
{
    [DefaultExecutionOrder(-9)]
    [DisallowMultipleComponent]
    public class DraggablePhysicsRaycaster : DraggableRaycasterBase
    {
        private RaycastHit[] _hits;

        [Range(1, 1000)]
        public int MaxRaycastHits = 50;

        protected override void Awake()
        {
            base.Awake();

            _hits = new RaycastHit[MaxRaycastHits];
        }

        protected override void UpdateDrag(Ray ray, DraggingContext context)
        {
            var count = Physics.RaycastNonAlloc(ray, _hits);

            context.RaycastHits.Clear();
            for (var i = 0; i < count; i++)
                context.RaycastHits.Add(_hits[i]);
        }
    }
}