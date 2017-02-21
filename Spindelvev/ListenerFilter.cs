using System.Collections.Generic;
using System.Linq;

namespace Spindelvev
{
    public class ListenerFilter : IListenerFilter
    {
        public bool ApplyFilters => Hostnames.Any() || Routes.Any();

        public IEnumerable<string> Hostnames { get; set; }

        public IEnumerable<string> Routes { get; set; }
        
        public ListenerFilter()
        {
            Hostnames = new List<string>();
            Routes = new List<string>();
        }
    }
}