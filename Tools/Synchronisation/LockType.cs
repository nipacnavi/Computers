using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Synchronisation
{
    public enum LockType {
            NoLock = 0,
            SpinLock = 1,
            ClrLock = 2
    }
}
