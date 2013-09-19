using System;
using System.Collections;

using JetBrains.Annotations;

namespace Interop.Core.GarbageCollection
{
    public interface IWeakCollection : ICollection, IDisposable
    {
        void Add([CanBeNull] object item);

        void Clear();

        bool Contains([CanBeNull] object item);

        void Remove([CanBeNull] object item);

        void Purge();
    }
}