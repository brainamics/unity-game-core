using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public readonly struct LogRecord<TLogEvent>
    {
        public readonly LogLevel Level;
        public readonly TLogEvent Event;
        public readonly object LogObject;
        public readonly object[] Params;
        public readonly Exception Exception;
        public readonly object Category;

        public LogRecord(LogLevel level, TLogEvent @event, object logObject, object[] @params = null, Exception exception = null, object category = null)
        {
            if (level == LogLevel.None)
                throw new ArgumentException($"{level} is not a valid log level.", nameof(level));
            Level = level;
            Event = @event;
            LogObject = logObject;
            Params = @params ?? Array.Empty<object>();
            Exception = exception;
            Category = category;
        }

        public override string ToString()
        {
            return $"[Level={Level}] [Event={Event}] [LogObj={LogObject}] [ParamsLen={Params.Length}] [Exception={Exception}]";
        }
    }
}
