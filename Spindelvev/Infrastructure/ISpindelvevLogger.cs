namespace Spindelvev.Infrastructure
{
    public delegate void LoggedMessage(LogEntry logEntry);

    public interface ISpindelvevLogger
    {
        event LoggedMessage LoggedMessage;

        void Verbose(string message, params object[] propertyValues);
        void Info(string message, params object[] propertyValues);
    }
}
