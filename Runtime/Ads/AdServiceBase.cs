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

        public bool StartAd(AdHookParameters @params, Action<bool> callback)
        {
            RejectCurrentHook();

            if (!IsVideoAvailable)
                return false;

            _hookCallback = callback;
            _hookParams = @params;
            _adHooked = true;
            ShowAd();
            return true;
        }

        protected abstract void ShowAd();

        protected void RejectCurrentHook()
        {
            if (!_adHooked)
                return;
            _adHooked = false;
            _hookParams = null;
            _hookCallback.Invoke(false);
        }

        protected void ApproveCurrentHook()
        {
            if (!_adHooked)
                return;
            _adHooked = false;
            _hookParams = null;
            _hookCallback.Invoke(true);
        }

        protected void SetAdAvailability(bool available)
        {
            _onAdVisibilityChanged.Invoke(available);
        }

        protected virtual void OnDestroyInternal()
        {
        }

        private void OnDestroy()
        {
            OnDestroyInternal();
        }
    }
}