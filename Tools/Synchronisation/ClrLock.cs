using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tools.Synchronisation
{
    internal class ClrLock : ILock
    {
        private readonly object _locker = new object();

        #region ILock Members

        public void Enter()
        {
            Monitor.Enter(_locker);
        }

        public void Leave()
        {
            Monitor.Exit(_locker);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Leave();
        }

        #endregion
    }
}
