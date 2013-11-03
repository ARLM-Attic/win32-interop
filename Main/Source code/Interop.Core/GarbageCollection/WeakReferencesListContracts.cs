using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Interop.Core.GarbageCollection
{
    [ContractClassFor(typeof(IWeakReferencesList))]
    internal abstract class WeakReferencesListContracts : IWeakReferencesList
    {
// ReSharper disable CodeAnnotationAnalyzer
        public object this[int index]
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
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsFixedSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        public int Add(object weakReference)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object weakReference)
        {
            throw new NotImplementedException();
        }

        public bool Contains(object weakReference)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object weakReference)
        {
            throw new NotImplementedException();
        }

        public void Remove(object weakReference)
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

        public IEnumerable Purge()
        {
            Contract.Ensures(Contract.Result<IEnumerable>() != null);
            return Enumerable.Empty<object>();
        }

        public void CopyTo(Array array, int index)
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