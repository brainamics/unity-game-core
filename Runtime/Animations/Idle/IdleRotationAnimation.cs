using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class IdleRotationAnimation : MonoBehaviour
    {
        public Transform Transform;
        public float RotationSpeed = 1f;
        public Vector3 RotationDelta = new(0, 1, 0);
        public bool Unscaled;

        private void Awake()
        {
            if (Transform == null)
                Transform = transform;
        }

        private void Update()
        {
            var deltaTime = Unscaled ? Time.unscaledDeltaTime : Time.deltaTime;
            Transform.Rotate(RotationSpeed * deltaTime * RotationDelta);
        }
    }
}
