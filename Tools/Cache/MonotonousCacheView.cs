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

        public T Get(int index)
        {
            return _cache.Get(index);
        }

        public void Free(int index)
        {
            _cache.Free(index);
        }

        public int Cache(T newObject)
        {
            _minIndex = _cache.Cache(newObject, _minIndex);
            return _minIndex;
        }
    }
}
