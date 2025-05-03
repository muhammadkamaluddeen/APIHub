using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIHubCore.Interfaces
{
    public interface IRedisCacheService
    {
       public Task<T> GetCacheValueAsync<T>(string key);
       public Task SetCacheValueAsync<T>(string key, T value);
       public  Task RemoveCacheValueAsync(string key);
    }

}
