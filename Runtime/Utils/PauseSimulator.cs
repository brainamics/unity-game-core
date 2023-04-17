using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Brainamics.Core
{
    public class PauseSimulator
    {
        private readonly List<AudioListener> _pausedAudioListeners = new();
        private bool _paused;
        private float _timeScale;

        public bool IsPaused => _paused;

        protected virtual bool LookUpSceneForAudioListeners => false;

        public void Pause()
        {
            if (_paused)
                return;
            _paused = true;

            _timeScale = Time.timeScale;
            Time.timeScale = 0;
            foreach (var listener in EnumerateAudioListeners())
                if (listener.enabled)
                {
                    listener.enabled = false;
                    _pausedAudioListeners.Add(listener);
                }
        }

        public void Resume()
        {
            if (_paused)
                return;
            _paused = false;

            Time.timeScale = _timeScale;
            foreach (var listener in _pausedAudioListeners)
                listener.enabled = true;
            _pausedAudioListeners.Clear();
        }

        private IEnumerable<AudioListener> EnumerateAudioListeners()
        {
            if (!LookUpSceneForAudioListeners)
            {
                if (Camera.main.TryGetComponent<AudioListener>(out var listener))
                    yield return listener;
                yield break;
            }

            var scene = SceneManager.GetActiveScene();
            foreach (var rootObj in scene.GetRootGameObjects())
            {
                var listeners = rootObj.GetComponentsInChildren<AudioListener>();
                foreach (var listener in listeners)
                    yield return listener;
            }
        }
    }
}
