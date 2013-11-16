using System;
using System.Collections.Generic;
using System.Linq;

using Interop.Core.GarbageCollection;

using JetBrains.Annotations;

namespace Interop.Core.Tests.GarbageCollection.Helpers
{
    public static class EnumerableExtensions
    {
        [NotNull]
        public static LinkedList<T> Enumerate<T>(int count, [NotNull] Func<T> initializer)
            where T : class
        {
            var list = new LinkedList<T>();
            //var array = new T[count];
            for (var index = 0; index < count; index++)
            {
                list.AddLast(initializer());
                //array[index] = initializer();
            }
            return list;
            //return array;
        }

        [NotNull]
        public static IEnumerable<Core.GarbageCollection.WeakReference<T>> WeakReferences<T>([NotNull] IEnumerable<T> items)
            where T : class
        {
            return items.Select(item => item != null ? new Core.GarbageCollection.WeakReference<T>(item) : null).AsEnumerable();
        }

        [NotNull]
        public static WeakReferencesStorage<T> WeakReferencesStorage<T>([NotNull] IEnumerable<Core.GarbageCollection.WeakReference<T>> weakReferences)
            where T : class
        {
            return new WeakReferencesStorage<T>(weakReferences);
        }
    }
}