using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public abstract class MusicPlayerBase : MonoBehaviour
    {
        private AudioClip _activeClip;
        private bool _playing;

        [SerializeField]
        private AudioSource _musicAudioSource;

        [SerializeField]
        private AudioClip _musicClip;

        public AudioClip MusicClip
        {
            get => _musicClip;
            set => _musicClip = value;
        }

        protected abstract IAudioService AudioService { get; }

        private void Start()
        {
            AudioService.ConfigurationChanged.AddListener(UpdatePlayState);
        }

        private void Update()
        {
            CheckForClipChanges();
        }

        private void OnDestroy()
        {
            AudioService.ConfigurationChanged.RemoveListener(UpdatePlayState);
        }

        private void CheckForClipChanges()
        {
            if (_activeClip == _musicClip)
                return;
            UpdatePlayState();
        }

        private void UpdatePlayState()
        {
            var shouldPlay = AudioService.CanPlay(AudioKind.Music) && _musicClip != null;
            if (_activeClip == _musicClip && _playing == shouldPlay)
                return;

            _musicAudioSource.Stop();

            if (!shouldPlay)
            {
                _playing = false;
                _activeClip = null;
                _musicAudioSource.clip = null;
                return;
            }
            _activeClip = _musicClip;
            _musicAudioSource.clip = _musicClip;
            AudioService.Play(AudioKind.Music, _musicAudioSource);
            _playing = AudioService.CanPlay(AudioKind.Music);
        }
    }
}
