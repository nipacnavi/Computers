using System;
using System.Collections.Generic;
using DAG;
using Tools;
using Tools.OneToMany;

namespace AgentFactory.Implementation1
{
    using Callback = Action<CacheManager<IData>>;

    class PubSub : IPubSub
    {
        private readonly CacheManager<IData> _dataCache = new CacheManager<IData>();
        private readonly CacheManager<Callback> _callbackCache = new CacheManager<Callback>();
        private readonly IOneToMany _dataToRegisteredCallbacks = OneToMany.Create();
        private readonly Dictionary<int,int> _publishedDataToPublishingCallback = new Dictionary<int, int>();
        private readonly IOneToMany _callbackToData = OneToMany.Create();

        private readonly IGraph _graph = Graph.CreateNewIGraph();

        private void TryAddEdge(int dataId, int callbackId)
        {
            // The callback is called when the data is modified
            _dataToRegisteredCallbacks.AddOneToOne(dataId, callbackId);

            int publishingCallback;
            if (_publishedDataToPublishingCallback.TryGetValue(dataId, out publishingCallback))
            {
                // we have found the id of a callback who publishes dataId when called
                _graph.AddEdge(publishingCallback, callbackId);
            }
        }

        public int CreateRegistration(int dataId, Callback callback)
        {
            // Creates a callback id. Note: we don't check whether the callback is already cached
            //TODO: have a thread check that the callback is note already IDed
            var callbackId = _callbackCache.Cache(callback);

            TryAddEdge(dataId, callbackId);

            return callbackId;
        }

        public int CreateRegistration(IEnumerable<int> dataIds, Callback callback)
        {
            if(dataIds == null)
                throw new Exception("null data id list");

            // Creates a callback id. Note: we don't check whether the callback is already cached
            //TODO: have a thread check that the callback is note already IDed
            var callbackId = _callbackCache.Cache(callback);

            foreach (var dataId in dataIds)
            {
                TryAddEdge(dataId, callbackId);
            }

            return callbackId;
        }

        public void RevokeRegistration(int registrationId)
        {
            _callbackCache.Free(registrationId);


        }

        public IData CreatePublication(int dataId, int registrationId)
        {
            throw new NotImplementedException();
        }

        public void RevokePublication(int dataId, int registrationId)
        {
            throw new NotImplementedException();
        }
    }
}
