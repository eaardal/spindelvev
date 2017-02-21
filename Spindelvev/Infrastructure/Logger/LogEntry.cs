using System;
using System.Linq;

namespace Spindelvev.Infrastructure.Logger
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public LogSeverity Severity { get; set; }
        public string Message { get; set; }
        public object[] PropertyValues { get; set; } = new object[0];

        public override string ToString()
        {
            return PropertyValues.Any() ? string.Format(Message, PropertyValues) : Message;
        }
    }
}