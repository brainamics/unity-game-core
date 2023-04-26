using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public abstract class AdServiceBase : ScriptableObject, IAdService
    {
        private bool _exclusiveAdHooked;
        private Action<bool> _exclusiveHookCallback;
        private AdHookParameters _exclusiveHookParams;
        private PauseSimulator _pauseSimulator;

        [SerializeField]
        private UnityEvent<Action> _onAdShown = new();

        [SerializeField]
        private UnityEvent<bool> _onAdComplete = new();

        [SerializeField]
        private UnityEvent<bool> _onAdAvailabilityChanged = new();

        [SerializeField]
        private bool _logging = true;

        public UnityEvent<Action> OnAdShown => _onAdShown;

        public UnityEvent<bool> OnAdComplete => _onAdComplete;

        public UnityEvent<bool> OnAdAvailabilityChanged => _onAdAvailabilityChanged;

        public abstract bool IsVideoAvailable { get; }

        public abstract bool IsInterstitialAvailable { get; }

        public abstract bool IsBannerAdAvailable { get; }

        protected AdHookParameters CurrentExclusiveHookParams => _exclusiveHookParams;

        public bool IsExclusiveAdActive => _exclusiveAdHooked;

        public bool StartAd(AdHookParameters @params, Action<bool> callback, out object handle)
        {
            if (@params == null)
                throw new ArgumentNullException(nameof(@params));
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));
            Log($"unity-script: [AdServiceBase] StartAd (placement={@params.PlacementId}, sourceName={@params.SourceName})");

            var presentationMode = GetPresentationMode(@params);

            switch (presentationMode)
            {
                case AdPresentationMode.Exclusive:
                    handle = null;
                    return ShowExclusiveAd(@params, callback);
                case AdPresentationMode.Concurrent:
                    return ShowConcurrentAd(@params, callback, out handle);
                default:
                    throw new NotImplementedException($"Starting an ad in the presentation mode '{presentationMode}' is not implemented.");
            }
        }

        /// <summary>
        /// Cancels a previously started ad.
        /// </summary>
        /// <returns>True either if ad was stopped successfully or already stopped; otherwise, false.</returns>
        public virtual bool Cancel(object handle)
        {
            return false;
        }

        private bool ShowExclusiveAd(AdHookParameters @params, Action<bool> callback)
        {
            RejectCurrentExclusiveHook();
            _exclusiveHookCallback = callback;
            _exclusiveHookParams = @params;
            _exclusiveAdHooked = true;
            if (!ShowExclusiveAd())
            {
                _exclusiveHookCallback = null;
                _exclusiveHookParams = null;
                _exclusiveAdHooked = false;
                return false;
            }
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

        /// <summary>
        /// Gets invoked by <see cref="AdServiceBase"/> to show an ad.
        /// </summary>
        /// <remarks>
        /// Information about the ad request will be accessible via <see cref="CurrentExclusiveHookParams"/>.
        /// </remarks>
        protected abstract bool ShowExclusiveAd();

        /// <summary>
        /// Gets invoked to show a concurrent ad.
        /// </summary>
        /// <returns>A handle by which the ad can be manipulated.</returns>
        /// <exception cref="NotSupportedException">Thrown if the requested ad is not supported by the implementation.</exception>
        protected virtual bool ShowConcurrentAd(AdHookParameters @params, Action<bool> callback, out object handle)
        {
            throw new NotSupportedException();
        }

        protected AdPresentationMode GetPresentationMode(AdHookParameters @params)
        {
            return @params.Kind switch
            {
                AdKind.Video or AdKind.Interstitial => AdPresentationMode.Exclusive,
                AdKind.Banner => AdPresentationMode.Concurrent,
                _ => throw new NotImplementedException($"Unknown ad kind '{@params.Kind}'."),
            };
        }

        protected void RejectCurrentExclusiveHook()
        {
            if (!_exclusiveAdHooked)
                return;
            ClearCurrentExclusiveHook();
            _exclusiveHookCallback.Invoke(false);
        }

        protected void ApproveCurrentExclusiveHook()
        {
            if (!_exclusiveAdHooked)
                return;
            ClearCurrentExclusiveHook();
            _exclusiveHookCallback.Invoke(true);
        }

        protected void SetAdAvailability(bool available)
        {
            _onAdAvailabilityChanged.Invoke(available);
        }

        protected virtual void OnEnableInternal()
        {
        }

        protected virtual void OnDestroyInternal()
        {
        }

        protected void Log(object o)
        {
            if (_logging)
                Debug.Log(o);
        }

        private void ClearCurrentExclusiveHook()
        {
            _exclusiveAdHooked = false;
            _exclusiveHookParams = null;
            _pauseSimulator?.Resume();
        }

        private void OnEnable()
        {
            OnEnableInternal();
        }

        private void OnDestroy()
        {
            OnDestroyInternal();
        }
    }
}
