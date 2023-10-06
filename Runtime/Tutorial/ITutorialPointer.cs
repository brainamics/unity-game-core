using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    public static class TutorialPointerExtensions
    {
        public static void KeepPointingAt(this ITutorialPointer pointer, Func<TutorialPointAtRequest> getRequest)
        {
            if (pointer is not MonoBehaviour b)
                throw new InvalidOperationException("Pointer is not a MonoBehaviour.");
            b.StartCoroutine(RunLoop());

            IEnumerator RunLoop()
            {
                var request = pointer.ActiveRequest;

                while (true)
                {
                    yield return null;

                    var newRequest = getRequest();
                    if (request == newRequest)
                        break;
                    request = newRequest;
                    pointer.PointAt(request);
                }
            }
        }
    }
}
