using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Password entry dialog popup.
    /// Port of the legacy <c>Popup_EnterPassword</c>.
    /// </summary>
    public sealed class PasswordScreen : Window
    {
        private readonly TextBox _passwordBox;
        private bool _okPressed;

        public PasswordScreen(string title = "Password", string message = "Enter password:")
        {
            Title = title;
            Width = 400;
            Height = 200;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CanResize = false;
            ShowInTaskbar = false;

            var panel = new StackPanel { Margin = new Thickness(24), Spacing = 16 };

            var messageBlock = new TextBlock { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            panel.Children.Add(messageBlock);

            _passwordBox = new TextBox { Watermark = "Password" };
            panel.Children.Add(_passwordBox);

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, HorizontalAlignment = HorizontalAlignment.Right };
            var btnCancel = new Button { Content = "Cancel", Width = 90 };
            btnCancel.Click += (s, e) => Close();
            var btnOk = new Button { Content = "OK", Width = 90 };
            btnOk.Click += (s, e) => { _okPressed = true; Close(); };
            btnRow.Children.Add(btnCancel);
            btnRow.Children.Add(btnOk);
            panel.Children.Add(btnRow);

            Content = panel;
        }

        public string Password => _passwordBox.Text;
        public bool OkPressed => _okPressed;
    }
}