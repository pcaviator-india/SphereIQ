using System;
using System.IO;
using System.Text;

namespace SphereIQ.App
{
    /// <summary>
    /// Phase 4 groundwork (platform-independent part): a crash/diagnostic log
    /// written to the per-user application-support folder. Exists because the
    /// primary dev machine is Windows while the target is macOS arm64 — when a
    /// build misbehaves on the Mac we need the stack trace on disk, since
    /// stdout of a .app bundle launched from Finder is discarded.
    ///
    /// Log location (created on demand):
    ///   macOS   ~/Library/Application Support/SphereIQ/Logs/sphereiq.log
    ///   Windows %APPDATA%\SphereIQ\Logs\sphereiq.log
    /// </summary>
    public static class CrashLog
    {
        private static readonly object Gate = new object();
        private static string _path;

        /// <summary>Absolute path of the log file, computed once.</summary>
        public static string Path
        {
            get
            {
                if (_path == null)
                {
                    _path = ResolvePath();
                }
                return _path;
            }
        }

        private static string ResolvePath()
        {
            string dir;
            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                string home = Environment.GetEnvironmentVariable("HOME");
                if (string.IsNullOrEmpty(home))
                {
                    home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                }
                dir = System.IO.Path.Combine(home, "Library", "Application Support", "SphereIQ", "Logs");
            }
            else
            {
                dir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SphereIQ", "Logs");
            }

            try
            {
                Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("CrashLog: cannot create " + dir + " - " + ex.Message);
                return System.IO.Path.Combine(System.IO.Path.GetTempPath(), "sphereiq.log");
            }
            return System.IO.Path.Combine(dir, "sphereiq.log");
        }

        /// <summary>Appends a line: UTC timestamp, runtime info, message.</summary>
        public static void Write(string source, string message)
        {
            lock (Gate)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss'Z'"));
                sb.Append(" [");
                sb.Append(source);
                sb.Append("] ");
                sb.Append(message == null ? "(null)" : message);
                sb.Append(Environment.NewLine);

                Console.Error.Write(sb.ToString());

                try
                {
                    File.AppendAllText(Path, sb.ToString(), Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("CrashLog: write failed - " + ex.Message);
                }
            }
        }

        /// <summary>Logs an exception with full type, message and stack trace.</summary>
        public static void WriteException(string source, Exception ex)
        {
            if (ex == null)
            {
                Write(source, "(null exception)");
                return;
            }
            Write(source, ex.GetType().FullName + ": " + ex.Message + Environment.NewLine + ex.StackTrace);
        }

        /// <summary>One-time environment banner so we can tell builds apart.</summary>
        public static void WriteEnvironment()
        {
            Write("env",
                "SphereIQ " + typeof(CrashLog).Assembly.GetName().Version +
                " | runtime " + Environment.Version +
                " | os " + Environment.OSVersion +
                " | procarch " + System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture +
                " | 64bit " + Environment.Is64BitProcess +
                " | cwd " + Directory.GetCurrentDirectory());
        }
    }
}
