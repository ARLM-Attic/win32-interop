using System;
using System.Collections.Generic;

using Interop.Core.GarbageCollection;

using JetBrains.Annotations;

namespace Interop.Core.Tests.GarbageCollection.Helpers
{
    public static class EnumerableExtensions
    {
        [NotNull]
        public static IEnumerable<T> Enumerate<T>(int count, [NotNull] Func<T> initializer)
            where T : class
        {
            for (var index = 0; index < count; index++)
            {
                yield return initializer();
            }
        }

        [NotNull]
        public static IEnumerable<Core.GarbageCollection.WeakReference<T>> WeakReferences<T>([NotNull] IEnumerable<T> items)
            where T : class
        {
            foreach (var item in items)
            {
                if (item == null)
                {
                    yield return null;
                }
                else
                {
                    yield return new Core.GarbageCollection.WeakReference<T>(item);
                }
            }
        }

        [NotNull]
        public static WeakReferencesStorage<T> WeakReferencesStorage<T>([NotNull] IEnumerable<Core.GarbageCollection.WeakReference<T>> weakReferences)
            where T : class
        {
            return new WeakReferencesStorage<T>(weakReferences);
        }
    }
}