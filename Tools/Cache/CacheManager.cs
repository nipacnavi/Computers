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
    public class CacheManager<T> : ICache<T>
    {
        private readonly List<T> _objectList = new List<T>();
        private readonly Queue<int> _freedObjects = new Queue<int>();
        private readonly ILock _lock;

        private CacheManager(LockType lockType)
        {
            _lock = LockFactory.Create(lockType);
        }

        public static ICache<T> Create(LockType lockType)
        {
            return new CacheManager<T>(lockType);
        }

        public T Get(Int32 index)
        {
            _lock.Enter();
            using(_lock)
            {
                return _objectList[index];
            }
        }

        public void Free(Int32 index)
        {
            _lock.Enter();
            using (_lock)
            {
                _objectList[index] = default(T);
                _freedObjects.Enqueue(index);
            }            
        }

        public int Cache(T newObject)
        {
            if(null == newObject)
                throw new Exception("Cannot cache null objects");
            int newIndex;
            _lock.Enter();
            using (_lock)
            {
                newIndex = _freedObjects.Count == 0 ? _objectList.Count : _freedObjects.Dequeue();
                if (newIndex == _objectList.Count)
                    _objectList.Add(default(T));
                _objectList[newIndex] = newObject;
            }
            return newIndex;
        }

        public void EmptyCache()
        {
            _lock.Enter();
            using (_lock)
            {
                _objectList.Clear();
                _freedObjects.Clear();
            }
        }
    }
}
