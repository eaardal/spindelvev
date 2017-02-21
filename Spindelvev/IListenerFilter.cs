using System.Collections.Generic;

namespace Spindelvev
{
    public interface IListenerFilter
    {
        bool ApplyFilters { get; }
        IEnumerable<string> Hostnames { get; set; }
        IEnumerable<string> Routes { get; set; }
        IEnumerable<string> Verbs { get; set; }
    }
}