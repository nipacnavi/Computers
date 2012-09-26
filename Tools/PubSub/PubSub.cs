using System;
using System.Collections.Generic;
using Tools.Cache;
using Tools.DAG;

namespace Tools.PubSub
{
    class PubSub
    {
        private readonly ICache<Action> _cache = CacheManager<Action>.Create();
        private readonly IGraph _graph = Graph.CreateNewIGraph();

        public int AddAction(Action action)
        {
            return _cache.Cache(action);
        }

        public void RemoveAction(int actionIndex)
        {
            _cache.Free(actionIndex);
            _graph.RemoveVertex(actionIndex);
        }

        public void LinkActions(int actionIndex1, int actionIndex2)
        {
            _graph.AddEdge(actionIndex1, actionIndex2);
        }

        public void UnlinkActions(int actionIndex1, int actionIndex2)
        {
            _graph.RemoveEdge(actionIndex1, actionIndex2);
        }

        public void TriggerActions(IEnumerable<int> actionList)
        {
            var l = _graph.TopologicalSorting(actionList);

            foreach (var i in l)
            {
                _cache.Get(i)();
            }
        }
    }
}
