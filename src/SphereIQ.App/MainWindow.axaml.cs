using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace SphereIQ.App
{
    public partial class MainWindow : Window
    {
        private readonly AppSession _session;
        private readonly Dictionary<ScreenId, string> _titles = new Dictionary<ScreenId, string>();
        private IScreen _current;
        private readonly Stack<ScreenId> _history = new Stack<ScreenId>();

        public MainWindow()
        {
        }

        public MainWindow(AppSession session)
        {
            _session = session;
            InitializeComponent();
            BuildTitles();
            if (_session != null)
            {
                _session.FileChanged += delegate { UpdateHeader(); };
            }
            NavigateTo(ScreenId.Welcome, null);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void BuildTitles()
        {
            _titles[ScreenId.Welcome] = "Welcome";
            _titles[ScreenId.HoleManagement] = "Hole Management";
            _titles[ScreenId.CreateNewHole] = "Create New Hole";
            _titles[ScreenId.OpenHoleFile] = "Open Hole File";
            _titles[ScreenId.HolePlanning] = "Drill Hole Planning";
            _titles[ScreenId.EditSurveyData] = "Edit Survey Data";
            _titles[ScreenId.Import] = "Import Surveys";
            _titles[ScreenId.BuildToTarget] = "Build To Target";
            _titles[ScreenId.TrendAnalysis] = "Trend Analysis";
            _titles[ScreenId.Extrapolate] = "Extrapolate";
            _titles[ScreenId.EditDetails] = "Edit Hole Details";
            _titles[ScreenId.SideTrackHole] = "Side Track Hole";
            _titles[ScreenId.Reports] = "Reports";
        }

        public void NavigateTo(ScreenId id, string filePath)
        {
            if (id == ScreenId.Exit)
            {
                Close();
                return;
            }

            IScreen screen = ScreenFactory.Create(id, _session, filePath);
            if (screen == null)
            {
                return;
            }

            if (_current != null)
            {
                _current.Navigate -= OnScreenNavigate;
            }

            _current = screen;
            _current.Session = _session;
            _current.Navigate += OnScreenNavigate;

            ContentControl host = this.FindControl<ContentControl>("ScreenHost");
            object content = screen.Content;
            if (content is Control control)
            {
                host.Content = control;
            }
            screen.OnShown();

            if (_history.Count == 0 || _history.Peek() != id)
            {
                _history.Push(id);
            }
            UpdateHeader();
        }

        private void OnScreenNavigate(object sender, NavigateEventArgs e)
        {
            NavigateTo(e.Destination, e.FilePath);
        }

        private void UpdateHeader()
        {
            if (_current == null)
            {
                return;
            }
            string title;
            if (!_titles.TryGetValue(_current.Id, out title))
            {
                title = "SphereIQ";
            }
            TextBlock titleText = this.FindControl<TextBlock>("TitleText");
            if (titleText != null)
            {
                titleText.Text = title;
            }
            TextBlock subtitle = this.FindControl<TextBlock>("SubtitleText");
            if (subtitle != null && _session != null)
            {
                string hole = _session.HoleFile.HoleID;
                subtitle.Text = string.IsNullOrEmpty(hole) ? "" : "Hole: " + hole;
            }
            TextBlock holeFile = this.FindControl<TextBlock>("HoleFileText");
            if (holeFile != null && _session != null)
            {
                holeFile.Text = string.IsNullOrEmpty(_session.CurrentFilePath) ? "" : _session.CurrentFilePath;
            }
            Button back = this.FindControl<Button>("BackButton");
            if (back != null)
            {
                back.IsVisible = _history.Count > 1;
            }
        }

        private void OnBack(object sender, RoutedEventArgs e)
        {
            if (_history.Count <= 1)
            {
                return;
            }
            _history.Pop();
            ScreenId prev = _history.Peek();
            NavigateTo(prev, _session != null ? _session.CurrentFilePath : "");
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
