//using Microsoft.Extensions.Caching.Memory;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace APIHubCore.Services
//{
//    public class MyService
//    {
//        private readonly IMemoryCache _memoryCache;

//        public MyService(IMemoryCache memoryCache)
//        {
//            _memoryCache = memoryCache;
//        }

//        public void SetCacheItem(string key, object value)
//        {
//            var cacheEntryOptions = new MemoryCacheEntryOptions()
//                .SetAbsoluteExpiration(TimeSpan.FromHours(24)); // Set expiration to 24 hours

//            _memoryCache.Set(key, value, cacheEntryOptions);
//        }

//        public object GetCacheItem(string key)
//        {
//            _memoryCache.TryGetValue(key, out var value);
//            return value;
//        }
//    }


//}
