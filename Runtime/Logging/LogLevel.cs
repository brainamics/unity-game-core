using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [Flags]
    public enum LogLevel
    {
        None = 0,
        Debug = 1,
        Trace = 2,
        Info = 4,
        Warning = 8,
        Error = 16,
        Critical = 32,

        /// <summary>
        /// The log is intended for another component to catch through the OnLog event.
        /// </summary>
        Event = 64,

        Everything = Debug | Trace | Info | Warning | Error | Critical | Event,
    }
}
