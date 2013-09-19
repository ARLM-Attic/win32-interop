using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Interop.Core.Helpers;

namespace Interop.Core.GarbageCollection
{
    [ContractClassFor(typeof(IWeakReferencesList<>))]
    public sealed class WeakReferencesListOfTContracts<T> : IWeakReferencesList<T>
        where T : class
    {
// ReSharper disable ValueParameterNotUsed
// ReSharper disable CodeAnnotationAnalyzer
        public WeakReference<T> this[int index]
        {
            get
            {
                ValidationHelper.InOpenInterval(index, "index", 0, Count);
                return default(WeakReference<T>);
            }

            set
            {
                ValidationHelper.InOpenInterval(index, "index", 0, Count);
            }
        }

        public int Alive
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return default(int);
            }
        }

        public int Count
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                return default(int);
            }
        }

        public bool IsReadOnly
        {
            get { return default(bool); }
        }

        public void Add(WeakReference<T> weakReference)
        {
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
        }

        public void Insert(int index, WeakReference<T> weakReference)
        {
            ValidationHelper.InOpenInterval(index, "index", 0, Count);
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
        }

        [Pure]
        public bool Contains(WeakReference<T> weakReference)
        {
            return default(bool);
        }

        [Pure]
        public int IndexOf(WeakReference<T> weakReference)
        {
            Contract.Ensures(Contract.Result<int>() >= -1);
            Contract.Ensures(Contract.Result<int>() < Count);
            return default(int);
        }

        public bool Remove(WeakReference<T> weakReference)
        {
            Contract.Ensures(Contract.Result<bool>() == false || Count == Contract.OldValue(Count) - 1);
            return default(bool);
        }

        public void RemoveAt(int index)
        {
            ValidationHelper.InHalfClosedInterval(index, "index", 0, Count);
            Contract.Ensures(Count == Contract.OldValue(Count) - 1);
        }

        public void Clear()
        {
            Contract.Ensures(Count == 0);
        }

        public IEnumerable<WeakReference<T>> Purge()
        {
            Contract.Ensures(Contract.Result<IEnumerable<WeakReference<T>>>() != null);
            return Enumerable.Empty<WeakReference<T>>();
        }

        public void CopyTo(WeakReference<T>[] array, int index)
        {
            ValidationHelper.NotNull(array, "array");
        }

        [Pure]
        IEnumerator<WeakReference<T>> IEnumerable<WeakReference<T>>.GetEnumerator()
        {
            return Enumerable.Empty<WeakReference<T>>().GetEnumerator();
        }

        [Pure]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Enumerable.Empty<object>().GetEnumerator();
        }

// ReSharper restore CodeAnnotationAnalyzer
// ReSharper restore ValueParameterNotUsed
    }
}