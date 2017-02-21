using System.Collections.Generic;

namespace Spindelvev
{
    class ListenerFilterBuilder
    {
        private readonly List<string> _hostnames = new List<string>();
        private readonly List<string> _routes = new List<string>();

        public ListenerFilterBuilder WithHostname(string hostname)
        {
            _hostnames.Add(hostname);
            return this;
        }

        public ListenerFilterBuilder WithRoute(string route)
        {
            _routes.Add(route);
            return this;
        }

        public IListenerFilter Build()
        {
            return new ListenerFilter
            {
                Hostnames = _hostnames,
                Routes = _routes
            };
        }
    }
}