using Spindelvev.Cache;
using Spindelvev.Infrastructure;

namespace Spindelvev.Startup
{
    public class SpindelvevBootstrapper
    {
        public static TinyIoCContainer Wire()
        {
            var ioc = TinyIoCContainer.Current;
            ioc.Register<ISpindelvevLogger, SpindelvevLogger>().AsSingleton();
            ioc.Register<IListenerConnection, FiddlerConnection>().AsSingleton();
            ioc.Register<IResponseCache, ResponseCache>().AsSingleton();
            ioc.Register<ITrafficListener, TrafficListener>().AsSingleton();
            ioc.AutoRegister(DuplicateImplementationActions.RegisterSingle);
            return ioc;
        }
    }
}
