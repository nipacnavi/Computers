using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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


        /// <summary>
        ///A test for SpinLock
        ///</summary>
        [TestMethod]
        public void SpinLockBenchmark()
        {
            var count = 0;

            Action<Action, Action, Action> loop = (enter, action, leave) =>
                                                      {
                                                          var counter = 1000000;
                                                          while (counter-- > 0)
                                                          {
                                                              enter();
                                                              action();
                                                              leave();
                                                          }
                                                      };

            Func<Action, Action, bool> wait = (enter, leave) =>
                                                  {
                                                      var run = true;
                                                      while (run)
                                                      {
                                                          enter();
                                                          var copy = count;
                                                          Thread.Sleep(0);
                                                          if (copy != count)
                                                              // count was changed in a lock -> impossible!
                                                              return false;
                                                          if (count <= 0)
                                                              run = false;
                                                          leave();
                                                      }
                                                      return true;
                                                  };

            var mySpinLock = new SpinLock();

            count = 1000000;

            ThreadPool.QueueUserWorkItem(o => loop(mySpinLock.Enter, () => count--, mySpinLock.Leave));

            wait(mySpinLock.Enter, mySpinLock.Leave);

            Assert.IsTrue(count == 0);
        }

        private static Int32 _spinLockTest1Counter;

        [TestMethod]
        public void SpinLockTest1()
        {
            SpinLockTest1Loop(Test);
        }

        [TestMethod]
        public void SpinLockTest2()
        {
            SpinLockTest1Loop(TestWithUsing);
        }

        internal void SpinLockTest1Loop(Action<SpinLock> action)
        {
            const int looNb = 1000000;

            _spinLockTest1Counter = 0;

            var mySpinLock = new SpinLock();

            for (var i = 0; i < looNb; i++)
                TestWithUsing(mySpinLock);

            Thread.Sleep(1000);

            Assert.IsTrue(_spinLockTest1Counter == looNb);
        }

        private static void Test(SpinLock mySpinLock)
        {
            mySpinLock.Enter();

            InnerTest(mySpinLock);

            mySpinLock.Leave();
        }

        private static void TestWithUsing(SpinLock mySpinLock)
        {
            mySpinLock.Enter();
            using (mySpinLock)
                InnerTest(mySpinLock);
        }

        private static void InnerTest(SpinLock mySpinLock)
        {
            var copy = _spinLockTest1Counter;

            ThreadPool.QueueUserWorkItem(o =>
            {
                mySpinLock.Enter();
                _spinLockTest1Counter++;
                mySpinLock.Leave();
            });

            Assert.IsTrue(_spinLockTest1Counter == copy);
        }
    }
}
