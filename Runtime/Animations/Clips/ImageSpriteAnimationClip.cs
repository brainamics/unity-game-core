using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Canvas/Image Sprite")]
    public class ImageSpriteAnimationClip : AnimationClipBase
    {
        public Image Image;
        public Sprite Sprite;

        public override void PlayImmediate(MonoBehaviour behaviour)
        {
            ApplySprite();
        }

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            ApplySprite();
            yield break;
        }

        private void ApplySprite()
        {
            Image.sprite = Sprite;
        }
    }
}