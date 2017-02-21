using System.Collections.Generic;
using System.Linq;

namespace Spindelvev
{
    public class ListenerFilter : IListenerFilter
    {
        public bool ApplyFilters => Hostnames.Any() || Routes.Any() || Verbs.Any();

        public IEnumerable<string> Hostnames { get; set; }

        public IEnumerable<string> Routes { get; set; }

        public IEnumerable<string> Verbs { get; set; }

        public ListenerFilter()
        {
            Hostnames = new List<string>();
            Routes = new List<string>();
            Verbs = new List<string>();
        }
    }
}