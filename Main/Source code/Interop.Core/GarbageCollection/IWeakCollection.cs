using System.Collections.Generic;

namespace Interop.Core.GarbageCollection
{
    public interface IWeakCollection<T> : ICollection<T>, IWeakEnumerable<T>
        where T : class
    {
        IEnumerable<WeakReference<T>> References { get; }

        IEnumerable<WeakReference<T>> Live { get; }

        int ReferenceCount { get; }

        void AddReference(WeakReference<T> reference);

        bool ContainsReference(WeakReference<T> reference);

        void CopyReferencesTo(WeakReference<T>[] array, int arrayIndex);

        bool RemoveReference(WeakReference<T> item);

        void Purge();
    }
}