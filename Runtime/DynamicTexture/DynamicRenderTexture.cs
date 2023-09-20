using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [CreateAssetMenu(menuName = "Game/Dynamic Render Texture")]
    public class DynamicRenderTexture : ScriptableObject
    {
        private RenderTexture _texture;

        public float ResolutionMultiplier = 1f;

        public RenderTexture RenderTexture => GetOrCreateRenderTexture();

        private int ExpectedWidth => (int)(Screen.width * ResolutionMultiplier);

        private int ExpectedHeight => (int)(Screen.height * ResolutionMultiplier);

        private RenderTexture GetOrCreateRenderTexture()
        {
            var width = ExpectedWidth;
            var height = ExpectedHeight;

            if (_texture != null && width == _texture.width && height == _texture.height)
                return _texture;

            _texture?.Release();
            _texture = new RenderTexture(width, height, 24);
            return _texture;
        }
    }
}