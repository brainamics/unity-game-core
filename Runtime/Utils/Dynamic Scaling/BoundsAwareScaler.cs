using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Brainamics.Core
{
    public class BoundsAwareScaler : BoundsAwareScalerBase
    {
        [Header("Target Object")]
        public Transform TargetTransform;
        public Renderer ReferenceRenderer;
        public Collider ReferenceCollider;

        public virtual Bounds CurrentBounds
        {
            get
            {
                if (ReferenceRenderer)
                    return ReferenceRenderer.bounds;
                if (ReferenceCollider)
                    return ReferenceCollider.bounds;
                throw new System.NotSupportedException();
            }
        }

        [Button("Adjust")]
        public void Adjust()
        {
            var (targetBounds, scaleMultiplier) = Adjust(CurrentBounds);
            if (TargetTransform)
            {
                if (AffectPosition)
                    TargetTransform.position = targetBounds.center;
                if (AffectScale)
                    TargetTransform.localScale = Vector3.Scale(TargetTransform.localScale, scaleMultiplier);
            }
        }

        protected virtual void Awake()
        {
        }

        protected virtual void OnEnable()
        {
            Adjust();
        }

        protected virtual void OnDisable()
        {
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            if (!Application.isPlaying)
                return;

            var bounds = CurrentBounds;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}