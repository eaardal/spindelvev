using System;
using Fiddler;
using Spindelvev.Cache;
using Spindelvev.Infrastructure;

namespace Spindelvev
{
    public class AutoRespondTrafficHandler : ITrafficHandler
    {
        private readonly IResponseCache _responseCache;
        private readonly ISpindelvevLogger _logger;

        public AutoRespondTrafficHandler(IResponseCache responseCache, ISpindelvevLogger logger)
        {
            if (responseCache == null) throw new ArgumentNullException(nameof(responseCache));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _responseCache = responseCache;
            _logger = logger;
        }

        public void HandleResponse(Session session)
        {
            var key = $"{session.RequestMethod}_{session.hostname}_{session.url}";

            _logger.Verbose("{0} {1}", session.RequestMethod, session.url);

            if (IsResponseSuccessful(session))
            {
                _logger.Verbose(">> Response was successful");

                if (!_responseCache.IsCached(key))
                {
                    _logger.Info(">> Response was not previously cached, adding");

                    var responseBodyBytes = new byte[session.responseBodyBytes.Length];
                    session.responseBodyBytes.CopyTo(responseBodyBytes, 0);

                    _responseCache.Add(key, new ResponseCacheItem
                    {
                        ResponseBodyBytes = responseBodyBytes,
                        ResponseCode = session.responseCode
                    });
                }
                else
                {
                    _logger.Verbose(">> Response was previously cached");
                }
            }
            else
            {
                _logger.Verbose(">> Response was not successful");

                if (_responseCache.IsCached(key))
                {
                    _logger.Info(">> Response was previously cached");

                    var cachedSession = _responseCache.Get(key);
                    session.responseCode = cachedSession.ResponseCode;

                    var newResponseBodyBytes = new byte[cachedSession.ResponseBodyBytes.Length];
                    cachedSession.ResponseBodyBytes.CopyTo(newResponseBodyBytes, 0);
                    session.responseBodyBytes = newResponseBodyBytes;

                    _logger.Info(">> Replaced failed response with cached response");
                }
                else
                {
                    _logger.Verbose(">> Response was not previously cached");
                }
            }
        }

        private static bool IsResponseSuccessful(Session session)
        {
            return session.responseCode < 400;
        }
    }
}