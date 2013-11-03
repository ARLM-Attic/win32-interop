using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Interop.Core.GarbageCollection
{
    [ContractClassFor(typeof(IWeakReferencesList<>))]
    internal abstract class WeakReferencesListOfTContracts<T> : IWeakReferencesList<T>
        where T : class
    {
// ReSharper disable CodeAnnotationAnalyzer
        public WeakReference<T> this[int index]
        {
            get { throw new NotImplementedException(); }

            set { throw new NotImplementedException(); }
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
            get { throw new NotImplementedException(); }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public void Add(WeakReference<T> weakReference)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, WeakReference<T> weakReference)
        {
            throw new NotImplementedException();
        }

        public bool Contains(WeakReference<T> weakReference)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(WeakReference<T> weakReference)
        {
            throw new NotImplementedException();
        }

        public bool Remove(WeakReference<T> weakReference)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WeakReference<T>> Purge()
        {
            Contract.Ensures(Contract.Result<IEnumerable<WeakReference<T>>>() != null);
            return Enumerable.Empty<WeakReference<T>>();
        }

        public void CopyTo(WeakReference<T>[] array, int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator<WeakReference<T>> IEnumerable<WeakReference<T>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

// ReSharper restore CodeAnnotationAnalyzer
    }
}