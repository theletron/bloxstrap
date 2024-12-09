using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Bloxstrap.Utility
{
    internal static class Filesystem
    {
        internal static long GetFreeDiskSpace(string path)
        {
            try
            {
                var isUri = Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var u);
                if (!Path.IsPathRooted(path) || !Path.IsPathFullyQualified(path) || (isUri && (u?.IsUnc ?? false)))
                {
                    return -1;
                }
                var drive = new DriveInfo(path);
                return drive.AvailableFreeSpace;
            }
            catch (ArgumentException)
            {
                App.Logger.WriteLine("Filesystem::BadPath", $"The path: {path} does not contain a valid drive info.");
                return -1;
            }
            catch (IOException e)
            {
                App.Logger.WriteLine("Filesystem::IOException", $"An I/O error occurred while accessing the path: {path}. Exception: {e.Message}");
                return -1;
            }
            catch (UnauthorizedAccessException e)
            {
                App.Logger.WriteLine("Filesystem::UnauthorizedAccess", $"Access to the path: {path} is denied. Exception: {e.Message}");
                return -1;
            }
        }

        internal static void AssertReadOnly(string filePath)
        {
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists || !fileInfo.IsReadOnly)
                return;

            fileInfo.IsReadOnly = false;
            App.Logger.WriteLine("Filesystem::AssertReadOnly", $"The following file was set as read-only: {filePath}");
        }
    }
}