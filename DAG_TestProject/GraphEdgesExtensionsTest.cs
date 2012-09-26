using System;
using System.Linq;
using DAG;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GAD_TestProject2
{
    
    
    /// <summary>
    ///This is a test class for GraphEdgesExtensionsTest and is intended
    ///to contain all GraphEdgesExtensionsTest Unit Tests
    ///</summary>
    [TestClass]
    public class GraphEdgesExtensionsTest
    {

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        /// Create a SubGraph. This Example can be found @ http://en.wikipedia.org/wiki/Topological_sort
        ///</summary>
        internal static Graph CreateTestGraph(IEnumerable<Tuple<int,int>> partialOrder)
        {
            var g = Graph.CreateNewGraph();

            foreach (var tuple in partialOrder)
            {
                g.AddEdge(tuple.Item1, tuple.Item2);
            }

            return g;
        }      

        internal static IEnumerable<Tuple<int,int>> CreatePartialOrder()
        {

            var result = new List<Tuple<int, int>>();
            result.Add(new Tuple<int, int>(7, 11));
            result.Add(new Tuple<int, int>(7, 8));
            result.Add(new Tuple<int, int>(5, 11));
            result.Add(new Tuple<int, int>(3, 8));
            result.Add(new Tuple<int, int>(3, 10));
            result.Add(new Tuple<int, int>(11, 2));
            result.Add(new Tuple<int, int>(11, 9));
            result.Add(new Tuple<int, int>(11, 10));
            result.Add(new Tuple<int, int>(8, 9));

            return result;
        }

        /// <summary>
        ///A test for GetTopologicalSorting
        ///</summary>
        [TestMethod]
        public void GetTopologicalSortingTest1()
        {
            var partialOrder = CreatePartialOrder();
            var graph = CreateTestGraph(partialOrder);
            var topologicalSorting = graph.TopologicalSorting(new[] {3, 7});
            Assert.IsTrue(CompareEnumerable(topologicalSorting, new[] { 3, 7, 8, 11, 10, 9, 2 }));
            Assert.IsTrue(CheckPartialOrder(topologicalSorting, partialOrder));
        }

        /// <summary>
        ///A test for GetTopologicalSorting
        ///</summary>
        [TestMethod]
        public void GetTopologicalSortingTest2()
        {
            var partialOrder = CreatePartialOrder();
            var graph = CreateTestGraph(partialOrder);
            var topologicalSorting = graph.TopologicalSorting(null);
            Assert.IsTrue(CompareEnumerable(topologicalSorting, new[] { 7, 5, 11, 2, 3, 8, 9, 10 }));
            Assert.IsTrue(CheckPartialOrder(topologicalSorting, partialOrder));
        }/// <summary>

        ///A test for GetTopologicalSorting
        ///</summary>
        [TestMethod]
        public void GetTopologicalSortingTest3()
        {
            var partialOrder = CreatePartialOrder();
            var graph = CreateTestGraph(partialOrder);
            graph.RemoveEdge(7,8);
            graph.AddEdge(8,7);
            var topologicalSorting = graph.TopologicalSorting(null);
            Assert.IsFalse(CheckPartialOrder(topologicalSorting, partialOrder));
        }

        [TestMethod, ExpectedException(typeof(Exception))]
        public void GetTopologicalSortingTest4()
        {
            var partialOrder = CreatePartialOrder();
            var graph = CreateTestGraph(partialOrder);
            graph.AddEdge(10, 7); // creates circular reference
            graph.TopologicalSorting(null);
        }

        [TestMethod]
        public void RemoveVertexTest1()
        {
            var partialOrder = CreatePartialOrder();
            var graph = CreateTestGraph(partialOrder);
            graph.RemoveVertex(11);
            Assert.IsTrue(!graph.GetNumberOfEdgesGoingFrom(11).Any());
            Assert.IsTrue(graph.GetNumberOfEdgesGointTo(11) == 0);
            var topologicalSorting = graph.TopologicalSorting(null);
            Assert.IsFalse(topologicalSorting.Contains(11));  
            Assert.IsTrue(CheckPartialOrder(topologicalSorting, partialOrder));
        }

        public static bool CompareEnumerable(IEnumerable<int> e1, IEnumerable<int> e2)
        {
            if (e1 == null || e2 == null)
                return false;

            if (ReferenceEquals(e1, e2))
                return true;

            var l1 = new List<int>(e1);
            var l2 = new List<int>(e2);
             
            return l1.Count == l2.Count && l1.All(l2.Remove);
        }

        public static bool CheckPartialOrder(IEnumerable<int> e1, IEnumerable<Tuple<int,int>> orderedTuples)
        {
            var l1 = e1.ToList();

            foreach (var orderedTuple in orderedTuples)
            {
                bool value2Found = false;
                foreach (var i in l1)
                {
                    if (i == orderedTuple.Item2)
                        value2Found = true;
                    else if (i == orderedTuple.Item1 && value2Found)
                        // I have found item1 but item2 was already seen
                        return false;
                }
            }

            return true;
        }
    }
}
