using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class DestroyAnimationClip : TransformAnimationClipBase
    {
        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            var transform = GetTargetTransform(behaviour);
            Object.Destroy(transform.gameObject);
            yield break;
        }
    }
}
