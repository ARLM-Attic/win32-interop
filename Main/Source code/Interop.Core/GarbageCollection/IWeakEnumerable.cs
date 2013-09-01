using System;
using System.Collections.Generic;

namespace Interop.Core.GarbageCollection
{
    public interface IWeakEnumerable<T> : IEnumerable<T>, IDisposable
        where T : class
    {
        IEnumerator<WeakReference<T>> GetWeakEnumerator();

        IEnumerator<WeakReference<T>> GetLiveEnumerator();
    }
}