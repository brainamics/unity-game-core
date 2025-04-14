using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    public sealed class AnimationPlatformSettings
    {
        public bool AnyPlatform = true;

        public bool Android;

        [InspectorName("iOS")]
        public bool IOS;

        public bool IsApplicable
        {
            get
            {
                if (AnyPlatform)
                    return true;
                return Application.platform switch
                {
                    RuntimePlatform.Android => Android,
                    RuntimePlatform.IPhonePlayer => IOS,
                    _ => true,
                };
            }
        }
    }
}
