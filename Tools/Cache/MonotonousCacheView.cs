using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Cache
{
    /// <summary>
    /// Cache that returns monotonously increasing indices
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class MonotonousCacheView<T> : ICache<T>
    {
        private readonly CacheManager<T> _cache;
        private int _minIndex = -1;

        public MonotonousCacheView(CacheManager<T> cache)
        {
            _cache = cache;

        }

        public T Get(ICacheKey key)
        {
            return _cache.Get(key);
        }

        public void Free(ICacheKey key)
        {
            _cache.Free(key);
        }

        public ICacheKey Cache(T newObject)
        {
            var key = _cache.Cache(newObject, _minIndex);
            _minIndex = key.Index;
            return key;
        }
    }
}
