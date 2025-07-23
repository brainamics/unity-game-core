using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class AdHookParameters
    {
        public AdHookParameters(AdKind kind = AdKind.Video)
        {
            Kind = kind;
        }

        public AdKind Kind { get; set; }

        public string PlacementId { get; set; }

        public string SourceName { get; set; }

        public AdBannerSize BannerSize { get; set; } = AdBannerSize.Standard;

        public AdBannerPosition BannerPosition { get; set; } = AdBannerPosition.Top;

        public override string ToString()
        {
            return $"{Kind} (placement={PlacementId}) (src={SourceName})";
        }

        public void AssignFrom(AdHookParameters hookParams)
        {
            if (hookParams == null)
                throw new System.ArgumentNullException(nameof(hookParams));

            Kind = hookParams.Kind;
            PlacementId = hookParams.PlacementId;
            SourceName = hookParams.SourceName;
            BannerSize = hookParams.BannerSize;
            BannerPosition = hookParams.BannerPosition;
        }

        public AdHookParameters Clone()
        {
            var clone = new AdHookParameters();
            clone.AssignFrom(this);
            return clone;
        }
    }
}
