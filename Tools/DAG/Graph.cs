using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Tools.OneToMany;

[assembly: InternalsVisibleTo("Tools_TestProject")]

namespace Tools.DAG
{
    public class Graph : IGraph
    {
        private Graph()
        {
        }

        internal static Graph CreateNewGraph()
        {
            return new Graph();
        }

        public static IGraph CreateNewIGraph()
        {
            return CreateNewGraph();
        }

        private readonly IOneToMany _edges = OneToMany.OneToMany.Create();

        private int _numberOfEdges;

        internal bool HasHedges
        {
            get { return _numberOfEdges > 0; }
        }

        public bool AddEdge(int origin, int target)
        {
            var result = _edges.AddOneToOne(origin, target);

            if (result)
                _numberOfEdges++;

            return result;
        }

        internal bool EdgeAlreadyExists(int origin, int target)
        {
            return _edges.AlreadyExists(origin, target);
        }

        public bool RemoveEdge(int origin, int target)
        {
            var result = _edges.RemoveOneToOne(origin, target);

            if (result)
                _numberOfEdges--;

            return result;
        }

        internal int GetNumberOfEdgesGointTo(int target)
        {
            return _edges.HowManyToOne(target);
        }

        internal IEnumerable<int> GetNumberOfEdgesGoingFrom(int origin)
        {
            return _edges.GetManyFromOne(origin);
        }

        internal IEnumerable<int> StartingPoints
        {
            get
            {
                //TODO: the list of starting points can be cached and recomputed only when an edge id added or removed
                return _edges.Origins.Where(origin => GetNumberOfEdgesGointTo(origin) == 0);
            }
        }

        /// <summary>
        /// Returns a topoligical ordered vertex list of the subgraph that starts at origins. If origins == null, returns a calling graph's topologicl order
        /// </summary>
        /// <param name="origins"></param>
        /// <returns></returns>
        public IEnumerable<int> TopologicalSorting(IEnumerable<int> origins)
        {
            return origins == null ? this.GetTopologicalSorting() :  this.GetTopologicalSorting(origins);
        }

        public void RemoveVertex(int vertexIndex)
        {
            if(_edges.HowManyToOne(vertexIndex)>0)
            {
                foreach (var origin in _edges.Origins)
                {
                    RemoveEdge(origin, vertexIndex);
                }    
            }
            
            _edges.RemoveOne(vertexIndex);
        }
    }
}
