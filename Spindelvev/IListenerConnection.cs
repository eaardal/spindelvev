namespace Spindelvev
{
    public interface IListenerConnection
    {
        event BeforeResponse OnBeforeResponse;

        int Port { get; set; }

        void Connect();
        void Disconnect();
    }
}