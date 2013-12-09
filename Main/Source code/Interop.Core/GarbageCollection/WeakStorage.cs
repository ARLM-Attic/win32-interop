using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Interop.Helpers;

using JetBrains.Annotations;

namespace Interop.Core.GarbageCollection
{
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    [PublicAPI]
    public class WeakStorage<T> : IWeakCollection<T>, IWeakCollection
        where T : class
    {
        [NotNull]
        private readonly WeakReferencesStorage<T> _weakReferencesStorage;

// ReSharper disable once MemberCanBePrivate.Global
        protected bool Disposed
        {
            [JetBrains.Annotations.Pure]
            [System.Diagnostics.Contracts.Pure]
            get;
            private set;
        }

        #region Constructors

        public WeakStorage()
        {
            _weakReferencesStorage = new WeakReferencesStorage<T>();
            SyncRoot = _weakReferencesStorage.SyncRoot;
        }

        public WeakStorage([CanBeNull] IEnumerable<T> enumerable)
        {
// ReSharper disable PossibleMultipleEnumeration
            ValidationHelper.NotNull(enumerable, "enumerable");
            _weakReferencesStorage = new WeakReferencesStorage<T>(enumerable.Select(item => item != null ? new WeakReference<T>(item) : null));
// ReSharper restore PossibleMultipleEnumeration
            SyncRoot = _weakReferencesStorage.SyncRoot;
        }

        #endregion

        #region Properties

        public int Count
        {
            [JetBrains.Annotations.Pure]
            [System.Diagnostics.Contracts.Pure]
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                lock (SyncRoot)
                {
                    return GetCount();
                }
            }
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        protected virtual int GetCount()
        {
            Contract.Ensures(Contract.Result<int>() >= 0);
            ValidationHelper.NotDisposed(Disposed);
            return _weakReferencesStorage.GetAlive();
        }

        bool ICollection<T>.IsReadOnly
        {
            [JetBrains.Annotations.Pure]
            [System.Diagnostics.Contracts.Pure]
            get
            {
                Contract.Ensures(Contract.Result<bool>() == false);
                return false;
            }
        }

        bool ICollection.IsSynchronized
        {
            [JetBrains.Annotations.Pure]
            [System.Diagnostics.Contracts.Pure]
            get
            {
                Contract.Ensures(Contract.Result<bool>());
                return true;
            }
        }

        public object SyncRoot
        {
            [JetBrains.Annotations.Pure]
            [System.Diagnostics.Contracts.Pure]
            get;
            private set;
        }

        #endregion

        #region Methods

        void IWeakCollection.Add(object item)
        {
            ValidationHelper.OfType<T>(item, "item");
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            lock (SyncRoot)
            {
                AddImpl((T)item);
            }
        }

        public void Add([CanBeNull] T item)
        {
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            lock (SyncRoot)
            {
                AddImpl(item);
            }
        }

        protected virtual void AddImpl([CanBeNull] T item)
        {
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            ValidationHelper.NotDisposed(Disposed);
            _weakReferencesStorage.AddImpl(item != null ? new WeakReference<T>(item) : null);
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        bool IWeakCollection.Contains(object item)
        {
            lock (SyncRoot)
            {
                return ValidationHelper.OfType<T>(item) && ContainsImpl((T)item);
            }
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        public bool Contains([CanBeNull] T item)
        {
            lock (SyncRoot)
            {
                return ContainsImpl(item);
            }
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        protected virtual bool ContainsImpl([CanBeNull] T item)
        {
            ValidationHelper.NotDisposed(Disposed);
            if (item == null)
            {
                return _weakReferencesStorage.ContainsImpl(null);
            }
            var comparer = EqualityComparer<T>.Default;
            for (var index = 0; index < _weakReferencesStorage.GetCount(); index++)
            {
                var weakReference = _weakReferencesStorage.GetItem(index);
                if (weakReference != null && weakReference.IsAlive && comparer.Equals(weakReference.Target, item))
                {
                    return true;
                }
            }
            return false;
        }

        void IWeakCollection.Remove(object item)
        {
            lock (SyncRoot)
            {
                if (ValidationHelper.OfType<T>(item))
                {
                    Remove((T)item);
                }
            }
        }

        public bool Remove([CanBeNull] T item)
        {
            Contract.Ensures(Contract.Result<bool>() == false || Count == Contract.OldValue(Count) - 1);
            lock (SyncRoot)
            {
                return RemoveImpl(item);
            }
        }

        protected virtual bool RemoveImpl([CanBeNull] T item)
        {
            Contract.Ensures(Contract.Result<bool>() == false || Count == Contract.OldValue(Count) - 1);
            ValidationHelper.NotDisposed(Disposed);
            if (item == null)
            {
                return _weakReferencesStorage.RemoveImpl(null);
            }
            var comparer = EqualityComparer<T>.Default;
            for (var index = 0; index < _weakReferencesStorage.GetCount(); index++)
            {
                var weakReference = _weakReferencesStorage.GetItem(index);
                if (weakReference != null && weakReference.IsAlive && comparer.Equals(weakReference.Target, item))
                {
                    _weakReferencesStorage.RemoveAtImpl(index);
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            Contract.Ensures(Count == 0);
            lock (SyncRoot)
            {
                ClearImpl();
            }
        }

        protected virtual void ClearImpl()
        {
            Contract.Ensures(Count == 0);
            ValidationHelper.NotDisposed(Disposed);
            _weakReferencesStorage.Clear();
        }

        public void Purge()
        {
            lock (SyncRoot)
            {
                PurgeImpl();
            }
        }

        protected virtual void PurgeImpl()
        {
            ValidationHelper.NotDisposed(Disposed);
            foreach (var weakReference in _weakReferencesStorage.PurgeImpl().Where(weakReference => weakReference != null))
            {
                weakReference.Dispose();
            }
        }

        void ICollection.CopyTo([CanBeNull] Array array, int index)
        {
            ValidationHelper.NotNull(array, "array");
            ValidationHelper.NotMultidimensional(array, "array");
            lock (SyncRoot)
            {
                CopyToImpl(array, index);
            }
        }

        protected virtual void CopyToImpl([CanBeNull] Array array, int index)
        {
            ValidationHelper.NotNull(array, "array");
            ValidationHelper.NotMultidimensional(array, "array");
            try
            {
                Array.Copy(Items(), 0, array, index, Count);
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("Invalid array type", "array");
            }
        }

        public void CopyTo([CanBeNull] T[] array, int index)
        {
            ValidationHelper.NotNull(array, "array");
            lock (SyncRoot)
            {
                CopyToImpl(array, index);
            }
        }

        protected virtual void CopyToImpl([CanBeNull] T[] array, int index)
        {
            ValidationHelper.NotNull(array, "array");
            Array.Copy(Items(), 0, array, index, GetCount());
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        [NotNull]
        private T[] Items()
        {
// ReSharper disable once PossibleNullReferenceException
            Contract.Ensures(Contract.Result<T[]>() != null && Contract.Result<T[]>().Length == Count);
            var items = new T[Count];
            var currentIndex = 0;
            foreach (var weakReference in _weakReferencesStorage)
            {
                if (weakReference == null)
                {
                    items[currentIndex] = null;
                    currentIndex++;
                }
                else if (weakReference.IsAlive)
                {
                    items[currentIndex] = weakReference.Target;
                    currentIndex++;
                }
            }
            return items;
        }

        #endregion

        #region Implementation of IEnumerable

        [PublicAPI]
        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        public virtual Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Enumerator

        public struct Enumerator : IEnumerator<T>
        {
            private WeakReferencesStorage<T>.Enumerator _weakReferencesEnumerator;

            internal Enumerator([NotNull] WeakStorage<T> storage)
            {
                _weakReferencesEnumerator = storage._weakReferencesStorage.GetEnumerator();
            }

            #region Implementation of IEnumerator

            [CanBeNull]
            public T Current
            {
                [JetBrains.Annotations.Pure]
                [System.Diagnostics.Contracts.Pure]
                get
                {
                    var current = _weakReferencesEnumerator.Current;
                    return current != null ? current.Target : null;
                }
            }

            [CanBeNull]
            object IEnumerator.Current
            {
                get
                {
                    var current = (WeakReference<T>)((IEnumerator)_weakReferencesEnumerator).Current;
                    return current != null ? current.Target : null;
                }
            }

            public bool MoveNext()
            {
                var result = _weakReferencesEnumerator.MoveNext();
                while (result && _weakReferencesEnumerator.Current != null && !_weakReferencesEnumerator.Current.IsAlive)
                {
                    result = _weakReferencesEnumerator.MoveNext();
                }
                return result;
            }

            void IEnumerator.Reset()
            {
                ((IEnumerator)_weakReferencesEnumerator).Reset();
            }

            #endregion

            #region Implementation of IDisposable

            public void Dispose()
            {
                _weakReferencesEnumerator.Dispose();
            }

            #endregion
        }

        #endregion

        #region Destructors

        ~WeakStorage()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (SyncRoot)
            {
                if (Disposed)
                {
                    return;
                }

                if (disposing)
                {
                    for (var index = 0; index < _weakReferencesStorage.GetCount(); index++)
                    {
                        var weakReference = _weakReferencesStorage.GetItem(index);
                        if (weakReference != null)
                        {
                            weakReference.Dispose();
                        }
                    }
                }

                Disposed = true;
            }
        }

        #endregion
    }
}