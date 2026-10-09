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
                MainWindow window = new MainWindow(Session);
                desktop.MainWindow = window;
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}
