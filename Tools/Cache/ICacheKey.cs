using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools.Cache
{
    public interface ICacheKey
    {
        int Index { get; }
        int Version { get; }
    }
}
