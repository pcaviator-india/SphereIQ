using System;
using Avalonia.Controls;
using EzyPlanData;
using SphereIQ.App.Controls;
using SphereIQ.Reports;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Port of the legacy Body_DrillHolePlanning + Toolbar_DrillHolePlanning
    /// split: the chart region plus the read-only result labels, with a
    /// Mac-style action bar (Build / Advanced / Side Track / Extrapolate /
    /// Trend Analysis / Report / Back). All math runs through
    /// HolePlanningEngine; this screen only renders DrillHoleStatus.
    /// </summary>
    public sealed class DrillHolePlanningScreen : IScreen
    {
        private readonly AppSession _session;
        private readonly HolePlanningEngine _engine;

        private TextBlock _txtMinDl;
        private TextBlock _txtEndOfHole;
        private TextBlock _txtSurveyAtTarget;
        private TextBlock _txtDistanceFromTarget;
        private TextBlock _txtDistanceAtTargetTvd;
        private TextBlock _txtStraightPath;
        private ChartHost _chart;

        public ScreenId Id { get { return ScreenId.HolePlanning; } }

        public object Content { get; private set; }

        public event EventHandler<NavigateEventArgs> Navigate;

        public AppSession Session { get { return _session; } set { } }

        public DrillHolePlanningScreen(AppSession session)
        {
            _session = session;
            _engine = new HolePlanningEngine();
            Build();
        }

        public void OnShown()
        {
            Recalculate();
        }

        private void Build()
        {
            Grid root = new Grid();
            root.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            root.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            root.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

            root.Children.Add(BuildActionBar(root));
            Grid.SetRow(root.Children[0], 0);

            Grid body = BuildBody();
            root.Children.Add(body);
            Grid.SetRow(body, 1);

            Content = root;
        }

        private StackPanel BuildActionBar(Grid root)
        {
            StackPanel bar = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = 6,
                Margin = new Avalonia.Thickness(12, 8)
            };

            Button back = CreateButton("Hole Management", () => Raise(ScreenId.HoleManagement));
            bar.Children.Add(back);

            bar.Children.Add(new Separator { Width = 1, Opacity = 0.3, Margin = new Avalonia.Thickness(8, 4) });

            bar.Children.Add(CreateButton("Build", OnBuild));
            bar.Children.Add(CreateButton("Advanced", OnAdvanced));
            bar.Children.Add(CreateButton("Side Track Hole", () => Raise(ScreenId.SideTrackHole)));
            bar.Children.Add(CreateButton("Extrapolate", () => Raise(ScreenId.Extrapolate)));
            bar.Children.Add(CreateButton("Trend Analysis", () => Raise(ScreenId.TrendAnalysis)));
            bar.Children.Add(CreateButton("Report", () => Raise(ScreenId.Reports)));

            return bar;
        }

        private Grid BuildBody()
        {
            Grid body = new Grid
            {
                Margin = new Avalonia.Thickness(12, 0, 12, 12),
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Star),
                    new ColumnDefinition(GridLength.Auto)
                }
            };

            _chart = new ChartHost();
            if (_session != null)
            {
                _chart.ChartData = _session.Chart;
            }
            body.Children.Add(_chart);
            Grid.SetColumn(_chart, 0);

            body.Children.Add(BuildResultPanel());
            Grid.SetColumn(body.Children[1], 1);

            return body;
        }

        private StackPanel BuildResultPanel()
        {
            StackPanel panel = new StackPanel
            {
                Width = 260,
                Margin = new Avalonia.Thickness(12, 0, 0, 0),
                Spacing = 10
            };

            _txtMinDl = ResultBlock(panel, "Minimum Dogleg to Target");
            _txtEndOfHole = ResultBlock(panel, "End of Hole (MD / TVD)");
            _txtSurveyAtTarget = ResultBlock(panel, "Survey at Target TVD");
            _txtDistanceFromTarget = ResultBlock(panel, "Current Distance from Target");
            _txtDistanceAtTargetTvd = ResultBlock(panel, "Distance at Target TVD");
            _txtStraightPath = ResultBlock(panel, "Straight Path Distance");

            return panel;
        }

        private TextBlock ResultBlock(Panel panel, string caption)
        {
            panel.Children.Add(new TextBlock { Text = caption, FontSize = 12, Opacity = 0.7 });
            TextBlock value = new TextBlock { Text = "—", FontSize = 16, TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            panel.Children.Add(value);
            return value;
        }

        private Button CreateButton(string text, Action onClick)
        {
            Button b = new Button { Content = text, Margin = new Avalonia.Thickness(0, 0, 4, 0), Padding = new Avalonia.Thickness(12, 6) };
            b.Click += (s, e) => onClick();
            return b;
        }

        private void Recalculate()
        {
            if (_session == null || _session.HoleFile == null)
            {
                return;
            }
            DrillHoleStatus status = _engine.Recalculate(_session.HoleFile);
            _session.HoleStatus.CopyFrom(status);
            if (_chart != null)
            {
                _chart.Refresh();
            }
            Render(_session.HoleStatus);
        }

        private void Render(DrillHoleStatus s)
        {
            if (s == null)
            {
                return;
            }

            _txtMinDl.Text = string.IsNullOrEmpty(s.MinimumDLStateString) ? "—" : s.MinimumDLStateString;
            _txtMinDl.Foreground = SeverityBrush(s.MinimumDLSeverity);

            _txtEndOfHole.Text = Describe(s.EndOfHoleSurvey);
            _txtSurveyAtTarget.Text = Describe(s.SurveyAtTargetTVD);
            _txtDistanceFromTarget.Text = Describe(s.CurrentDistanceFromTarget);
            _txtDistanceAtTargetTvd.Text = Describe(s.DistanceFromTargetAtTargetTVD);
            _txtStraightPath.Text = Describe(s.StraightPathDistance);
        }

        private static Avalonia.Media.IBrush SeverityBrush(MinDLSeverity severity)
        {
            switch (severity)
            {
                case MinDLSeverity.Severe:
                    return Avalonia.Media.Brushes.Red;
                case MinDLSeverity.Warning:
                    return Avalonia.Media.Brushes.Orange;
                default:
                    return Avalonia.Media.Brushes.Black;
            }
        }

        private static string Describe(AbsoluteSurvey s)
        {
            if (s == null)
            {
                return "—";
            }
            return "MD " + s.MD + "  /  TVD " + s.TVD;
        }

        private static string Describe(RelativePosition p)
        {
            if (p == null)
            {
                return "—";
            }
            return p.NorthingString + "   " + p.EastingString + "   " + p.VerticalString + "   " + p.HorizontalString;
        }

        private void OnBuild()
        {
            Recalculate();
        }

        private void OnAdvanced()
        {
            Raise(ScreenId.BuildToTarget);
        }

        private void Raise(ScreenId destination)
        {
            string path = _session != null ? _session.CurrentFilePath : "";
            EventHandler<NavigateEventArgs> handler = Navigate;
            if (handler != null)
            {
                handler(this, new NavigateEventArgs(destination, path));
            }
        }
    }
}
