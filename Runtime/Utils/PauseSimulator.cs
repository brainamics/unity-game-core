using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Brainamics.Core
{
    public class PauseSimulator
    {
        private bool _paused;
        private float _timeScale;

        public bool IsPaused => _paused;

        public void Pause()
        {
            if (_paused)
                return;
            _paused = true;

            _timeScale = Time.timeScale;
            Time.timeScale = 0;
            AudioListener.pause = true;
        }

        public void Resume()
        {
            if (!_paused)
                return;
            _paused = false;

            Time.timeScale = _timeScale;
            AudioListener.pause = false;
        }
    }
}
