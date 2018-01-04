using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace NoSleep
{
    internal static class NativeMethods
    {
        [Flags]
        internal enum EXECUTION_STATE : uint
        {
            ES_CONTINUOUS = 0x80000000,
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_USER_PRESENT = 0x00000004,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
        }

        [DllImport("KERNEL32", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
    }
}
