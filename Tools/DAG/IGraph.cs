using System.Collections.Generic;

namespace Tools.DAG
{
    public interface IGraph
    {
        bool AddEdge(int origin, int target);
        bool RemoveEdge(int origin, int target);
        IEnumerable<int> TopologicalSorting(IEnumerable<int> origins);
        void RemoveVertex(int vertexIndex);
    }
}