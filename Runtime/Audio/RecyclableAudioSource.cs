using Brainamics.Core;
using UnityEngine;

namespace Brainamics.Core
{
    [RequireComponent(typeof(AudioSource))]
    public class RecyclableAudioSource : MonoBehaviour, IRecyclable
    {
        private AudioSource _audioSource;
    
        public void Recycle()
        {
            _audioSource.Stop();
            _audioSource.volume = 1f;
            _audioSource.pitch = 1f;
            _audioSource.loop = false;
            _audioSource.clip = null;
            _audioSource.enabled = true;
        }
    
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }
}
