#if NETFX
using System.Reflection;
#endif
using System.Runtime.InteropServices;

using Interop.Core.GarbageCollection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Interop.Core.Tests.GarbageCollection
{
    [TestClass]
    public class SafeGCHandleTests
    {
#if NETFX
        [TestMethod]
        public void TypeSecurity()
        {
            var safeGCHandle = typeof(SafeGCHandle);
            Assert.IsTrue(safeGCHandle.IsSecurityCritical);
            Assert.IsFalse(safeGCHandle.IsSecuritySafeCritical);
        }

        [TestMethod]
        public void ConstructorSecurity()
        {
            var constructor = typeof(SafeGCHandle).GetConstructor(new[] { typeof(object), typeof(GCHandleType) });
            Assert.IsTrue(constructor.IsSecurityCritical);
            Assert.IsFalse(constructor.IsSecuritySafeCritical);
        }
#endif

        [TestMethod]
        public void IsTargetWork()
        {
            var testObject = new object();
            var safeGCHandle = new SafeGCHandle(testObject, GCHandleType.Normal);
            Assert.AreEqual(testObject, safeGCHandle.Target);
            safeGCHandle.Dispose();
        }

#if NETFX
        [TestMethod]
        public void TargetSecurity()
        {
            var target = typeof(SafeGCHandle).GetMethod("get_Target");
            Assert.IsTrue(target.IsSecurityCritical);
            Assert.IsFalse(target.IsSecuritySafeCritical);
        }

        [TestMethod]
        public void IsInvalidSecurity()
        {
            var isInvalid = typeof(SafeGCHandle).GetMethod("get_IsInvalid");
            Assert.IsTrue(isInvalid.IsSecurityCritical);
            Assert.IsFalse(isInvalid.IsSecuritySafeCritical);
        }
#endif

        [TestMethod]
        public void IsEqualityWork()
        {
// ReSharper disable EqualExpressionComparison
// ReSharper disable RedundantCast
            var testObject = new object();
            var safeGCHandle = new SafeGCHandle(testObject, GCHandleType.Normal);
            Assert.IsTrue(safeGCHandle == safeGCHandle);
            Assert.IsFalse(safeGCHandle == null);
            Assert.IsFalse(null == safeGCHandle);
            Assert.IsTrue((SafeGCHandle)null == (SafeGCHandle)null);
            safeGCHandle.Dispose();
// ReSharper restore RedundantCast
// ReSharper restore EqualExpressionComparison
        }

#if NETFX
        [TestMethod]
        public void OpEqualitySecurity()
        {
            var opEquality = typeof(SafeGCHandle).GetMethod("op_Equality");
            Assert.IsTrue(opEquality.IsSecurityCritical);
            Assert.IsFalse(opEquality.IsSecuritySafeCritical);
        }

        [TestMethod]
        public void OpInequalitySecurity()
        {
            var opInequality = typeof(SafeGCHandle).GetMethod("op_Inequality");
            Assert.IsTrue(opInequality.IsSecurityCritical);
            Assert.IsFalse(opInequality.IsSecuritySafeCritical);
        }
#endif

        [TestMethod]
        public void IsEqualsWork()
        {
// ReSharper disable EqualExpressionComparison
            var testObject = new object();
            var safeGCHandle = new SafeGCHandle(testObject, GCHandleType.Normal);
            Assert.IsTrue(safeGCHandle.Equals(safeGCHandle));
            Assert.IsFalse(safeGCHandle.Equals(null));
            safeGCHandle.Dispose();
// ReSharper restore EqualExpressionComparison
        }

#if NETFX
        [TestMethod]
        public void EqualsSecurity()
        {
            var equals = typeof(SafeGCHandle).GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public);
            Assert.IsTrue(equals.IsSecurityCritical);
            Assert.IsTrue(equals.IsSecuritySafeCritical);
        }

        [TestMethod]
        public void GetHashCodeSecurity()
        {
            var getHashCode = typeof(SafeGCHandle).GetMethod("GetHashCode");
            Assert.IsTrue(getHashCode.IsSecurityCritical);
            Assert.IsTrue(getHashCode.IsSecuritySafeCritical);
        }

        [TestMethod]
        public void ReleaseHandleSecurity()
        {
            var releaseHandle = typeof(SafeGCHandle).GetMethod("ReleaseHandle", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsTrue(releaseHandle.IsSecurityCritical);
            Assert.IsFalse(releaseHandle.IsSecuritySafeCritical);
        }
#endif
    }
}