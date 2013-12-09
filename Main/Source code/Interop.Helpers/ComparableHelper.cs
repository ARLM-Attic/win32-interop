using System;
using System.Diagnostics;

using JetBrains.Annotations;

namespace Interop.Helpers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [System.Diagnostics.Contracts.Pure]
    public static class ComparableHelper
    {
// ReSharper disable CompareNonConstrainedGenericWithNull
        [Pure]
        public static bool IsGreaterThan<T>(this T instance, T value)
            where T : IComparable<T>
        {
            return instance == null ? value == null : instance.CompareTo(value) > 0;
        }

        [Pure]
        public static bool IsGreaterThanOrEqual<T>(this T instance, T value)
            where T : IComparable<T>
        {
            return instance == null ? value == null : instance.CompareTo(value) >= 0;
        }

        [Pure]
        public static bool IsLessThan<T>(this T instance, T value)
            where T : IComparable<T>
        {
            return instance == null ? value == null : instance.CompareTo(value) < 0;
        }

        [Pure]
        public static bool IsLessThanOrEqual<T>(this T instance, T value)
            where T : IComparable<T>
        {
            return instance == null ? value == null : instance.CompareTo(value) <= 0;
        }

// ReSharper restore CompareNonConstrainedGenericWithNull
    }
}