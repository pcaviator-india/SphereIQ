using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace SphereIQ.App
{
    public class App : Application
    {
        public AppSession Session { get; } = new AppSession();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avalonia's own logger (platform init, XAML parse, renderer
                // warnings) goes to the same file, so a blank window on macOS
                // still leaves evidence behind.
                Avalonia.Logging.Logger.Sink = new CrashLogSink();

                // Avalonia 11: UI-thread exceptions surface on the Dispatcher,
                // not on the lifetime. Handled=true keeps the app alive so the
                // rest of the session is still testable on the Mac.
                Avalonia.Threading.Dispatcher.UIThread.UnhandledException += (s, e) =>
                {
                    CrashLog.WriteException("UI", e.Exception);
                    e.Handled = true;
                };

                desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnMainWindowClose;

                MainWindow window = new MainWindow(Session);
                desktop.MainWindow = window;
                CrashLog.Write("ui", "MainWindow created");
            }
            base.OnFrameworkInitializationCompleted();
        }

        /// <summary>
        /// Routes <see cref="Avalonia.Logging.Logger"/> output into
        /// <see cref="CrashLog"/> without a logging dependency.
        /// </summary>
        private sealed class CrashLogSink : Avalonia.Logging.ILogSink
        {
            public bool IsEnabled(Avalonia.Logging.LogEventLevel level, string area)
            {
                return level >= Avalonia.Logging.LogEventLevel.Warning;
            }

            public void Log(Avalonia.Logging.LogEventLevel level, string area, object source, string messageTemplate)
            {
                CrashLog.Write("avalonia", level + " (" + area + "): " + messageTemplate);
            }

            public void Log(Avalonia.Logging.LogEventLevel level, string area, object source, string messageTemplate, params object[] propertyValues)
            {
                CrashLog.Write("avalonia", level + " (" + area + "): " + messageTemplate);
            }
        }
    }
}
