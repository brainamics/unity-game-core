using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Renderer/Color")]
    public class RendererColorAnimationClip : AnimationClipBase
    {
        public Renderer Renderer;
        public int MaterialIndex;
        public string ColorParamName;

        public bool AutoFrom;
        public Color From, To;

        private Material Material
        {
            get => Renderer.materials[MaterialIndex];
            set
            {
                var materials = Renderer.materials;
                materials[MaterialIndex] = value;
                Renderer.materials = materials;
            }
        }

        private Color CurrentColor
        {
            get => string.IsNullOrEmpty(ColorParamName) ? Material.color : Material.GetColor(ColorParamName);
            set
            {
                if (string.IsNullOrEmpty(ColorParamName))
                {
                    Material.color = value;
                    return;
                }
                Material.SetColor(ColorParamName, value);
            }
        }

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var from = AutoFrom ? CurrentColor : From;
            return RunTimedLoop(lerp =>
            {
                CurrentColor = Color.LerpUnclamped(from, To, lerp);
            });
        }
    }
}