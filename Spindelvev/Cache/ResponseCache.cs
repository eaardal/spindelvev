using System.Collections.Concurrent;

namespace Spindelvev.Cache
{
    public class ResponseCache : IResponseCache
    {
        private readonly ConcurrentDictionary<string, ResponseCacheItem> _cache = new ConcurrentDictionary<string, ResponseCacheItem>();

        public void Add(string key, ResponseCacheItem value)
        {
            if (!_cache.ContainsKey(key))
            {
                _cache.AddOrUpdate(key, value, (existingKey, existingValue) => value);
            }
        }

        public bool IsCached(string key)
        {
            return _cache.ContainsKey(key);
        }

        public ResponseCacheItem Get(string key)
        {
            return _cache[key];
        }
    }
}