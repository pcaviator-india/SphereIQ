using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using EzyNavMath;
using EzyPlanData;
using SphereIQ.App.Controls;
using SphereIQ.Reports;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Port of the legacy <c>Procedure_Extrapolate</c> +
    /// <c>Body_Extrapolate_Entry</c> + <c>Body_Extrapolate_Results</c> pair.
    /// <para>
    /// Two internal views:
    /// <list type="bullet">
    ///   <item><c>Entry</c> — enter target MD, dogleg, and toolface setting.</item>
    ///   <item><c>Results</c> — show extrapolated survey end point and chart.</item>
    /// </list>
    /// </para>
    /// </summary>
    public sealed class ExtrapolateScreen : IScreen
    {
        public ScreenId Id { get { return ScreenId.Extrapolate; } }
        public object Content { get; private set; }
        public event EventHandler<NavigateEventArgs> Navigate;
        public AppSession Session { get { return _session; } set { } }

        private readonly AppSession _session;
        private readonly StackPanel _root;
        private readonly ContentControl _viewHost;
        private readonly TextBlock _status;

        private Panel _entryView;
        private Panel _resultsView;

        // Entry view controls
        private TextBox _targetMD;
        private TextBox _dogleg;
        private TextBox _toolface;
        private Button _entryExtrapolate;
        private Button _entryCancel;

        // Results view controls
        private TextBlock _txtEndMD;
        private TextBlock _txtEndTVD;
        private TextBlock _txtEndNorthing;
        private TextBlock _txtEndEasting;
        private TextBlock _txtEndDip;
        private TextBlock _txtEndAzimuth;
        private TextBlock _txtEndDL;
        private TextBlock _txtEndToolface;
        private Button _resultsBack;
        private Button _resultsFinish;

        private ChartHost _entryChart;
        private ChartHost _resultsChart;

        private ExtrapolatedAbsoluteSurvey _extrapolated;
        private AbsoluteSurvey _lastSurvey;

        public ExtrapolateScreen(AppSession session)
        {
            _session = session;
            _root = new StackPanel { Margin = new Thickness(12), Spacing = 8 };
            _viewHost = new ContentControl();
            _status = new TextBlock { TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            _root.Children.Add(_status);
            _root.Children.Add(_viewHost);
            Content = _root;

            BuildEntryView();
            BuildResultsView();
            ShowEntry();
        }

        public void OnShown()
        {
            _targetMD.Text = "";
            _dogleg.Text = "3";
            _toolface.Text = "180";
            _status.Text = "Enter target MD, dogleg, and toolface to extrapolate.";
        }

        private void BuildEntryView()
        {
            _entryView = new StackPanel { Spacing = 10 };

            var heading = new TextBlock
            {
                Text = "Extrapolate Survey Data — Entry",
                FontSize = FontMap.ToDips(15.75),
                Margin = new Thickness(0, 0, 0, 8)
            };
            _entryView.Children.Add(heading);

            var instr = new TextBlock
            {
                Text = "Extrapolates the wellpath from the last survey to a target measured depth using constant dogleg and toolface.",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 12)
            };
            _entryView.Children.Add(instr);

            var mdRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 0, 0, 6) };
            mdRow.Children.Add(new TextBlock { Text = "Target MD:", VerticalAlignment = VerticalAlignment.Center, Width = 90 });
            _targetMD = new TextBox { Width = 120 };
            mdRow.Children.Add(_targetMD);
            mdRow.Children.Add(new TextBlock { Text = "Dogleg (°/30m):", VerticalAlignment = VerticalAlignment.Center, Width = 100 });
            _dogleg = new TextBox { Width = 100, Text = "3" };
            mdRow.Children.Add(_dogleg);
            mdRow.Children.Add(new TextBlock { Text = "Toolface (°):", VerticalAlignment = VerticalAlignment.Center, Width = 90 });
            _toolface = new TextBox { Width = 100, Text = "180" };
            mdRow.Children.Add(_toolface);
            _entryView.Children.Add(mdRow);

            _entryChart = new ChartHost { Height = 250, HorizontalAlignment = HorizontalAlignment.Stretch };
            _entryView.Children.Add(_entryChart);

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 12, 0, 0) };
            _entryExtrapolate = new Button { Content = "Extrapolate", Width = 120 };
            _entryExtrapolate.Click += (s, e) => Extrapolate();
            _entryCancel = new Button { Content = "Cancel", Width = 100 };
            _entryCancel.Click += (s, e) => Raise(ScreenId.HolePlanning, _session.CurrentFilePath);
            btnRow.Children.Add(_entryExtrapolate);
            btnRow.Children.Add(_entryCancel);
            _entryView.Children.Add(btnRow);
        }

        private void BuildResultsView()
        {
            _resultsView = new StackPanel { Spacing = 10 };

            var heading = new TextBlock
            {
                Text = "Extrapolate Survey Data — Results",
                FontSize = FontMap.ToDips(15.75),
                Margin = new Thickness(0, 0, 0, 8)
            };
            _resultsView.Children.Add(heading);

            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(GridLength.Star)
                },
                Margin = new Thickness(0, 0, 0, 12)
            };

            void AddRow(int row, string label, TextBlock value)
            {
                grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
                grid.Children.Add(new TextBlock { Text = label, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 8, 4) });
                Grid.SetColumn(grid.Children[^1], 0);
                Grid.SetRow(grid.Children[^1], row);
                grid.Children.Add(value);
                Grid.SetColumn(value, 1);
                Grid.SetRow(value, row);
            }

            _txtEndMD = new TextBlock { Text = "—" };
            _txtEndTVD = new TextBlock { Text = "—" };
            _txtEndNorthing = new TextBlock { Text = "—" };
            _txtEndEasting = new TextBlock { Text = "—" };
            _txtEndDip = new TextBlock { Text = "—" };
            _txtEndAzimuth = new TextBlock { Text = "—" };
            _txtEndDL = new TextBlock { Text = "—" };
            _txtEndToolface = new TextBlock { Text = "—" };

            AddRow(0, "End MD:", _txtEndMD);
            AddRow(1, "End TVD:", _txtEndTVD);
            AddRow(2, "End Northing:", _txtEndNorthing);
            AddRow(3, "End Easting:", _txtEndEasting);
            AddRow(4, "End Dip:", _txtEndDip);
            AddRow(5, "End Azimuth:", _txtEndAzimuth);
            AddRow(6, "Dog Leg:", _txtEndDL);
            AddRow(7, "Toolface:", _txtEndToolface);

            _resultsView.Children.Add(grid);

            _resultsChart = new ChartHost { Height = 250, HorizontalAlignment = HorizontalAlignment.Stretch };
            _resultsView.Children.Add(_resultsChart);

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 8, 0, 0) };
            _resultsBack = new Button { Content = "Back", Width = 100 };
            _resultsBack.Click += (s, e) => ShowEntry();
            _resultsFinish = new Button { Content = "Finish", Width = 100 };
            _resultsFinish.Click += (s, e) => Raise(ScreenId.HolePlanning, _session.CurrentFilePath);
            btnRow.Children.Add(_resultsBack);
            btnRow.Children.Add(_resultsFinish);
            _resultsView.Children.Add(btnRow);
        }

        private void ShowEntry()
        {
            _viewHost.Content = _entryView;
        }

        private void ShowResults()
        {
            _viewHost.Content = _resultsView;
            RenderResults();
        }

        // ------------------------------------------------------------------
        // Extrapolate calculation
        // ------------------------------------------------------------------

        private void Extrapolate()
        {
            if (_session == null || _session.HoleFile == null)
            {
                _status.Text = "No hole file open.";
                return;
            }

            var holeFile = _session.HoleFile;
            var collar = holeFile.Collar;
            var target = holeFile.Target;
            var surveyList = holeFile.SurveyList;

            if (collar == null || target == null || surveyList == null || surveyList.Count() == 0)
            {
                _status.Text = "Collar, target, or survey data is missing.";
                return;
            }

            if (!decimal.TryParse(_targetMD.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var targetMD))
            {
                _status.Text = "Invalid target MD.";
                return;
            }
            if (!decimal.TryParse(_dogleg.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var dl))
            {
                _status.Text = "Invalid dogleg.";
                return;
            }
            if (!decimal.TryParse(_toolface.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var tf))
            {
                _status.Text = "Invalid toolface.";
                return;
            }

            // Build absolute survey list
            var absList = new AbsoluteSurveyList();
            var navMath = new NavMath();
            var rc = navMath.AbsoluteSurvey(collar, surveyList, ref absList);
            if (rc != NavMath.AbsoluteSurveyEnum.NoError)
            {
                _status.Text = "Failed to build absolute survey list.";
                return;
            }

            _lastSurvey = absList.RecordAt(absList.Count() - 1);

            // Extrapolate to target MD
            var rc2 = navMath.ExtrapolateToMD(targetMD, dl, tf, _lastSurvey, out var extrapolated);
            if (rc2 != NavMath.ExtrapolateToMDResultEnum.NoError)
            {
                _status.Text = "Extrapolation failed: " + NavMath.ToHumanReadable(rc2);
                return;
            }

            _extrapolated = extrapolated;

            // Update entry chart with current survey
            var chartData = new HoleChartData();
            chartData.SetCollar(collar, visible: true);
            chartData.SetTarget(target, visible: true);
            chartData.SetSurveyData(absList, visible: true);
            _entryChart.ChartData = chartData;

            ShowResults();
        }

        private void RenderResults()
        {
            if (_extrapolated == null) return;

            _txtEndMD.Text = _extrapolated.MD.ToString("N2", CultureInfo.InvariantCulture);
            _txtEndTVD.Text = _extrapolated.TVD.ToString("N2", CultureInfo.InvariantCulture);
            _txtEndNorthing.Text = _extrapolated.Northing.ToString("N2", CultureInfo.InvariantCulture);
            _txtEndEasting.Text = _extrapolated.Easting.ToString("N2", CultureInfo.InvariantCulture);
            _txtEndDip.Text = _extrapolated.Dip.ToString("N2", CultureInfo.InvariantCulture);
            _txtEndAzimuth.Text = _extrapolated.Azimuth.ToString("N2", CultureInfo.InvariantCulture);
            _txtEndDL.Text = _extrapolated.DL.ToString("N2", CultureInfo.InvariantCulture);
            _txtEndToolface.Text = _extrapolated.Toolface.ToString("N2", CultureInfo.InvariantCulture);

            var chartData = new HoleChartData();
            var holeFile = _session.HoleFile;
            chartData.SetCollar(holeFile.Collar, visible: true);
            chartData.SetTarget(holeFile.Target, visible: true);
            var absList = new AbsoluteSurveyList();
            var navMath = new NavMath();
            navMath.AbsoluteSurvey(holeFile.Collar, holeFile.SurveyList, ref absList);
            chartData.SetSurveyData(absList, visible: true);
            if (_extrapolated.GetAbsoluteSurveyList() != null)
            {
                foreach (var s in _extrapolated.GetAbsoluteSurveyList())
                {
                    // Add extrapolated points to chart as UserExtrapolated series
                }
            }
            _resultsChart.ChartData = chartData;
        }

        // ------------------------------------------------------------------
        // Navigation
        // ------------------------------------------------------------------

        private void Raise(ScreenId destination, string filePath)
        {
            var handler = Navigate;
            if (handler != null)
            {
                handler(this, new NavigateEventArgs(destination, filePath));
            }
        }
    }
}