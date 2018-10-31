using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Core.AOP
{
    public class MemoryCaching : ICaching
    {
        private IMemoryCache _cache;

        public MemoryCaching(IMemoryCache cache)
        {
            this._cache = cache;
        }

        public object Get(string cacheKey)
        {
            return this._cache.Get(cacheKey);
        }

        public void Set(string cacheKey, object cacheValue)
        {
            _cache.Set(cacheKey, cacheValue, TimeSpan.FromSeconds(7200));
        }
    }
}
