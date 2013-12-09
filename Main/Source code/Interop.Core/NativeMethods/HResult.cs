using JetBrains.Annotations;
// ReSharper disable once CheckNamespace


namespace Interop.Core
{
// ReSharper disable InconsistentNaming
    public static partial class NativeMethods
    {
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public enum HResult
        {
            S_OK = 0,
            S_FALSE = 1,
            E_UNEXPECTED = unchecked((int)0x8000FFFF),
            E_NOTIMPL = unchecked((int)0x80004001),
            E_OUTOFMEMORY = unchecked((int)0x8007000E),
            E_INVALIDARG = unchecked((int)0x80070057),
            E_NOINTERFACE = unchecked((int)0x80004002),
            E_POINTER = unchecked((int)0x80004003),
            E_HANDLE = unchecked((int)0x80070006),
            E_ABORT = unchecked((int)0x80004004),
            E_FAIL = unchecked((int)0x80004005),
            E_ACCESSDENIED = unchecked((int)0x80070005)
        }
    }

// ReSharper restore InconsistentNaming
}