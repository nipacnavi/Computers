using System;
using System.Collections.Generic;
using Tools.Synchronisation; 

namespace Tools.Cache
{
    /// <summary>
    /// Thread safe class able to cache objects.
    /// Note1: the Cache method can be called several times with the same object. This class doesn't enforce unicity of cached objects.
    /// Note2: the Cache method throws an exception when called with a null object 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CacheManager<T> : ICacheManager<T>
    {
        private readonly List<Tuple<T, int>> _objectList = new List<Tuple<T, int>>();
        private readonly Queue<int> _freedObjects = new Queue<int>();
        private readonly ILock _lock;

        private CacheManager(LockType lockType)
        {
            _lock = LockFactory.Create(lockType);
        }

        public static ICacheManager<T> Create(LockType lockType)
        {
            return new CacheManager<T>(lockType);
        }

        public T Get(ICacheKey key)
        {
            using(_lock.EnterAndReturnLock())
            {
                var cachedData = _objectList[key.Index];
                if (cachedData.Item2 > key.Version)
                    throw new Exception("in CacheManager.Get: too old version number");
                return _objectList[key.Index].Item1;
            }
        }

        public void Free(ICacheKey key)
        {
            using (_lock.EnterAndReturnLock())
            {
                var cachedData = _objectList[key.Index];
                if (cachedData.Item2 > key.Version)
                    throw new Exception("in CacheManager.Free: too old version number");
                _objectList[key.Index] = new Tuple<T, int>(default(T), cachedData.Item2 + 1);
                _freedObjects.Enqueue(key.Index);
            }
        }

        public ICacheKey Cache(T newObject)
        {
            return Cache(newObject, -1);
        }

        /// <summary>
        /// Cache a new objects with an index that is strictly greater than minIndex
        /// </summary>
        /// <param name="newObject">the object to be cached</param>
        /// <param name="minIndex">minimum index that can be returned</param>
        /// <returns>the cached object's index</returns>
        public ICacheKey Cache(T newObject, int minIndex)
        {
            int newIndex;
            int newVersion;
            using (_lock.EnterAndReturnLock())
            {
                if (_freedObjects.Count != 0 && _freedObjects.Peek() > minIndex)
                {
                    newIndex = _freedObjects.Dequeue();
                    newVersion = _objectList[newIndex].Item2;
                }
                else
                {
                    newIndex = _objectList.Count;
                    newVersion = 0;
                }

                _objectList[newIndex] = new Tuple<T, int>(newObject, newVersion);
            }
            return new CacheKey(newIndex, newVersion);
        }

        public ICache<T> CreateMonotonousCache()
        {
            return new MonotonousCacheView<T>(this);
        }

        public void Clear()
        {
            using (_lock.EnterAndReturnLock())
            {
                _objectList.Clear();
                _freedObjects.Clear();
            }
        }
    }
}
