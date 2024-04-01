using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Vanara.PInvoke;

namespace PickColor.Helpers;

internal static class ShortcutHelper
{
    public static void CreateShortcut(string directory, string shortcutName, string targetPath, string arguments = null!, string description = null!, string iconLocation = null!)
    {
        if (!Directory.Exists(directory))
        {
            _ = Directory.CreateDirectory(directory);
        }

        string shortcutPath = Path.Combine(directory, $"{shortcutName}.lnk");

        dynamic shell = null!;
        dynamic shortcut = null!;

        try
        {
            // Microsoft Visual C++ 2013 Redistributable
            shell = Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
            shortcut = shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = targetPath;
            shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);
            shortcut.WindowStyle = 1;
            shortcut.Arguments = arguments;
            shortcut.Description = description;
            shortcut.IconLocation = string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation;
            shortcut.Save();
        }
        finally
        {
            if (shortcut != null)
            {
                _ = Marshal.FinalReleaseComObject(shortcut);
            }
            if (shell != null)
            {
                _ = Marshal.FinalReleaseComObject(shell);
            }
        }
    }

    public static void AddToRecent(string targetPath)
    {
        Shell32.SHAddToRecentDocs(Shell32.SHARD.SHARD_PATHW, targetPath);
        if (Marshal.GetLastWin32Error() != 0)
        {
            _ = new Win32Exception(nameof(Shell32.SHAddToRecentDocs));
        }
    }

    public static void CreateStartMenuShortcut(string folderName, string targetPath)
    {
        if (RuntimeHelper.IsElevated)
        {
            string startMenuPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Microsoft\Windows\Start Menu\Programs");

            CreateShortcut(startMenuPath, folderName, targetPath);
            AddToRecent(Path.Combine(startMenuPath, $"{folderName}.lnk"));
        }
        else
        {
            string startMenuPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu), "Programs");
            CreateShortcut(startMenuPath, folderName, targetPath);
            AddToRecent(Path.Combine(startMenuPath, $"{folderName}.lnk"));
        }
    }
}
