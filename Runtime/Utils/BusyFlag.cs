#if UNITASK
using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
#if PRIMETWEEN
using PrimeTween;
#endif

namespace Brainamics.Core
{
    public sealed class BusyFlag : AsyncCounterFlag
    {
        private readonly Action _decrementCounter;

        public BusyFlag()
        {
            _decrementCounter = DecrementCounter;
        }

        public static UniTask Wait(IEnumerable<AsyncCounterFlag> flags)
        {
            return WaitForAll(flags, false);
        }

        public UniTask WaitForAsync()
            => WaitForAsync(false);

        public System.IDisposable SetBusy()
        {
            Counter++;
            return new CallbackDisposable(_decrementCounter);
        }

        #if PRIMETWEEN
        public void SetBusy(float duration, System.Action callback = null)
        {
            if (duration <= 0)
                return;

            Counter++;
            Tween.Delay(duration, () =>
            {
                DecrementCounter();
                callback?.Invoke();
            });
        }        
        #endif

        private void DecrementCounter()
        {
            Counter--;
        }
    }
}
#endif
