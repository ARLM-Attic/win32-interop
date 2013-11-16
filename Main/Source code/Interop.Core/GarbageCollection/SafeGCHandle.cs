// ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// <copyright file="SafeGCHandle.cs" company="Aleksandr Vishnyakov">
//   Copyright (c) 2013 Aleksandr Vishnyakov
//
//   Based on SafeGCHandle.cs by Nito.KitchenSink
//   Copyright (c) 2009-2010 Nito Programs
//
//   Microsoft Public License (Ms-PL), http://nitokitchensink.codeplex.com/license
// </copyright>
// <summary>
//   Safe handle for a <see cref="GCHandle"/> class.
// </summary>
// ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

using JetBrains.Annotations;

namespace Interop.Core.GarbageCollection
{
    /// <summary>
    /// Helper class to help with managing <see cref="GCHandle"/> resources.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Note that this class can only be used to represent <see cref="GCHandle"/> objects that should be freed when garbage collected (or disposed).
    /// This class cannot be used in several interop situations, such as passing ownership of an object to a callback function.
    /// </para>
    /// </remarks>
    [SecurityCritical]
    public class SafeGCHandle : SafeHandle
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeGCHandle"/> class referring to the target in the given way.
        /// </summary>
        /// <param name="target">The object to reference.</param>
        /// <param name="type">The way to reference the object.</param>
        public SafeGCHandle([CanBeNull] object target, GCHandleType type)
            : base(IntPtr.Zero, true)
        {
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                // It is required to create a Constrained Execution Region
            }
            finally
            {
                SetHandle((IntPtr)GCHandle.Alloc(target, type));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the object this handle represents.
        /// </summary>
        [CanBeNull]
        public object Target
        {
            get
            {
                return handle != IntPtr.Zero ? ((GCHandle)handle).Target : null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the handle value is invalid.
        /// </summary>
        public override bool IsInvalid
        {
            [SecurityCritical]
            [PrePrepareMethod]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            get
            {
                Contract.Ensures(Contract.Result<bool>() == (handle == IntPtr.Zero));
                return handle == IntPtr.Zero;
            }
        }

        #endregion

        #region Equality members

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static bool operator ==(SafeGCHandle first, SafeGCHandle second)
        {
            if (Equals(first, null) && Equals(second, null))
            {
                return true;
            }
            if (Equals(first, null) || Equals(second, null))
            {
                return false;
            }
            return first.handle == second.handle;
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        public static bool operator !=(SafeGCHandle first, SafeGCHandle second)
        {
            return !(first == second);
        }

        [SecuritySafeCritical]
        public override bool Equals([CanBeNull] object obj)
        {
            var other = obj as SafeGCHandle;
            if (other == null)
            {
                return false;
            }
            return handle == other.handle;
        }

        [SecuritySafeCritical]
        public override int GetHashCode()
        {
            // handle is immutable
// ReSharper disable NonReadonlyFieldInGetHashCode
            Contract.Ensures(Contract.Result<int>() == handle.GetHashCode());
            return handle.GetHashCode();
// ReSharper restore NonReadonlyFieldInGetHashCode
        }

        #endregion

        #region Overrides of Object

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        [SecuritySafeCritical]
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1407:ArithmeticExpressionsMustDeclarePrecedence", Justification = "Reviewed. Suppression is OK here.")]
        public override string ToString()
        {
            return IntPtr.Size == sizeof(int) ? "0x" + handle.ToInt32().ToString("X" + IntPtr.Size * 2) : "0x" + handle.ToInt64().ToString("X" + IntPtr.Size * 2);
        }

        #endregion

        #region Destructors

        /// <summary>
        /// Frees the garbage collection handle.
        /// </summary>
        /// <returns>Whether the handle was released successfully.</returns>
        [SecurityCritical]
        [PrePrepareMethod]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            if (handle != IntPtr.Zero)
            {
                ((GCHandle)handle).Free();
                return true;
            }
            return false;
        }

        #endregion
    }
}