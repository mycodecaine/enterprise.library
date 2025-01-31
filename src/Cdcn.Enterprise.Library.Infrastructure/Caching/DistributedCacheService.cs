using Cdcn.Enterprise.Library.Application.Core.Abstraction.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace Cdcn.Enterprise.Library.Infrastructure.Caching
{
    public class DistributedCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public DistributedCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public bool CacheItemExists(string key)
        {
            var cacheValue =  _cache.GetString(key);
            bool keyExists = cacheValue != null;
            return keyExists;
        }

        public string GetCacheItem(string key)
        {
            return _cache.GetString(key) ?? string.Empty;
        }

        public bool RemoveCacheItem(string key)
        {
            _cache.Remove(key);
            return true;
        }

        public void SetCacheItem(string key, string value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry 
            };
            _cache.SetString(key, value, options);
        }
    }
}
