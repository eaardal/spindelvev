using System;
using System.Linq;

namespace Spindelvev.Infrastructure
{
    class SpindelvevLogger : ISpindelvevLogger
    {
        public event LoggedMessage LoggedMessage;

        public void Verbose(string message, params object[] propertyValues)
        {
            LogMessage(LogSeverity.Verbose, message, propertyValues);
        }

        public void Info(string message, params object[] propertyValues)
        {
            LogMessage(LogSeverity.Info, message, propertyValues);
        }

        private void LogMessage(LogSeverity severity, string message, params object[] propertyValues)
        {
            LoggedMessage?.Invoke(new LogEntry
            {
                Message = message,
                PropertyValues = propertyValues.ToArray(),
                Severity = severity,
                Timestamp = DateTime.Now
            });
        }
    }
}