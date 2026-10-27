using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using EzyPlanData;
using EzyPlanStorage;
using SphereIQ.Reports;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Port of the legacy <c>Procedure_EditDetails</c> +
    /// <c>Body_CreateNewHole_EnterDetails</c> +
    /// <c>Body_CreateNewHole_EnterDetails2</c> pair.
    /// Two-step wizard: Step 1 = hole identification (ID, Rig, Location, Area,
    /// Client, Contractors); Step 2 = collar and target coordinates.
    /// Changes are saved back to the open hole file.
    /// </summary>
    public sealed class EditDetailsScreen : IScreen
    {
        public ScreenId Id { get { return ScreenId.EditDetails; } }
        public object Content { get; private set; }
        public event EventHandler<NavigateEventArgs> Navigate;
        public AppSession Session { get { return _session; } set { } }

        private readonly AppSession _session;
        private readonly StackPanel _root;
        private readonly ContentControl _viewHost;
        private readonly TextBlock _status;

        private Panel _step1View;
        private Panel _step2View;

        // Step 1 controls
        private TextBox _holeId;
        private TextBox _rigId;
        private TextBox _location;
        private TextBox _area;
        private TextBox _client;
        private TextBox _drillingContractor;
        private TextBox _surveyingContractor;
        private Button _step1Next;
        private Button _step1Cancel;

        // Step 2 controls
        private TextBox _collarEasting;
        private TextBox _collarNorthing;
        private TextBox _targetEasting;
        private TextBox _targetNorthing;
        private TextBox _targetTVD;
        private Button _step2Save;
        private Button _step2Back;

        private DrillHoleFile _workingCopy;
        private bool _step2FirstRun = true;

        public EditDetailsScreen(AppSession session)
        {
            _session = session;
            _root = new StackPanel { Margin = new Thickness(12), Spacing = 8 };
            _viewHost = new ContentControl();
            var status = new TextBlock { TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            _root.Children.Add(status);
            _root.Children.Add(_viewHost);
            Content = _root;

            _status = status;

            BuildStep1View();
            BuildStep2View();
            ShowStep1();
        }

        public void OnShown()
        {
            if (_session == null || _session.HoleFile == null)
            {
                _status.Text = "No hole file open.";
                return;
            }
            _workingCopy = new DrillHoleFile();
            _workingCopy.Copy(_session.HoleFile, copyStream: false);
            _step2FirstRun = true;
            ShowStep1();
        }

        private void BuildStep1View()
        {
            _step1View = new StackPanel { Spacing = 10 };

            var heading = new TextBlock
            {
                Text = "Edit Hole Details — Step 1 of 2",
                FontSize = FontMap.ToDips(15.75),
                Margin = new Thickness(0, 0, 0, 8)
            };
            _step1View.Children.Add(heading);

            var instr = new TextBlock
            {
                Text = "Edit hole identification and contractor information.",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 12)
            };
            _step1View.Children.Add(instr);

            var row1 = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 0, 0, 6) };
            row1.Children.Add(new TextBlock { Text = "Hole ID:", Width = 100, VerticalAlignment = VerticalAlignment.Center });
            _holeId = new TextBox { Width = 200 };
            row1.Children.Add(_holeId);
            row1.Children.Add(new TextBlock { Text = "Rig ID:", Width = 80, VerticalAlignment = VerticalAlignment.Center });
            _rigId = new TextBox { Width = 200 };
            row1.Children.Add(_rigId);
            _step1View.Children.Add(row1);

            var row2 = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 0, 0, 6) };
            row2.Children.Add(new TextBlock { Text = "Location:", Width = 100, VerticalAlignment = VerticalAlignment.Center });
            _location = new TextBox { Width = 200 };
            row2.Children.Add(_location);
            row2.Children.Add(new TextBlock { Text = "Area:", Width = 80, VerticalAlignment = VerticalAlignment.Center });
            _area = new TextBox { Width = 200 };
            row2.Children.Add(_area);
            _step1View.Children.Add(row2);

            var row3 = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 0, 0, 6) };
            row3.Children.Add(new TextBlock { Text = "Client:", Width = 100, VerticalAlignment = VerticalAlignment.Center });
            _client = new TextBox { Width = 200 };
            row3.Children.Add(_client);
            row3.Children.Add(new TextBlock { Text = "Drilling Contractor:", Width = 120, VerticalAlignment = VerticalAlignment.Center });
            _drillingContractor = new TextBox { Width = 200 };
            row3.Children.Add(_drillingContractor);
            _step1View.Children.Add(row3);

            var row4 = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 0, 0, 12) };
            row4.Children.Add(new TextBlock { Text = "Surveying Contractor:", Width = 120, VerticalAlignment = VerticalAlignment.Center });
            _surveyingContractor = new TextBox { Width = 200 };
            row4.Children.Add(_surveyingContractor);
            _step1View.Children.Add(row4);

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 12, 0, 0) };
            _step1Next = new Button { Content = "Next", Width = 100 };
            _step1Next.Click += (s, e) => ShowStep2();
            _step1Cancel = new Button { Content = "Cancel", Width = 100 };
            _step1Cancel.Click += (s, e) => Raise(ScreenId.HolePlanning, _session.CurrentFilePath);
            btnRow.Children.Add(_step1Next);
            btnRow.Children.Add(_step1Cancel);
            _step1View.Children.Add(btnRow);
        }

        private void BuildStep2View()
        {
            _step2View = new StackPanel { Spacing = 10 };

            var heading = new TextBlock
            {
                Text = "Edit Hole Details — Step 2 of 2",
                FontSize = FontMap.ToDips(15.75),
                Margin = new Thickness(0, 0, 0, 8)
            };
            _step2View.Children.Add(heading);

            var instr = new TextBlock
            {
                Text = "Edit collar and target coordinates.",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 12)
            };
            _step2View.Children.Add(instr);

            var collarGroup = new StackPanel { Margin = new Thickness(0, 0, 0, 8) };
            collarGroup.Children.Add(new TextBlock { Text = "Collar:", FontWeight = Avalonia.Media.FontWeight.Bold, Margin = new Thickness(0, 0, 0, 4) });
            var collarRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            collarRow.Children.Add(new TextBlock { Text = "Easting:", Width = 80, VerticalAlignment = VerticalAlignment.Center });
            _collarEasting = new TextBox { Width = 150 };
            collarRow.Children.Add(_collarEasting);
            collarRow.Children.Add(new TextBlock { Text = "Northing:", Width = 80, VerticalAlignment = VerticalAlignment.Center });
            _collarNorthing = new TextBox { Width = 150 };
            collarRow.Children.Add(_collarNorthing);
            collarGroup.Children.Add(collarRow);
            _step2View.Children.Add(collarGroup);

            var targetGroup = new StackPanel { Margin = new Thickness(0, 0, 0, 12) };
            targetGroup.Children.Add(new TextBlock { Text = "Target:", FontWeight = Avalonia.Media.FontWeight.Bold, Margin = new Thickness(0, 0, 0, 4) });
            var targetRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            targetRow.Children.Add(new TextBlock { Text = "Easting:", Width = 80, VerticalAlignment = VerticalAlignment.Center });
            _targetEasting = new TextBox { Width = 150 };
            targetRow.Children.Add(_targetEasting);
            targetRow.Children.Add(new TextBlock { Text = "Northing:", Width = 80, VerticalAlignment = VerticalAlignment.Center });
            _targetNorthing = new TextBox { Width = 150 };
            targetRow.Children.Add(_targetNorthing);
            targetRow.Children.Add(new TextBlock { Text = "TVD:", Width = 60, VerticalAlignment = VerticalAlignment.Center });
            _targetTVD = new TextBox { Width = 120 };
            targetRow.Children.Add(_targetTVD);
            targetGroup.Children.Add(targetRow);
            _step2View.Children.Add(targetGroup);

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 12, 0, 0) };
            _step2Back = new Button { Content = "Back", Width = 100 };
            _step2Back.Click += (s, e) => ShowStep1();
            _step2Save = new Button { Content = "Save", Width = 100 };
            _step2Save.Click += (s, e) => SaveChanges();
            btnRow.Children.Add(_step2Back);
            btnRow.Children.Add(_step2Save);
            _step2View.Children.Add(btnRow);
        }

        private void ShowStep1()
        {
            _viewHost.Content = _step1View;
            _status.Text = "Edit hole identification fields.";
            if (_workingCopy != null)
            {
                _holeId.Text = _workingCopy.HoleID;
                _rigId.Text = _workingCopy.RigID;
                _location.Text = _workingCopy.Location;
                _area.Text = _workingCopy.Area;
                _client.Text = _workingCopy.Client;
                _drillingContractor.Text = _workingCopy.DrillingContractor;
                _surveyingContractor.Text = _workingCopy.SurveyingContractor;
            }
        }

        private void ShowStep2()
        {
            _viewHost.Content = _step2View;
            _status.Text = "Edit collar and target coordinates.";
            if (_workingCopy != null)
            {
                if (_workingCopy.Collar != null)
                {
                    _collarEasting.Text = _workingCopy.Collar.Easting.ToString(CultureInfo.InvariantCulture);
                    _collarNorthing.Text = _workingCopy.Collar.Northing.ToString(CultureInfo.InvariantCulture);
                }
                if (_workingCopy.Target != null)
                {
                    _targetEasting.Text = _workingCopy.Target.Easting.ToString(CultureInfo.InvariantCulture);
                    _targetNorthing.Text = _workingCopy.Target.Northing.ToString(CultureInfo.InvariantCulture);
                    _targetTVD.Text = _workingCopy.Target.TVD.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        private void SaveChanges()
        {
            if (_session == null || _session.HoleFile == null || _workingCopy == null)
            {
                _status.Text = "No hole file open.";
                return;
            }

            // Update working copy from step 1 fields
            _workingCopy.HoleID = _holeId.Text;
            _workingCopy.RigID = _rigId.Text;
            _workingCopy.Location = _location.Text;
            _workingCopy.Area = _area.Text;
            _workingCopy.Client = _client.Text;
            _workingCopy.DrillingContractor = _drillingContractor.Text;
            _workingCopy.SurveyingContractor = _surveyingContractor.Text;

            // Update collar from step 2 fields
            if (_workingCopy.Collar != null)
            {
                if (decimal.TryParse(_collarEasting.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var ce))
                {
                    _workingCopy.Collar.SetEasting(ce);
                }
                if (decimal.TryParse(_collarNorthing.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var cn))
                {
                    _workingCopy.Collar.SetNorthing(cn);
                }
            }

            // Update target from step 2 fields
            if (_workingCopy.Target != null)
            {
                if (decimal.TryParse(_targetEasting.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var te))
                {
                    _workingCopy.Target.SetEasting(te);
                }
                if (decimal.TryParse(_targetNorthing.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var tn))
                {
                    _workingCopy.Target.SetNorthing(tn);
                }
                if (decimal.TryParse(_targetTVD.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out var tt))
                {
                    _workingCopy.Target.SetTVD(tt);
                }
            }

            // Check if anything changed
            if (!_session.HoleFile.Equals(_workingCopy))
            {
                _session.HoleFile.Copy(_workingCopy, copyStream: false);
                var saveRc = _session.HoleFile.Save();
                if (saveRc != DrillHoleFile.Save_ErrorsEnum.NoError)
                {
                    _status.Text = "Save failed: " + saveRc;
                    return;
                }
                _status.Text = "Hole details saved.";
            }
            else
            {
                _status.Text = "No changes to save.";
            }

            Raise(ScreenId.HolePlanning, _session.CurrentFilePath);
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