using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.DAG
{
    static class GraphEdgesExtensions
    {
        private static Graph SubGraph(this Graph graph, Action<int> addToCollectionToExplore, Func<int> nextValue, Func<bool> isEmpty)
        {
            var result = Graph.CreateNewGraph();

            while (!isEmpty())
            {
                var origin = nextValue();
                var targetVertices = graph.GetNumberOfEdgesGoingFrom(origin);
                foreach (var target in targetVertices.Where(target => !result.EdgeAlreadyExists(origin, target)))
                {
                    result.AddEdge(origin, target);
                    addToCollectionToExplore(target);
                }                                
            }

            return result;
        }

        private static IEnumerable<int> GetTopologicalSorting(this Graph graph, Action<int> addToListOfObjectsToExplore, Func<int> nextValue, Func<bool> isEmpty)
        {
            var result = new List<int>();
            while(!isEmpty())
            {
                var origin = nextValue();
                result.Add(origin);
                foreach (var target in graph.GetNumberOfEdgesGoingFrom(origin).ToList())
                {
                    graph.RemoveEdge(origin, target);
                    if (graph.GetNumberOfEdgesGointTo(target) == 0)
                        addToListOfObjectsToExplore(target);
                }                
            }
            return result;
        }

        public static IEnumerable<int> GetTopologicalSorting(this Graph graph, IEnumerable<int> origins)
        {
            var l = new Stack<int>(origins);
            var subGraph = graph.SubGraph(l.Push, l.Pop, () => l.Count == 0);
            l = new Stack<int>(origins);
            var result = subGraph.GetTopologicalSorting(l.Push, l.Pop, () => l.Count == 0);
            if(subGraph.HasHedges)
                throw new Exception("circular reference");
            return result;
        }

        public static IEnumerable<int> GetTopologicalSorting(this Graph graph)
        {
            return graph.TopologicalSorting(graph.StartingPoints);
        }
    }
}