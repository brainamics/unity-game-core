using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    // [CreateAssetMenu(menuName = "Game/Services/Audio Service")]
    public abstract class AudioServiceBase : ScriptableObject, IAudioService
    {
        [SerializeField]
        private UnityEvent _configChanged = new();

        public UnityEvent ConfigurationChanged => _configChanged;

        public abstract bool CanPlay(AudioKind kind);

        public void Play(AudioKind kind, AudioSource source)
        {
            if (source == null || !CanPlay(kind))
                return;
            source.Play();
        }

        public void PlayOneShot(AudioKind kind, AudioSource audio, AudioClip clip)
        {
            if (audio == null || clip == null || !CanPlay(kind))
                return;
            audio.PlayOneShot(clip);
        }
    }
}
