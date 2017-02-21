using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Spindelvev.Infrastructure;
using Spindelvev.Startup;

namespace Spindelvev.Application
{
    class MainWindowViewModel
    {
        public ObservableCollection<LogEntry> LogEntries { get; set; }

        public MainWindowViewModel()
        {
            LogEntries = new ObservableCollection<LogEntry>();

            var ioc = SpindelvevBootstrapper.Wire();
            var logger = ioc.Resolve<ISpindelvevLogger>();

            logger.LoggedMessage += LoggerLoggedMessage;

            var trafficListener = ioc.Resolve<ITrafficListener>();

            var fiddlerConnection = ioc.Resolve<IListenerConnection>();
            fiddlerConnection.Port = 30000;

            var filter = new ListenerFilter
            {
                Hostnames = new List<string> {Environment.MachineName, "localhost"},
                Routes = new List<string> { "dbank" }
            };

            var trafficHandler = ioc.Resolve<ITrafficHandler>();
            trafficListener.StartListening(fiddlerConnection, filter, trafficHandler);
        }

        private void LoggerLoggedMessage(LogEntry logEntry)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                LogEntries.Add(logEntry);
            });
        }
    }
}
