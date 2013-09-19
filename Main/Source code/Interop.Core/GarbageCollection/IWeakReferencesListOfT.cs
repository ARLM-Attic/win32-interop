using System.Collections.Generic;

using JetBrains.Annotations;

namespace Interop.Core.GarbageCollection
{
    public interface IWeakReferencesList<T> : IList<WeakReference<T>>
        where T : class
    {
        int Alive { get; }

        [NotNull]
        IEnumerable<WeakReference<T>> Purge();
    }
}