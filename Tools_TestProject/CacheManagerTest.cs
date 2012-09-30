using Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tools.Cache;
using Tools.Synchronisation;

namespace Tools_TestProject
{
    
    
    /// <summary>
    ///This is a test class for CacheManagerTest and is intended
    ///to contain all CacheManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CacheManagerTest
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

        [TestMethod]
        public void CacheTest1()
        {
            var cache = CacheManager<object>.Create(LockType.NoLock);

            var o = new object();

            var i = cache.Cache(o);

            Assert.IsTrue(cache.Get(i).Equals(o));
        }

        [TestMethod]
        public void CacheTest2()
        {
            var cache = CacheManager<object>.Create(LockType.NoLock);

            var o = new object();

            var i = cache.Cache(o);

            cache.Free(i);

            Assert.IsNull(cache.Get(i));
        }

        [TestMethod]
        public void CacheTest3()
        {
            var cache = CacheManager<object>.Create(LockType.NoLock);

            var o = new object();

            var i1 = cache.Cache(o);
            cache.Cache(o);
            cache.Free(i1);
            var i3 = cache.Cache(o);
            Assert.IsTrue(i3 == i1);
        }
    }
}
