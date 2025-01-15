using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public interface ILoggingService<TEventType>
    {
        UnityEvent<LogRecord<TEventType>> OnLog { get; }
        
        UnityEvent<LogRecord<TEventType>> OnPreviewLog { get; }

        TEventType UnknownEventType { get; }

        bool IsLogSkipped(LogRecord<TEventType> record);

        void Log(LogRecord<TEventType> record);
    }
}
