using System;
using System.Collections.Generic;

namespace SphereIQ.Core.Data
{
    /// <summary>
    /// Abstraction for persistent application settings, replacing
    /// <c>System.Configuration</c> / <c>My.Settings</c> which are not
    /// available on modern .NET / macOS.
    /// </summary>
    public interface ISettingsStore
    {
        T Get<T>(string key, T defaultValue);
        void Set<T>(string key, T value);
        bool Contains(string key);
        void Remove(string key);
        void Save();
    }

    /// <summary>
    /// JSON-based implementation of <see cref="ISettingsStore"/>.
    /// Settings are persisted to a user-specific JSON file:
    ///   macOS   ~/Library/Application Support/SphereIQ/settings.json
    ///   Windows %APPDATA%\\SphereIQ\\settings.json
    /// </summary>
    public sealed class JsonSettingsStore : ISettingsStore
    {
        private readonly string _path;
        private readonly Dictionary<string, object> _values = new();
        private bool _dirty;

        public JsonSettingsStore(string appName)
        {
            string dir;
            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                string home = Environment.GetEnvironmentVariable("HOME")
                    ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                dir = System.IO.Path.Combine(home, "Library", "Application Support", appName);
            }
            else
            {
                dir = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    appName);
            }

            System.IO.Directory.CreateDirectory(dir);
            _path = System.IO.Path.Combine(dir, "settings.json");
            Load();
        }

        private void Load()
        {
            if (!System.IO.File.Exists(_path)) return;
            try
            {
                string json = System.IO.File.ReadAllText(_path);
                var doc = System.Text.Json.JsonDocument.Parse(json);
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    _values[prop.Name] = prop.Value;
                }
            }
            catch
            {
                // Corrupt settings file - start fresh.
            }
        }

        public void Save()
        {
            if (!_dirty) return;
            try
            {
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    WriteIndented = true
                };
                System.IO.File.WriteAllText(_path,
                    System.Text.Json.JsonSerializer.Serialize(_values, options));
                _dirty = false;
            }
            catch
            {
                // Best effort - settings persistence is not critical.
            }
        }

        public T Get<T>(string key, T defaultValue)
        {
            if (_values.TryGetValue(key, out var raw))
            {
                try
                {
                    var json = raw.ToString();
                    return System.Text.Json.JsonSerializer.Deserialize<T>(json);
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        public void Set<T>(string key, T value)
        {
            _values[key] = value;
            _dirty = true;
        }

        public bool Contains(string key) => _values.ContainsKey(key);

        public void Remove(string key)
        {
            if (_values.Remove(key)) _dirty = true;
        }
    }
}