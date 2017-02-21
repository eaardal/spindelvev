using System;
using System.Configuration;
using Fiddler;

namespace Spindelvev
{
    public class FiddlerConnection : IListenerConnection
    {
        public event BeforeResponse OnBeforeResponse;

        public int Port { get; set; }
        
        public void Connect()
        {
            if (Port == 0)
            {
                throw new ConfigurationErrorsException("Port not set");
            }

            const FiddlerCoreStartupFlags flags = FiddlerCoreStartupFlags.CaptureLocalhostTraffic 
                | FiddlerCoreStartupFlags.MonitorAllConnections
                | FiddlerCoreStartupFlags.OptimizeThreadPool
                | FiddlerCoreStartupFlags.DecryptSSL
                | FiddlerCoreStartupFlags.ChainToUpstreamGateway
                | FiddlerCoreStartupFlags.RegisterAsSystemProxy
                ;

            FiddlerApplication.Startup(Port, flags);
            FiddlerApplication.BeforeResponse += FiddlerApplicationOnBeforeResponse;
        }

        private void FiddlerApplicationOnBeforeResponse(Session session)
        {
            OnBeforeResponse?.Invoke(session);
        }

        public void Disconnect()
        {
            FiddlerApplication.BeforeResponse -= FiddlerApplicationOnBeforeResponse;
            FiddlerApplication.Shutdown();
        }
    }
}