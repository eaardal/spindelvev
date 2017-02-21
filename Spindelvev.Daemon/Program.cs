using Spindelvev.Infrastructure;
using Spindelvev.Startup;
using Topshelf;

namespace Spindelvev.Daemon
{
    class Program
    {
        static void Main(string[] args)
        {
            const string serviceName = "Spindelvev";

            var ioc = SpindelvevBootstrapper.Wire();

            var appConfig = ioc.Resolve<IAppConfiguration>();
            var logger = ioc.Resolve<ISpindelvevLogger>();

            HostFactory.Run(hostConfig =>
            {
                hostConfig.Service<ITrafficListener>(serviceConfig =>
                {
                    serviceConfig.ConstructUsing(() => ioc.Resolve<ITrafficListener>());

                    serviceConfig.WhenStarted((service, hostControl) =>
                    {
                        var fiddlerConnection = ioc.Resolve<IListenerConnection>();
                        fiddlerConnection.Port = 30000;

                        var filter = new ListenerFilter
                        {
                            Hostnames = appConfig.HostnameFilters,
                            Routes = appConfig.RouteFilters
                        };

                        var trafficHandler = ioc.Resolve<ITrafficHandler>();

                        service.StartListening(fiddlerConnection, filter, trafficHandler);
                        return true;
                    });

                    serviceConfig.WhenStopped(service =>
                    {
                        var fiddlerConnection = ioc.Resolve<IListenerConnection>();
                        service.StopListening(fiddlerConnection);
                        ioc.Dispose();
                    });

                    serviceConfig.BeforeStartingService(() => logger.Info("Starting {0}", serviceName));
                    serviceConfig.AfterStartingService(() => logger.Info("Started {0}", serviceName));
                    serviceConfig.BeforeStoppingService(() => logger.Info("Stopping {0}", serviceName));
                    serviceConfig.AfterStoppingService(() => logger.Info("Stopped {0}", serviceName));
                });

                hostConfig.RunAsLocalSystem();
                hostConfig.SetDisplayName(serviceName);
                hostConfig.SetServiceName(serviceName);
                hostConfig.SetDisplayName(serviceName);
                hostConfig.StartAutomatically();
            });
        }
    }
}
