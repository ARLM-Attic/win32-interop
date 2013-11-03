using Interop.Core.GarbageCollection;

using JetBrains.Annotations;

namespace Interop.Core.Tests.GarbageCollection.Helpers
{
    public class LinkedObject
    {
        public LinkedObject([CanBeNull] LinkedObject previous)
        {
            Previous = previous != null ? new WeakReference<LinkedObject>(previous) : null;
        }

        [CanBeNull]
        public WeakReference<LinkedObject> Previous { get; set; }
    }
}