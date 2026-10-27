using System;
using System.Collections.Generic;
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
    /// Port of the legacy <c>Procedure_TrendAnalysis</c> +
    /// <c>Body_TrendAnalysis_SelectRange</c> + <c>Body_TrendAnalysis_Results</c> pair.
    /// <para>
    /// Two internal views:
    /// <list type="bullet">
    ///   <item><c>Range Select</c> — pick the MD range for trend analysis.</item>
    ///   <item><c>Results</c> — show trend statistics (dogleg, build/turn rates, tool face) and chart.</item>
    /// </list>
    /// </para>
    /// </summary>
    public sealed class TrendAnalysisScreen : IScreen
    {
        public ScreenId Id { get { return ScreenId.TrendAnalysis; } }
        public object Content { get; private set; }
        public event EventHandler<NavigateEventArgs> Navigate;
        public AppSession Session { get { return _session; } set { } }

        private readonly AppSession _session;
        private readonly StackPanel _root;
        private readonly ContentControl _viewHost;
        private readonly TextBlock _status;

        private Panel _rangeView;
        private Panel _resultsView;

        // Range view controls
        private TextBox _startMD;
        private TextBox _endMD;
        private Button _rangeAnalyze;
        private Button _rangeCancel;

        // Results view controls
        private TextBlock _txtDogLeg;
        private TextBlock _txtBuildRate;
        private TextBlock _txtTurnRate;
        private TextBlock _txtToolFace;
        private TextBlock _txtAngleChange;
        private TextBlock _txtAzimuthTrend;
        private TextBlock _txtInclinationTrend;
        private Button _resultsBack;
        private Button _resultsFinish;

        private ChartHost _chart;
        private ChartHost _resultsChart;

        private TrendAnalysisResults _results;
        private AbsoluteSurvey _endOfRealHoleSurvey;

        private const decimal DefaultSevereDL = 3m;
        private const decimal DefaultExtremeDL = 7m;

        public TrendAnalysisScreen(AppSession session)
        {
            _session = session;
            _root = new StackPanel { Margin = new Thickness(12), Spacing = 8 };
            _viewHost = new ContentControl();
            _status = new TextBlock { TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            _root.Children.Add(_status);
            _root.Children.Add(_viewHost);
            Content = _root;

            BuildRangeView();
            BuildResultsView();
            ShowRange();
        }

        public void OnShown()
        {
            _startMD.Text = "0";
            _endMD.Text = "";
            _status.Text = "Enter the MD range for trend analysis.";
        }

        // ------------------------------------------------------------------
        // View construction
        // ------------------------------------------------------------------

        private void BuildRangeView()
        {
            _rangeView = new StackPanel { Spacing = 10 };

            var heading = new TextBlock
            {
                Text = "Trend Analysis — Select Range",
                FontSize = FontMap.ToDips(15.75),
                Margin = new Thickness(0, 0, 0, 8)
            };
            _rangeView.Children.Add(heading);

            var instr = new TextBlock
            {
                Text = "Enter the measured depth (MD) range over which to calculate the wellpath trend.",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 12)
            };
            _rangeView.Children.Add(instr);

            var mdRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 0, 0, 8) };
            mdRow.Children.Add(new TextBlock { Text = "Start MD:", VerticalAlignment = VerticalAlignment.Center, Width = 90 });
            _startMD = new TextBox { Width = 120, Text = "0" };
            mdRow.Children.Add(_startMD);
            mdRow.Children.Add(new TextBlock { Text = "End MD:", VerticalAlignment = VerticalAlignment.Center, Width = 80 });
            _endMD = new TextBox { Width = 120 };
            mdRow.Children.Add(_endMD);
            _rangeView.Children.Add(mdRow);

            _chart = new ChartHost
            {
                Height = 250,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            _rangeView.Children.Add(_chart);

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 12, 0, 0) };
            _rangeAnalyze = new Button { Content = "Analyze", Width = 100 };
            _rangeAnalyze.Click += (s, e) => AnalyzeRange();
            _rangeCancel = new Button { Content = "Cancel", Width = 100 };
            _rangeCancel.Click += (s, e) => Raise(ScreenId.HolePlanning, _session.CurrentFilePath);
            btnRow.Children.Add(_rangeAnalyze);
            btnRow.Children.Add(_rangeCancel);
            _rangeView.Children.Add(btnRow);
        }

        private void BuildResultsView()
        {
            _resultsView = new StackPanel { Spacing = 10 };

            var heading = new TextBlock
            {
                Text = "Trend Analysis — Results",
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

            _txtDogLeg = new TextBlock { Text = "—" };
            _txtBuildRate = new TextBlock { Text = "—" };
            _txtTurnRate = new TextBlock { Text = "—" };
            _txtToolFace = new TextBlock { Text = "—" };
            _txtAngleChange = new TextBlock { Text = "—" };
            _txtAzimuthTrend = new TextBlock { Text = "—" };
            _txtInclinationTrend = new TextBlock { Text = "—" };

            AddRow(0, "Dog Leg (°/30m):", _txtDogLeg);
            AddRow(1, "Build Rate (°/30m):", _txtBuildRate);
            AddRow(2, "Turn Rate (°/30m):", _txtTurnRate);
            AddRow(3, "Tool Face (°):", _txtToolFace);
            AddRow(4, "Angle Change (°):", _txtAngleChange);
            AddRow(5, "Azimuth Trend:", _txtAzimuthTrend);
            AddRow(6, "Inclination Trend:", _txtInclinationTrend);

            _resultsView.Children.Add(grid);

            _resultsChart = new ChartHost
            {
                Height = 250,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            _resultsView.Children.Add(_resultsChart);

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 8, 0, 0) };
            _resultsBack = new Button { Content = "Back", Width = 100 };
            _resultsBack.Click += (s, e) => ShowRange();
            _resultsFinish = new Button { Content = "Finish", Width = 100 };
            _resultsFinish.Click += (s, e) => Raise(ScreenId.HolePlanning, _session.CurrentFilePath);
            btnRow.Children.Add(_resultsBack);
            btnRow.Children.Add(_resultsFinish);
            _resultsView.Children.Add(btnRow);
        }

        private void ShowRange()
        {
            _viewHost.Content = _rangeView;
        }

        private void ShowResults()
        {
            _viewHost.Content = _resultsView;
            RenderResults();
        }

        // ------------------------------------------------------------------
        // Trend analysis calculation
        // ------------------------------------------------------------------

        private void AnalyzeRange()
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

            if (collar == null || target == null || surveyList == null || surveyList.Count() < 2)
            {
                _status.Text = "Need collar, target, and at least 2 surveys.";
                return;
            }

            if (!decimal.TryParse(_startMD.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var startMD))
            {
                _status.Text = "Invalid start MD.";
                return;
            }
            if (!decimal.TryParse(_endMD.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var endMD))
            {
                _status.Text = "Invalid end MD.";
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

            // Filter to the selected MD range
            var rangeList = new SortableBindingList<AbsoluteSurvey>();
            foreach (var s in absList)
            {
                if (s.MD >= startMD && s.MD <= endMD)
                {
                    rangeList.Add(s);
                }
            }

            if (rangeList.Count < 2)
            {
                _status.Text = "Range contains fewer than 2 surveys.";
                return;
            }

            var endOfRealHoleSurvey = new AbsoluteSurvey();
            var trendResults = new TrendAnalysisResults();
            var trendRc = navMath.TrendAnalysis(target, rangeList, ref trendResults, ref endOfRealHoleSurvey);

            if (trendRc != NavMath.TrendAnalysisEnum.Success)
            {
                _status.Text = "Trend analysis failed: " + NavMath.ToHumanReadable(trendRc);
                return;
            }

            _results = trendResults;
            _endOfRealHoleSurvey = endOfRealHoleSurvey;

            // Update chart with range surveys
            var chartData = new HoleChartData();
            chartData.SetCollar(collar, visible: true);
            chartData.SetTarget(target, visible: true);
            chartData.SetSurveyData(absList, visible: true);
            _chart.ChartData = chartData;

            ShowResults();
        }

        private void RenderResults()
        {
            if (_results == null) return;

            _txtDogLeg.Text = _results.DogLegSeverity.ToString("N2", CultureInfo.InvariantCulture);
            _txtBuildRate.Text = _results.BuildRate.ToString("N2", CultureInfo.InvariantCulture);
            _txtTurnRate.Text = _results.TurnRate.ToString("N2", CultureInfo.InvariantCulture);
            _txtToolFace.Text = _results.ToolFaceSetting.ToString("N2", CultureInfo.InvariantCulture);
            _txtAngleChange.Text = _results.AngleChange.ToString("N2", CultureInfo.InvariantCulture);
            _txtAzimuthTrend.Text = _results.AzimuthTrend.ToString();
            _txtInclinationTrend.Text = _results.InclinationTrend.ToString();

            // Update results chart with extrapolated trend data
            var chartData = new HoleChartData();
            var holeFile = _session.HoleFile;
            chartData.SetCollar(holeFile.Collar, visible: true);
            chartData.SetTarget(holeFile.Target, visible: true);
            if (_results.ExtrapolatedSurveyData != null && _results.ExtrapolatedSurveyData.Count() > 0)
            {
                chartData.SetSurveyData(_results.ExtrapolatedSurveyData, visible: true);
            }
            if (_resultsChart != null)
            {
                _resultsChart.ChartData = chartData;
            }
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