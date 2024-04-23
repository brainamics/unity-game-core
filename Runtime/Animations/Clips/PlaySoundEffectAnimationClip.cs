using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    [AnimationClip(MenuName = "Audio/Sound Effect")]
    public class PlaySoundEffectAnimationClip : AnimationClipBase
    {
        public AudioServiceBase AudioService;
        public AudioKind Kind = AudioKind.Effect;
        public AudioSource Source;
        public AudioClip[] RandomClips;

        public override void PlayImmediate(MonoBehaviour behaviour)
        {
        }

        protected override IEnumerator PlayCoroutine(MonoBehaviour behaviour)
        {
            if (AudioService == null)
            {
                Source.PlayOneShot(RandomUtils.Select(RandomClips));
                yield break;
            }

            AudioService.PlayRandomEffect(Source, RandomClips);
        }
    }
}
