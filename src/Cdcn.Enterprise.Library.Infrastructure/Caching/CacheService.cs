using Cdcn.Enterprise.Library.Application.Core.Abstraction.Caching;
using Cdcn.Enterprise.Library.Infrastructure.Caching.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Cdcn.Enterprise.Library.Infrastructure.Caching
{
    public class CacheService : ICacheService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public CacheService(IOptions<CachingSetting> cacheSetting)
        {
            _redis = ConnectionMultiplexer.Connect(cacheSetting.Value.ConnectionString);
            _db = _redis.GetDatabase();
        }

        public void SetCacheItem(string key, string value, TimeSpan? expiry = null)
        {
            _db.StringSet(key, value, expiry);
        }

        public string GetCacheItem(string key)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _db.StringGet(key);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public bool RemoveCacheItem(string key)
        {
            return _db.KeyDelete(key);
        }

        public bool CacheItemExists(string key)
        {
            return _db.KeyExists(key);
        }
    }
}