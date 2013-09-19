using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using Interop.Core.Helpers;

using JetBrains.Annotations;

namespace Interop.Core.GarbageCollection
{
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    [PublicAPI]
    public class WeakReferencesStorage<T> : IWeakReferencesList<T>, IWeakReferencesList
        where T : class
    {
        #region Private members

        [NotNull]
        private readonly WeakReference<T>[] _emptyWeakReferences = new WeakReference<T>[0];

        [NotNull]
        private WeakReference<T>[] _weakReferences;

        protected virtual long Version
        {
            [JetBrains.Annotations.Pure]
            [System.Diagnostics.Contracts.Pure]
            get
            {
                return _version;
            }
        }

        private long _version = long.MinValue;

        #endregion

        #region Constructors

        public WeakReferencesStorage()
        {
            _weakReferences = _emptyWeakReferences;
            SyncRoot = new object();
        }

        public WeakReferencesStorage([CanBeNull] IEnumerable<WeakReference<T>> enumerable)
        {
// ReSharper disable once PossibleMultipleEnumeration
            ValidationHelper.NotNull(enumerable, "enumerable");

            var collection = enumerable as ICollection<WeakReference<T>>;
            if (collection != null)
            {
                if (collection.Count == 0)
                {
                    _weakReferences = _emptyWeakReferences;
                }
                else
                {
                    _weakReferences = new WeakReference<T>[collection.Count];
                    collection.CopyTo(_weakReferences, 0);
                    _count = collection.Count;
                }
            }
            else
            {
                _weakReferences = _emptyWeakReferences;
                _count = 0;

// ReSharper disable once PossibleMultipleEnumeration
                using (var enumerator = enumerable.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        _count++;
                    }
                    if (_count > 0)
                    {
                        enumerator.Reset();
                        _weakReferences = new WeakReference<T>[_count];
                        for (var index = 0; index < _count; index++)
                        {
                            enumerator.MoveNext();
                            _weakReferences[index] = enumerator.Current;
                        }
                    }
                }
            }
            SyncRoot = new object();
        }

        #endregion

        #region Properties

        [CanBeNull]
        object IList.this[int index]
        {
            get
            {
                ValidationHelper.InOpenInterval(index, "index", 0, _count);
                lock (SyncRoot)
                {
                    return GetItem(index);
                }
            }

            set
            {
                ValidationHelper.InOpenInterval(index, "index", 0, _count);
                ValidationHelper.OfType<WeakReference<T>>(value, "value");
                Contract.Ensures(Version == Contract.OldValue(Version) + 1);
                lock (SyncRoot)
                {
                    SetItem(index, (WeakReference<T>)value);
                }
            }
        }

        [CanBeNull]
        public WeakReference<T> this[int index]
        {
            get
            {
                ValidationHelper.InOpenInterval(index, "index", 0, _count);
                lock (SyncRoot)
                {
                    return GetItem(index);
                }
            }

            set
            {
                ValidationHelper.InOpenInterval(index, "index", 0, _count);
                Contract.Ensures(Version == Contract.OldValue(Version) + 1);
                lock (SyncRoot)
                {
                    SetItem(index, value);
                }
            }
        }

        [CanBeNull]
        protected internal virtual WeakReference<T> GetItem(int index)
        {
            ValidationHelper.InOpenInterval(index, "index", 0, _count);
            return _weakReferences[index];
        }

        protected virtual void SetItem(int index, [CanBeNull] WeakReference<T> weakReference)
        {
            ValidationHelper.InOpenInterval(index, "index", 0, _count);
            Contract.Ensures(Version == Contract.OldValue(Version) + 1);
            _weakReferences[index] = weakReference;
            _version++;
        }

        public int Alive
        {
            [JetBrains.Annotations.Pure]
            [System.Diagnostics.Contracts.Pure]
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);
                lock (SyncRoot)
                {
                    return GetAlive();
                }
            }
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        protected internal virtual int GetAlive()
        {
            Contract.Ensures(Contract.Result<int>() >= 0);
            var result = 0;
            for (var index = 0; index < _count; index++)
            {
                var weakReference = _weakReferences[index];
                if (weakReference == null || weakReference.IsAlive)
                {
                    result++;
                }
            }
            return result;
        }

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

        [ContractPublicPropertyName("Count")]
        private int _count;

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        protected internal virtual int GetCount()
        {
            Contract.Ensures(Contract.Result<int>() >= 0);
            return _count;
        }

        bool ICollection<WeakReference<T>>.IsReadOnly
        {
            [JetBrains.Annotations.Pure]
            [System.Diagnostics.Contracts.Pure]
            get
            {
                Contract.Ensures(Contract.Result<bool>() == false);
                return false;
            }
        }

        bool IList.IsReadOnly
        {
            [JetBrains.Annotations.Pure]
            [System.Diagnostics.Contracts.Pure]
            get
            {
                Contract.Ensures(Contract.Result<bool>() == false);
                return false;
            }
        }

        bool IList.IsFixedSize
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

        int IList.Add([CanBeNull] object weakReference)
        {
            ValidationHelper.OfType<WeakReference<T>>(weakReference, "weakReference");
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            Contract.Ensures(Version == Contract.OldValue(Version) + 1);
            lock (SyncRoot)
            {
                AddImpl((WeakReference<T>)weakReference);
                return _count - 1;
            }
        }

        public void Add([CanBeNull] WeakReference<T> weakReference)
        {
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            Contract.Ensures(Version == Contract.OldValue(Version) + 1);
            lock (SyncRoot)
            {
                AddImpl(weakReference);
            }
        }

        protected internal virtual void AddImpl([CanBeNull] WeakReference<T> weakReference)
        {
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            Contract.Ensures(Version == Contract.OldValue(Version) + 1);
            Resize(_count + 1);
            _weakReferences[_count] = weakReference;
            _count++;
            _version++;
        }

        void IList.Insert(int index, [CanBeNull] object weakReference)
        {
            ValidationHelper.OfType<WeakReference<T>>(weakReference, "weakReference");
            ValidationHelper.InOpenInterval(index, "index", 0, _count);
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            Contract.Ensures(Version == Contract.OldValue(Version) + 1);
            lock (SyncRoot)
            {
                InsertImpl(index, (WeakReference<T>)weakReference);
            }
        }

        public void Insert(int index, [CanBeNull] WeakReference<T> weakReference)
        {
            ValidationHelper.InOpenInterval(index, "index", 0, _count);
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            Contract.Ensures(Version == Contract.OldValue(Version) + 1);
            lock (SyncRoot)
            {
                InsertImpl(index, weakReference);
            }
        }

        protected virtual void InsertImpl(int index, [CanBeNull] WeakReference<T> weakReference)
        {
            ValidationHelper.InOpenInterval(index, "index", 0, _count);
            Contract.Ensures(Count == Contract.OldValue(Count) + 1);
            Contract.Ensures(Version == Contract.OldValue(Version) + 1);
            Resize(0, index, _count + 1, index, _count - index);
            _weakReferences[index] = weakReference;
            _count++;
            _version++;
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        bool IList.Contains([CanBeNull] object weakReference)
        {
            lock (SyncRoot)
            {
                return ValidationHelper.OfType<WeakReference<T>>(weakReference) && ContainsImpl((WeakReference<T>)weakReference);
            }
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        public bool Contains([CanBeNull] WeakReference<T> weakReference)
        {
            lock (SyncRoot)
            {
                return ContainsImpl(weakReference);
            }
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        protected internal virtual bool ContainsImpl([CanBeNull] WeakReference<T> weakReference)
        {
            if (weakReference == null)
            {
                for (var i = 0; i < _count; i++)
                {
                    if (_weakReferences[i] == null)
                    {
                        return true;
                    }
                }
                return false;
            }
            for (var index = 0; index < _count; index++)
            {
                if (WeakReference<T>.Equals(_weakReferences[index], weakReference))
                {
                    return true;
                }
            }
            return false;
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        int IList.IndexOf([CanBeNull] object weakReference)
        {
            Contract.Ensures(Contract.Result<int>() >= -1);
            Contract.Ensures(Contract.Result<int>() < Count);
            lock (SyncRoot)
            {
                return ValidationHelper.OfType<WeakReference<T>>(weakReference) ? IndexOfImpl((WeakReference<T>)weakReference) : -1;
            }
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        public int IndexOf([CanBeNull] WeakReference<T> weakReference)
        {
            Contract.Ensures(Contract.Result<int>() >= -1);
            Contract.Ensures(Contract.Result<int>() < Count);
            lock (SyncRoot)
            {
                return IndexOfImpl(weakReference);
            }
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        protected virtual int IndexOfImpl([CanBeNull] WeakReference<T> weakReference)
        {
            Contract.Ensures(Contract.Result<int>() >= -1);
            Contract.Ensures(Contract.Result<int>() < Count);
            return Array.IndexOf(_weakReferences, weakReference);
        }

        void IList.Remove([CanBeNull] object weakReference)
        {
            lock (SyncRoot)
            {
                if (ValidationHelper.OfType<WeakReference<T>>(weakReference))
                {
                    Remove((WeakReference<T>)weakReference);
                }
            }
        }

        public bool Remove([CanBeNull] WeakReference<T> weakReference)
        {
            Contract.Ensures(Contract.Result<bool>() == false || Count == Contract.OldValue(Count) - 1);
            Contract.Ensures(Contract.Result<bool>() == false || Version == Contract.OldValue(Version) + 1);
            lock (SyncRoot)
            {
                return RemoveImpl(weakReference);
            }
        }

        protected internal virtual bool RemoveImpl([CanBeNull] WeakReference<T> weakReference)
        {
            Contract.Ensures(Contract.Result<bool>() == false || Count == Contract.OldValue(Count) - 1);
            Contract.Ensures(Contract.Result<bool>() == false || Version == Contract.OldValue(Version) + 1);
            var index = IndexOfImpl(weakReference);
            if (index >= 0)
            {
                RemoveAtImpl(index);
                return true;
            }
            return false;
        }

        void IList.RemoveAt(int index)
        {
            ValidationHelper.InHalfClosedInterval(index, "index", 0, _count);
            Contract.Ensures(Count == Contract.OldValue(Count) - 1);
            Contract.Ensures(Version == Contract.OldValue(Version) + 1);
            lock (SyncRoot)
            {
                RemoveAtImpl(index);
            }
        }

        public void RemoveAt(int index)
        {
            ValidationHelper.InHalfClosedInterval(index, "index", 0, _count);
            Contract.Ensures(Count == Contract.OldValue(Count) - 1);
            Contract.Ensures(Version == Contract.OldValue(Version) + 1);
            lock (SyncRoot)
            {
                RemoveAtImpl(index);
            }
        }

        protected internal virtual void RemoveAtImpl(int index)
        {
            ValidationHelper.InHalfClosedInterval(index, "index", 0, _count);
            Contract.Ensures(Count == Contract.OldValue(Count) - 1);
            Contract.Ensures(Version == Contract.OldValue(Version) + 1);
            Resize(0, index, _count - 1, index + 1, _count - index);
            _count--;
            _version++;
        }

        public void Clear()
        {
            Contract.Ensures(Count == 0);
            Contract.Ensures(Version == Contract.OldValue(Version) + 1);
            lock (SyncRoot)
            {
                ClearImpl();
            }
        }

        protected virtual void ClearImpl()
        {
            Contract.Ensures(Count == 0);
            Contract.Ensures(Version == Contract.OldValue(Version) + 1);
            _weakReferences = _emptyWeakReferences;
            _count = 0;
            _version++;
        }

        IEnumerable IWeakReferencesList.Purge()
        {
            Contract.Ensures(Contract.Result<IEnumerable>() != null);
            Contract.Ensures(Count == 0 || Version == Contract.OldValue(Version) + 1);
            lock (SyncRoot)
            {
                return PurgeImpl();
            }
        }

        public IEnumerable<WeakReference<T>> Purge()
        {
            Contract.Ensures(Contract.Result<IEnumerable<WeakReference<T>>>() != null);
            Contract.Ensures(Count == 0 || Version == Contract.OldValue(Version) + 1);
            lock (SyncRoot)
            {
                return PurgeImpl();
            }
        }

        [NotNull]
        protected internal virtual IEnumerable<WeakReference<T>> PurgeImpl()
        {
            Contract.Ensures(Count == 0 || Version == Contract.OldValue(Version) + 1);
            if (_count > 0)
            {
                var index = 0;
                var lastIndex = 0;
                while (index < _count)
                {
                    var weakReference = _weakReferences[index];
                    if (weakReference != null && !weakReference.IsAlive)
                    {
                        yield return weakReference;
                        lastIndex++;
                    }
                    else
                    {
                        _weakReferences[index - lastIndex] = weakReference;
                    }
                    index++;
                }
                var count = lastIndex + 1;
                Resize(count);
                _count = count;
                _version++;
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
                Array.Copy(_weakReferences, 0, array, index, _count);
            }
            catch (ArrayTypeMismatchException)
            {
                throw new ArgumentException("Invalid array type", "array");
            }
        }
        
        public void CopyTo([CanBeNull] WeakReference<T>[] array, int index)
        {
            ValidationHelper.NotNull(array, "array");
            lock (SyncRoot)
            {
                CopyToImpl(array, index);
            }
        }

        protected virtual void CopyToImpl([CanBeNull] WeakReference<T>[] array, int index)
        {
            ValidationHelper.NotNull(array, "array");
            Array.Copy(_weakReferences, 0, array, index, _count);
        }

        private void Resize(int count)
        {
            var length = Math.Min(count, _count);
            var oldWeakReferences = _weakReferences;
            _weakReferences = new WeakReference<T>[count];
            Array.Copy(oldWeakReferences, _weakReferences, length);
        }

        private void Resize(int startIndex, int startCount, int count, int endIndex, int endCount)
        {
            var oldWeakReferences = _weakReferences;
            _weakReferences = new WeakReference<T>[startCount + count + endCount];
            Array.Copy(oldWeakReferences, startIndex, _weakReferences, startIndex + startCount, startCount);
            Array.Copy(oldWeakReferences, endIndex, _weakReferences, endIndex + endCount, endCount);
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
        IEnumerator<WeakReference<T>> IEnumerable<WeakReference<T>>.GetEnumerator()
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

        public struct Enumerator : IEnumerator<WeakReference<T>>
        {
            [NotNull]
            private readonly WeakReferencesStorage<T> _storage;

            private readonly long _version;

            [CanBeNull]
            private WeakReference<T> _current;

            private int _index;

            internal Enumerator([NotNull] WeakReferencesStorage<T> storage)
            {
                _storage = storage;
                _current = null;
                _index = 0;
                _version = storage.Version;
            }

            #region Implementation of IEnumerator

            [CanBeNull]
            public WeakReference<T> Current
            {
                [JetBrains.Annotations.Pure]
                [System.Diagnostics.Contracts.Pure]
                get
                {
                    return _current;
                }
            }

            [CanBeNull]
            object IEnumerator.Current
            {
                get
                {
                    if (_index == 0 || _index == _storage.Count + 1)
                    {
                        throw new InvalidOperationException();
                    }
                    return _current;
                }
            }

            public bool MoveNext()
            {
                if (_version == _storage.Version)
                {
                    if (_index < _storage.Count)
                    {
                        _current = _storage._weakReferences[_index];
                        _index++;
                        return true;
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
                _current = null;
                _index = _storage.Count + 1;
                return false;
            }

            void IEnumerator.Reset()
            {
                if (_version != _storage.Version)
                {
                    throw new InvalidOperationException();
                }
                _current = null;
                _index = 0;
            }

            #endregion

            #region Implementation of IDisposable

            public void Dispose()
            {
            }

            #endregion
        }

        #endregion
    }
}