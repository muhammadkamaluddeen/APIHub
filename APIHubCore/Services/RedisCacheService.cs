using APIHubCore.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace APIHubCore.Services
{

    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;

        public RedisCacheService(IDistributedCache cache, IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
        }

        // Method to get data from the Redis cache
        public async Task<T> GetCacheValueAsync<T>(string key)
        {
            try
            {
                var cachedData = await _cache.GetStringAsync(key);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    return JsonSerializer.Deserialize<T>(cachedData);
                }
            }
            catch (Exception ex)
            {

            }

            return default(T); // return null or default if no data found
        }

        // Method to set data in the Redis cache
        public async Task SetCacheValueAsync<T>(string key, T value)
        {
            var serializedData = JsonSerializer.Serialize(value);
           var cacheDuration = Convert.ToInt16(_configuration.GetSection("Redis:CacheDuration").Value);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheDuration) 
            };

            await _cache.SetStringAsync(key, serializedData, options);
        }

        // Method to remove data from the Redis cache
        public async Task RemoveCacheValueAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }

}
