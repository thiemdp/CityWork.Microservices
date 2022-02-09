using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWork.Infrastructure
{
    public interface ICacheServices
    {
        void Set<T>(string key, T item, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpirationTime = null);
        void Set<T>(string key, T item, DistributedCacheEntryOptions options);
        Task SetAsync<T>(string key, T item, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpirationTime = null);
        Task SetAsync<T>(string key , T item, DistributedCacheEntryOptions options);
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        bool TryGetValue<T>(string key, out T value);
        T GetOrSet<T>(string key, Func< T> addItemFactory, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpirationTime = null);
        Task<T> GetOrSetAsync<T>(string key, Func< Task<T>> addItemFactory, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpirationTime = null);
        T GetOrSet<T>(string key, Func<T> addItemFactory, DistributedCacheEntryOptions options);
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> addItemFactoryAsync, DistributedCacheEntryOptions options);
        void Remove(string key);
        Task RemoveAsync(string key);
    }
}
