using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public abstract class AdServiceBase : ScriptableObject, IAdService
    {
        private bool _adHooked;
        private Action<bool> _hookCallback;
        private AdHookParameters _hookParams;
        private PauseSimulator _pauseSimulator;

        [SerializeField]
        private UnityEvent<Action> _onAdShown = new();

        [SerializeField]
        private UnityEvent<bool> _onAdComplete = new();

        [SerializeField]
        private UnityEvent<bool> _onAdVisibilityChanged = new();

        public UnityEvent<Action> OnAdShown => _onAdShown;

        public UnityEvent<bool> OnAdComplete => _onAdComplete;

        public UnityEvent<bool> OnAdAvailabilityChanged => _onAdVisibilityChanged;

        public abstract bool IsVideoAvailable { get; }

        protected AdHookParameters CurrentHookParams => _hookParams;

        public bool IsAdActive => _adHooked;

        public bool StartAd(AdHookParameters @params, Action<bool> callback)
        {
            RejectCurrentHook();

            if (!IsVideoAvailable)
                return false;

            _hookCallback = callback;
            _hookParams = @params;
            _adHooked = true;
            ShowAd();
            _pauseSimulator ??= CreatePauseSimulator();
            _pauseSimulator?.Pause();
            return true;
        }

        protected virtual PauseSimulator CreatePauseSimulator()
        {
#if UNITY_IOS
            return new PauseSimulator();
#else
            return null;
#endif
        }

        protected abstract void ShowAd();

        protected void RejectCurrentHook()
        {
            if (!_adHooked)
                return;
            ClearCurrentHook();
            _hookCallback.Invoke(false);
        }

        protected void ApproveCurrentHook()
        {
            if (!_adHooked)
                return;
            ClearCurrentHook();
            _hookCallback.Invoke(true);
        }

        protected void SetAdAvailability(bool available)
        {
            _onAdVisibilityChanged.Invoke(available);
        }

        protected virtual void OnDestroyInternal()
        {
        }

        private void ClearCurrentHook()
        {
            _adHooked = false;
            _hookParams = null;
            _pauseSimulator?.Resume();
        }

        private void OnDestroy()
        {
            OnDestroyInternal();
        }
    }
}
