using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

using JetBrains.Annotations;

namespace Interop.Helpers
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    [DebuggerNonUserCode]
    [System.Diagnostics.Contracts.Pure]
    public static class ValidationHelper
    {
        [ContractAnnotation("halt <= argument: null")]
        [ContractArgumentValidator]
        public static void NotNull<T>([CanBeNull] [ValidatedNotNull] T argument, [NotNull] Expression<Func<T>> parameterExpression, [CanBeNull] string reason = null)
        {
            NotNull(argument, ((MemberExpression)parameterExpression.Body).Member.Name, reason);
        }

        [ContractAnnotation("halt <= argument: null")]
        [ContractArgumentValidator]
        public static void NotNull<T>([CanBeNull] [ValidatedNotNull] T argument, [NotNull] string parameterName, [CanBeNull] string reason = null)
        {
// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (argument == null)
            {
                throw new ArgumentNullException(parameterName, reason);
            }
            Contract.EndContractBlock();
        }

        [ContractAnnotation("halt <= disposed: true")]
        public static void NotDisposed(bool disposed)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(null);
            }
        }

        [ContractArgumentValidator]
        public static void OfType<T>([CanBeNull] object argument, [NotNull] Expression<Func<object>> parameterExpression, [CanBeNull] string reason = null)
        {
            OfType<T>(argument, ((MemberExpression)parameterExpression.Body).Member.Name, reason);
        }

        [ContractArgumentValidator]
        public static void OfType<T>([CanBeNull] object argument, [NotNull] string parameterName, [CanBeNull] string reason = null)
        {
// ReSharper disable once CompareNonConstrainedGenericWithNull
            if (!(argument is T) && !(argument == null && default(T) == null))
            {
                throw new ArgumentException(reason ?? parameterName + " must be of type: " + typeof(T).Name, parameterName);
            }
            Contract.EndContractBlock();
        }

        [JetBrains.Annotations.Pure]
        [System.Diagnostics.Contracts.Pure]
        public static bool OfType<T>([CanBeNull] object argument)
        {
// ReSharper disable once CompareNonConstrainedGenericWithNull
            return (argument is T) || (argument == null && default(T) == null);
        }

        [ContractArgumentValidator]
        public static void InOpenInterval<T>(T argument, [NotNull] Expression<Func<T>> parameterExpression, T minimum, T maximum, [CanBeNull] string reason = null)
            where T : struct, IComparable<T>
        {
            InOpenInterval(argument, ((MemberExpression)parameterExpression.Body).Member.Name, minimum, maximum, reason);
        }

        [ContractArgumentValidator]
        public static void InOpenInterval<T>(T argument, [NotNull] string parameterName, T minimum, T maximum, [CanBeNull] string reason = null)
            where T : struct, IComparable<T>
        {
            if (argument.IsLessThan(minimum) || argument.IsGreaterThan(maximum))
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void InHalfClosedInterval<T>(T argument, [NotNull] Expression<Func<T>> parameterExpression, T minimum, T maximum, [CanBeNull] string reason = null)
            where T : struct, IComparable<T>
        {
            InHalfClosedInterval(argument, ((MemberExpression)parameterExpression.Body).Member.Name, minimum, maximum, reason);
        }

        [ContractArgumentValidator]
        public static void InHalfClosedInterval<T>(T argument, [NotNull] string parameterName, T minimum, T maximum, [CanBeNull] string reason = null)
            where T : struct, IComparable<T>
        {
            if (argument.IsLessThan(minimum) || argument.IsGreaterThanOrEqual(maximum))
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void NotZeroLength([CanBeNull] Array argument, [NotNull] Expression<Func<Array>> parameterExpression, [CanBeNull] string reason = null)
        {
            NotZeroLength(argument, ((MemberExpression)parameterExpression.Body).Member.Name, reason);
        }

        [ContractArgumentValidator]
        public static void NotZeroLength([CanBeNull] Array argument, [NotNull] string parameterName, [CanBeNull] string reason = null)
        {
            if (argument != null && argument.Length <= 0)
            {
                throw new ArgumentException(reason ?? "Zero-length array not supported.", parameterName);
            }
            Contract.EndContractBlock();
        }

        [ContractArgumentValidator]
        public static void NotMultidimensional([CanBeNull] Array argument, [NotNull] Expression<Func<Array>> parameterExpression, [CanBeNull] string reason = null)
        {
            NotMultidimensional(argument, ((MemberExpression)parameterExpression.Body).Member.Name, reason);
        }

        [ContractArgumentValidator]
        public static void NotMultidimensional([CanBeNull] Array argument, [NotNull] string parameterName, [CanBeNull] string reason = null)
        {
            if (argument != null && argument.Rank != 1)
            {
                throw new ArgumentException(reason ?? "Multidimensional array not supported.", parameterName);
            }
            Contract.EndContractBlock();
        }
    }
}