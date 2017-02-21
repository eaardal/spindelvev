using System;
using System.Linq;
using Fiddler;
using Spindelvev.Infrastructure;

namespace Spindelvev
{
    public delegate void BeforeResponse(Session session);

    public class TrafficListener : ITrafficListener
    {
        private readonly ISpindelvevLogger _logger;

        public TrafficListener(ISpindelvevLogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        public void StartListening(IListenerConnection listenerConnection, IListenerFilter listenerFilter, ITrafficHandler trafficHandler)
        {
            _logger.Debug($"Starting to listen using {listenerConnection.GetType().Name}");

            listenerConnection.Connect();
            listenerConnection.OnBeforeResponse += session => OnBeforeResponse(session, listenerFilter, trafficHandler);
        }

        public void StopListening(IListenerConnection listenerConnection)
        {
            _logger.Debug($"Stopping listening on {listenerConnection.GetType().Name}");

            listenerConnection.Disconnect();
        }

        private void OnBeforeResponse(Session session, IListenerFilter filter, ITrafficHandler handler)
        {
            _logger.Verbose("BeforeResponse for {0} {1}", session.RequestMethod, session.url);

            if (filter.ApplyFilters)
            {
                _logger.Verbose("Applying filters. Hostnames: {@Hostnames}, Routes: {@Routes}", filter.Hostnames, filter.Routes);

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
