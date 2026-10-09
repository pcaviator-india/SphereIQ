using System;
using System.Collections.Generic;
using EzyPlanData;

namespace SphereIQ.Reports
{
    public sealed partial class ReportDocument
    {
        public string ReportName { get; set; }
        public string ReportAuthor { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public List<ReportPage> Pages { get; } = new List<ReportPage>();
        public int NumberOfPages => Pages.Count;
        public ReportDocument() { DateTimeCreated = DateTime.Now; }
        public ReportDocument(string name, string author) : this()
        { ReportName = name; ReportAuthor = author; }
        public void AppendTablePage(string pageName, string[] columns, List<string[]> rows)
        { Pages.Add(new ReportPage { PageName = pageName, Kind = ReportPageKind.Table, Columns = columns, Rows = rows ?? new List<string[]>() }); }
        public void AppendSurveyPage(string pageName, IEnumerable<AbsoluteSurvey> surveys)
        {
            string[] cols = new string[] { "MD", "Dip", "Azi", "TVD", "Northing", "Easting", "DL" };
            List<string[]> rows = new List<string[]>();
            foreach (AbsoluteSurvey s in surveys)
                rows.Add(new string[] { s.MD.ToString(), s.Dip.ToString(), s.Azimuth.ToString(), s.TVD.ToString(), s.Northing.ToString(), s.Easting.ToString(), s.DL.ToString() });
            AppendTablePage(pageName, cols, rows);
        }
        public void AppendCoverPage(string pageName, string title, string subtitle)
        { Pages.Add(new ReportPage { PageName = pageName, Kind = ReportPageKind.Cover, Title = title, Subtitle = subtitle }); }
        public void AppendHoleStatusPage(string pageName, DrillHoleStatus status)
        { Pages.Add(new ReportPage { PageName = pageName, Kind = ReportPageKind.HoleStatus, HoleStatusText = status?.MinimumDLStateString, HoleStatusSeverity = status?.MinimumDLSeverity ?? MinDLSeverity.Ok }); }
    }
}
