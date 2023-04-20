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

        public AdKind Kind { get; }

        public string PlacementId { get; set; }

        public string SourceName { get; set; }
    }
}