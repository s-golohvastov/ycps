using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace yc.auth
{
    public sealed class AuthCache
    {
        private MemoryCache _cache;
        private static readonly Lazy<AuthCache> _instance = new Lazy<AuthCache>( () => new AuthCache() );

        private AuthCache()
        {
            _cache =  new MemoryCache( new MemoryCacheOptions());
        }
        public static AuthCache Instance { get { return _instance.Value; } }
        public  string GetAuthHeader()
        {
            return GetEntry("AuthHeader");
        }

        public  void AddEntry(string key, string value, DateTimeOffset expiresAt)
        {
            if (!_cache.TryGetValue(key, out _) )
            {
                _cache.Set(key, value, expiresAt);
            }
        }

        public  string GetEntry(string key)
        {
            string value;
            _cache.TryGetValue(key, out value);
            return value;
        }

    }
}