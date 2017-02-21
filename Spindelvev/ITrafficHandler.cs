using Fiddler;

namespace Spindelvev
{
    public interface ITrafficHandler
    {
        void HandleResponse(Session session);
    }
}