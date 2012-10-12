using System;

namespace Tools.Cache
{
    public interface ICache<T>
    {
        T Get(Int32 index);
        void Free(Int32 index);
        int Cache(T newObject);
    }
}