using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Text/TextMeshProUGUI Color")]
    public class TextMeshProUGUIColorAnimationClip : AnimationClipBase
    {
        public TextMeshProUGUI Target;

        public bool AutoFrom;
        public Color From, To;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var from = AutoFrom ? Target.color : From;
            return RunTimedLoop(lerp =>
            {
                Target.color = Color.LerpUnclamped(from, To, lerp);
            });
        }
    }
}