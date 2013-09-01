using System;
using System.Reflection;

using Interop.Core.GarbageCollection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Interop.Core.Tests.GarbageCollection
{
    [TestClass]
    public class WeakReferenceTests
    {
#if NETFX
        [TestMethod]
        public void TypeSecurity()
        {
            var weakReference = typeof(WeakReference<>);
            Assert.IsTrue(weakReference.IsSecurityTransparent);
        }

        [TestMethod]
        public void SafeHandleSecurity()
        {
            var safeHandle = typeof(WeakReference<>).GetField("_safeHandle", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsTrue(safeHandle.IsSecurityCritical);
            Assert.IsFalse(safeHandle.IsSecuritySafeCritical);
        }

        [TestMethod]
        public void ConstructorSecurity()
        {
            var weakReference = typeof(WeakReference<>);
            var constructor = weakReference.GetConstructor(weakReference.GetGenericArguments());
            Assert.IsTrue(constructor.IsSecurityCritical);
            Assert.IsTrue(constructor.IsSecuritySafeCritical);
        }
#endif

        [TestMethod]
        public void IsTargetAlive()
        {
            var testObject = new object();
            var weakReference = new WeakReference<object>(testObject);
            Assert.AreEqual(testObject, weakReference.Target);
            GC.KeepAlive(testObject);
            weakReference.Dispose();
        }

        [TestMethod]
        public void IsTargetDead()
        {
            var testObject = new object();
            var weakReference = new WeakReference<object>(testObject);
            Assert.AreEqual(testObject, weakReference.Target);
            GC.KeepAlive(testObject);
// ReSharper disable once RedundantAssignment
            testObject = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Assert.IsNull(weakReference.Target);
            weakReference.Dispose();
        }

#if NETFX
        [TestMethod]
        public void TargetSecurity()
        {
            var target = typeof(WeakReference<>).GetMethod("get_Target");
            Assert.IsTrue(target.IsSecurityCritical);
            Assert.IsTrue(target.IsSecuritySafeCritical);
        }
#endif

        [TestMethod]
        public void IsAlive()
        {
            var testObject = new object();
            var weakReference = new WeakReference<object>(testObject);
            Assert.IsTrue(weakReference.IsAlive);
            GC.KeepAlive(testObject);
            weakReference.Dispose();
        }

        [TestMethod]
        public void IsDead()
        {
            var testObject = new object();
            var weakReference = new WeakReference<object>(testObject);
            Assert.IsTrue(weakReference.IsAlive);
            GC.KeepAlive(testObject);
            // ReSharper disable once RedundantAssignment
            testObject = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Assert.IsFalse(weakReference.IsAlive);
// ReSharper disable once HeuristicUnreachableCode
            weakReference.Dispose();
        }

#if NETFX
        [TestMethod]
        public void IsAliveSecurity()
        {
            var isAlive = typeof(WeakReference<>).GetMethod("get_IsAlive");
            Assert.IsTrue(isAlive.IsSecurityCritical);
            Assert.IsTrue(isAlive.IsSecuritySafeCritical);
        }
#endif

        [TestMethod]
        public void IsEqualityWork()
        {
// ReSharper disable EqualExpressionComparison
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable RedundantCast
            var testObject = new object();
            var weakReference = new WeakReference<object>(testObject);
            Assert.IsTrue(weakReference == weakReference);
            Assert.IsFalse(weakReference == null);
            Assert.IsFalse(null == weakReference);
            Assert.IsTrue((WeakReference<object>)null == (WeakReference<object>)null);
            weakReference.Dispose();
// ReSharper restore RedundantCast
// ReSharper restore ConditionIsAlwaysTrueOrFalse
// ReSharper restore EqualExpressionComparison
        }

#if NETFX
        [TestMethod]
        public void OpEqualitySecurity()
        {
            var opEquality = typeof(WeakReference<>).GetMethod("op_Equality");
            Assert.IsTrue(opEquality.IsSecurityCritical);
            Assert.IsTrue(opEquality.IsSecuritySafeCritical);
        }

        [TestMethod]
        public void OpInequalitySecurity()
        {
            var opInequality = typeof(WeakReference<>).GetMethod("op_Inequality");
            Assert.IsTrue(opInequality.IsSecurityCritical);
            Assert.IsTrue(opInequality.IsSecuritySafeCritical);
        }
#endif

        [TestMethod]
        public void IsEqualsWork()
        {
            // ReSharper disable EqualExpressionComparison
            var testObject = new object();
            var weakReference = new WeakReference<object>(testObject);
            Assert.IsTrue(weakReference.Equals(weakReference));
            Assert.IsFalse(weakReference.Equals(null));
            weakReference.Dispose();
            // ReSharper restore EqualExpressionComparison
        }

#if NETFX
        [TestMethod]
        public void EqualsSecurity()
        {
            var equals = typeof(WeakReference<>).GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public);
            Assert.IsTrue(equals.IsSecurityCritical);
            Assert.IsTrue(equals.IsSecuritySafeCritical);
        }

        [TestMethod]
        public void GetHashCodeSecurity()
        {
            var getHashCode = typeof(WeakReference<>).GetMethod("GetHashCode");
            Assert.IsTrue(getHashCode.IsSecurityCritical);
            Assert.IsTrue(getHashCode.IsSecuritySafeCritical);
        }

        [TestMethod]
        public void DisposeSecurity()
        {
            var getHashCode = typeof(WeakReference<>).GetMethod("Dispose");
            Assert.IsTrue(getHashCode.IsSecurityCritical);
            Assert.IsTrue(getHashCode.IsSecuritySafeCritical);
        }
#endif
    }
}
