using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Serializable]
    public sealed class AnimationPlatformSettings : IEquatable<AnimationPlatformSettings>, ISerializationCallbackReceiver
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
        
        public bool Equals(AnimationPlatformSettings other)
        {
            if (other == null)
                return false;

            return AnyPlatform == other.AnyPlatform && Android == other.Android && IOS == other.IOS;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is AnimationPlatformSettings other)
                return Equals(other);
            return false;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(AnyPlatform, Android, IOS);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (AnyPlatform || Android || IOS)
                return;

            AnyPlatform = true;
        }
    }
}
