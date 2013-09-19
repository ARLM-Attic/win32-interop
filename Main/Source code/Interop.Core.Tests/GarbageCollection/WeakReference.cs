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
            var member = typeof(WeakReference<>);
            Assert.IsTrue(member.IsSecurityTransparent);
        }

        [TestMethod]
        public void SafeHandleSecurity()
        {
            var member = typeof(WeakReference<>).GetField("_safeHandle", BindingFlags.Instance | BindingFlags.NonPublic);
            if (member != null)
            {
                Assert.IsTrue(member.IsSecurityCritical);
                Assert.IsFalse(member.IsSecuritySafeCritical);
            }
            else
            {
                Assert.Fail("Field _safeHandle in type" + typeof(WeakReference<>).Name + " not found!");
            }
        }

        [TestMethod]
        public void ConstructorSecurity()
        {
            var weakReference = typeof(WeakReference<>);
            var member = weakReference.GetConstructor(weakReference.GetGenericArguments());
            if (member != null)
            {
                Assert.IsTrue(member.IsSecurityCritical);
                Assert.IsTrue(member.IsSecuritySafeCritical);
            }
            else
            {
                Assert.Fail("Constructor of type " + typeof(WeakReference<>).Name + " with signature .ctor(`0) not found!");
            }
        }

        [TestMethod]
        public void ConstructorWithResurreptionSecurity()
        {
            var weakReference = typeof(WeakReference<>);
            var member = weakReference.GetConstructor(new[] { weakReference.GetGenericArguments()[0], typeof(bool) });
            if (member != null)
            {
                Assert.IsTrue(member.IsSecurityCritical);
                Assert.IsTrue(member.IsSecuritySafeCritical);
            }
            else
            {
                Assert.Fail("Constructor of type " + typeof(WeakReference<>).Name + " with signature .ctor(`0, System.Boolean) not found!");
            }
        }
#endif

        [TestMethod]
        public void IsTargetAlive()
        {
            var testObject = new object();
            var weakReference = new WeakReference<object>(testObject);
            Assert.AreEqual(testObject, weakReference.Target);
            System.GC.KeepAlive(testObject);
            weakReference.Dispose();
        }

        [TestMethod]
        public void IsTargetDead()
        {
            var testObject = new object();
            var weakReference = new WeakReference<object>(testObject);
            Assert.AreEqual(testObject, weakReference.Target);
            System.GC.KeepAlive(testObject);
// ReSharper disable once RedundantAssignment
            testObject = null;
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
            Assert.IsNull(weakReference.Target);
            weakReference.Dispose();
        }

        [TestMethod]
        public void IsTargetResurrected()
        {
            var testObject = new ResurrectingObject();
            var weakReference = new WeakReference<ResurrectingObject>(testObject, true);
            Assert.AreEqual(testObject, weakReference.Target);
            System.GC.KeepAlive(testObject);
// ReSharper disable once RedundantAssignment
            testObject = null;
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
            Assert.IsNotNull(weakReference.Target);
            weakReference.Dispose();
        }

        [TestMethod]
        public void IsTargetCompletelyDead()
        {
            var testObject = new ResurrectingObject();
            var weakReference = new WeakReference<ResurrectingObject>(testObject, true);
            Assert.AreEqual(testObject, weakReference.Target);
            System.GC.KeepAlive(testObject);
// ReSharper disable once RedundantAssignment
            testObject = null;
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
            Assert.IsNotNull(weakReference.Target);
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
            Assert.IsNull(weakReference.Target);
            weakReference.Dispose();
        }

#if NETFX
        [TestMethod]
        public void TargetSecurity()
        {
            var member = typeof(WeakReference<>).GetMethod("get_Target");
            Assert.IsTrue(member.IsSecurityCritical);
            Assert.IsTrue(member.IsSecuritySafeCritical);
        }
#endif

        [TestMethod]
        public void IsAlive()
        {
            var testObject = new object();
            var weakReference = new WeakReference<object>(testObject);
            Assert.IsTrue(weakReference.IsAlive);
            System.GC.KeepAlive(testObject);
            weakReference.Dispose();
        }

        [TestMethod]
        public void IsDead()
        {
            var testObject = new object();
            var weakReference = new WeakReference<object>(testObject);
            Assert.IsTrue(weakReference.IsAlive);
            System.GC.KeepAlive(testObject);
// ReSharper disable once RedundantAssignment
            testObject = null;
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
            Assert.IsFalse(weakReference.IsAlive);
            weakReference.Dispose();
        }

        [TestMethod]
        public void IsResurrected()
        {
            var testObject = new ResurrectingObject();
            var weakReference = new WeakReference<ResurrectingObject>(testObject, true);
            Assert.IsTrue(weakReference.IsAlive);
            System.GC.KeepAlive(testObject);
// ReSharper disable once RedundantAssignment
            testObject = null;
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
            Assert.IsTrue(weakReference.IsAlive);
            weakReference.Dispose();
        }

        [TestMethod]
        public void IsCompletelyDead()
        {
            var testObject = new ResurrectingObject();
            var weakReference = new WeakReference<ResurrectingObject>(testObject, true);
            Assert.IsTrue(weakReference.IsAlive);
            System.GC.KeepAlive(testObject);
// ReSharper disable once RedundantAssignment
            testObject = null;
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
            Assert.IsTrue(weakReference.IsAlive);
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            System.GC.Collect();
            Assert.IsFalse(weakReference.IsAlive);
            weakReference.Dispose();
        }

#if NETFX
        [TestMethod]
        public void IsAliveSecurity()
        {
            var member = typeof(WeakReference<>).GetMethod("get_IsAlive");
            Assert.IsTrue(member.IsSecurityCritical);
            Assert.IsTrue(member.IsSecuritySafeCritical);
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
            var member = typeof(WeakReference<>).GetMethod("op_Equality");
            Assert.IsTrue(member.IsSecurityCritical);
            Assert.IsTrue(member.IsSecuritySafeCritical);
        }

        [TestMethod]
        public void OpInequalitySecurity()
        {
            var member = typeof(WeakReference<>).GetMethod("op_Inequality");
            Assert.IsTrue(member.IsSecurityCritical);
            Assert.IsTrue(member.IsSecuritySafeCritical);
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
        public void EqualsStaticSecurity()
        {
            var member = typeof(WeakReference<>).GetMethod("Equals", BindingFlags.Static | BindingFlags.Public);
            Assert.IsTrue(member.IsSecurityCritical);
            Assert.IsTrue(member.IsSecuritySafeCritical);
        }

        [TestMethod]
        public void EqualsSecurity()
        {
            var member = typeof(WeakReference<>).GetMethod("Equals", BindingFlags.Instance | BindingFlags.Public);
            Assert.IsTrue(member.IsSecurityCritical);
            Assert.IsTrue(member.IsSecuritySafeCritical);
        }

        [TestMethod]
        public void GetHashCodeSecurity()
        {
            var member = typeof(WeakReference<>).GetMethod("GetHashCode");
            Assert.IsTrue(member.IsSecurityCritical);
            Assert.IsTrue(member.IsSecuritySafeCritical);
        }
#endif

        [TestMethod]
        public void ToStringTest()
        {
// ReSharper disable once ConvertToConstant.Local
            var testObject = "Test string";
            var weakReference = new WeakReference<string>(testObject);
            Assert.AreEqual(weakReference.ToString(), "String: Test string");
            weakReference.Dispose();
        }

        [TestMethod]
        public void NullToStringTest()
        {
            var weakReference = new WeakReference<object>(null);
            Assert.AreEqual(weakReference.ToString(), "Object: <null>");
            weakReference.Dispose();
        }

#if NETFX
        [TestMethod]
        public void ToStringSecurity()
        {
            var member = typeof(WeakReference<>).GetMethod("ToString");
            Assert.IsTrue(member.IsSecurityTransparent);
        }

        [TestMethod]
        public void DisposeSecurity()
        {
            var member = typeof(WeakReference<>).GetMethod("Dispose");
            Assert.IsTrue(member.IsSecurityCritical);
            Assert.IsTrue(member.IsSecuritySafeCritical);
        }
#endif
    }
}