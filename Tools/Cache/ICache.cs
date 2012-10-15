using System;

namespace Tools.Cache
{
    public interface ICache<T>
    {
        T Get(ICacheKey index);
        void Free(ICacheKey index);
        ICacheKey Cache(T newObject);
    }
}