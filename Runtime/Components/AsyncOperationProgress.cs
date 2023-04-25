using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class AsyncOperationProgress : MonoBehaviour
    {
        private IProgress<float> _progress;
        private AsyncOperation _asyncOperation;
        private float _lastProgress;

        public AsyncOperation AsyncOperation
        {
            get => _asyncOperation;
            set
            {
                if (_asyncOperation == value)
                    return;
                if (_asyncOperation != null)
                    _asyncOperation.completed -= AsyncOperation_completed;
                _asyncOperation = value;
                if (value != null)
                    value.completed += AsyncOperation_completed;
                Initialize();
            }
        }

        public float CurrentProgress => _asyncOperation.isDone ? 1f : _asyncOperation.progress;

        private void AsyncOperation_completed(AsyncOperation op)
        {
            UpdateProgress();
            AsyncOperation = null;
        }

        public IProgress<float> Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                Initialize();
            }
        }

        private void Update()
        {
            if (AsyncOperation == null || Progress == null)
                return;
            UpdateProgress();
        }

        private void Initialize()
        {
            if (AsyncOperation == null || Progress == null)
                return;
            _lastProgress = CurrentProgress;
            _progress.Report(_lastProgress);
        }

        private void UpdateProgress()
        {
            if (_lastProgress == _asyncOperation.progress)
                return;
            _lastProgress = CurrentProgress;
            _progress.Report(_lastProgress);
        }
    }
}
