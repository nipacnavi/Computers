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
        private readonly List<T> _objectList = new List<T>();
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

        public T Get(Int32 index)
        {
            using(_lock.EnterAndReturnLock())
            {
                return _objectList[index];
            }
        }

        public void Free(Int32 index)
        {
            using (_lock.EnterAndReturnLock())
            {
                _objectList[index] = default(T);
                _freedObjects.Enqueue(index);
            }            
        }

        public int Cache(T newObject)
        {
            return Cache(newObject, -1);
        }

        /// <summary>
        /// Cache a new objects with an index that is strictly greater than minIndex
        /// </summary>
        /// <param name="newObject">the object to be cached</param>
        /// <param name="minIndex">minimum index that can be returned</param>
        /// <returns>the cached object's index</returns>
        public int Cache(T newObject, int minIndex)
        {
            if (null == newObject)
                throw new Exception("Cannot cache null objects");
            int newIndex;
            using (_lock.EnterAndReturnLock())
            {
                if (_freedObjects.Count != 0 && _freedObjects.Peek() > minIndex)
                {
                    newIndex = _freedObjects.Dequeue();
                }
                else
                {
                    newIndex = _objectList.Count;
                    _objectList.Add(default(T));
                }

                _objectList[newIndex] = newObject;
            }
            return newIndex;
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
