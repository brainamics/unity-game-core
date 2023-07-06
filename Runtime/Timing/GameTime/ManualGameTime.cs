using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class ManualGameTime : IGameTime
    {
        private double _unscaledTime, _time;
        private float _unscaledDeltaTime, _deltaTime;

        public float TimeScale { get; set; } = 1f;

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
