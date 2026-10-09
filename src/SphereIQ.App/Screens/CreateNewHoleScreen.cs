using System;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using EzyPlanData;
using EzyPlanStorage;
using FilePickerFileType = Avalonia.Platform.Storage.FilePickerFileType;
using FilePickerSaveOptions = Avalonia.Platform.Storage.FilePickerSaveOptions;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Create New Hole wizard (legacy Body_CreateNewHole_EnterDetails ->
    /// EnterDetails2 -> SaveFile), ported as one screen with 3 internal steps
    /// bound to Session.HoleFile. Same outcome as the legacy
    /// FinishedStateEnum: Save (-> HolePlanning) or Cancel (-> Welcome).
    /// Step 3 uses the native save picker instead of the Windows file list box.
    /// </summary>
    public sealed class CreateNewHoleScreen : IScreen
    {
        public ScreenId Id { get { return ScreenId.CreateNewHole; } }
        public object Content { get; private set; }
        public event EventHandler<NavigateEventArgs> Navigate;
        public AppSession Session { get; set; }

        private int _step = 1;
        private Panel _root;
        private TextBlock _status;
        private ContentControl _pageHost;
        private Button _back;
        private Button _next;
        private string _lastError;

        private TextBox _holeId, _rigId, _location, _area, _drillContractor, _client, _surveyContractor;
        private TextBox _collarE, _collarN, _targetE, _targetN, _targetTvd;

        public CreateNewHoleScreen() { Build(); }
        public void OnShown() { ShowStep(1); }

        private void Build()
        {
            _root = new StackPanel { Margin = new Thickness(40), Spacing = 12, MaxWidth = 680 };
            _root.Children.Add(new TextBlock { Text = "Create New Hole", FontSize = 26, Margin = new Thickness(0, 0, 0, 8) });
            _status = new TextBlock { TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            _root.Children.Add(_status);

            _pageHost = new ContentControl();
            _root.Children.Add(_pageHost);

            StackPanel nav = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 16, 0, 0) };
            _back = new Button { Content = "Back", IsVisible = false };
            _back.Click += (s, e) => ShowStep(_step - 1);
            _next = new Button { Content = "Next" };
            _next.Click += OnNext;
            Button cancel = new Button { Content = "Cancel" };
            cancel.Click += (s, e) => Raise(ScreenId.Welcome, "");
            nav.Children.Add(_back);
            nav.Children.Add(_next);
            nav.Children.Add(cancel);
            _root.Children.Add(nav);
            Content = _root;
        }

        private void ShowStep(int step)
        {
            _step = step;
            DrillHoleFile f = Hole();
            if (step == 1)
            {
                _pageHost.Content = BuildStep1(f);
                _back.IsVisible = false;
                _next.Content = "Next";
                _status.Text = "Step 1 of 3 - Hole details.";
            }
            else if (step == 2)
            {
                _pageHost.Content = BuildStep2(f);
                _back.IsVisible = true;
                _next.Content = "Next";
                _status.Text = "Step 2 of 3 - Collar and target coordinates.";
            }
            else
            {
                _pageHost.Content = BuildStep3(f);
                _back.IsVisible = true;
                _next.Content = "Save";
                _status.Text = "Step 3 of 3 - Choose where to save the hole file.";
            }
        }

        private Panel BuildStep1(DrillHoleFile f)
        {
            StackPanel panel = new StackPanel { Spacing = 10 };
            _holeId = Row(panel, "Hole ID", Safe(f.HoleID));
            _rigId = Row(panel, "Rig ID", Safe(f.RigID));
            _location = Row(panel, "Location", Safe(f.Location));
            _area = Row(panel, "Area", Safe(f.Area));
            _drillContractor = Row(panel, "Drilling Contractor", Safe(f.DrillingContractor));
            _surveyContractor = Row(panel, "Surveying Contractor", Safe(f.SurveyingContractor));
            _client = Row(panel, "Client", Safe(f.Client));
            return panel;
        }

        private Panel BuildStep2(DrillHoleFile f)
        {
            StackPanel panel = new StackPanel { Spacing = 10 };
            panel.Children.Add(new TextBlock { Text = "Collar", FontSize = 16 });
            _collarE = Row(panel, "Easting", f.Collar != null ? f.Collar.Easting.ToString(CultureInfo.InvariantCulture) : "");
            _collarN = Row(panel, "Northing", f.Collar != null ? f.Collar.Northing.ToString(CultureInfo.InvariantCulture) : "");
            panel.Children.Add(new TextBlock { Text = "Target", FontSize = 16, Margin = new Thickness(0, 12, 0, 0) });
            _targetE = Row(panel, "Easting", f.Target != null ? f.Target.Easting.ToString(CultureInfo.InvariantCulture) : "");
            _targetN = Row(panel, "Northing", f.Target != null ? f.Target.Northing.ToString(CultureInfo.InvariantCulture) : "");
            _targetTvd = Row(panel, "TVD", f.Target != null ? f.Target.TVD.ToString(CultureInfo.InvariantCulture) : "");
            return panel;
        }

        private Panel BuildStep3(DrillHoleFile f)
        {
            StackPanel panel = new StackPanel { Spacing = 10 };
            panel.Children.Add(new TextBlock
            {
                Text = "The hole file will be saved as \"" + DefaultName() + EzyPlanStorage.HoleFileFormat.Extension + "\". Click Save to choose the location.",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap
            });
            panel.Children.Add(new TextBlock { Text = "Hole ID: " + Safe(f.HoleID), TextWrapping = Avalonia.Media.TextWrapping.Wrap });
            panel.Children.Add(new TextBlock { Text = "Rig ID: " + Safe(f.RigID), TextWrapping = Avalonia.Media.TextWrapping.Wrap });
            return panel;
        }

        private TextBox Row(Panel panel, string label, string value)
        {
            StackPanel row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            row.Children.Add(new TextBlock { Text = label, Width = 170, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center });
            TextBox box = new TextBox { Width = 260, Text = value ?? "" };
            row.Children.Add(box);
            panel.Children.Add(row);
            return box;
        }

        private DrillHoleFile Hole() { return Session != null ? Session.HoleFile : new DrillHoleFile(); }

        private void OnNext(object sender, EventArgs e)
        {
            if (_step == 1)
            {
                if (!CommitStep1()) { return; }
                ShowStep(2);
            }
            else if (_step == 2)
            {
                if (!CommitStep2()) { return; }
                ShowStep(3);
            }
            else
            {
                _ = OnSaveAsync();
            }
        }

        private bool CommitStep1()
        {
            _lastError = null;
            if (string.IsNullOrWhiteSpace(_holeId.Text))
            {
                _lastError = "A value is required for the Hole ID field.";
                ShowError();
                return false;
            }
            if (string.IsNullOrWhiteSpace(_rigId.Text))
            {
                _lastError = "A value is required for the Rig ID field.";
                ShowError();
                return false;
            }
            DrillHoleFile f = Hole();
            f.HoleID = _holeId.Text.Trim();
            f.RigID = _rigId.Text.Trim();
            if (!string.IsNullOrWhiteSpace(_location.Text)) { f.Location = _location.Text.Trim(); }
            if (!string.IsNullOrWhiteSpace(_area.Text)) { f.Area = _area.Text.Trim(); }
            if (!string.IsNullOrWhiteSpace(_drillContractor.Text)) { f.DrillingContractor = _drillContractor.Text.Trim(); }
            if (!string.IsNullOrWhiteSpace(_surveyContractor.Text)) { f.SurveyingContractor = _surveyContractor.Text.Trim(); }
            if (!string.IsNullOrWhiteSpace(_client.Text)) { f.Client = _client.Text.Trim(); }
            return true;
        }

        private bool CommitStep2()
        {
            _lastError = null;
            decimal collarE, collarN, targetE, targetN, targetTvd;
            if (!TryParse(_collarE, "Collar Easting", out collarE)) { return false; }
            if (!TryParse(_collarN, "Collar Northing", out collarN)) { return false; }
            if (!TryParse(_targetE, "Target Easting", out targetE)) { return false; }
            if (!TryParse(_targetN, "Target Northing", out targetN)) { return false; }
            if (!TryParse(_targetTvd, "Target TVD", out targetTvd)) { return false; }

            Collar collar = new Collar();
            if (collar.SetEasting(collarE) != Collar.SetEastingEnum.Set) { _lastError = "Collar Easting: the value is out of range."; ShowError(); return false; }
            if (collar.SetNorthing(collarN) != Collar.SetNorthingEnum.Set) { _lastError = "Collar Northing: the value is out of range."; ShowError(); return false; }

            Target target = new Target();
            if (target.SetEasting(targetE) != Target.SetEastingEnum.Set) { _lastError = "Target Easting: the value is out of range."; ShowError(); return false; }
            if (target.SetNorthing(targetN) != Target.SetNorthingEnum.Set) { _lastError = "Target Northing: the value is out of range."; ShowError(); return false; }
            if (target.SetTVD(targetTvd) != Target.SetTVDEnum.Set) { _lastError = "Target TVD: the value is out of range."; ShowError(); return false; }

            try
            {
                Hole().Collar = collar;
                Hole().Target = target;
                return true;
            }
            catch (ArgumentException)
            {
                _lastError = "The collar and target are too far apart. The maximum horizontal distance allowed between them is 24,000 metres.";
                ShowError();
                return false;
            }
        }

        private bool TryParse(TextBox box, string field, out decimal value)
        {
            value = 0m;
            if (string.IsNullOrWhiteSpace(box.Text))
            {
                _lastError = "A value is required for the " + field + " field.";
                ShowError();
                return false;
            }
            if (!decimal.TryParse(box.Text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out value))
            {
                _lastError = field + ": the value is not a valid number.";
                ShowError();
                return false;
            }
            return true;
        }

        private void ShowError()
        {
            if (!string.IsNullOrEmpty(_lastError) && _status != null)
            {
                _status.Text = _lastError;
            }
        }

        private async Task OnSaveAsync()
        {
            Control owner = Content as Control;
            if (owner == null)
            {
                return;
            }
            TopLevel top = TopLevel.GetTopLevel(owner);
            if (top == null || top.StorageProvider == null)
            {
                _status.Text = "The file picker is not available on this platform.";
                return;
            }

            FilePickerSaveOptions options = new FilePickerSaveOptions
            {
                Title = "Save Hole File",
                SuggestedFileName = DefaultName(),
                DefaultExtension = EzyPlanStorage.HoleFileFormat.Extension
            };
            options.FileTypeChoices = new System.Collections.Generic.List<FilePickerFileType>
            {
                new FilePickerFileType("EzyNav Hole File") { Patterns = new System.Collections.Generic.List<string> { "*" + EzyPlanStorage.HoleFileFormat.Extension } },
                new FilePickerFileType("All Files") { Patterns = new System.Collections.Generic.List<string> { "*" } }
            };

            IStorageFile target = await top.StorageProvider.SaveFilePickerAsync(options);
            if (target == null || target.Path == null)
            {
                return;
            }

            string path = target.Path.LocalPath;
            DrillHoleFile f = Hole();
            DrillHoleFile.SaveAs_ErrorsEnum rc = f.SaveAs(path);
            if (rc == DrillHoleFile.SaveAs_ErrorsEnum.NoError)
            {
                if (Session != null)
                {
                    Session.CurrentFilePath = path;
                    Session.NotifyFileChanged();
                }
                Raise(ScreenId.HolePlanning, path);
                return;
            }
            _status.Text = "Could not save the hole file: " + rc.ToString();
        }

        private static string Safe(string value)
        {
            return string.IsNullOrEmpty(value) ? "" : value;
        }

        private string DefaultName()
        {
            string holeId = Safe(Hole().HoleID);
            return string.IsNullOrEmpty(holeId) ? "NewHole" : holeId;
        }

        private void Raise(ScreenId next, string arg)
        {
            EventHandler<NavigateEventArgs> handler = Navigate;
            if (handler != null)
            {
                handler(this, new NavigateEventArgs(next, arg));
            }
        }
    }
}

