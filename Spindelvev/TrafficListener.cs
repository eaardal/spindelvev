using System.Linq;
using Fiddler;

namespace Spindelvev
{
    public delegate void BeforeResponse(Session session);

    public class TrafficListener : ITrafficListener
    {
        public void StartListening(IListenerConnection listenerConnection, IListenerFilter listenerFilter, ITrafficHandler trafficHandler)
        {
            listenerConnection.Connect();
            listenerConnection.OnBeforeResponse += session => OnBeforeResponse(session, listenerFilter, trafficHandler);
        }

        public void StopListening(IListenerConnection listenerConnection)
        {
            listenerConnection.Disconnect();
        }

        private void OnBeforeResponse(Session session, IListenerFilter filter, ITrafficHandler handler)
        {
            if (filter.ApplyFilters)
            {
                if (ShouldFilterHostnameAndRoute(filter))
                {
                    if (MatchesHostnameAndRouteFilter(session, filter))
                    {
                        handler.HandleResponse(session);
                    }
                }
                else if (ShouldFilterOnlyOnHostname(filter))
                {
                    if (ResponseHostnameMatchesAnyFilterHostname(session, filter))
                    {
                        handler.HandleResponse(session);
                    }
                }
                else if (ShouldFilterOnlyOnRoute(filter))
                {
                    if (ResponseRouteMatchesAnyFilterRoute(session, filter))
                    {
                        handler.HandleResponse(session);
                    }
                }
            }
            else
            {
                handler.HandleResponse(session);
            }
        }

        private static bool ShouldFilterHostnameAndRoute(IListenerFilter filter)
        {
            return filter.Hostnames.Any() && filter.Routes.Any();
        }

        private static bool ShouldFilterOnlyOnRoute(IListenerFilter filter)
        {
            return filter.Routes.Any() && !filter.Hostnames.Any();
        }

        private static bool ShouldFilterOnlyOnHostname(IListenerFilter filter)
        {
            return filter.Hostnames.Any() && !filter.Routes.Any();
        }

        private static bool MatchesHostnameAndRouteFilter(Session session, IListenerFilter filter)
        {
            return ResponseHostnameMatchesAnyFilterHostname(session, filter) &&
                   ResponseRouteMatchesAnyFilterRoute(session, filter);
        }
        
        private static bool ResponseRouteMatchesAnyFilterRoute(Session session, IListenerFilter filter)
        {
            return filter.Routes.Any(session.uriContains);
        }

        private static bool ResponseHostnameMatchesAnyFilterHostname(Session session, IListenerFilter filter)
        {
            return filter.Hostnames.Any(hostnameFilter => session.hostname.Contains(hostnameFilter.ToLower()));
        }
    }
}
