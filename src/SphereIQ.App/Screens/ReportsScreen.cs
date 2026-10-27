using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using EzyPlanData;
using SphereIQ.Reports;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Port of the legacy <c>Popup_ReportPreview</c> + <c>Report_*</c> screens.
    /// Generates PDF reports from the open hole data using PDFsharp 6.
    /// </summary>
    public sealed class ReportsScreen : IScreen
    {
        public ScreenId Id { get { return ScreenId.Reports; } }
        public object Content { get; private set; }
        public event EventHandler<NavigateEventArgs> Navigate;
        public AppSession Session { get { return _session; } set { } }

        private readonly AppSession _session;
        private readonly StackPanel _root;
        private readonly TextBlock _status;

        private Button _btnHoleStatus;
        private Button _btnBuildResults;
        private Button _btnTrendAnalysis;
        private Button _btnExtrapolate;
        private Button _btnFaxCover;
        private Button _btnCancel;

        public ReportsScreen(AppSession session)
        {
            _session = session;
            _root = new StackPanel { Margin = new Thickness(12), Spacing = 8 };

            var heading = new TextBlock
            {
                Text = "Reports & Preview",
                FontSize = FontMap.ToDips(15.75),
                Margin = new Thickness(0, 0, 0, 8)
            };
            _root.Children.Add(heading);

            var instr = new TextBlock
            {
                Text = "Generate and preview PDF reports for the open hole file.",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 12)
            };
            _root.Children.Add(instr);

            _status = new TextBlock { Text = "Choose a report type.", TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            _root.Children.Add(_status);

            _btnHoleStatus = CreateReportButton("Hole Status Report", OnHoleStatus);
            _btnBuildResults = CreateReportButton("Build Results Report", OnBuildResults);
            _btnTrendAnalysis = CreateReportButton("Trend Analysis Report", OnTrendAnalysis);
            _btnExtrapolate = CreateReportButton("Extrapolate Report", OnExtrapolate);
            _btnFaxCover = CreateReportButton("Fax Cover Sheet", OnFaxCover);
            _btnCancel = CreateReportButton("Cancel", OnCancel);

            _root.Children.Add(_btnHoleStatus);
            _root.Children.Add(_btnBuildResults);
            _root.Children.Add(_btnTrendAnalysis);
            _root.Children.Add(_btnExtrapolate);
            _root.Children.Add(_btnFaxCover);
            _root.Children.Add(_btnCancel);

            Content = _root;
        }

        public void OnShown()
        {
            _status.Text = _session != null && _session.HoleFile != null
                ? "Hole: " + _session.HoleFile.HoleID
                : "No hole file open.";
        }

        private Button CreateReportButton(string text, Action onClick)
        {
            var b = new Button { Content = text, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch, Height = 40, Margin = new Thickness(0, 2) };
            b.Click += (s, e) => onClick();
            return b;
        }

        private void OnHoleStatus()
        {
            GenerateAndPreview("Hole Status Report", doc =>
            {
                doc.AppendCoverPage("Hole Status", _session.HoleFile?.HoleID ?? "Hole", "SphereIQ Drilling Report");
                if (_session.HoleStatus != null)
                {
                    doc.AppendHoleStatusPage("Hole Status", _session.HoleStatus);
                }
            });
        }

        private void OnBuildResults()
        {
            GenerateAndPreview("Build Results Report", doc =>
            {
                doc.AppendCoverPage("Build Results", _session.HoleFile?.HoleID ?? "Hole", "Build To Target Report");
                doc.AppendBuildResultsPage("Build Results", new BuildAndHoldResults());
            });
        }

        private void OnTrendAnalysis()
        {
            GenerateAndPreview("Trend Analysis Report", doc =>
            {
                doc.AppendCoverPage("Trend Analysis", _session.HoleFile?.HoleID ?? "Hole", "Trend Analysis Report");
                doc.AppendTrendAnalysisPage("Trend Analysis", new TrendAnalysisResults(), new RelativePosition());
            });
        }

        private void OnExtrapolate()
        {
            GenerateAndPreview("Extrapolate Report", doc =>
            {
                doc.AppendCoverPage("Extrapolate Report", _session.HoleFile?.HoleID ?? "Hole", "Extrapolation Report");
                doc.AppendExtrapolatedPage("Extrapolated Surveys", System.Array.Empty<ExtrapolatedAbsoluteSurvey>());
            });
        }

        private void OnFaxCover()
        {
            GenerateAndPreview("Fax Cover Sheet", doc =>
            {
                doc.AppendCoverPage("Fax Cover Sheet", _session.HoleFile?.HoleID ?? "Hole", "EzyNav Fax Report");
                // Fax feature renders as PDF cover page only (visual parity, no transport)
            });
        }

        private void OnCancel()
        {
            Raise(ScreenId.HolePlanning, _session.CurrentFilePath);
        }

        private void GenerateAndPreview(string reportName, Action<ReportDocument> buildDoc)
        {
            if (_session == null || _session.HoleFile == null)
            {
                _status.Text = "No hole file open.";
                return;
            }

            try
            {
                var doc = new ReportDocument(reportName, "SphereIQ");
                buildDoc(doc);

                var defaultName = reportName.Replace(" ", "_") + ".pdf";
                var savePath = System.IO.Path.Combine(
                    System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
                    defaultName);

                ReportPdfRenderer.Save(doc, savePath);

                _status.Text = "Report saved: " + savePath;

                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = savePath, UseShellExecute = true });
                }
                catch
                {
                    // Ignore if we can't open it
                }
            }
            catch (Exception ex)
            {
                _status.Text = "Report generation failed: " + ex.Message;
            }
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