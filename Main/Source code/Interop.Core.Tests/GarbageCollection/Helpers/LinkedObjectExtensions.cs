using System.Collections.Generic;

using Interop.Core.GarbageCollection;

using JetBrains.Annotations;

namespace Interop.Core.Tests.GarbageCollection.Helpers
{
    public static class LinkedObjectExtensions
    {
        public static bool IsOrdered([NotNull] this IList<WeakReference<LinkedObject>> list)
        {
            var ordered = true;
            for (var index = 1; index < list.Count; index++)
            {
                ordered &= list[index].Target.Previous.Target == list[index - 1].Target;
            }
            return ordered;
        }
    }
}