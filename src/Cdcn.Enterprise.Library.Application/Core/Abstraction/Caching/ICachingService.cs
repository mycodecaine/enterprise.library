using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.Caching
{
    public interface ICachingService
    {
        void SetCacheItem(string key, string value, TimeSpan? expiry = null);
        string GetCacheItem(string key);
        bool RemoveCacheItem(string key);
        bool CacheItemExists(string key);
    }
}
