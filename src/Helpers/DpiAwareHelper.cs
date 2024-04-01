using System;
using Vanara.PInvoke;

namespace PickColor.Helpers;

internal static class DpiAwareHelper
{
    public static bool SetProcessDpiAwareness()
    {
        if (NtDll.RtlGetVersion(out NtDll.OSVERSIONINFOW osv) == NTStatus.STATUS_SUCCESS)
        {
            Version osVersion = new((int)osv.dwMajorVersion, (int)osv.dwMinorVersion, (int)osv.dwBuildNumber, (int)osv.dwPlatformId);

            if (Environment.OSVersion.Platform == PlatformID.Win32NT && osVersion >= new Version(6, 3))
            {
                if (SHCore.SetProcessDpiAwareness(SHCore.PROCESS_DPI_AWARENESS.PROCESS_PER_MONITOR_DPI_AWARE) == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
