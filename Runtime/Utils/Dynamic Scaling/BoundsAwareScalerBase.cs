using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public abstract class BoundsAwareScalerBase : MonoBehaviour
    {
        public bool AffectPosition = true;
        public bool AffectScale = true;

        [SerializeField]
        private Vector3 _referenceCenter;

        [SerializeField]
        private Vector3 _referenceSize = new(4, 0, 5);

        [Min(0)]
        public float MaxScale = float.MaxValue;

        public Bounds ExpectedBounds => new(_referenceCenter, _referenceSize);

        public (Bounds, Vector3 scaleMultiplier) Adjust(Bounds currentBounds)
        {
            var multiplier3 = Vector3.one;
            var expectedBounds = ExpectedBounds;
            var multiplierZ = expectedBounds.size.z / currentBounds.size.z;
            var multiplierX = expectedBounds.size.x / currentBounds.size.x;
            var actualMultiplier = Mathf.Min(MaxScale, Mathf.Min(multiplierZ, multiplierX));
            if (AffectPosition)
            {
                currentBounds.center = expectedBounds.center;
                // transform.position += expectedBounds.center - currentBounds.center;
            }

            if (AffectScale)
            {
                multiplier3 = new Vector3(actualMultiplier, 1, actualMultiplier);
                currentBounds.size = Vector3.Scale(currentBounds.size, multiplier3);
                // transform.localScale = Vector3.Scale(transform.localScale, multiplier3);
            }

            return (currentBounds, multiplier3);
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            var bounds = ExpectedBounds;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}