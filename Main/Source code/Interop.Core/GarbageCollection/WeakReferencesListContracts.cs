using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Linq;

using Interop.Core.Helpers;

namespace Interop.Core.GarbageCollection
{
    [ContractClassFor(typeof(IWeakReferencesList))]
    public sealed class WeakReferencesListContracts : IWeakReferencesList
    {
// ReSharper disable ValueParameterNotUsed
// ReSharper disable CodeAnnotationAnalyzer
        public object this[int index]
        {
            get
            {
                ValidationHelper.InOpenInterval(index, "index", 0, Count);
                return default(object);
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

        public bool IsFixedSize
        {
            get { return default(bool); }
        }

        public bool IsSynchronized
        {
            [Pure]
            get
            {
                Contract.Ensures(Contract.Result<bool>());
                return true;
            }
        }

        public object SyncRoot
        {
            [Pure]
            get
            {
// ReSharper disable once AssignNullToNotNullAttribute
                return default(object);
            }
        }

        public int Add(object weakReference)
        {
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            return default(int);
        }

        public void Insert(int index, object weakReference)
        {
            ValidationHelper.InOpenInterval(index, "index", 0, Count);
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
        }

        [Pure]
        public bool Contains(object weakReference)
        {
            return default(bool);
        }

        [Pure]
        public int IndexOf(object weakReference)
        {
            Contract.Ensures(Contract.Result<int>() >= -1);
            Contract.Ensures(Contract.Result<int>() < Count);
            return default(int);
        }

        public void Remove(object weakReference)
        {
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

        public IEnumerable Purge()
        {
            Contract.Ensures(Contract.Result<IEnumerable>() != null);
            return Enumerable.Empty<object>();
        }

        public void CopyTo(Array array, int index)
        {
            ValidationHelper.NotNull(array, "array");
            ValidationHelper.NotMultidimensional(array, "array");
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