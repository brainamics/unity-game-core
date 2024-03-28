using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brainamics.Core
{
    public class AnimationGroup : MonoBehaviour
    {
        private readonly HashSet<AnimationClipBase> _activeClips = new();
        private bool _playing;

        [SerializeReference]
        private List<AnimationClipBase> _clips = new();

        public bool PlayOnEnable;
        public bool Loop;

        public IList<AnimationClipBase> Clips => _clips;

        public void Play()
        {
            _playing = true;
            _activeClips.Clear();
            foreach (var clip in _clips)
            {
                clip.Play(this);
                _activeClips.Add(clip);
            }
        }

        public void PlayImmediate()
        {
            _playing = false;
            foreach (var clip in _clips)
                clip.PlayImmediate(this);
        }

        public void Stop()
        {
            _playing = false;
            foreach (var clip in _clips)
                clip.Stop();
        }

        public void Kill()
        {
            _playing = false;
            _activeClips.Clear();
            foreach (var clip in _clips)
                clip.Kill(this);
        }

        private void Awake()
        {
            foreach (var clip in _clips)
                clip.OnPlayComplete += HandleClipComplete;
        }

        private void OnEnable()
        {
            if (PlayOnEnable)
                Play();
        }

        private void OnValidate()
        {
            while (_clips.Remove(null)) ;
        }

        private void HandleClipComplete(AnimationClipBase clip)
        {
            if (!_activeClips.Remove(clip))
                return;
            if (_activeClips.Count == 0 && Loop && _playing)
                Play();
        }
    }
}
