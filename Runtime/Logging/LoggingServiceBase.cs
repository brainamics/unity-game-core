using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    // [CreateAssetMenu(menuName = "Game/Services/Logging Service")]
    public abstract class LoggingServiceBase<TEventType> : ScriptableObject, ILoggingService<TEventType>
    {
        [SerializeField]
        private UnityEvent<LogRecord<TEventType>> _onLog = new();

        [SerializeField]
        private LogLevel _unityLogTypes = LogLevel.Everything;

        [SerializeField]
        private LogLevel _eventMinimumLogType = LogLevel.Event;

        public UnityEvent<LogRecord<TEventType>> OnLog => _onLog;

        /// <summary>
        /// Gets the log type that represents a generic unknown event.
        /// </summary>
        public abstract TEventType UnknownEventType { get; }

        public virtual bool IsLogSkipped(LogRecord<TEventType> record)
        {
            return false;
        }

        public virtual void Log(LogRecord<TEventType> record)
        {
            if (IsLogSkipped(record))
                return;
            DoUnityLog(record);
            DoEventLog(record);
        }

        /// <summary>
        /// Formats a log record as string.
        /// </summary>
        protected virtual string FormatLog(LogRecord<TEventType> record)
        {
            string message;
            if (record.LogObject == null)
            {
                message = null;
                if (record.Level == LogLevel.Event && record.Params != null && record.Params.Length > 0)
                    message = record.Params[0].ToString();
            }
            else if (record.Params == null || record.Params.Length == 0)
                message = record.LogObject.ToString();
            else
                message = string.Format(record.LogObject.ToString(), record.Params);
            if (Equals(record.Event, UnknownEventType))
                return message;
            return $"[Event: {record.Event}] {message}";
        }

        protected void DoUnityLog(LogRecord<TEventType> record)
        {
            if (record.Level < _unityLogTypes || _unityLogTypes == LogLevel.None)
                return;

            var message = FormatLog(record);
            switch (record.Level)
            {
                case LogLevel.Debug:
                case LogLevel.Trace:
                case LogLevel.Info:
                    Debug.Log(message);
                    break;

                case LogLevel.Warning:
                    Debug.LogWarning(message);
                    break;

                case LogLevel.Error:
                case LogLevel.Critical:
                    Debug.LogError(message);
                    if (record.Exception != null)
                        Debug.LogException(record.Exception);
                    break;

                case LogLevel.Event:
                    Debug.Log($"[Event] {message}");
                    break;

                default:
                    break;
            }
        }

        private void DoEventLog(LogRecord<TEventType> record)
        {
            if (record.Level < _eventMinimumLogType || _eventMinimumLogType == LogLevel.None)
                return;
            try
            {
                _onLog?.Invoke(record);
            } catch (Exception exception) {
                Debug.LogError("A log handler failed when handling: " + record.ToString());
                Debug.LogError(exception);
            }
        }
    }
}
