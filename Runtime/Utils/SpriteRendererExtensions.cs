using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class SpriteRendererExtensions
    {
        public static void StretchToSize(this SpriteRenderer spriteRenderer, Vector2 targetSize)
        {
            if (!spriteRenderer || !spriteRenderer.sprite)
            {
                throw new System.ArgumentException("Invalid SpriteRenderer or Sprite.", nameof(spriteRenderer));
                return;
            }

            var originalSize = spriteRenderer.sprite.bounds.size;
            var scaleFactors = new Vector3(
                targetSize.x / originalSize.x,
                targetSize.y / originalSize.y,
                1f
            );

            spriteRenderer.transform.localScale = scaleFactors;
        }
    }
}