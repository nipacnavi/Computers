using System;
using System.Collections.Generic;
using Tools.Cache;
using Tools.DAG;
using Tools.Synchronisation;

namespace Tools.PubSub
{
    class PubSub
    {
        private readonly ICache<Action> _cache = CacheManager<Action>.Create(LockType.NoLock);
        private readonly IGraph _graph = Graph.CreateNewIGraph();
        private readonly ILock _lock;

        private PubSub(LockType lockType)
        {
            _lock = LockFactory.Create(lockType);
        }

        public int AddAction(Action action)
        {
            int index;
            using(_lock)
                index = _cache.Cache(action);
            return index;
        }

        public void RemoveAction(int actionIndex)
        {
            using (_lock)
            {
                _cache.Free(actionIndex);
                _graph.RemoveVertex(actionIndex);
            }
        }

        public void LinkActions(int actionIndex1, int actionIndex2)
        {
            using (_lock)
                _graph.AddEdge(actionIndex1, actionIndex2);
        }

        public void UnlinkActions(int actionIndex1, int actionIndex2)
        {
            using (_lock)
                _graph.RemoveEdge(actionIndex1, actionIndex2);
        }

        public void TriggerActions(IEnumerable<int> actionList)
        {
            IEnumerable<int> topologicalSorting;
            using(_lock)
                topologicalSorting = _graph.TopologicalSorting(actionList);

            foreach (var i in topologicalSorting)
            {
                _cache.Get(i)();
            }
        }
    }
}
