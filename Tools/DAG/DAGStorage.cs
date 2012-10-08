using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.Synchronisation;
using Tools.Cache;

namespace Tools.DAG
{
    class DagStorage<T> : IEnumerable<T>
    {
        class DataContainer
        {
            public DataContainer(T data, int storageIndex)
            {
                Data = data;
                StorageIndex = storageIndex;
            }
            public T Data { get; private set; }
            public int StorageIndex { get; private set; }
        }

        private readonly ICache<DataContainer> _cache = CacheManager<DataContainer>.Create(LockType.NoLock);
        private readonly IGraph _graph = Graph.CreateNewIGraph();
        private readonly ILock _lock;
        private int _storageIndex;

        private DagStorage(LockType lockType)
        {
            _lock = LockFactory.Create(lockType);
        }

        public int AddData(T data)
        {
            int index;
            using (_lock.EnterAndReturnLock())
                index = _cache.Cache(new DataContainer(data, _storageIndex++));
            return index;
        }

        public void RemoveData(int dataIndex)
        {
            using (_lock.EnterAndReturnLock())
            {
                _cache.Free(dataIndex);
                _graph.RemoveVertex(dataIndex);
            }
        }

        public void LinkData(int dataIndex1, int dataIndex2)
        {
            using (_lock.EnterAndReturnLock())
                _graph.AddEdge(dataIndex1, dataIndex2);
        }

        public void UnlinkData(int dataIndex1, int dataIndex2)
        {
            using (_lock.EnterAndReturnLock())
                _graph.RemoveEdge(dataIndex1, dataIndex2);
        }

        //public IEnumerable<T> EnumerateDataInTopologicalOrder(IEnumerable<int> actionList)
        //{
        //    IEnumerable<int> topologicalSorting;
        //    using (_lock)
        //        topologicalSorting = _graph.TopologicalSorting(actionList);

        //    foreach (var i in topologicalSorting)
        //    {
        //        _cache.Get(i)();
        //    }
        //}

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            IEnumerable<int> topologicalSorting;
            using (_lock)
                topologicalSorting = _graph.TopologicalSorting(null);

            return topologicalSorting.Select(i => _cache.Get(i).Data).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
