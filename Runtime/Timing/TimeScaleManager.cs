using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class TimeScaleManager : MonoBehaviour
    {
        private bool _paused;
        private float _normalTimescale = 1f;

        public bool IsPaused
        {
            get => _paused;
            set
            {
                if (_paused == value) return;
                _paused = value;
                UpdateTimescale();
            }
        }

        public void AddMultiplier(float timescaleMultiplier)
        {
            _normalTimescale *= timescaleMultiplier;
            UpdateTimescale();
        }

        public void RemoveMultiplier(float timescaleMultiplier)
        {
            _normalTimescale /= timescaleMultiplier;
            UpdateTimescale();
        }
        
        public void Reset()
        {
            _normalTimescale = 1f;
            UpdateTimescale();
        }

        private void UpdateTimescale()
        {
            Time.timeScale = _paused ? 0f : _normalTimescale;
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}
