namespace Spindelvev
{
    public interface ITrafficListener
    {
        void StartListening(IListenerConnection listenerConnection, IListenerFilter listenerFilter, ITrafficHandler trafficHandler);
        void StopListening(IListenerConnection listenerConnection);
    }
}