using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace figma.DAL
{
    public class InMemoryCache : ICacheService
    {
        private IMemoryCache _cache;

        public InMemoryCache(IMemoryCache cache)
        {
            _cache = cache;
        }
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, DateTime time) where T : class
        {
            var item = _cache.Get(cacheKey) as T;
            if (item != null) return item;
            item = getItemCallback();
            _cache.Set(cacheKey, item, time);
            return item;
        }

        public void RemoveCache(string cacheKey)
        {
            _cache.Remove(cacheKey);
        }
    }

    interface ICacheService
    {
        T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, DateTime time) where T : class;
        void RemoveCache(string cacheKey);
    }
}
