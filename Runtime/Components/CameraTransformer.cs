using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [RequireComponent(typeof(Camera))]
    public class CameraTransformer : MonoBehaviour
    {
        private Vector3 _lastPosition;

        public UnityEvent<CameraTransformer> OnPositionChanged;

        private void Start()
        {
            _lastPosition = transform.position;
        }

        private void Update()
        {
            if (transform.position == _lastPosition)
                return;
            OnPositionChanged.Invoke(this);
            _lastPosition = transform.position;
        }

        public void Move(Vector3 newPos)
        {
            transform.position = newPos;
        }
    }
}