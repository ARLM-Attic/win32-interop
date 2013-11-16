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
using System.Diagnostics.Contracts;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

using JetBrains.Annotations;

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
    [SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
    public class WeakReference<T> : IDisposable
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

// ReSharper disable once IntroduceOptionalParameters.Global
        [PublicAPI]
        [SecuritySafeCritical]
        public WeakReference([CanBeNull] T target) : this(target, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeakReference{T}"/> class, referencing the specified object.
        /// </summary>
        /// <param name="target">The object to track. May not be null.</param>
        /// <param name="isResurrectionSupported">True, if we must support object resurrection; otherwise, False.</param>
        [PublicAPI]
        [SecuritySafeCritical]
        public WeakReference([CanBeNull] T target, bool isResurrectionSupported)
        {
            Contract.Ensures(_safeHandle != null);
            _safeHandle = new SafeGCHandle(target, isResurrectionSupported ? GCHandleType.WeakTrackResurrection : GCHandleType.Weak);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the referenced object. Will return null if the object has been garbage collected.
        /// </summary>
        [CanBeNull]
        public T Target
        {
            [SecuritySafeCritical]
            get
            {
                return _safeHandle.Target as T;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the object is still alive (has not been garbage collected).
        /// </summary>
        public bool IsAlive
        {
            [SecuritySafeCritical]
            get
            {
                return !_safeHandle.IsInvalid && _safeHandle.Target != null;
            }
        }

        #endregion

        #region Equality members

        [SecuritySafeCritical]
        public static bool operator ==(WeakReference<T> first, WeakReference<T> second)
        {
            return Equals(first, second);
        }

        [SecuritySafeCritical]
        public static bool operator !=(WeakReference<T> first, WeakReference<T> second)
        {
            return !Equals(first, second);
        }

        [SecuritySafeCritical]
        public static bool Equals([CanBeNull] WeakReference<T> first, [CanBeNull] WeakReference<T> second)
        {
            if (object.Equals(first, null) && object.Equals(second, null))
            {
                return true;
            }
            if (object.Equals(first, null) || object.Equals(second, null))
            {
                return false;
            }
            return first._safeHandle == second._safeHandle;
        }

        [SecuritySafeCritical]
        public override bool Equals([CanBeNull] object obj)
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
            Contract.Ensures(Contract.Result<int>() == _safeHandle.GetHashCode());
            return _safeHandle.GetHashCode();
        }

        #endregion

        #region Overrides of Object

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            var target = Target;
            if (target == null)
            {
                return typeof(T).Name + ": <null>";
            }
            return typeof(T).Name + ": " + target;
        }

        #endregion

        #region Destructors

        /// <summary>
        /// Frees the weak reference.
        /// </summary>
        [PrePrepareMethod]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SecuritySafeCritical]
        public void Dispose()
        {
            _safeHandle.Dispose();
        }

        #endregion
    }
}