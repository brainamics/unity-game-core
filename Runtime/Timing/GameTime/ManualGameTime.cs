using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class ManualGameTime : MonoBehaviour, IGameTime
    {
        private double _unscaledTime, _time;
        private float _unscaledDeltaTime, _deltaTime;

        [SerializeField]
        [Range(0, 1)]
        private float _timeScale = 1f;

        public float TimeScale
        {
            get => _timeScale;
            set
            {
                if (float.IsNaN(value))
                    throw new System.ArgumentOutOfRangeException(nameof(value), "TimeScale cannot be NaN");
                if (value < 0 || value > 1)
                    throw new System.ArgumentOutOfRangeException(nameof(value), "TimeScale must be between 0 and 1");
                _timeScale = value;
            }
        }

        public float Time => (float)_time;

        public double TimeAsDouble => _time;

        public float DeltaTime => _deltaTime;

        public float UnscaledDeltaTime => _unscaledDeltaTime;

        public float UnscaledTime => (float)_unscaledTime;

        public double UnscaledTimeAsDouble => _unscaledTime;

        public void SetTime(double unscaledTime)
        {
            _unscaledDeltaTime = (float)(unscaledTime - _unscaledTime);
            _unscaledTime = unscaledTime;
            _time = _unscaledTime * TimeScale;
            _deltaTime = _unscaledDeltaTime * TimeScale;
        }
    }
}
