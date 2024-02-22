using Brainamics.Core;
using Facebook.Unity.Example;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Text/TextMeshProUGUI Font Style")]
    public class TextMeshProUGUIFontStyleAnimationClip : AnimationClipBase
    {
        public TextMeshProUGUI Target;
        public FontStyles Style;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            Target.fontStyle = Style;
            yield break;
        }
    }
}