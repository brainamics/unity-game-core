using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class BoundsHint : MonoBehaviour, IBoundsProvider
    {
        [SerializeField]
        private Collider _collider;

        public SpaceBounds Bounds
        {
            get
            {
                if (_collider != null)
                    return new SpaceBounds(CoordinateMode.World, _collider.bounds);

                throw new System.NotSupportedException($"Getting the bounds of this object is not supported.");
            }
        }
    }
}
