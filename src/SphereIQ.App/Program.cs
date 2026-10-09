using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace SphereIQ.App
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // Anything that escapes Avalonia (AvaloniaNative load failure, bad
            // apphost, missing Skia) dies before any window exists. On a .app
            // bundle launched from Finder, stdout is discarded - so capture it
            // to the log file and keep going, then let Avalonia log its own
            // unhandled exceptions there too.
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                CrashLog.WriteException("UnhandledException", e.ExceptionObject as Exception);
            };

            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                CrashLog.WriteException("UnobservedTask", e.Exception);
                e.SetObserved();
            };

            CrashLog.WriteEnvironment();

            string filePath = "";
            if (args != null)
            {
                foreach (string a in args)
                {
                    filePath = filePath + " " + a;
                }
                filePath = filePath.Trim();
            }
            CrashLog.Write("args", "count=" + (args == null ? 0 : args.Length) + " file='" + filePath + "'");

            bool createdNew;
            using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, "PCAVIATOR SphereIQ", out createdNew))
            {
                if (!createdNew)
                {
                    Console.WriteLine("SphereIQ is already running.");
                    return;
                }
                try
                {
                    BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
                    CrashLog.Write("shutdown", "clean exit");
                }
                catch (Exception ex)
                {
                    CrashLog.WriteException("Main", ex);
                    throw;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
        }
    }
}
