using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using EzyPlanData;
using EzyPlanStorage;
using SphereIQ.Reports;
using FilePickerFileType = Avalonia.Platform.Storage.FilePickerFileType;
using FilePickerOpenOptions = Avalonia.Platform.Storage.FilePickerOpenOptions;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Port of the legacy <c>Procedure_Import</c> import CSV flow.
    /// <para>
    /// Two-step process:
    /// <list type="bullet">
    ///   <item><c>SelectFile</c> — choose a CSV file via the native file picker.</item>
    ///   <item><c>Review</c> — preview imported surveys and save to the hole file.</item>
    /// </list>
    /// </para>
    /// </summary>
    public sealed class ImportScreen : IScreen
    {
        public ScreenId Id { get { return ScreenId.Import; } }
        public object Content { get; private set; }
        public event EventHandler<NavigateEventArgs> Navigate;
        public AppSession Session { get { return _session; } set { } }

        private readonly AppSession _session;
        private readonly StackPanel _root;
        private readonly ContentControl _viewHost;
        private readonly TextBlock _status;

        private Panel _selectView;
        private Panel _reviewView;

        // Select view controls
        private Button _selectBrowse;
        private Button _selectCancel;

        // Review view controls
        private DataGrid _reviewGrid;
        private TextBlock _reviewCount;
        private Button _reviewSave;
        private Button _reviewBack;

        private SurveyList _importedSurveys;
        private string _selectedFilePath;

        public ImportScreen(AppSession session)
        {
            _session = session;
            _root = new StackPanel { Margin = new Thickness(12), Spacing = 8 };
            _viewHost = new ContentControl();
            var status = new TextBlock { TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            _root.Children.Add(status);
            _root.Children.Add(_viewHost);
            Content = _root;

            _status = status;

            BuildSelectView();
            BuildReviewView();
            ShowSelect();
        }

        public void OnShown()
        {
            _status.Text = "Select a CSV file to import.";
        }

        private void BuildSelectView()
        {
            _selectView = new StackPanel { Spacing = 10 };

            var heading = new TextBlock
            {
                Text = "Import Surveys — Select File",
                FontSize = FontMap.ToDips(15.75),
                Margin = new Thickness(0, 0, 0, 8)
            };
            _selectView.Children.Add(heading);

            var instr = new TextBlock
            {
                Text = "Choose a CSV file containing survey data to import into the open hole.",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 12)
            };
            _selectView.Children.Add(instr);

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 0, 0, 12) };
            _selectBrowse = new Button { Content = "Browse...", Width = 120 };
            _selectBrowse.Click += async (s, e) => await BrowseCsv();
            _selectCancel = new Button { Content = "Cancel", Width = 100 };
            _selectCancel.Click += (s, e) => Raise(ScreenId.HolePlanning, _session.CurrentFilePath);
            btnRow.Children.Add(_selectBrowse);
            btnRow.Children.Add(_selectCancel);
            _selectView.Children.Add(btnRow);
        }

        private void BuildReviewView()
        {
            _reviewView = new StackPanel { Spacing = 10 };

            var heading = new TextBlock
            {
                Text = "Import Surveys — Review",
                FontSize = FontMap.ToDips(15.75),
                Margin = new Thickness(0, 0, 0, 8)
            };
            _reviewView.Children.Add(heading);

            _reviewCount = new TextBlock { Text = "0 surveys imported", Margin = new Thickness(0, 0, 0, 8) };
            _reviewView.Children.Add(_reviewCount);

            _reviewGrid = new DataGrid
            {
                Height = 300,
                CanUserSortColumns = true,
                AutoGenerateColumns = false,
                IsReadOnly = true,
                SelectionMode = DataGridSelectionMode.Single
            };
            _reviewGrid.Columns.Add(new DataGridTextColumn { Header = "MD", Binding = new Avalonia.Data.Binding("MD") { StringFormat = "N2" } });
            _reviewGrid.Columns.Add(new DataGridTextColumn { Header = "Dip", Binding = new Avalonia.Data.Binding("Dip") { StringFormat = "N2" } });
            _reviewGrid.Columns.Add(new DataGridTextColumn { Header = "Azimuth", Binding = new Avalonia.Data.Binding("Azimuth") { StringFormat = "N2" } });
            _reviewGrid.Columns.Add(new DataGridTextColumn { Header = "TVD", Binding = new Avalonia.Data.Binding("TVD") { StringFormat = "N2" } });
            _reviewGrid.Columns.Add(new DataGridTextColumn { Header = "Northing", Binding = new Avalonia.Data.Binding("Northing") { StringFormat = "N2" } });
            _reviewGrid.Columns.Add(new DataGridTextColumn { Header = "Easting", Binding = new Avalonia.Data.Binding("Easting") { StringFormat = "N2" } });
            _reviewGrid.Columns.Add(new DataGridTextColumn { Header = "DL", Binding = new Avalonia.Data.Binding("DL") { StringFormat = "N2" } });
            _reviewView.Children.Add(_reviewGrid);

            var btnRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Thickness(0, 12, 0, 0) };
            _reviewBack = new Button { Content = "Back", Width = 100 };
            _reviewBack.Click += (s, e) => ShowSelect();
            _reviewSave = new Button { Content = "Save", Width = 100 };
            _reviewSave.Click += (s, e) => SaveImport();
            btnRow.Children.Add(_reviewBack);
            btnRow.Children.Add(_reviewSave);
            _reviewView.Children.Add(btnRow);
        }

        private void ShowSelect()
        {
            _viewHost.Content = _selectView;
        }

        private void ShowReview()
        {
            _viewHost.Content = _reviewView;
            _reviewCount.Text = _importedSurveys != null ? $"{_importedSurveys.Count()} survey(s) imported" : "0 surveys imported";
            _reviewGrid.ItemsSource = _importedSurveys;
        }

        private async System.Threading.Tasks.Task BrowseCsv()
        {
            if (_session == null)
            {
                _status.Text = "No session.";
                return;
            }

            TopLevel top = TopLevel.GetTopLevel(_root);
            if (top == null || top.StorageProvider == null)
            {
                _status.Text = "File picker is not available on this platform.";
                return;
            }

            var options = new FilePickerOpenOptions
            {
                Title = "Select CSV File",
                AllowMultiple = false
            };
            options.FileTypeFilter = new System.Collections.Generic.List<FilePickerFileType>
            {
                new FilePickerFileType("CSV Files") { Patterns = new System.Collections.Generic.List<string> { "*.csv" } },
                new FilePickerFileType("All Files") { Patterns = new System.Collections.Generic.List<string> { "*.*" } }
            };

            var files = await top.StorageProvider.OpenFilePickerAsync(options);
            var picked = files != null ? files.FirstOrDefault() : null;
            if (picked == null || picked.Path == null)
            {
                return;
            }

            _selectedFilePath = picked.Path.LocalPath;
            _status.Text = "Selected: " + _selectedFilePath;

            // Simple CSV import: MD,Dip,Azimuth,TVD,Northing,Easting,DL
            _importedSurveys = new SurveyList();
            try
            {
                var lines = System.IO.File.ReadAllLines(_selectedFilePath);
                bool firstLine = true;
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    if (firstLine) { firstLine = false; continue; } // skip header
                    var parts = line.Split(',');
                    if (parts.Length < 7) continue;
                    var survey = new Survey();
                    if (decimal.TryParse(parts[0].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var md)) survey.SetMD(md);
                    if (decimal.TryParse(parts[1].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var dip)) survey.SetDip(dip);
                    if (decimal.TryParse(parts[2].Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var azi)) survey.SetAzimuth(azi);
                    _importedSurveys.Append(survey);
                }
            }
            catch (Exception ex)
            {
                _status.Text = "Import error: " + ex.Message;
                return;
            }

            ShowReview();
        }

        private void SaveImport()
        {
            if (_session == null || _session.HoleFile == null || _importedSurveys == null)
            {
                _status.Text = "No hole file or no data to save.";
                return;
            }

            foreach (var survey in _importedSurveys)
            {
                _session.HoleFile.SurveyList.Append(survey);
            }

            var saveRc = _session.HoleFile.Save();
            if (saveRc != DrillHoleFile.Save_ErrorsEnum.NoError)
            {
                _status.Text = "Save failed: " + saveRc;
                return;
            }

            _status.Text = $"{_importedSurveys.Count()} survey(s) imported and saved.";
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