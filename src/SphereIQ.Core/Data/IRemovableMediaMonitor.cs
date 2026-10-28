using System;
using System.Collections.Generic;

namespace SphereIQ.Core.Data
{
    /// <summary>
    /// Abstraction for removable media (USB drive) mount/unmount notifications.
    /// Replaces the Windows-only <c>DriveDetector</c> (user32 P/Invoke).
    /// <para>
    /// macOS implementation uses DiskArbitration framework; Windows can use
    /// WM_DEVICECHANGE; a polling fallback is available on all platforms.
    /// </para>
    /// </summary>
    public interface IRemovableMediaMonitor : IDisposable
    {
        event EventHandler<MediaEventArgs> MediaArrived;
        event EventHandler<MediaEventArgs> MediaRemoved;

        /// <summary>Currently mounted removable volumes (e.g. "/Volumes/USB-DISK").</summary>
        IReadOnlyList<string> GetVolumes();

        /// <summary>Start monitoring for mount/unmount events.</summary>
        void Start();

        /// <summary>Stop monitoring.</summary>
        void Stop();
    }

    /// <summary>Event arguments for removable media events.</summary>
    public sealed class MediaEventArgs : EventArgs
    {
        public string VolumePath { get; }
        public string VolumeLabel { get; }

        public MediaEventArgs(string volumePath, string volumeLabel)
        {
            VolumePath = volumePath;
            VolumeLabel = volumeLabel;
        }
    }
}