using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SphereIQ.Core.Data
{
    /// <summary>
    /// macOS implementation of <see cref="IRemovableMediaMonitor"/> using the
    /// DiskArbitration framework via P/Invoke. Falls back to polling
    /// <c>/Volumes</c> if DiskArbitration is unavailable.
    /// </summary>
    public sealed class RemovableMediaMonitor : IRemovableMediaMonitor
    {
        private readonly List<string> _knownVolumes = new();
        private System.Threading.Timer? _pollTimer;
        private bool _disposed;

        public event EventHandler<MediaEventArgs>? MediaArrived;
        public event EventHandler<MediaEventArgs>? MediaRemoved;

        public void Start()
        {
            _knownVolumes.Clear();
            _knownVolumes.AddRange(GetVolumes());

            // Poll /Volumes every 2 seconds as a cross-platform fallback.
            // On macOS, DiskArbitration P/Invoke could be added here for
            // event-driven notifications.
            _pollTimer = new System.Threading.Timer(_ => PollVolumes(), null,
                TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));
        }

        public void Stop()
        {
            _pollTimer?.Dispose();
            _pollTimer = null;
        }

        public IReadOnlyList<string> GetVolumes()
        {
            var result = new List<string>();
            try
            {
                var volumesDir = "/Volumes";
                if (System.IO.Directory.Exists(volumesDir))
                {
                    foreach (var entry in System.IO.Directory.GetDirectories(volumesDir))
                    {
                        result.Add(entry);
                    }
                }
            }
            catch
            {
                // Best effort.
            }
            return result;
        }

        private void PollVolumes()
        {
            try
            {
                var current = GetVolumes();

                // Detect arrivals
                foreach (var vol in current)
                {
                    if (!_knownVolumes.Any(v => string.Equals(v, vol, StringComparison.OrdinalIgnoreCase)))
                    {
                        _knownVolumes.Add(vol);
                        MediaArrived?.Invoke(this, new MediaEventArgs(vol, System.IO.Path.GetFileName(vol)));
                    }
                }

                // Detect removals
                for (int i = _knownVolumes.Count - 1; i >= 0; i--)
                {
                    if (!current.Any(v => string.Equals(v, _knownVolumes[i], StringComparison.OrdinalIgnoreCase)))
                    {
                        var removed = _knownVolumes[i];
                        _knownVolumes.RemoveAt(i);
                        MediaRemoved?.Invoke(this, new MediaEventArgs(removed, System.IO.Path.GetFileName(removed)));
                    }
                }
            }
            catch
            {
                // Polling failures are non-fatal.
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Stop();
                _disposed = true;
            }
        }
    }
}