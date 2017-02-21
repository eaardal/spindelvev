using System.Collections.Generic;

namespace Spindelvev.Infrastructure
{
    public interface IAppConfiguration
    {
        IEnumerable<string> HostnameFilters { get; }
        IEnumerable<string> RouteFilters { get; }
    }
}