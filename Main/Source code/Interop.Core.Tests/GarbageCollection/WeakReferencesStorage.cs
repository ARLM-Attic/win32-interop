using System;
using System.Collections;
using System.Linq;
using System.Reflection;

using Interop.Core.GarbageCollection;
using Interop.Core.Tests.GarbageCollection.Helpers;

using JetBrains.Annotations;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Interop.Core.Tests.GarbageCollection
{
    [TestClass]
    public class WeakReferencesStorageTests
    {
#if NETFX
        [TestMethod]
        public void TypeSecurity()
        {
            var member = typeof(WeakReferencesStorage<>);
            Assert.IsTrue(member.IsSecurityTransparent);
        }
#endif

        private const int WeakReferencesCount = 3;

        [NotNull]
        private static object[] Enumerate()
        {
            return EnumerableExtensions.Enumerate(WeakReferencesCount, () => new object());
        }

        [NotNull]
        private static T[] Enumerate<T>([NotNull] Func<T> initializer)
            where T : class
        {
            return EnumerableExtensions.Enumerate(WeakReferencesCount, initializer);
        }

        private static void Execute<T>([NotNull] T[] items, [NotNull] Action<WeakReferencesStorage<T>> body, bool fromCollection = false, bool keepAliveItems = true, [CanBeNull] Action finalizer = null)
            where T : class
        {
            WeakReferencesStorage<T> storage = null;
            try
            {
                var weakReferences = EnumerableExtensions.WeakReferences(items);
                storage = EnumerableExtensions.WeakReferencesStorage(!fromCollection ? weakReferences : weakReferences.ToList());
                body(storage);
                if (keepAliveItems)
                {
                    GC.KeepAlive(items);
                }
            }
            finally
            {
                if (storage != null)
                {
                    foreach (var weakReference in storage)
                    {
                        weakReference.Dispose();
                    }
                }
                if (finalizer != null)
                {
                    finalizer();
                }
            }
        }

        [TestMethod]
        public void IsCollectionCopyingWork()
        {
            Execute(Enumerate(), storage => Assert.IsTrue(storage.Count == WeakReferencesCount), true);
        }

        [TestMethod]
        public void IsEnumerableCopyingWork()
        {
            Execute(Enumerate(), storage => Assert.IsTrue(storage.Count == WeakReferencesCount));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetItemIndexMinimumValidation()
        {
            Execute(Enumerate(), storage =>
            {
                var list = (IList)storage;
                var item = list[-1];
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedGetItemIndexMinimumValidation()
        {
            Execute(Enumerate(), storage =>
            {
                var item = storage[-1];
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetItemIndexMaximumValidation()
        {
            Execute(Enumerate(), storage =>
            {
                var list = (IList)storage;
                var item = list[WeakReferencesCount];
            });
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedGetItemIndexMaximumValidation()
        {
            Execute(Enumerate(), storage =>
            {
                var item = storage[WeakReferencesCount];
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetItemIndexMinimumValidation()
        {
            Execute(Enumerate(), storage =>
            {
                var list = (IList)storage;
                list[-1] = null;
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedSetItemIndexMinimumValidation()
        {
            Execute(Enumerate(), storage =>
            {
                storage[-1] = null;
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SetItemIndexMaximumValidation()
        {
            Execute(Enumerate(), storage =>
            {
                var list = (IList)storage;
                list[WeakReferencesCount] = null;
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedSetItemIndexMaximumValidation()
        {
            Execute(Enumerate(), storage =>
            {
                storage[WeakReferencesCount] = null;
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetItemTypeMatchValidation()
        {
            Execute(Enumerate(), storage =>
            {
                var list = (IList)storage;
                list[0] = new object();
            });
        }

        [TestMethod]
        public void SetItemVersionValidation()
        {
            Execute(Enumerate(), storage =>
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
            Execute(Enumerate<object>(() => null), storage => Assert.AreEqual(WeakReferencesCount, storage.Alive));
        }

        [TestMethod]
        public void IsAliveWork()
        {
            Execute(Enumerate(), storage => Assert.AreEqual(WeakReferencesCount, storage.Alive));
        }

        [TestMethod]
        public void IsAliveAfterCollectionWork()
        {
            Core.GarbageCollection.WeakReference<object> weakReference = null;
            Execute(Enumerate(), storage =>
            {
                var testObject = new object();
                weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Add(weakReference);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                Assert.AreEqual(1, storage.Alive);
                GC.KeepAlive(testObject);
            }, keepAliveItems: false, finalizer: () =>
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
            Execute(Enumerate(), storage =>
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
            Execute(Enumerate(() => item = new LinkedObject(item)), storage =>
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
            Execute(Enumerate(), storage =>
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
            Execute(Enumerate(), storage =>
            {
                var list = (IList)storage;
                list.Add(new object());
            });
        }

        [TestMethod]
        public void AddCountValidation()
        {
            Execute(Enumerate(), storage =>
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
            Execute(Enumerate(), storage =>
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
            Execute(Enumerate(), storage =>
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
            Execute(Enumerate(() => item = new LinkedObject(item)), storage =>
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
            Execute(Enumerate(), storage =>
            {
                var list = (IList)storage;
                list.Insert(InsertIndex, new object());
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertIndexMinimumValidation()
        {
            Execute(Enumerate(), storage =>
            {
                var list = (IList)storage;
                list.Insert(-1, null);
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedInsertIndexMinimumValidation()
        {
            Execute(Enumerate(), storage => storage.Insert(-1, null));
        }

        [TestMethod]
        public void InsertIndexHighestValidation()
        {
            Execute(Enumerate(), storage =>
            {
                var list = (IList)storage;
                list.Insert(WeakReferencesCount, null);
            });
        }

        [TestMethod]
        public void TypedInsertIndexHighestValidation()
        {
            Execute(Enumerate(), storage => storage.Insert(WeakReferencesCount, null));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertIndexMaximumValidation()
        {
            Execute(Enumerate(), storage =>
            {
                var list = (IList)storage;
                list.Insert(WeakReferencesCount + 1, null);
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedInsertIndexMaximumValidation()
        {
            Execute(Enumerate(), storage => storage.Insert(WeakReferencesCount + 1, null));
        }

        [TestMethod]
        public void InsertCountValidation()
        {
            Execute(Enumerate(), storage =>
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
            Execute(Enumerate(), storage =>
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
            Execute(Enumerate(), storage =>
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
            Execute(Enumerate(), storage =>
            {
                storage.Insert(InsertIndex, null);
                Assert.IsTrue(storage.Contains(null));
            });
        }

        [TestMethod]
        public void IsIndexOfMatch()
        {
            Execute(Enumerate(), storage =>
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
            Execute(Enumerate(), storage =>
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
            Execute(Enumerate(), storage =>
            {
                var testObject = new object();
                weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                var isRemoved = storage.Remove(weakReference);
                Assert.IsTrue(isRemoved && !storage.Contains(weakReference));
                GC.KeepAlive(testObject);
            }, finalizer: () =>
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
            Execute(Enumerate(), storage =>
            {
                var testObject = new object();
                weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                storage.Remove(weakReference);
                Assert.AreEqual(WeakReferencesCount, storage.Count);
                GC.KeepAlive(testObject);
            }, finalizer: () =>
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
            Execute(Enumerate(), storage =>
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
            }, finalizer: () =>
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
            Execute(Enumerate(), storage =>
            {
                var testObject = new object();
                weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                storage.RemoveAt(InsertIndex);
                Assert.IsFalse(storage.Contains(weakReference));
                GC.KeepAlive(testObject);
            }, finalizer: () =>
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
            Execute(Enumerate(), storage =>
            {
                var list = (IList)storage;
                list.RemoveAt(-1);
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedRemoveAtIndexMinimumValidation()
        {
            Execute(Enumerate(), storage => storage.RemoveAt(-1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveAtIndexMaximumValidation()
        {
            Execute(Enumerate(), storage =>
            {
                var list = (IList)storage;
                list.RemoveAt(WeakReferencesCount);
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TypedRemoveAtIndexMaximumValidation()
        {
            Execute(Enumerate(), storage => storage.RemoveAt(WeakReferencesCount));
        }

        [TestMethod]
        public void RemoveAtCountValidation()
        {
            Core.GarbageCollection.WeakReference<object> weakReference = null;
            Execute(Enumerate(), storage =>
            {
                var testObject = new object();
                weakReference = new Core.GarbageCollection.WeakReference<object>(testObject);
                storage.Insert(InsertIndex, weakReference);
                storage.RemoveAt(InsertIndex);
                Assert.AreEqual(WeakReferencesCount, storage.Count);
                GC.KeepAlive(testObject);
            }, finalizer: () =>
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
            Execute(Enumerate(), storage =>
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
            }, finalizer: () =>
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
            Execute(Enumerate(), storage =>
            {
                storage.Clear();
                Assert.AreEqual(0, storage.Count);
            });
        }

        [TestMethod]
        public void ClearVersionValidation()
        {
            Execute(Enumerate(), storage =>
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