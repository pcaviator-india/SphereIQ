using System;
using Avalonia.Controls;

namespace SphereIQ.App.Screens
{
    public sealed class WelcomeScreen : IScreen
    {
        public ScreenId Id { get { return ScreenId.Welcome; } }

        public object Content { get; private set; }

        public event EventHandler<NavigateEventArgs> Navigate;

        public AppSession Session { get; set; }

        private Panel _root;

        public void OnShown()
        {
        }

        public WelcomeScreen()
        {
            Build();
        }

        private void Build()
        {
            _root = new StackPanel { Margin = new Avalonia.Thickness(24), Spacing = 12 };

            TextBlock heading = new TextBlock { Text = "Welcome to SphereIQ", FontSize = 28, Margin = new Avalonia.Thickness(0, 0, 0, 8) };
            _root.Children.Add(heading);

            string previous = GetPreviousFilePath();
            Button createNew = CreateButton("Create New Hole", () => Raise(ScreenId.CreateNewHole));
            Button openExisting = CreateButton("Open Existing Hole...", OnOpenExisting);
            Button openPrevious = CreateButton("Open Previous Hole", () =>
            {
                if (string.IsNullOrEmpty(previous))
                {
                    return;
                }
                if (Session != null)
                {
                    Session.CurrentFilePath = previous;
                }
                RaiseFile(ScreenId.HolePlanning, previous);
            });
            Button holeManagement = CreateButton("Hole Management", () => Raise(ScreenId.HoleManagement));

            openPrevious.IsEnabled = !string.IsNullOrEmpty(previous);

            _root.Children.Add(createNew);
            _root.Children.Add(openExisting);
            _root.Children.Add(openPrevious);
            _root.Children.Add(holeManagement);

            ScrollViewer scroll = new ScrollViewer { Content = _root };
            Grid wrapper = new Grid { Margin = new Avalonia.Thickness(40) };
            wrapper.Children.Add(scroll);
            Content = wrapper;
        }

        private void OnOpenExisting()
        {
            Raise(ScreenId.OpenHoleFile);
        }

        private Button CreateButton(string text, Action onClick)
        {
            Button b = new Button { Content = text, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch, Height = 40, Margin = new Avalonia.Thickness(0, 2) };
            b.Click += (s, e) => onClick();
            return b;
        }

        private string GetPreviousFilePath()
        {
            if (Session == null || string.IsNullOrEmpty(Session.PreviousFilePath))
            {
                return "";
            }
            if (String.Equals(Session.PreviousFilePath, Session.CurrentFilePath, StringComparison.Ordinal))
            {
                return "";
            }
            return Session.PreviousFilePath;
        }

        private void Raise(ScreenId destination)
        {
            RaiseFile(destination, Session != null ? Session.CurrentFilePath : "");
        }

        private void RaiseFile(ScreenId destination, string filePath)
        {
            NavigateEventArgs e = new NavigateEventArgs(destination, filePath);
            if (Navigate != null)
            {
                Navigate(this, e);
            }
        }
    }
}
