using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using EzyNavMath;
using EzyPlanData;
using EzyPlanStorage;
using SphereIQ.App.Controls;
using SphereIQ.Reports;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Port of the legacy <c>Procedure_SideTrackHole</c> +
    /// <c>Body_SideTrackHole</c> pair.
    /// <para>
    /// Two internal views:
    /// <list type="bullet">
    ///   <item><c>CreateSideTrack</c> — enter sidetrack MD, target easting/northing/TVD.</item>
    ///   <item><c>SaveFile</c> — save the daughter hole to a new .ezn file.</item>
    /// </list>
    /// </para>
    /// </summary>
    public sealed class SideTrackHoleScreen : IScreen
    {
        public ScreenId Id { get { return ScreenId.SideTrackHole; } }
        public object Content { get; private set; }
        public event EventHandler<NavigateEventArgs> Navigate;
        public AppSession Session { get { return _session; } set { } }

        private readonly AppSession _session;
        private readonly StackPanel _root;
        private readonly ContentControl _viewHost;
        private readonly TextBlock _status;

        private Panel _createView;
        private Panel _saveView;

        // Create view controls
        private TextBox _sidetrackMD;
        private TextBox _targetEasting;
        private TextBox _targetNorthing;
        private TextBox _targetTVD;
        private TextBox _holeID;
        private TextBox _rigID;
        private TextBox _location;
        private TextBox _area;
        private TextBox _client;
        private TextBox _drillingContractor;
        private TextBox _surveyingContractor;
        private Button _createNext;
        private Button _createCancel;

        // Save view controls
        private TextBox _saveHoleID;
        private TextBox _saveFileName;
        private Button _saveSave;
        private Button _saveBack;

        private ChartHost _chart;

        private DrillHoleFile _sideTrackHole;
        private DrillHoleFile _originalHole;

        private const decimal DefaultSevereDL = 3m;
        private const decimal DefaultExtremeDL = 7m;

        public SideTrackHoleScreen(AppSession session)
        {
            _session = session;
            _root = new StackPanel { Margin = new Thickness(12), Spacing = 8 };
            _viewHost = new ContentControl();
            _status = new TextBlock { TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            _root.Children.Add(_status);
            _root.Children.Add(_viewHost);
            Content = _root;

            BuildCreateView();
            BuildSaveView();
            ShowCreate();
        }

        public void OnShown()
        {
            if (_session == null || _session.HoleFile == null)
            {
                _status.Text = "No hole file open.";
                return;
            }
            _originalHole = _session.HoleFile;
            _sideTrackHole = new DrillHoleFile();
            _sideTrackHole.HoleID = _originalHole.HoleID;
            _sideTrackHole.RigID = _originalHole.RigID;
            _sideTrackHole.Location = _originalHole.Location;
            _sideTrackHole.Area = _originalHole.Area;
            _sideTrackHole.Client = _originalHole.Client;
            _sideTrackHole.DrillingContractor = _originalHole.DrillingContractor;
            _sideTrackHole.SurveyingContractor = _originalHole.SurveyingContractor;
            _sideTrackHole.Collar = _originalHole.Collar;
            if (_originalHole.Target != null)
            {
                _sideTrackHole.Target = new Target(_originalHole.Target.Easting, _originalHole.Target.Northing, _originalHole.Target.TVD);
            }
            _sideTrackHole.SurveyList = _originalHole.SurveyList;

            _holeID.Text = _sideTrackHole.HoleID;
            _rigID.Text = _sideTrackHole.RigID;
            _location.Text = _sideTrackHole.Location;
            _area.Text = _sideTrackHole.Area;
            _client.Text = _sideTrackHole.Client;
            _drillingContractor.Text = _sideTrackHole.DrillingContractor;
            _surveyingContractor.Text = _sideTrackHole.SurveyingContractor;
            _sidetrackMD.Text = "";
            _targetEasting.Text = _sideTrackHole.Target?.Easting.ToString(CultureInfo.InvariantCulture) ?? "";
            _targetNorthing.Text = _sideTrackHole.Target?.Northing.ToString(CultureInfo.InvariantCulture) ?? "";
            _targetTVD.Text = _sideTrackHole.Target?.TVD.ToString(CultureInfo.InvariantCulture) ?? "";

            _status.Text = "Enter sidetrack depth and target coordinates.";
            UpdateChart();
        }

        private void BuildCreateView()
        {
            _createView = new StackPanel { Spacing = 10 };

            var heading = new TextBlock
            {
                Text = "Sidetrack Hole — Create",
                FontSize = FontMap.ToDips(15.75),
                Margin = new Thickness(0, 0, 0, 8)
            };
            _createView.Children.Add(heading);

            var instr = new TextBlock
            {
                Text = "Enter the MD at which to sidetrack and the target coordinates for the daughter hole.",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 12)
            };
            _createView.Children.Add(instr);

            var mdRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 0, 0, 6) };
            mdRow.Children.Add(new TextBlock { Text = "Sidetrack MD:", VerticalAlignment = VerticalAlignment.Center, Width = 120 });
            _sidetrackMD = new TextBox { Width = 120 };
            mdRow.Children.Add(_sidetrackMD);
            _createView.Children.Add(mdRow);

            var targetGroup = new StackPanel { Margin = new Thickness(0, 0, 0, 12) };
            targetGroup.Children.Add(new TextBlock { Text = "Target Coordinates:", FontWeight = Avalonia.Media.FontWeight.Bold, Margin = new Thickness(0, 0, 0, 4) });
            var targetRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            targetRow.Children.Add(new TextBlock { Text = "Easting:", Width = 80, VerticalAlignment = VerticalAlignment.Center });
            _targetEasting = new TextBox { Width = 120 };
            targetRow.Children.Add(_targetEasting);
            targetRow.Children.Add(new TextBlock { Text = "Northing:", Width = 80, VerticalAlignment = VerticalAlignment.Center });
            _targetNorthing = new TextBox { Width = 120 };
            targetRow.Children.Add(_targetNorthing);
            targetRow.Children.Add(new TextBlock { Text = "TVD:", Width = 60, VerticalAlignment = VerticalAlignment.Center });
            _targetTVD = new TextBox { Width = 100 };
            targetRow.Children.Add(_targetTVD);
            targetGroup.Children.Add(targetRow);
            _createView.Children.Add(targetGroup);

            var detailsGroup = new StackPanel { Margin = new Thickness(0, 0, 0, 12) };
            detailsGroup.Children.Add(new TextBlock { Text = "Hole Details:", FontWeight = Avalonia.Media.FontWeight.Bold, Margin = new Thickness(0, 0, 0, 4) });
            var detailRow1 = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            detailRow1.Children.Add(new TextBlock { Text = "Hole ID:", Width = 80, VerticalAlignment = VerticalAlignment.Center });
            _holeID = new TextBox { Width = 150 };
            detailRow1.Children.Add(_holeID);
            detailRow1.Children.Add(new TextBlock { Text = "Rig ID:", Width = 80, VerticalAlignment = VerticalAlignment.Center });
            _rigID = new TextBox { Width = 150 };
            detailRow1.Children.Add(_rigID);
            detailsGroup.Children.Add(detailRow1);
            var detailRow2 = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            detailRow2.Children.Add(new TextBlock { Text = "Location:", Width = 80, VerticalAlignment = VerticalAlignment.Center });
            _location = new TextBox { Width = 150 };
            detailRow2.Children.Add(_location);
            detailRow2.Children.Add(new TextBlock { Text = "Area:", Width = 80, VerticalAlignment = VerticalAlignment.Center });
            _area = new TextBox { Width = 150 };
            detailRow2.Children.Add(_area);
            detailsGroup.Children.Add(detailRow2);
            var detailRow3 = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            detailRow3.Children.Add(new TextBlock { Text = "Client:", Width = 80, VerticalAlignment = VerticalAlignment.Center });
            _client = new TextBox { Width = 150 };
            detailRow3.Children.Add(_client);
            detailRow3.Children.Add(new TextBlock { Text = "Drilling Contractor:", Width = 120, VerticalAlignment = VerticalAlignment.Center });
            _drillingContractor = new TextBox { Width = 150 };
            detailRow3.Children.Add(_drillingContractor);
            detailsGroup.Children.Add(detailRow3);
            var detailRow4 = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            detailRow4.Children.Add(new TextBlock { Text = "Surveying Contractor:", Width = 120, VerticalAlignment = VerticalAlignment.Center });
            _surveyingContractor = new TextBox { Width = 150 };
            detailRow4.Children.Add(_surveyingContractor);
            detailsGroup.Children.Add(detailRow4);
            _createView.Children.Add(detailsGroup);

            _chart = new ChartHost { Height = 250, HorizontalAlignment = HorizontalAlignment.Stretch };
            _createView.Children.Add(_chart);

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 12, 0, 0) };
            _createNext = new Button { Content = "Next", Width = 100 };
            _createNext.Click += (s, e) => CreateSideTrack();
            _createCancel = new Button { Content = "Cancel", Width = 100 };
            _createCancel.Click += (s, e) => Raise(ScreenId.HolePlanning, _session.CurrentFilePath);
            btnRow.Children.Add(_createNext);
            btnRow.Children.Add(_createCancel);
            _createView.Children.Add(btnRow);
        }

        private void BuildSaveView()
        {
            _saveView = new StackPanel { Spacing = 10 };

            var heading = new TextBlock
            {
                Text = "Sidetrack Hole — Save",
                FontSize = FontMap.ToDips(15.75),
                Margin = new Thickness(0, 0, 0, 8)
            };
            _saveView.Children.Add(heading);

            var instr = new TextBlock
            {
                Text = "Save the daughter hole plan to a new hole file.",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 12)
            };
            _saveView.Children.Add(instr);

            var idRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 0, 0, 6) };
            idRow.Children.Add(new TextBlock { Text = "Hole ID:", Width = 120, VerticalAlignment = VerticalAlignment.Center });
            _saveHoleID = new TextBox { Width = 200 };
            idRow.Children.Add(_saveHoleID);
            _saveView.Children.Add(idRow);

            var fileRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 0, 0, 12) };
            fileRow.Children.Add(new TextBlock { Text = "File Name:", Width = 120, VerticalAlignment = VerticalAlignment.Center });
            _saveFileName = new TextBox { Width = 200 };
            fileRow.Children.Add(_saveFileName);
            _saveView.Children.Add(fileRow);

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 12, 0, 0) };
            _saveSave = new Button { Content = "Save", Width = 100 };
            _saveSave.Click += (s, e) => SaveSideTrack();
            _saveBack = new Button { Content = "Back", Width = 100 };
            _saveBack.Click += (s, e) => ShowCreate();
            btnRow.Children.Add(_saveSave);
            btnRow.Children.Add(_saveBack);
            _saveView.Children.Add(btnRow);
        }

        private void ShowCreate()
        {
            _viewHost.Content = _createView;
        }

        private void ShowSave()
        {
            _viewHost.Content = _saveView;
            _saveHoleID.Text = _sideTrackHole.HoleID;
            _saveFileName.Text = _sideTrackHole.HoleID;
        }

        private void CreateSideTrack()
        {
            if (_session == null || _session.HoleFile == null)
            {
                _status.Text = "No hole file open.";
                return;
            }

            if (!decimal.TryParse(_sidetrackMD.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var sideTrackMD))
            {
                _status.Text = "Invalid sidetrack MD.";
                return;
            }

            _sideTrackHole.SideTrackMD = sideTrackMD;

            if (_sideTrackHole.Target != null)
            {
                if (!string.IsNullOrEmpty(_targetEasting.Text) &&
                    decimal.TryParse(_targetEasting.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var targetE))
                {
                    _sideTrackHole.Target.SetEasting(targetE);
                }
                if (!string.IsNullOrEmpty(_targetNorthing.Text) &&
                    decimal.TryParse(_targetNorthing.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var targetN))
                {
                    _sideTrackHole.Target.SetNorthing(targetN);
                }
                if (!string.IsNullOrEmpty(_targetTVD.Text) &&
                    decimal.TryParse(_targetTVD.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var targetTVD))
                {
                    _sideTrackHole.Target.SetTVD(targetTVD);
                }
            }

            _sideTrackHole.HoleID = _holeID.Text;
            _sideTrackHole.RigID = _rigID.Text;
            _sideTrackHole.Location = _location.Text;
            _sideTrackHole.Area = _area.Text;
            _sideTrackHole.Client = _client.Text;
            _sideTrackHole.DrillingContractor = _drillingContractor.Text;
            _sideTrackHole.SurveyingContractor = _surveyingContractor.Text;

            UpdateChart();
            ShowSave();
        }

        private void SaveSideTrack()
        {
            if (_session == null)
            {
                _status.Text = "No session.";
                return;
            }

            var path = _session.CurrentFilePath;
            if (string.IsNullOrEmpty(path))
            {
                _status.Text = "No file path available.";
                return;
            }

            var dir = System.IO.Path.GetDirectoryName(path);
            var newPath = System.IO.Path.Combine(dir ?? "", _saveHoleID.Text + EzyPlanStorage.HoleFileFormat.Extension);

            var saveRc = _sideTrackHole.SaveAs(newPath);
            if (saveRc != DrillHoleFile.SaveAs_ErrorsEnum.NoError)
            {
                _status.Text = "Save failed: " + saveRc;
                return;
            }

            _status.Text = "Sidetrack hole saved to " + newPath;
            Raise(ScreenId.HolePlanning, newPath);
        }

        private void UpdateChart()
        {
            if (_sideTrackHole == null || _sideTrackHole.Collar == null || _sideTrackHole.Target == null)
            {
                return;
            }

            var chartData = new HoleChartData();
            chartData.SetCollar(_sideTrackHole.Collar, visible: true);
            chartData.SetTarget(_sideTrackHole.Target, visible: true);

            var absList = new AbsoluteSurveyList();
            var navMath = new NavMath();
            navMath.AbsoluteSurvey(_sideTrackHole.Collar, _sideTrackHole.SurveyList, ref absList);
            if (absList.Count() > 0)
            {
                chartData.SetSurveyData(absList, visible: true);
            }

            _chart.ChartData = chartData;
        }

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