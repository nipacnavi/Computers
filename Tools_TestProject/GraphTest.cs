using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tools.DAG;

namespace Tools_TestProject
{
    
    
    /// <summary>
    ///This is a test class for GraphTest and is intended
    ///to contain all GraphTest Unit Tests
    ///</summary>
    [TestClass]
    public class GraphTest
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

        private Graph GetTestGraph()
        {
            return GraphEdgesExtensionsTest.CreateTestGraph(GraphEdgesExtensionsTest.CreatePartialOrder());
        }

        /// <summary>
        ///A test for StartingPoints
        ///</summary>
        [TestMethod]
        public void StartingPointsTest()
        {
            var target = GetTestGraph();
            Assert.IsTrue(GraphEdgesExtensionsTest.CompareEnumerable(target.StartingPoints, new[] { 7, 5, 3 }));
        }

        /// <summary>
        ///A test for HasHedges
        ///</summary>
        [TestMethod]
        public void HasHedgesTest1()
        {
            var target = Graph.CreateNewGraph();
            Assert.IsFalse(target.HasHedges);
        }

        /// <summary>
        ///A test for HasHedges
        ///</summary>
        [TestMethod]
        public void HasHedgesTest2()
        {
            var target = Graph.CreateNewGraph();
            target.AddEdge(1,2);
            Assert.IsTrue(target.HasHedges);
        }

        /// <summary>
        ///A test for HasHedges
        ///</summary>
        [TestMethod]
        public void HasHedgesTest3()
        {
            var target = Graph.CreateNewGraph();
            target.AddEdge(1,2);
            target.RemoveEdge(1,2);
            Assert.IsFalse(target.HasHedges);
        }

        /// <summary>
        ///A test for GetIncomingEdgeNumber
        ///</summary>
        [TestMethod]
        public void GetIncomingEdgeNumberTest()
        {
            var target = GetTestGraph();
            Assert.IsTrue(target.GetNumberOfEdgesGointTo(7)==0);
            Assert.IsTrue(target.GetNumberOfEdgesGointTo(5)==0);
            Assert.IsTrue(target.GetNumberOfEdgesGointTo(3)==0);
            Assert.IsTrue(target.GetNumberOfEdgesGointTo(11)==2);
            Assert.IsTrue(target.GetNumberOfEdgesGointTo(8)==2);
            Assert.IsTrue(target.GetNumberOfEdgesGointTo(2)==1);
            Assert.IsTrue(target.GetNumberOfEdgesGointTo(9)==2);
            Assert.IsTrue(target.GetNumberOfEdgesGointTo(10)==2);
        }

        /// <summary>
        ///A test for GetLinkedVertices
        ///</summary>
        [TestMethod]
        public void GetLinkedVerticesTest()
        {
            var target = GetTestGraph();
            Assert.IsTrue(GraphEdgesExtensionsTest.CompareEnumerable(target.GetNumberOfEdgesGoingFrom(11), new[] { 2, 9, 10 }));
        }

        /// <summary>
        ///A test for RemoveEdge
        ///</summary>
        public void RemoveEdgeTest1()
        {
            var target = Graph.CreateNewIGraph();
            Assert.IsFalse(target.RemoveEdge(1,1));
        }

        public void RemoveEdgeTest2()
        {
            var target = Graph.CreateNewIGraph();
            Assert.IsTrue(target.AddEdge(1, 1));
            Assert.IsFalse(target.RemoveEdge(1,2));
        }

        //[TestMethod, ExpectedException(typeof(Exception))]
        public void AddEdgeTest1()
        {
            var target = Graph.CreateNewIGraph();
            Assert.IsTrue(target.AddEdge(1, 1));
            Assert.IsFalse(target.AddEdge(1, 1));
        }
    }
}
