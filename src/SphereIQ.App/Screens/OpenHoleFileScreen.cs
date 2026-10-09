using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using EzyPlanStorage;
using FilePickerFileType = Avalonia.Platform.Storage.FilePickerFileType;
using FilePickerOpenOptions = Avalonia.Platform.Storage.FilePickerOpenOptions;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// "Open Existing Hole" slice. The legacy screen embedded a Windows
    /// drive box + directory/file list; the cross-platform replacement uses
    /// the native file picker (Finder on macOS, Explorer on Windows), which is
    /// the correct macOS equivalent. Same outcome contract as the legacy
    /// ButtonPressedEnum: Open (-> HolePlanning with the file) or Cancel
    /// (-> back to Welcome).
    /// </summary>
    public sealed class OpenHoleFileScreen : IScreen
    {
        public ScreenId Id { get { return ScreenId.OpenHoleFile; } }

        public object Content { get; private set; }

        public event EventHandler<NavigateEventArgs> Navigate;

        public AppSession Session { get; set; }

        private TextBlock _status;
        private Button _openButton;

        public OpenHoleFileScreen()
        {
            Build();
        }

        public void OnShown()
        {
            SetStatus("Choose a hole file to open.");
        }

        private void Build()
        {
            StackPanel root = new StackPanel { Margin = new Avalonia.Thickness(40), Spacing = 12, MaxWidth = 640 };

            TextBlock heading = new TextBlock { Text = "Open Existing Hole", FontSize = 26, Margin = new Avalonia.Thickness(0, 0, 0, 8) };
            _status = new TextBlock { Text = "Choose a hole file to open.", TextWrapping = Avalonia.Media.TextWrapping.Wrap };

            Button browse = new Button { Content = "Browse...", Height = 40, Margin = new Avalonia.Thickness(0, 2) };
            browse.Click += async (s, e) => await OnBrowse();

            _openButton = new Button { Content = "Open Hole File", Height = 40, Margin = new Avalonia.Thickness(0, 2), IsEnabled = false };
            _openButton.Click += OnOpen;

            Button back = new Button { Content = "Back", Height = 40, Margin = new Avalonia.Thickness(0, 12) };
            back.Click += (s, e) => Raise(ScreenId.Welcome, "");

            root.Children.Add(heading);
            root.Children.Add(_status);
            root.Children.Add(browse);
            root.Children.Add(_openButton);
            root.Children.Add(back);

            Content = root;
        }

        private string _selectedPath = "";

        private async System.Threading.Tasks.Task OnBrowse()
        {
            TopLevel top = TopLevel.GetTopLevel((Control)Content);
            if (top == null)
            {
                return;
            }
            IStorageProvider storage = top.StorageProvider;
            if (storage == null)
            {
                SetStatus("File picker is not available on this platform.");
                return;
            }

            FilePickerOpenOptions options = new FilePickerOpenOptions
            {
                Title = "Open Hole File",
                AllowMultiple = false
            };
            options.FileTypeFilter = new System.Collections.Generic.List<FilePickerFileType>
            {
                new FilePickerFileType("EzyNav Hole File") { Patterns = new System.Collections.Generic.List<string> { "*" + EzyPlanStorage.HoleFileFormat.Extension } },
                new FilePickerFileType("All Files") { Patterns = new System.Collections.Generic.List<string> { "*" } }
            };

            System.Collections.Generic.IReadOnlyList<IStorageFile> files = await storage.OpenFilePickerAsync(options);
            IStorageFile picked = files != null ? files.FirstOrDefault() : null;
            if (picked == null)
            {
                return;
            }

            string path = picked.Path != null ? picked.Path.LocalPath : null;
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            _selectedPath = path;
            _openButton.IsEnabled = true;
            SetStatus("Selected: " + path);
        }

        private void OnOpen(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedPath))
            {
                SetStatus("Select a file first.");
                return;
            }

            DrillHoleFile file = Session != null ? Session.HoleFile : new DrillHoleFile();
            DrillHoleFile.OpenFile_ErrorsEnum rc = file.Open(_selectedPath);
            if (rc == DrillHoleFile.OpenFile_ErrorsEnum.NoError)
            {
                if (Session != null)
                {
                    Session.CurrentFilePath = _selectedPath;
                    Session.NotifyFileChanged();
                }
                Raise(ScreenId.HolePlanning, _selectedPath);
                return;
            }

            string detail = DrillHoleFile.ExplainError(rc);
            SetStatus("The selected file could not be opened. " + detail + ".");
        }

        private void SetStatus(string message)
        {
            if (_status != null)
            {
                _status.Text = message;
            }
        }

        private void Raise(ScreenId destination, string filePath)
        {
            NavigateEventArgs e = new NavigateEventArgs(destination, filePath);
            if (Navigate != null)
            {
                Navigate(this, e);
            }
        }
    }
}
