namespace Spindelvev.Cache
{
    public interface IResponseCache
    {
        void Add(string key, ResponseCacheItem value);
        bool IsCached(string key);
        ResponseCacheItem Get(string key);
    }
}