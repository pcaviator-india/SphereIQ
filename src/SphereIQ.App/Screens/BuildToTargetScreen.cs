using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using EzyNavMath;
using EzyPlanData;
using EzyPlanMath;
using SphereIQ.App.Controls;
using SphereIQ.Reports;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Port of the legacy <c>Procedure_BuildToTarget</c> +
    /// <c>Body_BuildToTarget_Select</c> + <c>Body_BuildToTarget_Results</c> pair.
    /// Two internal views: Select (choose a build scenario) and Results
    /// (show calculated values). Math via NavMath/Math.BuildToTarget;
    /// chart rendering via ChartHost.
    /// </summary>
    public sealed class BuildToTargetScreen : IScreen
    {
        public ScreenId Id { get { return ScreenId.BuildToTarget; } }
        public object Content { get; private set; }
        public event EventHandler<NavigateEventArgs> Navigate;
        public AppSession Session { get { return _session; } set { } }

        private readonly AppSession _session;
        private readonly StackPanel _root;
        private readonly ContentControl _viewHost;
        private readonly TextBlock _status;

        private Panel _selectView;
        private Panel _resultsView;

        private DataGrid _buildGrid;
        private ComboBox _stepByCombo;
        private TextBlock _severeDLText;
        private TextBlock _extremeDLText;
        private Button _selectNext;
        private Button _selectCancel;

        private TextBlock _txtNaviStop;
        private TextBlock _txtNaviStart;
        private TextBlock _txtNaviDistance;
        private TextBlock _txtToolFace;
        private TextBlock _txtDogLeg;
        private TextBlock _txtStraightDistance;
        private Button _resultsBack;
        private Button _resultsFinish;

        private ChartHost _chart;

        private readonly List<BuildAndHoldResults> _buildList = new();
        private BuildAndHoldResults _selectedBuild;
        private AbsoluteSurvey _lastSurvey;

        private const decimal DefaultSevereDL = 3m;
        private const decimal DefaultExtremeDL = 7m;

        public BuildToTargetScreen(AppSession session)
        {
            _session = session;
            _root = new StackPanel { Margin = new Thickness(12), Spacing = 8 };
            _viewHost = new ContentControl();
            _status = new TextBlock { TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            _root.Children.Add(_status);
            _root.Children.Add(_viewHost);
            Content = _root;

            BuildSelectView();
            BuildResultsView();
            ShowSelect();
        }

        public void OnShown()
        {
            RecalculateBuilds();
        }

        // ------------------------------------------------------------------
        // View construction
        // ------------------------------------------------------------------

        private void BuildSelectView()
        {
            _selectView = new StackPanel { Spacing = 10 };

            var heading = new TextBlock
            {
                Text = "Build To Target — Select Build",
                FontSize = FontMap.ToDips(15.75),
                Margin = new Thickness(0, 0, 0, 8)
            };
            _selectView.Children.Add(heading);

            var stepRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 0, 0, 8) };
            stepRow.Children.Add(new TextBlock { Text = "Step by (dogleg):", VerticalAlignment = VerticalAlignment.Center });
            _stepByCombo = new ComboBox { Width = 100, VerticalAlignment = VerticalAlignment.Center };
            _stepByCombo.ItemsSource = new List<string> { "1", "2", "3", "5", "10" };
            _stepByCombo.SelectedIndex = 2;
            _stepByCombo.SelectionChanged += (s, e) => RecalculateBuilds();
            stepRow.Children.Add(_stepByCombo);
            _selectView.Children.Add(stepRow);

            var dlRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 16, Margin = new Thickness(0, 0, 0, 8) };
            _severeDLText = new TextBlock { Text = "Severe DL: 3.00", VerticalAlignment = VerticalAlignment.Center };
            _extremeDLText = new TextBlock { Text = "Extreme DL: 7.00", VerticalAlignment = VerticalAlignment.Center };
            dlRow.Children.Add(_severeDLText);
            dlRow.Children.Add(_extremeDLText);
            _selectView.Children.Add(dlRow);

            _buildGrid = new DataGrid
            {
                Height = 300,
                CanUserSortColumns = true,
                CanUserResizeColumns = true,
                AutoGenerateColumns = false,
                IsReadOnly = true,
                SelectionMode = DataGridSelectionMode.Single
            };
            _buildGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Dog Leg",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Binding = new Avalonia.Data.Binding("DogLeg") { StringFormat = "N2" }
            });
            _buildGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Build Length",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Binding = new Avalonia.Data.Binding("BuildLength") { StringFormat = "N2" }
            });
            _buildGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Straight Length",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                Binding = new Avalonia.Data.Binding("StraightLength") { StringFormat = "N2" }
            });
            _buildGrid.SelectionChanged += OnBuildGridSelectionChanged;
            _selectView.Children.Add(_buildGrid);

            _chart = new ChartHost
            {
                Height = 250,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            _selectView.Children.Add(_chart);

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 8, 0, 0) };
            _selectNext = new Button { Content = "Next", IsEnabled = false, Width = 100 };
            _selectNext.Click += (s, e) => ShowResults();
            _selectCancel = new Button { Content = "Cancel", Width = 100 };
            _selectCancel.Click += (s, e) => Raise(ScreenId.HolePlanning, _session.CurrentFilePath);
            btnRow.Children.Add(_selectNext);
            btnRow.Children.Add(_selectCancel);
            _selectView.Children.Add(btnRow);
        }

        private void BuildResultsView()
        {
            _resultsView = new StackPanel { Spacing = 10 };

            var heading = new TextBlock
            {
                Text = "Build To Target — Results",
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
                RowDefinitions =
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(GridLength.Auto)
                },
                Margin = new Thickness(0, 0, 0, 12)
            };

            void AddRow(int row, string label, TextBlock value)
            {
                grid.Children.Add(new TextBlock { Text = label, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 8, 4) });
                Grid.SetColumn(grid.Children[^1], 0);
                Grid.SetRow(grid.Children[^1], row);
                grid.Children.Add(value);
                Grid.SetColumn(value, 1);
                Grid.SetRow(value, row);
            }

            _txtNaviStop = new TextBlock { Text = "—" };
            _txtNaviStart = new TextBlock { Text = "—" };
            _txtNaviDistance = new TextBlock { Text = "—" };
            _txtToolFace = new TextBlock { Text = "—" };
            _txtDogLeg = new TextBlock { Text = "—" };
            _txtStraightDistance = new TextBlock { Text = "—" };

            AddRow(0, "Navi Stop (MD):", _txtNaviStop);
            AddRow(1, "Navi Start (MD):", _txtNaviStart);
            AddRow(2, "Navi Distance:", _txtNaviDistance);
            AddRow(3, "Tool Face (°):", _txtToolFace);
            AddRow(4, "Dog Leg (°/30m):", _txtDogLeg);
            AddRow(5, "Straight Distance:", _txtStraightDistance);

            _resultsView.Children.Add(grid);

            var chartHost = new ChartHost
            {
                Height = 250,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            _resultsView.Children.Add(chartHost);

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 8, 0, 0) };
            _resultsBack = new Button { Content = "Back", Width = 100 };
            _resultsBack.Click += (s, e) => ShowSelect();
            _resultsFinish = new Button { Content = "Finish", Width = 100 };
            _resultsFinish.Click += (s, e) => Raise(ScreenId.HolePlanning, _session.CurrentFilePath);
            btnRow.Children.Add(_resultsBack);
            btnRow.Children.Add(_resultsFinish);
            _resultsView.Children.Add(btnRow);
        }

        private void ShowSelect()
        {
            _viewHost.Content = _selectView;
            _status.Text = "Select a build scenario from the list.";
        }

        private void ShowResults()
        {
            if (_selectedBuild == null) return;
            _viewHost.Content = _resultsView;
            RenderResults();
            _status.Text = "Build results for the selected scenario.";
        }

        // ------------------------------------------------------------------
        // Build calculation
        // ------------------------------------------------------------------

        private void RecalculateBuilds()
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

            var absList = new AbsoluteSurveyList();
            var navMath = new NavMath();
            navMath.AbsoluteSurvey(collar, surveyList, ref absList);

            if (absList.Count() == 0)
            {
                _status.Text = "No absolute survey data.";
                return;
            }

            _lastSurvey = absList.RecordAt(absList.Count() - 1);

            string stepText = _stepByCombo.SelectedItem as string ?? "3";
            decimal step = decimal.TryParse(stepText, NumberStyles.Number, CultureInfo.InvariantCulture, out var s) ? s : 3m;

            _buildList.Clear();
            _buildGrid.ItemsSource = null;

            for (decimal dogLeg = step; dogLeg <= DefaultExtremeDL + step; dogLeg += step)
            {
                var results = new BuildAndHoldResults();
                var rc = NavMathBuildToTarget(dogLeg, _lastSurvey, target, ref results, 10.0);
                if (rc == BuildToTarget_Errors.NoError)
                {
                    _buildList.Add(results);
                }
            }

            _buildGrid.ItemsSource = _buildList;
            _buildGrid.SelectedIndex = -1;
            _selectNext.IsEnabled = false;
            _selectedBuild = null;

            var chartData = new HoleChartData();
            chartData.SetCollar(collar, visible: true);
            chartData.SetTarget(target, visible: true);
            chartData.SetSurveyData(absList, visible: true);
            _chart.ChartData = chartData;

            _severeDLText.Text = $"Severe DL: {DefaultSevereDL:N2}";
            _extremeDLText.Text = $"Extreme DL: {DefaultExtremeDL:N2}";

            _status.Text = $"{_buildList.Count} build scenario(s) generated.";
        }

        private static BuildToTarget_Errors NavMathBuildToTarget(
            decimal dogLeg, AbsoluteSurvey lastSurvey, Target target,
            ref BuildAndHoldResults results, double buildExtrapCD)
        {
            var plan = new global::EzyPlanMath.Math.ThreeDWellPlan();
            return plan.BuildToTarget((double)dogLeg, lastSurvey, target, ref results, buildExtrapCD);
        }

        private void OnBuildGridSelectionChanged(object sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            if (_buildGrid.SelectedItem is BuildAndHoldResults selected)
            {
                _selectedBuild = selected;
                _selectNext.IsEnabled = true;
            }
        }

        private void RenderResults()
        {
            if (_selectedBuild == null) return;

            _txtNaviStop.Text = _selectedBuild.NaviStopMVD.ToString("N2", CultureInfo.InvariantCulture);
            _txtNaviStart.Text = _selectedBuild.NaviStartMVD.ToString("N2", CultureInfo.InvariantCulture);
            _txtNaviDistance.Text = _selectedBuild.NaviDistance.ToString("N2", CultureInfo.InvariantCulture);
            _txtToolFace.Text = _selectedBuild.ToolFaceSetting.ToString("N2", CultureInfo.InvariantCulture);
            _txtDogLeg.Text = _selectedBuild.DogLeg.ToString("N2", CultureInfo.InvariantCulture);
            _txtStraightDistance.Text = _selectedBuild.StraightDistance.ToString("N2", CultureInfo.InvariantCulture);

            var chartData = new HoleChartData();
            var holeFile = _session.HoleFile;
            chartData.SetCollar(holeFile.Collar, visible: true);
            chartData.SetTarget(holeFile.Target, visible: true);
            if (_selectedBuild.ProjectedSurveyData != null)
            {
                chartData.SetSurveyData(_selectedBuild.ProjectedSurveyData, visible: true);
            }
            _chart.ChartData = chartData;
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