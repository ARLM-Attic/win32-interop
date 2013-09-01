// ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="WeakReference.cs" company="Aleksandr Vishnyakov">
//   Copyright (c) 2013 Aleksandr Vishnyakov
//
//   Based on WeakReference.cs by Nito.KitchenSink
//   Copyright (c) 2009-2010 Nito Programs
//
//   Microsoft Public License (Ms-PL), http://nitokitchensink.codeplex.com/license
// </copyright>
// <summary>
//   Represents a weak reference, which references an object while still allowing that object to be reclaimed by garbage collection.
// </summary>
// ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using System;
#if !SILVERLIGHT
using System.Runtime.ConstrainedExecution;
#endif
using System.Runtime.InteropServices;
using System.Security;
#if !SILVERLIGHT
using System.Security.Permissions;
#endif

namespace Interop.Core.GarbageCollection
{
    /// <summary>
    /// Represents a weak reference, which references an object while still allowing that object to be reclaimed by garbage collection.
    /// </summary>
    /// <remarks>
    /// <para>
    /// We define our own type, unrelated to <see cref="System.WeakReference"/> both to provide type safety and
    /// because <see cref="System.WeakReference"/> is an incorrect implementation (it does not implement <see cref="IDisposable"/>).
    /// </para>
    /// </remarks>
    /// <typeparam name="T">The type of object to reference.</typeparam>
#if !SILVERLIGHT
    [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
#endif
    public sealed class WeakReference<T> : IDisposable
        where T : class
    {
        #region Private fields

        /// <summary>
        /// The contained <see cref="SafeGCHandle"/>.
        /// </summary>
        [SecurityCritical]
        private readonly SafeGCHandle _safeHandle;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReference{T}"/> class, referencing the specified object.
        /// </summary>
        /// <param name="target">The object to track. May not be null.</param>
        [SecuritySafeCritical]
        public WeakReference(T target)
        {
            _safeHandle = new SafeGCHandle(target, GCHandleType.Weak);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the referenced object. Will return null if the object has been garbage collected.
        /// </summary>
        public T Target
        {
            [SecuritySafeCritical]
            get { return _safeHandle.Target as T; }
        }

        /// <summary>
        /// Gets a value indicating whether the object is still alive (has not been garbage collected).
        /// </summary>
        public bool IsAlive
        {
            [SecuritySafeCritical]
            get { return !_safeHandle.IsInvalid && _safeHandle.Target != null; }
        }

        #endregion

        #region Equality members

        [SecuritySafeCritical]
        public static bool operator ==(WeakReference<T> first, WeakReference<T> second)
        {
            if (Equals(first, null) && Equals(second, null))
            {
                return true;
            }
            if (Equals(first, null) || Equals(second, null))
            {
                return false;
            }
            return first._safeHandle == second._safeHandle;
        }

        [SecuritySafeCritical]
        public static bool operator !=(WeakReference<T> first, WeakReference<T> second)
        {
            return !(first == second);
        }

        [SecuritySafeCritical]
        public override bool Equals(object obj)
        {
            var other = obj as WeakReference<T>;
            if (other == null)
            {
                return false;
            }
            return _safeHandle == other._safeHandle;
        }

        [SecuritySafeCritical]
        public override int GetHashCode()
        {
            return _safeHandle.GetHashCode();
        }

        #endregion

        #region Destructors

        /// <summary>
        /// Frees the weak reference.
        /// </summary>
#if !SILVERLIGHT
        [PrePrepareMethod]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
#endif
        [SecuritySafeCritical]
        public void Dispose()
        {
            _safeHandle.Dispose();
        }

        #endregion
    }
}