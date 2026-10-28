using System;

namespace SphereIQ.Core.Data
{
    /// <summary>
    /// Abstraction for file-type icons, replacing the Windows-only
    /// <c>shell32!SHGetFileInfo</c> (<c>IconExtractor</c>).
    /// <para>
    /// macOS implementation uses <c>NSWorkspace.icon(forFile:)</c> via
    /// ObjC interop; Windows can use SHGetFileInfo; a generic per-extension
    /// fallback is available on all platforms.
    /// </para>
    /// </summary>
    public interface IFileIconService
    {
        /// <summary>
        /// Get an icon for the given file path or extension.
        /// Returns a byte array containing PNG-encoded icon data, or null
        /// if no icon is available.
        /// </summary>
        byte[] GetIcon(string filePathOrExtension, int desiredSize = 32);
    }
}