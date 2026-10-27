using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Working indicator / busy spinner popup for long-running operations.
    /// Port of the legacy <c>Popup_WorkingIndicator</c>.
    /// </summary>
    public sealed class WorkingIndicator : Window
    {
        private readonly ProgressBar _progress;
        private readonly TextBlock _message;
        private bool _isIndeterminate;

        public WorkingIndicator(string title = "Working...", string message = "Please wait...")
        {
            Title = title;
            Width = 400;
            Height = 150;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CanResize = false;
            ShowInTaskbar = false;

            var panel = new StackPanel { Margin = new Thickness(24), Spacing = 16 };

            _message = new TextBlock { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            panel.Children.Add(_message);

            _progress = new ProgressBar { IsIndeterminate = true, Height = 20 };
            panel.Children.Add(_progress);

            Content = panel;
        }

        public void SetMessage(string message)
        {
            _message.Text = message;
        }

        public void SetIndeterminate(bool value)
        {
            _isIndeterminate = value;
            _progress.IsIndeterminate = value;
            if (!value)
            {
                _progress.Value = 0;
            }
        }
    }
}