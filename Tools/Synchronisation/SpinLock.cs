using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Tools.Synchronisation
{
    internal class SpinLock : IDisposable, Tools.Synchronisation.ILock
    {
        private Int32 _resourceInUse; // 0=false (default), 1=true

        public void Enter()
        {
            // Set the resource to in-use and if this thread
            // changed it from Free, then return
            while (Interlocked.Exchange(ref _resourceInUse, 1) != 0)
            {
                Thread.Sleep(0);
            }
        }

        public void Leave()
        {
            // Mark the resource as Free
            Thread.VolatileWrite(ref _resourceInUse, 0);
        }

        public ILock EnterAndReturnLock()
        {
            Enter();
            return this;
        }

        public void Dispose()
        {
            Leave();
        }
    }
}
