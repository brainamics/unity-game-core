using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class CameraLooker : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Optional camera reference, falls back to the main camera")]
        private Camera _camera;

        private void Update()
        {
            var camera = _camera;
            if (camera == null)
                camera = Camera.main;
            transform.LookAt(camera.transform.position, Vector3.up);
        }
    }
}