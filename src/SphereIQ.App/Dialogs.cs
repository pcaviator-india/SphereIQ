using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace SphereIQ.App
{
    /// <summary>
    /// Minimal reusable modal dialogs replacing the legacy
    /// Popup_MessageBox_OK / Popup_MessageBox_YesNo WinForms dialogs.
    /// Synchronous callers (event handlers) use the Async variants.
    /// </summary>
    public static class Dialogs
    {
        public static Task ShowMessageAsync(Control owner, string title, string message)
        {
            Window window = CreateBase(title, message);
            StackPanel panel = (StackPanel)window.Content;
            StackPanel buttons = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, HorizontalAlignment = HorizontalAlignment.Right };
            Button ok = new Button { Content = "OK", Width = 90 };
            ok.Click += (s, e) => window.Close();
            buttons.Children.Add(ok);
            panel.Children.Add(buttons);
            return Show(window, TopLevel.GetTopLevel(owner));
        }

        public static Task<bool> ShowConfirmAsync(Control owner, string title, string message)
        {
            ConfirmBox box = new ConfirmBox();
            Window window = CreateBase(title, message);
            StackPanel panel = (StackPanel)window.Content;
            StackPanel buttons = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, HorizontalAlignment = HorizontalAlignment.Right };
            Button yes = new Button { Content = "Yes", Width = 90 };
            Button no = new Button { Content = "No", Width = 90 };
            yes.Click += (s, e) => { box.Result = true; window.Close(); };
            no.Click += (s, e) => { box.Result = false; window.Close(); };
            buttons.Children.Add(yes);
            buttons.Children.Add(no);
            panel.Children.Add(buttons);
            Task task = Show(window, TopLevel.GetTopLevel(owner));
            return task.ContinueWith(t => box.Result);
        }

        private sealed class ConfirmBox { public bool Result; }

        private static Window CreateBase(string title, string message)
        {
            StackPanel panel = new StackPanel { Margin = new Thickness(20), Spacing = 16 };
            panel.Children.Add(new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap, MaxWidth = 440 });
            Window window = new Window
            {
                Title = title,
                Width = 500,
                SizeToContent = SizeToContent.Height,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false,
                Content = panel
            };
            return window;
        }

        private static Task Show(Window window, TopLevel owner)
        {
            Window parent = owner as Window;
            if (parent != null)
            {
                return window.ShowDialog(parent);
            }
            window.Show();
            return Task.CompletedTask;
        }
    }
}
