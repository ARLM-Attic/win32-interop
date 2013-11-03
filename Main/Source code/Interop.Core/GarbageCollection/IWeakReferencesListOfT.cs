using System.Collections.Generic;
using System.Diagnostics.Contracts;

using JetBrains.Annotations;

namespace Interop.Core.GarbageCollection
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [ContractClass(typeof(WeakReferencesListOfTContracts<>))]
    public interface IWeakReferencesList<T> : IList<WeakReference<T>>
        where T : class
    {
        int Alive { get; }

        [NotNull]
        IEnumerable<WeakReference<T>> Purge();
    }
}