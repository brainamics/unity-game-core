using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public interface IAudioService
    {
        /// <summary>
        /// Gets the event that gets invoked whenever there is a change in the audio configuration.
        /// </summary>
        UnityEvent ConfigurationChanged { get; }

        /// <summary>
        /// Returns whether audio of the specified <paramref name="kind"/> can be played.
        /// </summary>
        bool CanPlay(AudioKind kind);

        /// <summary>
        /// Plays the given audio source if enabled.
        /// </summary>
        void Play(AudioKind kind, AudioSource source);

        /// <summary>
        /// One shot plays the given audio clip if enabled.
        /// </summary>
        void PlayOneShot(AudioKind kind, AudioSource audio, AudioClip clip);
    }

    public static class AudioServiceExtensions
    {
        public static void PlayEffect(this IAudioService audioService, AudioSource source)
        {
            audioService.Play(AudioKind.Effect, source);
        }

        public static void PlayMusic(this IAudioService audioService, AudioSource source)
        {
            audioService.Play(AudioKind.Music, source);
        }

        [System.Obsolete("Use PlayOneShot overload instead")]
        public static void PlayEffectOneShot(this IAudioService audioService, AudioSource audio, AudioClip clip)
        {
            audioService.PlayOneShot(AudioKind.Effect, audio, clip);
        }

        public static void PlayOneShot(this IAudioService audioService, AudioSource audio, AudioClip clip)
        {
            audioService.PlayOneShot(AudioKind.Effect, audio, clip);
        }

        public static void PlayRandomEffect(this IAudioService audioService, AudioSource audio, IReadOnlyList<AudioClip> clips)
        {
            AudioClip clip = clips.Count switch
            {
                0 => null,
                1 => clips[0],
                _ => clips[Random.Range(0, clips.Count)],
            };
            audioService.PlayOneShot(audio, clip);
        }

#if DOTWEEN
        public static void PlayOneShotAndDestroy(this IAudioService audioService, AudioSource audio, AudioClip clip)
        {
            const float SafetyDuration = 0.5f;
            audioService.PlayOneShot(AudioKind.Effect, audio, clip);
            DG.Tweening.DOVirtual.DelayedCall(SafetyDuration, audio.gameObject.Destroy);
        }
#endif
    }
}
