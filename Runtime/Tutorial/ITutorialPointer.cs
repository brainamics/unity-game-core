using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public interface ITutorialPointer
    {
        SpaceCoordinates Coordinates { get; }

        bool IsVisible { get; }

        void PointAt(TutorialPointAtRequest request);

        void SetClickState(bool down);
    }
}
