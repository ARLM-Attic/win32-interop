using System;
using System.Collections;
using System.Collections.Generic;

namespace Interop.Core.GarbageCollection
{
    public interface IWeakCollection<T> : ICollection<T>, IDisposable
        where T : class
    {
        void Purge();
    }
}