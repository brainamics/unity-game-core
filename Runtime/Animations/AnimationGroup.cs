using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brainamics.Core
{
    public class AnimationGroup : MonoBehaviour
    {
        [SerializeReference]
        private List<AnimationClipBase> _clips = new();

        public bool PlayOnEnable;

        public IList<AnimationClipBase> Clips => _clips;

        public void Play()
        {
            foreach (var clip in _clips)
                clip.Play(this);
        }

        public void PlayImmediate()
        {
            foreach (var clip in _clips)
                clip.PlayImmediate(this);
        }

        public void Stop()
        {
            foreach (var clip in _clips)
                clip.Stop();
        }

        public void Kill()
        {
            foreach (var clip in _clips)
                clip.Kill(this);
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
    }
}
