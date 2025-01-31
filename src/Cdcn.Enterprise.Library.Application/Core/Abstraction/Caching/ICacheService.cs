using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.Caching
{
    /// <summary>
    /// Interface for cache service to handle caching operations.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Sets a cache item with the specified key and value, with an optional expiry time.
        /// </summary>
        /// <param name="key">The key of the cache item.</param>
        /// <param name="value">The value of the cache item.</param>
        /// <param name="expiry">The optional expiry time for the cache item.</param>
        void SetCacheItem(string key, string value, TimeSpan? expiry = null);

        /// <summary>
        /// Gets the cache item associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the cache item.</param>
        /// <returns>The value of the cache item.</returns>
        string GetCacheItem(string key);

        /// <summary>
        /// Removes the cache item associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the cache item.</param>
        /// <returns>True if the cache item was removed; otherwise, false.</returns>
        bool RemoveCacheItem(string key);

        /// <summary>
        /// Checks if a cache item exists for the specified key.
        /// </summary>
        /// <param name="key">The key of the cache item.</param>
        /// <returns>True if the cache item exists; otherwise, false.</returns>
        bool CacheItemExists(string key);
    }
}
