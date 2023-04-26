using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public interface IAdService
    {
        /// <summary>
        /// Gets whether an ad is currently being played.
        /// </summary>
        bool IsExclusiveAdActive { get; }

        UnityEvent<bool> OnAdAvailabilityChanged { get; }

        UnityEvent<Action> OnAdShown { get; }

        UnityEvent<bool> OnAdComplete { get; }

        bool IsVideoAvailable { get; }

        bool IsInterstitialAvailable { get; }

        bool IsBannerAdAvailable { get; }

        /// <summary>
        /// Attempts to start an ad.
        /// </summary>
        /// <param name="params">The ad hook parameters.</param>
        /// <param name="callback">The callback method that gets called when the ad is either successfully rewarded, or failed.</param>
        /// <param name="handle">The optional handle object that can be used to manipulate the ad. The handle can be NULL even if this method returns true.</param>
        /// <returns>Whether or not the ad was successfully shown.</returns>
        bool StartAd(AdHookParameters @params, Action<bool> callback, out object handle);
    }

    public static class AdServiceExtensions
    {
        public static bool StartAd(this IAdService adService, AdHookParameters @params, Action successCallback)
        {
            return adService.StartAd(@params, success =>
            {
                if (success)
                    successCallback.Invoke();
            }, out _);
        }

        public static bool StartAd(this IAdService adService, AdHookParameters @params, Action successCallback, out object handle)
        {
            return adService.StartAd(@params, success =>
            {
                if (success)
                    successCallback.Invoke();
            }, out handle);
        }
    }
}