
using System.Collections.Generic;

namespace Interop.Core.GarbageCollection
{
    public interface IWeakDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, WeakReference<TValue>>>
        where TValue : class
    {
         WeakReference<TValue> this[TKey key, bool onlyLive] { get; set; }
        
        IEnumerable<WeakReference<TValue>> References { get; }

        IEnumerable<WeakReference<TValue>> Live { get; }

        int ReferenceCount { get; }

        void AddReference(TKey key, WeakReference<TValue> reference);

        bool RemoveReference(TKey key, WeakReference<TValue> reference);

        bool TryGetReference(TKey key, out WeakReference<TValue> reference);

        void Purge();
    }
}