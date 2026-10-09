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
            string filePath = "";
            if (args != null)
            {
                foreach (string a in args)
                {
                    filePath = filePath + " " + a;
                }
                filePath = filePath.Trim();
            }

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
