using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Brainamics.Core
{
    [RequireComponent(typeof(RawImage))]
    public class RawImageDynamicTexture : MonoBehaviour
    {
        public DynamicRenderTexture RenderTexture;

        private void Awake()
        {
            var rawImage = GetComponent<RawImage>();
            if (RenderTexture != null)
                rawImage.texture = RenderTexture.RenderTexture;
        }
    }
}