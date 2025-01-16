using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Brainamics.Core
{
    [AnimationClip(MenuName = "Canvas/Text Color [TMP]")]
    [System.Serializable]
    public class TextColorAnimationClip : AnimationClipBase
    {
        public TextMeshProUGUI Text;
        public bool AutoFromColor;

        public Color FromColor = Color.white;
        public Color ToColor = Color.black;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var text = Text ? Text : behaviour.GetComponent<TextMeshProUGUI>();
            if (!text)
                throw new System.InvalidOperationException("Could not find the target canvas group.");
            var fromColor = AutoFromColor ? text.color : FromColor;
            return RunTimedLoop(lerp =>
            {
                var color = Color.Lerp(fromColor, ToColor, lerp);
                text.color = color;
            });
        }
    }
}
