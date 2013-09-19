using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Interop.Core.GarbageCollection
{
    public static class WeakExtensions
    {
        [Pure]
        [JetBrains.Annotations.Pure]
        public static IEnumerable<WeakReference<T>> AsAlive<T>(IEnumerable<WeakReference<T>> weakEnumerable, object syncRoot)
            where T : class
        {
            lock (syncRoot)
            {
                foreach (var weakReference in weakEnumerable)
                {
                    if (weakReference.IsAlive)
                    {
                        yield return weakReference;
                    }
                    else
                    {
                        weakReference.Dispose();
                    }
                }
            }
        }

        [Pure]
        [JetBrains.Annotations.Pure]
        public static int AliveCount<T>(IEnumerable<WeakReference<T>> weakEnumerable)
            where T : class
        {
            return weakEnumerable.Count(weakReference => weakReference.IsAlive);
        }

        public static void Purge<T>(ICollection<WeakReference<T>> weakCollection, object syncRoot)
            where T : class
        {
            lock (syncRoot)
            {
                foreach (var weakReference in weakCollection.Where(weakReference => !weakReference.IsAlive))
                {
                    weakCollection.Remove(weakReference);
                    weakReference.Dispose();
                }
            }
        }
    }
}