using System.Collections.Generic;
using EzyPlanData;

namespace SphereIQ.Reports
{
    public enum ReportPageKind
    {
        Cover,
        Table,
        HoleStatus,
        HoleId,
        FaxRequest,
        FaxCancel,
        BuildResults,
        Extrapolate,
        TrendAnalysis,
        Chart
    }

    public sealed class ReportPage
    {
        public string PageName { get; set; }
        public ReportPageKind Kind { get; set; }
        public string[] Columns { get; set; }
        public List<string[]> Rows { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string HoleStatusText { get; set; }
        public MinDLSeverity HoleStatusSeverity { get; set; }
        public List<SurveyBlock> SurveyBlocks { get; set; }
        public List<DistanceBlock> DistanceBlocks { get; set; }
        public BuildAndHoldResults BuildResults { get; set; }
        public TrendAnalysisResults TrendResults { get; set; }
        public HoleChartData Chart { get; set; }
        public ChartViewMode ChartView { get; set; }
    }

    public sealed class SurveyBlock
    {
        public string Heading { get; set; }
        public AbsoluteSurvey Survey { get; set; }
    }

    public sealed class DistanceBlock
    {
        public string Heading { get; set; }
        public RelativePosition Distance { get; set; }
        public bool FourDof { get; set; }
    }
}
