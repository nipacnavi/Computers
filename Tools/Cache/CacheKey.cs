using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Cache
{
    struct CacheKey : ICacheKey
    {
        private readonly int _index;
        private readonly int _version;

        public CacheKey(int index, int version)
        {
            _index = index;
            _version = version;
        }

        #region ICacheKey Members

        public int Index
        {
            get { return _index; }
        }

        public int Version
        {
            get { return _version; }
        }

        #endregion
    }
}
