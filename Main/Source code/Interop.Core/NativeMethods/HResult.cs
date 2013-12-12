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
            #region Generic

            S_OK = 0,

            S_FALSE = 1,

            E_NOTIMPL = unchecked((int)0x80004001),

            E_NOINTERFACE = unchecked((int)0x80004002),

            E_POINTER = unchecked((int)0x80004003),

            E_ABORT = unchecked((int)0x80004004),

            E_FAIL = unchecked((int)0x80004005),

            E_UNEXPECTED = unchecked((int)0x8000FFFF),

            E_ACCESSDENIED = unchecked((int)0x80070005),

            E_HANDLE = unchecked((int)0x80070006),

            E_OUTOFMEMORY = unchecked((int)0x8007000E),

            E_INVALIDARG = unchecked((int)0x80070057),

            #endregion

            #region COM

            CO_E_FIRST = unchecked((int)0x800401F0),

            CO_E_LAST = unchecked((int)0x800401FF),

            CO_S_FIRST = unchecked(0x000401F0),

            CO_S_LAST = unchecked(0x000401FF),

            CO_E_NOTINITIALIZED = unchecked((int)0x800401F0),

            CO_E_ALREADYINITIALIZED = unchecked((int)0x800401F1),

            CO_E_CANTDETERMINECLASS = unchecked((int)0x800401F2),

            CO_E_CLASSSTRING = unchecked((int)0x800401F3),

            CO_E_IIDSTRING = unchecked((int)0x800401F4),

            CO_E_APPNOTFOUND = unchecked((int)0x800401F5),

            CO_E_APPSINGLEUSE = unchecked((int)0x800401F6),

            CO_E_ERRORINAPP = unchecked((int)0x800401F7),

            CO_E_DLLNOTFOUND = unchecked((int)0x800401F8),

            CO_E_ERRORINDLL = unchecked((int)0x800401F9),

            CO_E_WRONGOSFORAPP = unchecked((int)0x800401FA),

            CO_E_OBJNOTREG = unchecked((int)0x800401FB),

            CO_E_OBJISREG = unchecked((int)0x800401FC),

            CO_E_OBJNOTCONNECTED = unchecked((int)0x800401FD),

            CO_E_APPDIDNTREG = unchecked((int)0x800401FE),

            CO_E_RELEASED = unchecked((int)0x800401FF),

            #endregion

            #region Class Factory

            CLASSFACTORY_E_FIRST = unchecked((int)0x80040110),

            CLASSFACTORY_E_LAST = unchecked((int)0x8004011F),

            CLASSFACTORY_S_FIRST = unchecked(0x00040110),

            CLASSFACTORY_S_LAST = unchecked(0x0004011F),

            CLASS_E_NOAGGREGATION = unchecked((int)0x80040110),

            CLASS_E_CLASSNOTAVAILABLE = unchecked((int)0x80040111),

            CLASS_E_NOTLICENSED = unchecked((int)0x80040112),

            #endregion

            #region Database

            REGDB_E_FIRST = unchecked((int)0x80040150),

            REGDB_E_LAST = unchecked((int)0x8004015F),

            REGDB_S_FIRST = unchecked(0x00040150),

            REGDB_S_LAST = unchecked(0x0004015F),

            REGDB_E_READREGDB = unchecked((int)0x80040150),

            REGDB_E_WRITEREGDB = unchecked((int)0x80040151),

            REGDB_E_KEYMISSING = unchecked((int)0x80040152),

            REGDB_E_INVALIDVALUE = unchecked((int)0x80040153),

            REGDB_E_CLASSNOTREG = unchecked((int)0x80040154),

            REGDB_E_IIDNOTREG = unchecked((int)0x80040155),

            REGDB_E_BADTHREADINGMODEL = unchecked((int)0x80040156)

            #endregion
        }
    }
}