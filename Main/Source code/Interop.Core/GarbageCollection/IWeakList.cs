using System.Collections.Generic;

namespace Interop.Core.GarbageCollection
{
    public interface IWeakList<T> : IList<T>, IWeakCollection<T>
        where T : class
    {
         WeakReference<T> this[int index, bool onlyLive] { get; set; }

        int IndexOfReference(WeakReference<T> item);

        void InsertReference(int index, WeakReference<T> item);

        void RemoveReferenceAt(int index);
    }
}