using System;
using System.IO;
using System.Linq;
using Serilog;

namespace Spindelvev.Infrastructure.Logger
{
    class SpindelvevLogger : ISpindelvevLogger
    {
        public event LoggedMessage LoggedMessage;

        private readonly ILogger _serilog;

        public SpindelvevLogger()
        {
            var logFilePath = GetLogFilePath();

            var loggerConfig = new LoggerConfiguration();
            loggerConfig.MinimumLevel.Verbose();
            loggerConfig.WriteTo.RollingFile(logFilePath, retainedFileCountLimit: 10);
            loggerConfig.WriteTo.ColoredConsole();

            _serilog = loggerConfig.CreateLogger();
        }

        public void Verbose(string message, params object[] propertyValues)
        {
            _serilog.Verbose(message, propertyValues);

            LogMessage(LogSeverity.Verbose, message, propertyValues);
        }

        public void Info(string message, params object[] propertyValues)
        {
            _serilog.Information(message, propertyValues);

            LogMessage(LogSeverity.Info, message, propertyValues);
        }

        public void Debug(string message, params object[] propertyValues)
        {
            _serilog.Debug(message, propertyValues);

            LogMessage(LogSeverity.Debug, message, propertyValues);
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
        private static string GetLogFilePath()
        {
            var spindelvevDir = GetLogDirectory();
            return Path.Combine(spindelvevDir, "Log-{Date}.log");
        }

        private static string GetLogDirectory()
        {
            var programDataDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var spindelvevDir = Path.Combine(programDataDir, "Spindelvev");

            EnsureLogDirectoryExists(spindelvevDir);

            return spindelvevDir;
        }

        private static void EnsureLogDirectoryExists(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}