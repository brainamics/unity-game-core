using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Brainamics.Core
{
    public static class LoggingServiceExtensions
    {
        public static void LogMessage<TEventType>(this ILoggingService<TEventType> service, LogLevel level, string message, object category = null)
        {
            service.Log(new LogRecord<TEventType>(level, service.UnknownEventType, message, category: category));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Debug<TEventType>(this ILoggingService<TEventType> service, object category, string message)
        {
#if DEBUG
            service.Log(new LogRecord<TEventType>(LogLevel.Debug, service.UnknownEventType, message, category: category));
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Debug<TEventType>(this ILoggingService<TEventType> service, string message)
        {
            service.Debug(null, message);
        }

        public static void Trace<TEventType>(this ILoggingService<TEventType> service, string message, object category = null)
        {
            service.LogMessage(LogLevel.Trace, message, category: category);
        }

        public static void Info<TEventType>(this ILoggingService<TEventType> service, string message, object category = null)
        {
            service.LogMessage(LogLevel.Info, message, category: category);
        }

        public static void Warning<TEventType>(this ILoggingService<TEventType> service, string message, object category = null)
        {
            service.LogMessage(LogLevel.Warning, message, category: category);
        }

        public static void Error<TEventType>(this ILoggingService<TEventType> service, string message, object category = null)
        {
            service.LogMessage(LogLevel.Error, message, category: category);
        }

        public static void Error<TEventType>(this ILoggingService<TEventType> service, Exception exception)
        {
            service.Log(new LogRecord<TEventType>(LogLevel.Error, service.UnknownEventType, exception, exception: exception));
        }

        public static void Critical<TEventType>(this ILoggingService<TEventType> service, string message)
        {
            service.LogMessage(LogLevel.Critical, message);
        }

        public static void Event<TEventType>(this ILoggingService<TEventType> service, TEventType eventType, string message = null, params object[] @params)
        {
            service.Log(new LogRecord<TEventType>(LogLevel.Event, eventType, message, @params));
        }
    }
}
