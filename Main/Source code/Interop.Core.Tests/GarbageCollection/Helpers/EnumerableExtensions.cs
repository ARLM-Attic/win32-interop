using System;
using System.Collections.Generic;

using Interop.Core.GarbageCollection;

using JetBrains.Annotations;

namespace Interop.Core.Tests.GarbageCollection.Helpers
{
    public static class EnumerableExtensions<T>
        where T : class
    {
        [NotNull]
        public static T[] Enumerate(int count, [NotNull] Func<T> initializer)
        {
            var result = new T[count];
            for (var index = 0; index < count; index++)
            {
                result[index] = initializer();
            }
            return result;
        }

        [NotNull]
        public static Core.GarbageCollection.WeakReference<T>[] EnumerateWeakReferences([NotNull] T[] items)
        {
            var result = new Core.GarbageCollection.WeakReference<T>[items.Length];
            for (var index = 0; index < items.Length; index++)
            {
                result[index] = items[index] == null ? null : new Core.GarbageCollection.WeakReference<T>(items[index]);
            }
            return result;
        }

        [NotNull]
        public static WeakReferencesStorage<T> WeakReferencesStorage([NotNull] IEnumerable<Core.GarbageCollection.WeakReference<T>> weakReferences)
        {
            return new WeakReferencesStorage<T>(weakReferences);
        }
    }
}