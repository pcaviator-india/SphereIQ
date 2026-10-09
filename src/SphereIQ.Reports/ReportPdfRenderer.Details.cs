using System.Collections.Generic;

namespace SphereIQ.Reports
{
    public static partial class ReportPdfRenderer
    {
        private static List<string[]> DetailLines(ReportPage page)
        {
            List<string[]> lines = new List<string[]>();
            if (page.Kind == ReportPageKind.HoleStatus)
            {
                lines.Add(new string[] { "Status", page.HoleStatusText ?? "" });
                lines.Add(new string[] { "Severity", page.HoleStatusSeverity.ToString() });
            }
            if (page.Title != null)
            {
                lines.Add(new string[] { "Title", page.Title });
            }
            if (page.Subtitle != null)
            {
                lines.Add(new string[] { "Detail", page.Subtitle });
            }
            if (page.BuildResults != null)
            {
                lines.Add(new string[] { "Build Results", page.BuildResults.ToString() });
            }
            if (page.TrendResults != null)
            {
                lines.Add(new string[] { "Trend Results", page.TrendResults.ToString() });
            }
            if (page.Chart != null)
            {
                lines.Add(new string[] { "Chart", page.Chart.Series.Count + " series (" + page.ChartView.ToString() + ")" });
            }
            if (page.Rows != null)
            {
                lines.AddRange(page.Rows);
            }
            if (page.SurveyBlocks != null)
            {
                foreach (SurveyBlock b in page.SurveyBlocks)
                {
                    lines.Add(new string[] { b.Heading ?? "Survey", b.Survey != null ? ("MD " + b.Survey.MD + " TVD " + b.Survey.TVD) : "" });
                }
            }
            if (page.DistanceBlocks != null)
            {
                foreach (DistanceBlock b in page.DistanceBlocks)
                {
                    lines.Add(new string[] { b.Heading ?? "Distance", b.Distance != null ? b.Distance.ToString() : "" });
                }
            }
            return lines;
        }
    }
}
