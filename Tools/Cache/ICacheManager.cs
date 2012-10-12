using System;

namespace Tools.Cache
{
    public interface ICacheManager<T> : ICache<T>
    {
        ICache<T> CreateMonotonousCache();
        void Clear();
    }
}