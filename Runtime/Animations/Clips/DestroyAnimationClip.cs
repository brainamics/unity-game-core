using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Game Object/Destroy")]
    public class DestroyAnimationClip : TransformAnimationClipBase
    {
        public bool SupportPooling = true;

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var transform = GetTargetTransform(behaviour);
            if (SupportPooling)
                transform.gameObject.Destroy();
            else
                Object.Destroy(transform.gameObject);
            yield break;
        }
    }
}
