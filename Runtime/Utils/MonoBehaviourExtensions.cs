using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class MonoBehaviourExtensions
    {
        public static void RunOnNextFrame(this MonoBehaviour monoBehaviour, Action action)
        {
            monoBehaviour.StartCoroutine(RunNextFrameCoroutine());

            IEnumerator RunNextFrameCoroutine()
            {
                yield return null;
                action();
            }
        }
    }
}