using System;
using System.Collections;
using System.Reflection;
using System.Threading;

using Interop.Core.GarbageCollection;
using Interop.Core.Tests.GarbageCollection.Helpers;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Interop.Core.Tests.GarbageCollection
{
    [TestClass]
    public class WeakReferencesStorageTests
    {
        [TestMethod]
        public void TypeSecurity()
        {
            var member = typeof(WeakReferencesStorage<>);
            Assert.IsTrue(member.IsSecurityTransparent);
        }

        private const int WeakReferencesCount = 3;

        private static void Execute<T>([NotNull] Func<T> initializer, [NotNull] Action<WeakReferencesStorage<T>> body, [CanBeNull] Action finalizer = null)
            where T : class
        {
            ExecuteHelper(() =>
            {
                var items = EnumerableExtensions<T>.Enumerate(WeakReferencesCount, initializer);
                var weakReferences = EnumerableExtensions<T>.EnumerateWeakReferences(items);
                var storage = EnumerableExtensions<T>.WeakReferencesStorage(weakReferences);
                body(storage);
                GC.KeepAlive(items);
                return storage;
            }, finalizer);
        }

        private static void WeakExecute<T>([NotNull] Func<T> initializer, [NotNull] Action<WeakReferencesStorage<T>> body, [CanBeNull] Action finalizer = null)
            where T : class
        {
            ExecuteHelper(() =>
            {
                var items = EnumerableExtensions<T>.Enumerate(WeakReferencesCount, initializer);
                var weakReferences = EnumerableExtensions<T>.EnumerateWeakReferences(items);
                var storage = EnumerableExtensions<T>.WeakReferencesStorage(weakReferences);
                body(storage);
                return storage;
            }, finalizer);
        }

        private static void ExecuteHelper<T>([NotNull] Func<WeakReferencesStorage<T>> body, [CanBeNull] Action finalizer = null)
            where T : class
        {
            WeakReferencesStorage<T> storage = null;
            try
            {
                storage = body();
            }
            finally
            {
                if (storage != null)
                {
                    foreach (var weakReference in storage)
                    {
                        if (weakReference != null)
                        {
                            weakReference.Dispose();
                        }
                    }
                }
                if (finalizer != null)
                {
                    finalizer();
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetItemIndexMinimumValidation()
        {
            Execute(() => new object(), storage =>
            {
                var list = (IList)storage;
                var item = list[-1];
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedGetItemIndexMinimumValidation()
        {
            Execute(() => new object(), storage =>
            {
                var item = storage[-1];
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetItemIndexMaximumValidation()
        {
            Execute(() => new object(), storage =>
            {
                var list = (IList)storage;
                var item = list[WeakReferencesCount];
            });
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedGetItemIndexMaximumValidation()
        {
            Execute(() => new object(), storage =>
            {
                var item = storage[WeakReferencesCount];
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetItemIndexMinimumValidation()
        {
            Execute(() => new object(), storage =>
            {
                var list = (IList)storage;
                list[-1] = null;
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedSetItemIndexMinimumValidation()
        {
            Execute(() => new object(), storage =>
            {
                storage[-1] = null;
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetItemIndexMaximumValidation()
        {
            Execute(() => new object(), storage =>
            {
                var list = (IList)storage;
                list[WeakReferencesCount] = null;
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedSetItemIndexMaximumValidation()
        {
            Execute(() => new object(), storage =>
            {
                storage[WeakReferencesCount] = null;
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetItemTypeMatchValidation()
        {
            Execute(() => new object(), storage =>
            {
                var list = (IList)storage;
                list[0] = new object();
            });
        }

        [TestMethod]
        public void SetItemVersionValidation()
        {
            Execute(() => new object(), storage =>
            {
                var storageType = typeof(WeakReferencesStorage<object>);
                var versionProperty = storageType.GetProperty("Version", BindingFlags.NonPublic | BindingFlags.Instance);
                var versionValue = versionProperty.GetValue(storage, null);
                storage[0] = null;
                Assert.AreNotEqual(versionValue, versionProperty.GetValue(storage, null));
            });
        }

        [TestMethod]
        public void IsAliveNullableWork()
        {
            Execute<object>(() => null, storage => Assert.AreEqual(WeakReferencesCount, storage.Alive));
        }

        [TestMethod]
        public void IsAliveWork()
        {
            Execute(() => new object(), storage => Assert.AreEqual(WeakReferencesCount, storage.Alive));
        }

        [TestMethod]
        public void IsAliveAfterCollectionWork()
        {
            if (JetBrains.Profiler.Core.Api.MemoryProfiler.IsActive)
            {
                JetBrains.Profiler.Core.Api.MemoryProfiler.EnableAllocations();
                JetBrains.Profiler.Core.Api.MemoryProfiler.EnableTraffic();
                JetBrains.Profiler.Core.Api.MemoryProfiler.Dump();
            }
            Core.GarbageCollection.WeakReference<MarkedObject> weakReference = null;
            WeakExecute(() => new MarkedObject(), storage =>
            {
                var testObject = new MarkedObject();
                weakReference = new Core.GarbageCollection.WeakReference<MarkedObject>(testObject);
                storage.Add(weakReference);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                if (JetBrains.Profiler.Core.Api.MemoryProfiler.IsActive)
                {
                    JetBrains.Profiler.Core.Api.MemoryProfiler.Dump();
                }
                Assert.AreEqual(1, storage.Alive);
                GC.KeepAlive(testObject);
            }, () =>
            {
                if (weakReference != null)
                {
                    weakReference.Dispose();
                }
            });
        }

        [TestMethod]
        public void IsAdded()
        {
            Execute(() => new object(), storage =>
            {
                var testObject = new object();
                var weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Add(weakReference);
                Assert.AreEqual(weakReference, storage[WeakReferencesCount]);
                GC.KeepAlive(testObject);
            });
        }

        [TestMethod]
        public void IsAddedStable()
        {
            LinkedObject item = null;
            Execute(() => item = new LinkedObject(item), storage =>
            {
                var testObject = new LinkedObject(storage[WeakReferencesCount - 1].Target);
                storage.Add(new Core.GarbageCollection.WeakReference<LinkedObject>(testObject));
                Assert.IsTrue(storage.IsOrdered());
                GC.KeepAlive(testObject);
            });
        }

        [TestMethod]
        public void AddIndexValidation()
        {
            Execute(() => new object(), storage =>
            {
                var testObject = new object();
                var weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Add(weakReference);
                Assert.AreEqual(WeakReferencesCount, storage.IndexOf(weakReference));
                GC.KeepAlive(testObject);
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTypeMatchValidation()
        {
            Execute(() => new object(), storage =>
            {
                var list = (IList)storage;
                list.Add(new object());
            });
        }

        [TestMethod]
        public void AddCountValidation()
        {
            Execute(() => new object(), storage =>
            {
                var testObject = new object();
                var weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Add(weakReference);
                Assert.AreEqual(WeakReferencesCount + 1, storage.Count);
                GC.KeepAlive(testObject);
            });
        }

        [TestMethod]
        public void AddVersionValidation()
        {
            Execute(() => new object(), storage =>
            {
                var testObject = new object();
                var weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                var storageType = typeof(WeakReferencesStorage<object>);
                var versionProperty = storageType.GetProperty("Version", BindingFlags.NonPublic | BindingFlags.Instance);
                var versionValue = versionProperty.GetValue(storage, null);
                storage.Add(weakReference);
                Assert.AreNotEqual(versionValue, versionProperty.GetValue(storage, null));
                GC.KeepAlive(testObject);
            });
        }

        private const int InsertIndex = 2;

        [TestMethod]
        public void IsInserted()
        {
            Execute(() => new object(), storage =>
            {
                var testObject = new object();
                var weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                Assert.AreEqual(weakReference, storage[InsertIndex]);
                GC.KeepAlive(testObject);
            });
        }

        [TestMethod]
        public void IsInsertStable()
        {
            LinkedObject item = null;
            Execute(() => item = new LinkedObject(item), storage =>
            {
                var testObject = new LinkedObject(storage[InsertIndex - 1].Target);
                var weakReference = new Core.GarbageCollection.WeakReference<LinkedObject>(testObject);
                storage[InsertIndex].Target.Previous = new Core.GarbageCollection.WeakReference<LinkedObject>(testObject);
                storage.Insert(InsertIndex, weakReference);
                Assert.IsTrue(storage.IsOrdered());
                GC.KeepAlive(testObject);
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InsertTypeMatchValidation()
        {
            Execute(() => new object(), storage =>
            {
                var list = (IList)storage;
                list.Insert(InsertIndex, new object());
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertIndexMinimumValidation()
        {
            Execute(() => new object(), storage =>
            {
                var list = (IList)storage;
                list.Insert(-1, null);
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedInsertIndexMinimumValidation()
        {
            Execute(() => new object(), storage => storage.Insert(-1, null));
        }

        [TestMethod]
        public void InsertIndexHighestValidation()
        {
            Execute(() => new object(), storage =>
            {
                var list = (IList)storage;
                list.Insert(WeakReferencesCount, null);
            });
        }

        [TestMethod]
        public void TypedInsertIndexHighestValidation()
        {
            Execute(() => new object(), storage => storage.Insert(WeakReferencesCount, null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertIndexMaximumValidation()
        {
            Execute(() => new object(), storage =>
            {
                var list = (IList)storage;
                list.Insert(WeakReferencesCount + 1, null);
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedInsertIndexMaximumValidation()
        {
            Execute(() => new object(), storage => storage.Insert(WeakReferencesCount + 1, null));
        }

        [TestMethod]
        public void InsertCountValidation()
        {
            Execute(() => new object(), storage =>
            {
                var testObject = new object();
                var weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                Assert.AreEqual(WeakReferencesCount + 1, storage.Count);
                GC.KeepAlive(testObject);
            });
        }

        [TestMethod]
        public void InsertVersionValidation()
        {
            Execute(() => new object(), storage =>
            {
                var storageType = typeof(WeakReferencesStorage<object>);
                var versionProperty = storageType.GetProperty("Version", BindingFlags.NonPublic | BindingFlags.Instance);
                var testObject = new object();
                var weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                var versionValue = versionProperty.GetValue(storage, null);
                storage.Insert(InsertIndex, weakReference);
                Assert.AreNotEqual(versionValue, versionProperty.GetValue(storage, null));
                GC.KeepAlive(testObject);
            });
        }

        [TestMethod]
        public void IsContains()
        {
            Execute(() => new object(), storage =>
            {
                var testObject = new object();
                var weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                Assert.IsTrue(storage.Contains(weakReference));
                GC.KeepAlive(testObject);
            });
        }

        [TestMethod]
        public void IsContainsNull()
        {
            Execute(() => new object(), storage =>
            {
                storage.Insert(InsertIndex, null);
                Assert.IsTrue(storage.Contains(null));
            });
        }

        [TestMethod]
        public void IsIndexOfMatch()
        {
            Execute(() => new object(), storage =>
            {
                var testObject = new object();
                var weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                Assert.AreEqual(InsertIndex, storage.IndexOf(weakReference));
                GC.KeepAlive(testObject);
            });
        }

        [TestMethod]
        public void IsIndexOfNotMatch()
        {
            Execute(() => new object(), storage =>
            {
                var testObject = new object();
                var weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                Assert.AreEqual(-1, storage.IndexOf(weakReference));
                GC.KeepAlive(testObject);
            });
        }

        [TestMethod]
        public void IsRemoved()
        {
            Core.GarbageCollection.WeakReference<object> weakReference = null;
            Execute(() => new object(), storage =>
            {
                var testObject = new object();
                weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                var isRemoved = storage.Remove(weakReference);
                Assert.IsTrue(isRemoved && !storage.Contains(weakReference));
                GC.KeepAlive(testObject);
            }, () =>
            {
                if (weakReference != null)
                {
                    weakReference.Dispose();
                }
            });
        }

        [TestMethod]
        public void RemoveCountValidation()
        {
            Core.GarbageCollection.WeakReference<object> weakReference = null;
            Execute(() => new object(), storage =>
            {
                var testObject = new object();
                weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                storage.Remove(weakReference);
                Assert.AreEqual(WeakReferencesCount, storage.Count);
                GC.KeepAlive(testObject);
            }, () =>
            {
                if (weakReference != null)
                {
                    weakReference.Dispose();
                }
            });
        }

        [TestMethod]
        public void RemoveVersionValidation()
        {
            Core.GarbageCollection.WeakReference<object> weakReference = null;
            Execute(() => new object(), storage =>
            {
                var storageType = typeof(WeakReferencesStorage<object>);
                var versionProperty = storageType.GetProperty("Version", BindingFlags.NonPublic | BindingFlags.Instance);
                var testObject = new object();
                weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                var versionValue = versionProperty.GetValue(storage, null);
                storage.Remove(weakReference);
                Assert.AreNotEqual(versionValue, versionProperty.GetValue(storage, null));
                GC.KeepAlive(testObject);
            }, () =>
            {
                if (weakReference != null)
                {
                    weakReference.Dispose();
                }
            });
        }

        [TestMethod]
        public void IsRemovedAt()
        {
            Core.GarbageCollection.WeakReference<object> weakReference = null;
            Execute(() => new object(), storage =>
            {
                var testObject = new object();
                weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                storage.RemoveAt(InsertIndex);
                Assert.IsFalse(storage.Contains(weakReference));
                GC.KeepAlive(testObject);
            }, () =>
            {
                if (weakReference != null)
                {
                    weakReference.Dispose();
                }
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveAtIndexMinimumValidation()
        {
            Execute(() => new object(), storage =>
            {
                var list = (IList)storage;
                list.RemoveAt(-1);
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedRemoveAtIndexMinimumValidation()
        {
            Execute(() => new object(), storage => storage.RemoveAt(-1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveAtIndexMaximumValidation()
        {
            Execute(() => new object(), storage =>
            {
                var list = (IList)storage;
                list.RemoveAt(WeakReferencesCount);
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedRemoveAtIndexMaximumValidation()
        {
            Execute(() => new object(), storage => storage.RemoveAt(WeakReferencesCount));
        }

        [TestMethod]
        public void RemoveAtCountValidation()
        {
            Core.GarbageCollection.WeakReference<object> weakReference = null;
            Execute(() => new object(), storage =>
            {
                var testObject = new object();
                weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                storage.RemoveAt(InsertIndex);
                Assert.AreEqual(WeakReferencesCount, storage.Count);
                GC.KeepAlive(testObject);
            }, () =>
            {
                if (weakReference != null)
                {
                    weakReference.Dispose();
                }
            });
        }

        [TestMethod]
        public void RemoveAtVersionValidation()
        {
            Core.GarbageCollection.WeakReference<object> weakReference = null;
            Execute(() => new object(), storage =>
            {
                var storageType = typeof(WeakReferencesStorage<object>);
                var versionProperty = storageType.GetProperty("Version", BindingFlags.NonPublic | BindingFlags.Instance);
                var testObject = new object();
                weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                var versionValue = versionProperty.GetValue(storage, null);
                storage.RemoveAt(InsertIndex);
                Assert.AreNotEqual(versionValue, versionProperty.GetValue(storage, null));
                GC.KeepAlive(testObject);
            }, () =>
            {
                if (weakReference != null)
                {
                    weakReference.Dispose();
                }
            });
        }

        [TestMethod]
        public void ClearCountValidation()
        {
            Execute(() => new object(), storage =>
            {
                storage.Clear();
                Assert.AreEqual(0, storage.Count);
            });
        }

        [TestMethod]
        public void ClearVersionValidation()
        {
            Execute(() => new object(), storage =>
            {
                var storageType = typeof(WeakReferencesStorage<object>);
                var versionProperty = storageType.GetProperty("Version", BindingFlags.NonPublic | BindingFlags.Instance);
                var versionValue = versionProperty.GetValue(storage, null);
                storage.Clear();
                Assert.AreNotEqual(versionValue, versionProperty.GetValue(storage, null));
            });
        }
    }
}