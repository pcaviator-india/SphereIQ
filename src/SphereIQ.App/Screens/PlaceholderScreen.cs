using System;
using Avalonia.Controls;

namespace SphereIQ.App.Screens
{
    /// <summary>Stand-in for a not-yet-ported screen so navigation stays live.</summary>
    public sealed class PlaceholderScreen : IScreen
    {
        public ScreenId Id { get; private set; }
        public object Content { get; private set; }
        public event EventHandler<NavigateEventArgs> Navigate;
        public AppSession Session { get; set; }

        public PlaceholderScreen(ScreenId id)
        {
            Id = id;
            Content = new TextBlock
            {
                Text = "Screen \"" + id + "\" is not yet ported.",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                FontSize = 18,
                Opacity = 0.6
            };
        }

        public void OnShown()
        {
        }
    }
}
