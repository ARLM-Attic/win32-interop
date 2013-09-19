using System.Collections;
using System.Diagnostics.Contracts;

using JetBrains.Annotations;

namespace Interop.Core.GarbageCollection
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [ContractClass(typeof(WeakReferencesListContracts))]
    public interface IWeakReferencesList : IList
    {
        int Alive { get; }

        [NotNull]
        IEnumerable Purge();
    }
}