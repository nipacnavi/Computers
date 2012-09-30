using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Synchronisation
{
    public static class LockFactory
    {
        public static ILock Create(LockType lockType)
        {
            switch (lockType)
            {
                case LockType.NoLock:
                    return new NoLock();

                case LockType.SpinLock:
                    return new SpinLock();

                case LockType.ClrLock:
                    return new ClrLock();

                default:
                    return null;
            }
        }
    }
}
