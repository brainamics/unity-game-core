using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public interface ITutorialPointer
    {
        TutorialPointAtRequest ActiveRequest { get; }

        SpaceCoordinates Coordinates { get; }

        bool IsVisible { get; }

        bool IsClickDown { get; }

        UnityEvent<ITutorialPointer, TutorialPointAtRequest> OnPoint { get; }

        UnityEvent<ITutorialPointer> OnClickStateChanged { get; }

        void PointAt(TutorialPointAtRequest request);

        void SetClickState(bool down);
    }
}
