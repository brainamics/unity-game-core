using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public interface IEventsRegistry
    {
        void Register<T>(Action<T> handler);

        void Unregister<T>(Action<T> handler);

        void Invoke<T>(T eventArgs);
    }
}
