using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class StoreRatingRedirector : MonoBehaviour
    {
        public void ShowRatingDialog()
        {
            var url = GetStoreUrl();
            if (!string.IsNullOrEmpty(url))
                Application.OpenURL(url);
        }

        public string GetStoreUrl()
        {
#if UNITY_ANDROID
            return $"market://details?id={Application.identifier}";
#elif UNITY_IPHONE
        return $"itms-apps://itunes.apple.com/app/id{Application.identifier}";
#else
        return null;
#endif
        }
    }
}