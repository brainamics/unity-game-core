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
        public static void KeepPointingAt(this ITutorialPointer pointer, Func<TutorialPointAtRequest> getRequest, bool stopAfterNoRequestUpdates = true)
        {
            if (pointer is not MonoBehaviour b)
                throw new InvalidOperationException("Pointer is not a MonoBehaviour.");
            b.StartCoroutine(RunLoop());

            IEnumerator RunLoop()
            {
                var request = getRequest();
                pointer.PointAt(request);

                while (request.Visible)
                {
                    yield return null;

                    if (request != pointer.ActiveRequest)
                        break;

                    var newRequest = getRequest();
                    if (request == newRequest)
                    {
                        if (stopAfterNoRequestUpdates)
                            break;
                        continue;
                    }

                    request = newRequest;
                    pointer.PointAt(request);
                }
            }
        }
    }
}
