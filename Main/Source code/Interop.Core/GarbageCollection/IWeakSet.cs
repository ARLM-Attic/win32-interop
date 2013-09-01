using System.Collections.Generic;

namespace Interop.Core.GarbageCollection
{
    public interface IWeakSet<T> : ISet<T>, IWeakCollection<T>
        where T : class
    {
        new bool AddReference(WeakReference<T> reference);

        void UnionWithReferences(IEnumerable<WeakReference<T>> other);

        void IntersectWithReferences(IEnumerable<WeakReference<T>> other);

        void ExceptWithReferences(IEnumerable<WeakReference<T>> other);

        void SymmetricExceptWithReferences(IEnumerable<WeakReference<T>> other);

        bool IsSubsetOfReferences(IEnumerable<WeakReference<T>> other);

        bool IsSupersetOfReferences(IEnumerable<WeakReference<T>> other);

        bool IsProperSupersetOfReferences(IEnumerable<WeakReference<T>> other);

        bool IsProperSubsetOfReferences(IEnumerable<WeakReference<T>> other);

        bool OverlapsReferences(IEnumerable<WeakReference<T>> other);

        bool WeakSetEquals(IEnumerable<WeakReference<T>> other);
    }
}