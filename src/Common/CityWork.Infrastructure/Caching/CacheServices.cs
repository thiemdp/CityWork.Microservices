using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CityWork.Infrastructure
{
    public class CacheServices : ICacheServices
    {
        private readonly IDistributedCache _cache;

        public CacheServices(IDistributedCache cache)
        {
            _cache = cache;
        }
        public T Get<T>(string key)
        {
            var jsonData =   _cache.GetString(key);
            if (jsonData is null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var jsonData = await _cache.GetStringAsync(key);
            if (jsonData is null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public T GetOrSet<T>(string key, Func<T> addItemFactory)
        {
            T output;
            if (!TryGetValue<T>(key, out output))
            {
                output =   addItemFactory();
                if (output != null)
                {
                      Set<T>(key, output);
                }
            }
            return output;
        }

        public T GetOrSet<T>(string key, Func<T> addItemFactory, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpirationTime = null)
        {
            T output;
            if (!TryGetValue<T>(key, out output))
            {
                output = addItemFactory();
                if (output != null)
                {
                    Set<T>(key, output, absoluteExpireTime,slidingExpirationTime);
                }
            }
            return output;
        }

        public T GetOrSet<T>(string key, Func<T> addItemFactory, DistributedCacheEntryOptions options)
        {
            T output;
            if (!TryGetValue<T>(key, out output))
            {
                output = addItemFactory();
                if (output != null)
                {
                    Set<T>(key, output,options);
                }
            }
            return output;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> addItemFactoryAsync)
        {
            T output;
            if(!TryGetValue<T>(key,out output))
            {
                output = await addItemFactoryAsync();
                if(output != null)
                {
                    await SetAsync<T>(key, output); 
                }    
            }
            return output;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> addItemFactoryAsync, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpirationTime = null)
        {
            T output;
            if (!TryGetValue<T>(key, out output))
            {
                output = await addItemFactoryAsync();
                if (output != null)
                {
                    await SetAsync<T>(key, output,absoluteExpireTime,slidingExpirationTime);
                }
            }
            return output;
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> addItemFactoryAsync, DistributedCacheEntryOptions options)
        {
            T output;
            if (!TryGetValue<T>(key, out output))
            {
                output = await addItemFactoryAsync();
                if (output != null)
                {
                    await SetAsync<T>(key, output, options);
                }
            }
            return output;
        }

        public void Remove(string key)
        {
            ValidateKey(key);
            _cache.Remove(key);
        }

        public async Task RemoveAsync(string key)
        {
            ValidateKey(key);
            await _cache.RemoveAsync(key);
        }

        public void Set<T>(string key, T item, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpirationTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            options.SlidingExpiration = slidingExpirationTime;
            Set<T>(key, item, options);
        }

        public void Set<T>(string key, T item, DistributedCacheEntryOptions options)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            ValidateKey(key);
            var jsonData = JsonSerializer.Serialize(item);
              _cache.SetString(key, jsonData, options);
        }

       
        public async Task SetAsync<T>(string key, T item, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpirationTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60);
            options.SlidingExpiration = slidingExpirationTime;
            await SetAsync<T>(key, item, options);
        }

        public async Task SetAsync<T>(string key, T item, DistributedCacheEntryOptions options)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            ValidateKey(key);
            var jsonData = JsonSerializer.Serialize(item);
            await _cache.SetStringAsync(key, jsonData, options);
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            ValidateKey(key);
            value = Get<T>(key);
            if (value is null || value.Equals(default(T)))
                return false;
            else
                return true;
        }

        protected virtual void ValidateKey(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentOutOfRangeException(nameof(key), "Cache keys cannot be empty or whitespace");
        }
    }
}
