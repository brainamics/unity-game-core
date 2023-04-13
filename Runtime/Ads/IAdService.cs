using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public interface IAdService
    {
        UnityEvent<bool> OnAdAvailabilityChanged { get; }

        UnityEvent<Action> OnAdShown { get; }

        UnityEvent<bool> OnAdComplete { get; }

        bool IsVideoAvailable { get; }

        bool StartAd(AdHookParameters @params, Action<bool> callback);
    }

    public static class AdServiceExtensions
    {
        public static bool StartAd(this IAdService adService, AdHookParameters @params, Action successCallback)
        {
            return adService.StartAd(@params, success =>
            {
                if (success)
                    successCallback.Invoke();
            });
        }
    }
}