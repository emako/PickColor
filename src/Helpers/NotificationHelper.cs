using Microsoft.Toolkit.Uwp.Notifications;
using System;
using Vanara.PInvoke;

namespace PickColor.Helpers;

internal static class NotificationHelper
{
    public static bool IsValid
    {
        get
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                if (NtDll.RtlGetVersion(out NtDll.OSVERSIONINFOW osv) == NTStatus.STATUS_SUCCESS)
                {
                    if (new Version((int)osv.dwMajorVersion, (int)osv.dwMinorVersion, (int)osv.dwBuildNumber, (int)osv.dwPlatformId) >= new Version(10, 0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public static void AddNotice(string header, string title, string detail = null!, ToastDuration duration = ToastDuration.Short)
    {
        if (!IsValid)
        {
            return;
        }

        new ToastContentBuilder()
            .AddHeader("AddNotice", header, "AddNotice")
            .AddText(title)
            .AddAttributionTextIf(!string.IsNullOrEmpty(detail), detail)
            .SetToastDuration(duration)
            .Show();
    }

    public static void AddNoticeWithButton(string header, string title, string button, (string, string) arg, ToastDuration duration = ToastDuration.Short)
    {
        if (!IsValid)
        {
            return;
        }

        new ToastContentBuilder()
            .AddHeader("AddNotice", header, "AddNotice")
            .AddText(title)
            .AddButton(new ToastButton().SetContent(button).AddArgument(arg.Item1, arg.Item2).SetBackgroundActivation())
            .SetToastDuration(duration)
            .Show();
    }

    public static void ClearNotice()
    {
        if (!IsValid)
        {
            return;
        }

        try
        {
            ToastNotificationManagerCompat.History.Clear();
        }
        catch
        {
        }
    }
}

file static class ToastContentBuilderExtensions
{
    public static ToastContentBuilder AddAttributionTextIf(this ToastContentBuilder builder, bool condition, string text)
    {
        if (condition)
        {
            return builder.AddAttributionText(text);
        }
        else
        {
            return builder;
        }
    }
}
