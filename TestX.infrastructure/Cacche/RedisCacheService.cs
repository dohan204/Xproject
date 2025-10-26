using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestX.application.Repositories;

namespace TestX.infrastructure.Cacche
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _distributedCache.GetStringAsync(key);
            return value != null ? JsonSerializer.Deserialize<T>(value) : default;
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var serialzed = JsonSerializer.Serialize(value);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry
            };
            await _distributedCache.SetStringAsync(key, serialzed, options);
        }
        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }
    }
}
