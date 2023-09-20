using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [RequireComponent(typeof(Camera))]
    public class CameraDynamicTexture : MonoBehaviour
    {
        public DynamicRenderTexture DynamicTexture;

        private void Awake()
        {
            if (DynamicTexture == null)
                return;

            var camera = GetComponent<Camera>();
            camera.targetTexture = DynamicTexture.RenderTexture;
        }
    }
}