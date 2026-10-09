using System;
using Avalonia.Controls;
using EzyNavMath;
using EzyPlanData;
using EzyPlanStorage;
using SphereIQ.App.Controls;
using SphereIQ.Reports;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Slice 3: Hole management. Port of the legacy
    /// <c>Toolbar_DrillHoleManagement</c> + <c>Body_DrillHoleManagement</c>
    /// pair: the five-button side toolbar (Home / Edit Details / Edit Survey
    /// Data / Import / Hole Planning), the read-only hole-detail field block
    /// and the sidetrack group, plus the chart region driven by
    /// <c>DisplayChartData</c>. The ribbon toolbar becomes a plain vertical
    /// button strip; the Nevron chart is the OxyPlot <see cref="ChartHost"/>.
    /// </summary>
    public sealed class DrillHoleManagementScreen : IScreen
    {
        private readonly AppSession _session;

        private TextBlock _holeId;
        private TextBlock _rigId;
        private TextBlock _location;
        private TextBlock _area;
        private TextBlock _drillingContractor;
        private TextBlock _surveyingContractor;
        private TextBlock _client;
        private TextBlock _createdDate;
        private TextBlock _collarNorthing;
        private TextBlock _collarEasting;
        private TextBlock _targetNorthing;
        private TextBlock _targetEasting;
        private TextBlock _targetTvd;
        private TextBlock _sidetrackMd;
        private TextBlock _sidetrackTvd;
        private ChartHost _chart;
        private Control _sidetrackGroup;

        public ScreenId Id { get { return ScreenId.HoleManagement; } }

        public object Content { get; private set; }

        public event EventHandler<NavigateEventArgs> Navigate;

        public AppSession Session { get { return _session; } set { } }

        public DrillHoleManagementScreen(AppSession session)
        {
            _session = session;
            Build();
        }

        public void OnShown()
        {
            Display();
        }

        private void Build()
        {
            Grid root = new Grid();
            root.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            root.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));

            root.Children.Add(BuildToolbar());
            Grid.SetColumn(root.Children[0], 0);

            root.Children.Add(BuildBody());
            Grid.SetColumn(root.Children[1], 1);

            Content = root;
        }

        // Toolbar_DrillHoleManagement.ButtonPressedEnum, in the same order the
        // legacy ribbon stacked them (Home first, Hole Planning last).
        private StackPanel BuildToolbar()
        {
            StackPanel bar = new StackPanel
            {
                Width = 200,
                Spacing = 4,
                Margin = new Avalonia.Thickness(8, 12)
            };

            bar.Children.Add(CreateToolbarButton("Home", ScreenId.Welcome));
            bar.Children.Add(CreateToolbarButton("Edit Details", ScreenId.EditDetails));
            bar.Children.Add(CreateToolbarButton("Edit Survey Data", ScreenId.EditSurveyData));
            bar.Children.Add(CreateToolbarButton("Import Survey Data", ScreenId.Import));
            bar.Children.Add(CreateToolbarButton("Hole Planning", ScreenId.HolePlanning));

            return bar;
        }

        private Button CreateToolbarButton(string text, ScreenId destination)
        {
            Button b = new Button
            {
                Content = text,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                Padding = new Avalonia.Thickness(14, 10),
                Margin = new Avalonia.Thickness(0, 0, 0, 4)
            };
            b.Click += (s, e) => Raise(destination);
            return b;
        }

        private Grid BuildBody()
        {
            Grid body = new Grid { Margin = new Avalonia.Thickness(12) };
            body.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            body.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));

            _chart = new ChartHost();
            if (_session != null)
            {
                _chart.ChartData = _session.Chart;
            }
            body.Children.Add(_chart);
            Grid.SetColumn(_chart, 0);

            body.Children.Add(BuildDetailsPanel());
            Grid.SetColumn(body.Children[1], 1);

            return body;
        }

        private ScrollViewer BuildDetailsPanel()
        {
            StackPanel panel = new StackPanel { Width = 300, Spacing = 8, Margin = new Avalonia.Thickness(12, 0, 0, 0) };

            Section(panel, "Hole Details");
            _holeId = Field(panel, "Hole ID");
            _rigId = Field(panel, "Rig ID");
            _location = Field(panel, "Location");
            _area = Field(panel, "Area");
            _drillingContractor = Field(panel, "Drilling Contractor");
            _surveyingContractor = Field(panel, "Surveying Contractor");
            _client = Field(panel, "Client");
            _createdDate = Field(panel, "Created Date");

            Section(panel, "Collar");
            _collarNorthing = Field(panel, "Northing");
            _collarEasting = Field(panel, "Easting");

            Section(panel, "Target");
            _targetNorthing = Field(panel, "Northing");
            _targetEasting = Field(panel, "Easting");
            _targetTvd = Field(panel, "TVD");

            _sidetrackGroup = BuildSidetrackGroup();
            panel.Children.Add(_sidetrackGroup);

            return new ScrollViewer { Content = panel };
        }

        private static void Section(Panel panel, string caption)
        {
            panel.Children.Add(new TextBlock { Text = caption, FontSize = 15, Margin = new Avalonia.Thickness(0, 8, 0, 2) });
        }

        private TextBlock Field(Panel panel, string caption)
        {
            panel.Children.Add(new TextBlock { Text = caption, FontSize = 12, Opacity = 0.7 });
            TextBlock value = new TextBlock { Text = "-", TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            panel.Children.Add(value);
            return value;
        }

        // Body_DrillHoleManagement.groupBox_Sidetrack - hidden for mother holes.
        private Control BuildSidetrackGroup()
        {
            StackPanel group = new StackPanel { Spacing = 8, IsVisible = false };
            group.Children.Add(new TextBlock { Text = "Sidetrack", FontSize = 15, Margin = new Avalonia.Thickness(0, 8, 0, 2) });
            _sidetrackMd = Field(group, "MD");
            _sidetrackTvd = Field(group, "TVD");
            return group;
        }

        /// <summary>Body_DrillHoleManagement.Display.</summary>
        private void Display()
        {
            DisplayFields();
            DisplayCalculatedData();
        }

        /// <summary>Body_DrillHoleManagement.DisplayFields (verbatim field order).</summary>
        private void DisplayFields()
        {
            DrillHoleFile file = HoleFile();
            if (file == null)
            {
                return;
            }

            _holeId.Text = file.HoleID;
            _rigId.Text = file.RigID;
            _location.Text = file.Location;
            _area.Text = file.Area;
            _drillingContractor.Text = file.DrillingContractor;
            _surveyingContractor.Text = file.SurveyingContractor;
            _client.Text = file.Client;
            _createdDate.Text = file.CreatedDate.ToString("dd/MM/yyyy");

            Collar collar = file.Collar;
            if (collar != null)
            {
                _collarNorthing.Text = collar.Northing.ToString();
                _collarEasting.Text = collar.Easting.ToString();
            }

            Target target = file.Target;
            if (target != null)
            {
                _targetNorthing.Text = target.Northing.ToString();
                _targetEasting.Text = target.Easting.ToString();
                _targetTvd.Text = target.TVD.ToString();
            }
        }

        /// <summary>
        /// Body_DrillHoleManagement.DisplayCalculatedData: builds the absolute
        /// survey list then branches on the hole type. The legacy modal popup
        /// for the "no survey at sidetrack" case becomes an awaited
        /// <see cref="Dialogs.ShowMessageAsync"/> so the chart still renders.
        /// </summary>
        private void DisplayCalculatedData()
        {
            DrillHoleFile file = HoleFile();
            if (file == null || file.Collar == null || file.Target == null || _session == null)
            {
                return;
            }

            Collar collar = file.Collar;
            Target target = file.Target;
            SurveyList surveyList = file.SurveyList;

            AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
            NavMath navMath = new NavMath();
            navMath.AbsoluteSurvey(collar, surveyList, ref absoluteSurveyList);

            HoleChartData chart = _session.Chart;
            AbsoluteSurvey extrapolatedSurvey;

            switch (file.DrillHoleType)
            {
                case DrillHoleFile.DrillHoleTypeEnum.motherHole:
                    HideSidetrackFields();
                    DisplayChartData(chart, collar, target, absoluteSurveyList, null);
                    break;

                case DrillHoleFile.DrillHoleTypeEnum.daughterHole:
                    navMath.SideTrackHole(absoluteSurveyList, file.SideTrackMD, out extrapolatedSurvey);
                    DisplaySidetrackFields(file.SideTrackMD, extrapolatedSurvey != null ? extrapolatedSurvey.TVD.ToString() : "");
                    DisplayChartData(chart, collar, target, absoluteSurveyList, extrapolatedSurvey);
                    break;

                default:
                    navMath.SideTrackHole(absoluteSurveyList, file.SideTrackMD, out extrapolatedSurvey);
                    DisplaySidetrackFields(file.SideTrackMD, "No survey data");
                    DisplayChartData(chart, collar, target, absoluteSurveyList, null);
                    WarnNoSurveyAtSidetrack(file.SideTrackMD);
                    break;
            }
        }

        /// <summary>Body_DrillHoleManagement.DisplayChartData (sidetrack overload).</summary>
        private void DisplayChartData(HoleChartData chart, Collar collar, Target target, AbsoluteSurveyList surveyData, AbsoluteSurvey sidetrackStart)
        {
            chart.ClearChartData();
            chart.SetTarget(target, visible: true);
            chart.SetCollar(collar, visible: true);
            chart.SetSurveyData(surveyData, visible: true);
            chart.SetHolePlan(sidetrackStart, target, visible: true);
            chart.SetSideTrack(sidetrackStart, visible: true);

            if (_chart != null)
            {
                _chart.Refresh();
            }
        }

        private void DisplaySidetrackFields(decimal sidetrackMD, string sidetrackTVDText)
        {
            _sidetrackGroup.IsVisible = true;
            _sidetrackTvd.Text = sidetrackTVDText;
            _sidetrackMd.Text = sidetrackMD.ToString();
        }

        private void HideSidetrackFields()
        {
            _sidetrackGroup.IsVisible = false;
            _sidetrackTvd.Text = "";
            _sidetrackMd.Text = "";
        }

        private async void WarnNoSurveyAtSidetrack(decimal sideTrackMD)
        {
            Control owner = Content as Control;
            if (owner == null)
            {
                return;
            }
            await Dialogs.ShowMessageAsync(owner,
                "Daughter hole plan error!",
                "There is no survey data at the MD of the sidetrack. The drilling plan cannot be displayed until the survey data reaches a MD of " + sideTrackMD + " meters.");
        }

        private DrillHoleFile HoleFile()
        {
            return _session != null ? _session.HoleFile : null;
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

