using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        private UnityEvent<AdHookParameters, object> _onAdStart = new();

        [SerializeField]
        private UnityEvent<AdHookParameters, bool> _onAdEnd = new();

        [SerializeField]
        private UnityEvent<bool> _onAdAvailabilityChanged = new();

        [SerializeField]
        private bool _logging = true;

        public UnityEvent<bool> OnAdAvailabilityChanged => _onAdAvailabilityChanged;

        public UnityEvent<AdHookParameters, object> OnAdStart => _onAdStart;

        public UnityEvent<AdHookParameters, bool> OnAdEnd => _onAdEnd;

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
            var callbackCalled = false;
            var startEventDispatched = false;

            var presentationMode = GetPresentationMode(@params);

            bool success;
            switch (presentationMode)
            {
                case AdPresentationMode.Exclusive:
                    success = ShowExclusiveAd(@params, HandleResult, out handle);
                    break;
                case AdPresentationMode.Concurrent:
                    success = ShowConcurrentAd(@params, HandleResult, out handle);
                    break;
                default:
                    throw new NotImplementedException($"Starting an ad in the presentation mode '{presentationMode}' is not implemented.");
            }
            DispatchStartEvent();
            if (!success)
                HandleResult(false);
            return success;

            void HandleResult(bool success)
            {
                if (callbackCalled)
                    return;
                callbackCalled = true;

                DispatchStartEvent();
                callback?.Invoke(success);
                OnAdEnd.Invoke(@params, success);
            }

            void DispatchStartEvent()
            {
                if (startEventDispatched)
                    return;
                startEventDispatched = true;
                OnAdStart.Invoke(@params, handle);
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

        private bool ShowExclusiveAd(AdHookParameters @params, Action<bool> callback, out object handle)
        {
            RejectCurrentExclusiveHook();
            _exclusiveHookCallback = callback;
            _exclusiveHookParams = @params;
            _exclusiveAdHooked = true;
            if (!ShowExclusiveAd(out handle))
            {
                Log($"ShowExclusiveAd failed for {@params}.");
                // _exclusiveHookCallback = null;
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
        protected abstract bool ShowExclusiveAd(out object handle);

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

        protected void RejectCurrentExclusiveHook([CallerMemberName] string callerName = null)
        {
            Log($"RejectCurrentExclusiveHook caller={callerName} state={StateToString()}");
            if (!_exclusiveAdHooked)
                return;
            ClearCurrentExclusiveHook();
            _exclusiveHookCallback.Invoke(false);
        }

        protected void ApproveCurrentExclusiveHook([CallerMemberName] string callerName = null)
        {
            Log($"ApproveCurrentExclusiveHook caller={callerName} state={StateToString()}");
            if (!_exclusiveAdHooked)
                return;
            ClearCurrentExclusiveHook();
            _exclusiveHookCallback.Invoke(true);
        }

        protected void SetAdAvailability(bool available)
        {
            _onAdAvailabilityChanged.Invoke(available);
        }

        protected void HandleAdClosed()
        {
            _pauseSimulator?.Resume();
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
                Debug.Log($"[AdService] {o}");
        }

        private void OnEnable()
        {
            OnEnableInternal();
        }

        private void OnDestroy()
        {
            OnDestroyInternal();
        }

        private void ClearCurrentExclusiveHook()
        {
            Log("Current exclusive hook clear requested.");
            _exclusiveAdHooked = false;
            _exclusiveHookParams = null;
            HandleAdClosed();
        }

        private string StateToString()
        {
            return $"hooked={_exclusiveAdHooked},params={_exclusiveHookParams},cb={_exclusiveHookCallback}";
        }
    }
}
