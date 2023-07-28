using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public class FrameRateCounter : MonoBehaviour
    {
        private int _frameCounter = 0;
        private float _timeCounter = 0.0f;
        private float _lastFramerate = 0.0f;

        public float RefreshTime = 0.5f;
        public UnityEvent OnFrameRate;

        public float FrameRate => _lastFramerate;

        private void Awake()
        {
            _lastFramerate = Application.targetFrameRate;
        }

        private void Update()
        {
            if (_timeCounter < RefreshTime)
            {
                _timeCounter += Time.unscaledDeltaTime;
                _frameCounter++;
            }
            else
            {
                _lastFramerate = _frameCounter / _timeCounter;
                _frameCounter = 0;
                _timeCounter = 0.0f;
                OnFrameRate.Invoke();
            }
        }
    }
}
