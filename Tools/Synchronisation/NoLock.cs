﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Synchronisation
{
    internal class NoLock : ILock
    {
        #region ILock Members

        public void Enter()
        {
        }

        public void Leave()
        {
        }

        public ILock EnterAndReturnLock()
        {
            return this;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
