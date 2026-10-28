using System;

namespace SphereIQ.Core.Data
{
    /// <summary>
    /// Fallback implementation of <see cref="IFileIconService"/> that returns
    /// null (no icon data). On macOS, this would be replaced with an
    /// implementation using <c>NSWorkspace.icon(forFile:)</c> via ObjC interop
    /// to return the actual file icon from the OS.
    /// </summary>
    public sealed class FileIconService : IFileIconService
    {
        /// <summary>
        /// Returns an icon for the given file path or extension.
        /// The fallback returns null; callers should use a generic placeholder
        /// icon when this returns null.
        /// </summary>
        /// <param name="filePathOrExtension">File path or extension (e.g. ".ezn").</param>
        /// <param name="desiredSize">Desired icon size in pixels (ignored by fallback).</param>
        /// <returns>PNG-encoded icon bytes, or null if no icon is available.</returns>
        public byte[]? GetIcon(string filePathOrExtension, int desiredSize = 32)
        {
            // Fallback: no icon data. The macOS implementation would use
            // NSWorkspace.icon(forFile:) via ObjC interop to return the
            // actual OS-provided icon for the file.
            return null;
        }
    }
}