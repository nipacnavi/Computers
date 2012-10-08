using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tools.Synchronisation;
using SpinLock = Tools.Synchronisation.SpinLock;

namespace Tools_TestProject
{


    /// <summary>
    ///This is a test class for SpinLockTest and is intended
    ///to contain all SpinLockTest Unit Tests
    ///</summary>
    [TestClass]
    public class SpinLockTest
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


        private static Int32 _spinLockTest1Counter;

        [TestMethod]
        public void LockTestLoop()
        {
            const int looNb = 1000000;

            _spinLockTest1Counter = 0;

            var mySpinLock = new SpinLock();
            //var mySpinLock = new NoLock(); // fails with NoLock

            for (var i = 0; i < looNb; i++)
                InnerTest(mySpinLock);

            Thread.Sleep(1000);

            Assert.IsTrue(_spinLockTest1Counter == looNb);
        }

        private static void InnerTest(ILock mySpinLock)
        {
            ThreadPool.QueueUserWorkItem(
                o =>
                    {
                        int copy1, copy2;
                        using (mySpinLock.EnterAndReturnLock())
                        {
                            copy1 = _spinLockTest1Counter;
                            _spinLockTest1Counter++;
                            copy2 = _spinLockTest1Counter;

                        }
                        Assert.IsTrue(copy1 + 1 == copy2);
                    }
            );
        }
    }
}
