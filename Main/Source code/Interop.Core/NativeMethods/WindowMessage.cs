using System;

using JetBrains.Annotations;

// ReSharper disable once CheckNamespace
namespace Interop.Core
{
    // ReSharper disable InconsistentNaming
    public static partial class NativeMethods
    {
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public enum WindowMessage
        {
            WM_NULL = 0x0000,

            WM_CREATE = 0x0001,

            WM_DESTROY = 0x0002,

            WM_MOVE = 0x0003,

            //// Not defined 0x0004

            WM_SIZE = 0x0005,

            WM_ACTIVATE = 0x0006,

            WM_SETFOCUS = 0x0007,

            WM_KILLFOCUS = 0x0008,

            //// Not defined 0x0009

            WM_ENABLE = 0x000A,

            WM_SETREDRAW = 0x000B,

            WM_SETTEXT = 0x000C,

            WM_GETTEXT = 0x000D,

            WM_GETTEXTLENGTH = 0x000E,

            WM_PAINT = 0x000F,

            WM_CLOSE = 0x0010,

            WM_QUERYENDSESSION = 0x0011,

            WM_QUIT = 0x0012,

            WM_QUERYOPEN = 0x0013,

            WM_ERASEBKGND = 0x0014,

            WM_SYSCOLORCHANGE = 0x0015,

            WM_ENDSESSION = 0x0016,

            [Obsolete("Win16 API")]
            WM_SYSTEMERROR = 0x0017,

            WM_SHOWWINDOW = 0x0018,

            [Obsolete("Win16 API")]
            WM_CTLCOLOR = 0x0019,

            WM_WININICHANGE = 0x001A,

            WM_SETTINGCHANGE = WM_WININICHANGE,

            WM_DEVMODECHANGE = 0x001B,

            WM_ACTIVATEAPP = 0x001C,

            WM_FONTCHANGE = 0x001D,

            WM_TIMECHANGE = 0x001E,

            WM_CANCELMODE = 0x001F,

            WM_SETCURSOR = 0x0020,

            WM_MOUSEACTIVATE = 0x0021,

            WM_CHILDACTIVATE = 0x0022,

            WM_QUEUESYNC = 0x0023,

            WM_GETMINMAXINFO = 0x0024,

            //// Not defined 0x0025

            WM_PAINTICON = 0x0026,

            WM_ICONERASEBKGND = 0x0027,

            WM_NEXTDLGCTL = 0x0028,

            //// Not defined 0x0029

            WM_SPOOLERSTATUS = 0x002A,

            WM_DRAWITEM = 0x002B,

            WM_MEASUREITEM = 0x002C,

            WM_DELETEITEM = 0x002D,

            WM_VKEYTOITEM = 0x002E,

            WM_CHARTOITEM = 0x002F,

            WM_SETFONT = 0x0030,

            WM_GETFONT = 0x0031,

            WM_SETHOTKEY = 0x0032,

            WM_GETHOTKEY = 0x0033,

            //// Not defined 0x0034 - 0x0036

            WM_QUERYDRAGICON = 0x0037,

            //// Not defined 0x0038

            WM_COMPAREITEM = 0x0039,

            //// Not defined 0x003A - 0x003C

            WM_GETOBJECT = 0x003D,

            //// Not defined 0x003E - 0x0040

            WM_COMPACTING = 0x0041,

            //// Not defined 0x0042

            //// Not defined 0x0043

            [Obsolete("Win16 API")]
            WM_COMMNOTIFY = 0x0044,

            //// Not defined 0x0045

            WM_WINDOWPOSCHANGING = 0x0046,

            WM_WINDOWPOSCHANGED = 0x0047,

            WM_POWER = 0x0048,

            [UndocumentedFeature]
            WM_COPYGLOBALDATA = 0x0049,

            WM_COPYDATA = 0x004A,

            WM_CANCELJOURNAL = 0x004B,

            //// Not defined 0x004C

            //// Not defined 0x004D

            WM_NOTIFY = 0x004E,

            //// Not defined 0x004F

            WM_INPUTLANGCHANGEREQUEST = 0x0050,

            WM_INPUTLANGCHANGE = 0x0051,

            WM_TCARD = 0x0052,

            WM_HELP = 0x0053,

            WM_USERCHANGED = 0x0054,

            WM_NOTIFYFORMAT = 0x0055,

            //// Not defined 0x0056 - 0x007A

            WM_CONTEXTMENU = 0x007B,

            WM_STYLECHANGING = 0x007C,

            WM_STYLECHANGED = 0x007D,

            WM_DISPLAYCHANGE = 0x007E,

            WM_GETICON = 0x007F,

            WM_SETICON = 0x0080,

            WM_NCCREATE = 0x0081,

            WM_NCDESTROY = 0x0082,

            WM_NCCALCSIZE = 0x0083,

            WM_NCHITTEST = 0x0084,

            WM_NCPAINT = 0x0085,

            WM_NCACTIVATE = 0x0086,

            WM_GETDLGCODE = 0x0087,

            WM_SYNCPAINT = 0x0088,

            //// Not defined 0x0089 - 0x009F

            WM_NCMOUSEMOVE = 0x00A0,

            WM_NCLBUTTONDOWN = 0x00A1,

            WM_NCLBUTTONUP = 0x00A2,

            WM_NCLBUTTONDBLCLK = 0x00A3,

            WM_NCRBUTTONDOWN = 0x00A4,

            WM_NCRBUTTONUP = 0x00A5,

            WM_NCRBUTTONDBLCLK = 0x00A6,

            WM_NCMBUTTONDOWN = 0x00A7,

            WM_NCMBUTTONUP = 0x00A8,

            WM_NCMBUTTONDBLCLK = 0x00A9,

            //// Not defined 0x00AA

            WM_NCXBUTTONDOWN = 0x00AB,

            WM_NCXBUTTONUP = 0x00AC,

            WM_NCXBUTTONDBLCLK = 0x00AD,

            //// Not defined 0x00AE - 0x00DF

            //// 0x00E0 - 0x00EB defined in SBM

            //// Not defined 0x00EC - 0x00FD

            WM_INPUT_DEVICE_CHANGE = 0x00FE,

            WM_INPUT = 0x00FF,

            WM_KEYFIRST = 0x0100,

            WM_KEYDOWN = 0x0100,

            WM_KEYUP = 0x0101,

            WM_CHAR = 0x0102,

            WM_DEADCHAR = 0x0103,

            WM_SYSKEYDOWN = 0x0104,

            WM_SYSKEYUP = 0x0105,

            WM_SYSCHAR = 0x0106,

            WM_SYSDEADCHAR = 0x0107,

            [Obsolete("Windows 2000 API")]
            WM_KEYLAST_WIN2000 = 0x0108,

            [Obsolete("Windows 2000 API")]
            WM_WNT_CONVERTREQUESTEX_WIN2000 = 0x0108,

            WM_KEYLAST = 0x0109,

            [UndocumentedFeature]
            WM_WNT_CONVERTREQUESTEX = 0x0109,

            WM_UNICHAR = 0x0109,

            [UndocumentedFeature]
            WM_CONVERTREQUEST = 0x010A,

            [UndocumentedFeature]
            WM_CONVERTRESULT = 0x010B,

            [UndocumentedFeature]
            WM_INTERIM = 0x010C,

            WM_IME_STARTCOMPOSITION = 0x010D,

            WM_IME_ENDCOMPOSITION = 0x010E,

            WM_IME_COMPOSITION = 0x010F,

            WM_IME_KEYLAST = 0x010F,

            WM_INITDIALOG = 0x0110,

            WM_COMMAND = 0x0111,

            WM_SYSCOMMAND = 0x0112,

            WM_TIMER = 0x0113,

            WM_HSCROLL = 0x0114,

            WM_VSCROLL = 0x0115,

            WM_INITMENU = 0x0116,

            WM_INITMENUPOPUP = 0x0117,

            [UndocumentedFeature]
            WM_SYSTIMER = 0x0118,

            [Version("WM_GESTURE", WINVER.Windows7, WINVER.None)]
            WM_GESTURE = 0x0119,

            [Version("WM_GESTURENOTIFY", WINVER.Windows7, WINVER.None)]
            WM_GESTURENOTIFY = 0x011A,

            WM_MENUSELECT = 0x011F,

            WM_MENUCHAR = 0x0120,

            WM_ENTERIDLE = 0x0121,

            WM_MENURBUTTONUP = 0x0122,

            WM_MENUDRAG = 0x0123,

            WM_MENUGETOBJECT = 0x0124,

            WM_UNINITMENUPOPUP = 0x0125,

            WM_MENUCOMMAND = 0x0126,

            WM_CHANGEUISTATE = 0x0127,

            WM_UPDATEUISTATE = 0x0128,

            WM_QUERYUISTATE = 0x0129,

            //// Not defined 0x0130

            //// Not defined 0x0131

            WM_CTLCOLORMSGBOX = 0x0132,

            WM_CTLCOLOREDIT = 0x0133,

            WM_CTLCOLORLISTBOX = 0x0134,

            WM_CTLCOLORBTN = 0x0135,

            WM_CTLCOLORDLG = 0x0136,

            WM_CTLCOLORSCROLLBAR = 0x0137,

            WM_CTLCOLORSTATIC = 0x0138,

            //// Not defined 0x0139 - 0x01E0

            WM_MN_GETHMENU = 0x01E1,

            //// Not defined 0x01E2 - 0x01FF

            WM_MOUSEFIRST = 0x0200,

            WM_MOUSEMOVE = 0x0200,

            WM_LBUTTONDOWN = 0x0201,

            WM_LBUTTONUP = 0x0202,

            WM_LBUTTONDBLCLK = 0x0203,

            WM_RBUTTONDOWN = 0x0204,

            WM_RBUTTONUP = 0x0205,

            WM_RBUTTONDBLCLK = 0x0206,

            WM_MBUTTONDOWN = 0x0207,

            WM_MBUTTONUP = 0x0208,

            WM_MBUTTONDBLCLK = 0x0209,

            [Obsolete("Windows 95 API")]
            WM_MOUSELAST_WIN95 = 0x0209,

            WM_MOUSEWHEEL = 0x020A,

            [Obsolete("Windows NT 4.0 Message")]
            WM_MOUSELAST_WIN98_WINNT4 = 0x020A,

            WM_XBUTTONDOWN = 0x020B,

            WM_XBUTTONUP = 0x020C,

            WM_XBUTTONDBLCLK = 0x020D,

            [Version("WM_MOUSELAST", WINVER.WindowsXP, WINVER.WindowsXPSP2)]
            WM_MOUSELAST_WINXP = 0x020D,

            [Version("WM_MOUSEHWHEEL", WINVER.WindowsVista, WINVER.None)]
            WM_MOUSEHWHEEL = 0x020E,

            [Version("WM_MOUSELAST", WINVER.WindowsVista, WINVER.None)]
            WM_MOUSELAST = 0x020E,

            //// Not defined 0x020F

            WM_PARENTNOTIFY = 0x0210,

            WM_ENTERMENULOOP = 0x0211,

            WM_EXITMENULOOP = 0x0212,

            WM_NEXTMENU = 0x0213,

            WM_SIZING = 0x0214,

            WM_CAPTURECHANGED = 0x0215,

            WM_MOVING = 0x0216,

            //// Not defined 0x0217

            WM_POWERBROADCAST = 0x0218,

            WM_DEVICECHANGE = 0x0219,

            //// Not defined 0x021A - 0x021F

            WM_MDICREATE = 0x0220,

            WM_MDIDESTROY = 0x0221,

            WM_MDIACTIVATE = 0x0222,

            WM_MDIRESTORE = 0x0223,

            WM_MDINEXT = 0x0224,

            WM_MDIMAXIMIZE = 0x0225,

            WM_MDITILE = 0x0226,

            WM_MDICASCADE = 0x0227,

            WM_MDIICONARRANGE = 0x0228,

            WM_MDIGETACTIVE = 0x0229,

            //// Not defined 0x021A - 0x021F,

            WM_MDISETMENU = 0x0230,

            WM_ENTERSIZEMOVE = 0x0231,

            WM_EXITSIZEMOVE = 0x0232,

            WM_DROPFILES = 0x0233,

            WM_MDIREFRESHMENU = 0x0234,

            //// Not defined 0x0235 - 0x0237,

            [Version("WM_POINTERDEVICECHANGE", WINVER.Windows8, WINVER.None)]
            WM_POINTERDEVICECHANGE = 0x238,

            [Version("WM_POINTERDEVICEINRANGE", WINVER.Windows8, WINVER.None)]
            WM_POINTERDEVICEINRANGE = 0x239,

            [Version("WM_POINTERDEVICEOUTOFRANGE", WINVER.Windows8, WINVER.None)]
            WM_POINTERDEVICEOUTOFRANGE = 0x23A,

            //// Not defined 0x023B - 0x023F

            [Version("WM_TOUCH", WINVER.Windows7, WINVER.None)]
            WM_TOUCH = 0x0240,

            [Version("WM_NCPOINTERUPDATE", WINVER.Windows8, WINVER.None)]
            WM_NCPOINTERUPDATE = 0x0241,

            [Version("WM_NCPOINTERDOWN", WINVER.Windows8, WINVER.None)]
            WM_NCPOINTERDOWN = 0x0242,

            [Version("WM_NCPOINTERUP", WINVER.Windows8, WINVER.None)]
            WM_NCPOINTERUP = 0x0243,

            //// Not defined 0x0244

            [Version("WM_POINTERUPDATE", WINVER.Windows8, WINVER.None)]
            WM_POINTERUPDATE = 0x0245,

            [Version("WM_POINTERDOWN", WINVER.Windows8, WINVER.None)]
            WM_POINTERDOWN = 0x0246,

            [Version("WM_POINTERUP", WINVER.Windows8, WINVER.None)]
            WM_POINTERUP = 0x0247,

            //// Not defined 0x0248

            [Version("WM_POINTERENTER", WINVER.Windows8, WINVER.None)]
            WM_POINTERENTER = 0x0249,

            [Version("WM_POINTERLEAVE", WINVER.Windows8, WINVER.None)]
            WM_POINTERLEAVE = 0x024A,

            [Version("WM_POINTERACTIVATE", WINVER.Windows8, WINVER.None)]
            WM_POINTERACTIVATE = 0x024B,

            [Version("WM_POINTERCAPTURECHANGED", WINVER.Windows8, WINVER.None)]
            WM_POINTERCAPTURECHANGED = 0x024C,

            [Version("WM_TOUCHHITTESTING", WINVER.Windows8, WINVER.None)]
            WM_TOUCHHITTESTING = 0x024D,

            [Version("WM_POINTERWHEEL", WINVER.Windows8, WINVER.None)]
            WM_POINTERWHEEL = 0x024E,

            [Version("WM_POINTERHWHEEL", WINVER.Windows8, WINVER.None)]
            WM_POINTERHWHEEL = 0x024F,

            //// Not defined 0x0250 - 0x27F

            [UndocumentedFeature]
            WM_IME_REPORT = 0x0280, //// Undocumented feature

            WM_IME_SETCONTEXT = 0x0281,

            WM_IME_NOTIFY = 0x0282,

            WM_IME_CONTROL = 0x0283,

            WM_IME_COMPOSITIONFULL = 0x0284,

            WM_IME_SELECT = 0x0285,

            WM_IME_CHAR = 0x0286,

            //// Not defined 0x0287

            WM_IME_REQUEST = 0x0288,

            //// Not defined 0x0289

            WM_IME_KEYDOWN = 0x0290,

            WM_IME_KEYUP = 0x0291,

            //// Not defined 0x0292 - 0x029F

            WM_NCMOUSEHOVER = 0x02A0,

            WM_MOUSEHOVER = 0x02A1,

            WM_NCMOUSELEAVE = 0x02A2,

            WM_MOUSELEAVE = 0x02A3,

            //// Not defined 0x02A4 - 0x02B0

            WM_WTSSESSION_CHANGE = 0x02B1,

            //// Not defined 0x02B2 - 0x02BF

            WM_TABLET_FIRST = 0x02C0,

            //// Not defined 0x02C1 - 0x02DE

            WM_TABLET_LAST = 0x02DF,

            //// Not defined 0x02E0 - 0x02FF

            WM_CUT = 0x0300,

            WM_COPY = 0x0301,

            WM_PASTE = 0x0302,

            WM_CLEAR = 0x0303,

            WM_UNDO = 0x0304,

            WM_RENDERFORMAT = 0x0305,

            WM_RENDERALLFORMATS = 0x0306,

            WM_DESTROYCLIPBOARD = 0x0307,

            WM_DRAWCLIPBOARD = 0x0308,

            WM_PAINTCLIPBOARD = 0x0309,

            WM_VSCROLLCLIPBOARD = 0x030A,

            WM_SIZECLIPBOARD = 0x030B,

            WM_ASKCBFORMATNAME = 0x030C,

            WM_CHANGECBCHAIN = 0x030D,

            WM_HSCROLLCLIPBOARD = 0x030E,

            WM_QUERYNEWPALETTE = 0x030F,

            WM_PALETTEISCHANGING = 0x0310,

            WM_PALETTECHANGED = 0x0311,

            WM_HOTKEY = 0x0312,

            //// Not defined 0x0313 - 0x0316

            WM_PRINT = 0x0317,

            WM_PRINTCLIENT = 0x0318,

            WM_APPCOMMAND = 0x0319,

            WM_THEMECHANGED = 0x031A,

            //// Not defined 0x031B

            //// Not defined 0x031C

            WM_CLIPBOARDUPDATE = 0x031D,

            [Version("WM_DWMCOMPOSITIONCHANGED", WINVER.WindowsVista, WINVER.None)]
            WM_DWMCOMPOSITIONCHANGED = 0x031E,

            [Version("WM_DWMNCRENDERINGCHANGED", WINVER.WindowsVista, WINVER.None)]
            WM_DWMNCRENDERINGCHANGED = 0x031F,

            [Version("WM_DWMCOLORIZATIONCOLORCHANGED", WINVER.WindowsVista, WINVER.None)]
            WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320,

            [Version("WM_DWMWINDOWMAXIMIZEDCHANGE", WINVER.WindowsVista, WINVER.None)]
            WM_DWMWINDOWMAXIMIZEDCHANGE = 0x0321,

            //// Not defined 0x0322

            [Version("WM_DWMSENDICONICTHUMBNAIL", WINVER.Windows7, WINVER.None)]
            WM_DWMSENDICONICTHUMBNAIL = 0x0323,

            //// Not defined 0x0324

            //// Not defined 0x0325

            [Version("WM_DWMSENDICONICLIVEPREVIEWBITMAP", WINVER.Windows7, WINVER.None)]
            WM_DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326,

            [Version("WM_GETTITLEBARINFOEX", WINVER.WindowsVista, WINVER.None)]
            WM_GETTITLEBARINFOEX = 0x033F,

            //// Not defined 0x0340 - 0x0357

            WM_HANDHELDFIRST = 0x0358,

            //// Not defined 0x0359 - 0x035E

            WM_HANDHELDLAST = 0x035F,

            WM_AFXFIRST = 0x0360,

            //// Not defined 0x0361 - 0x037E

            WM_AFXLAST = 0x037F,

            WM_PENWINFIRST = 0x0380,

            [UndocumentedFeature]
            WM_RCRESULT = 0x0381,

            [UndocumentedFeature]
            WM_HOOKRCRESULT = 0x0382,

            [UndocumentedFeature]
            WM_GLOBALRCCHANGE = 0x0383,

            [UndocumentedFeature]
            WM_PENMISCINFO = 0x0383,

            [UndocumentedFeature]
            WM_SKB = 0x0384,

            [UndocumentedFeature]
            WM_HEDITCTL = 0x0385,

            [UndocumentedFeature]
            WM_PENCTL = 0x0385,

            [UndocumentedFeature]
            WM_PENMISC = 0x0386,

            [UndocumentedFeature]
            WM_CTLINIT = 0x0387,

            [UndocumentedFeature]
            WM_PENEVENT = 0x0388,

            //// Not defined 0x0389 - 0x038E

            WM_PENWINLAST = 0x038F,

            //// Not defined 0x0390 - 0x039F

            WM_USER = 0x0400,

            WM_APP = 0x8000
        }
    }
}