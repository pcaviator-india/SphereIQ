using EzyPlanData;
using SphereIQ.Reports;
using Xunit;

namespace SphereIQ.Core.Tests
{
    public sealed class HoleChartTests
    {
        private static AbsoluteSurvey Station(decimal md, decimal e, decimal n, decimal tvd)
        {
            return new AbsoluteSurvey(md, 5m, 10m, tvd, n, e, 0.1m);
        }

        [Fact]
        public void SetMethods_MirrorNevronSemantics()
        {
            HoleChartData chart = new HoleChartData();
            Assert.Equal(13, chart.Series.Count);
            bool raised = false;
            chart.SeriesDataChanged += delegate { raised = true; };

            chart.SetCollar(new Collar(500000m, 7000000m), true);
            Assert.True(chart.Series["Collar"].Displayed);
            Assert.Equal(500000.0, chart.Series["Collar"].Points[0].X);
            Assert.Equal(0.0, chart.Series["Collar"].Points[0].Y);
            Assert.Equal(7000000.0, chart.Series["Collar"].Points[0].Z);
            Assert.True(raised);

            chart.SetTarget(new Target(500100m, 7000100m, 1000m), true);
            Assert.True(chart.Series["Target"].Displayed);
            Assert.Equal(1000.0, chart.Series["Target"].Points[0].Y);

            chart.SetCollar(null, true);
            Assert.False(chart.Series["Collar"].Displayed);
        }

        [Fact]
        public void StraightPath_EnforcesCoordinateRange()
        {
            HoleChartData chart = new HoleChartData();
            AbsoluteSurvey a = Station(0m, 500000m, 7000000m, 0m);
            AbsoluteSurvey b = Station(100m, 500100m, 7000100m, 100m);
            Assert.Equal(SetStraightPathResult.Set, chart.SetStraightPath(a, b, true));
            Assert.True(chart.Series["StraightPath"].Displayed);

            AbsoluteSurvey far = Station(200m, 99999999m, 7000000m, 200m);
            Assert.Equal(SetStraightPathResult.OutOfRange, chart.SetStraightPath(a, far, true));
            Assert.False(chart.Series["StraightPath"].Displayed);
            Assert.Equal(SetStraightPathResult.ArgumentNull, chart.SetStraightPath(null, b, true));
        }

        [Fact]
        public void Projection_MapsViewsToPlanes()
        {
            ChartPoint p = new ChartPoint(100.0, 50.0, 200.0);
            double x, y;
            HoleChartData.Project(p, ChartViewMode.TwoDTop, out x, out y);
            Assert.Equal(100.0, x); Assert.Equal(200.0, y);
            HoleChartData.Project(p, ChartViewMode.TwoDEasting, out x, out y);
            Assert.Equal(100.0, x); Assert.Equal(50.0, y);
            HoleChartData.Project(p, ChartViewMode.TwoDNorthing, out x, out y);
            Assert.Equal(200.0, x); Assert.Equal(50.0, y);
        }

        [Fact]
        public void ClearChartData_HidesAllSeries()
        {
            HoleChartData chart = new HoleChartData();
            chart.SetCollar(new Collar(1m, 2m), true);
            chart.SetTarget(new Target(3m, 4m, 5m), true);
            chart.ClearChartData();
            foreach (ChartSeries s in chart.Series.Values)
            {
                Assert.False(s.Displayed);
            }
        }

        [Fact]
        public void ReportBuilders_CaptureTemplateData()
        {
            ReportDocument doc = new ReportDocument("H", "A");
            doc.AppendHoleIdPage("Hole", "H-1", "RIG-1", "Perth", "Area", "Client");
            doc.AppendBuildResultsPage("Build", new BuildAndHoldResults());
            doc.AppendTrendAnalysisPage("Trend", new TrendAnalysisResults(), new RelativePosition());
            HoleChartData chart = new HoleChartData();
            chart.SetCollar(new Collar(1m, 2m), true);
            doc.AppendChartPage("Chart", chart, ChartViewMode.TwoDTop);
            doc.AppendExtrapolatedPage("Extra", new System.Collections.Generic.List<ExtrapolatedAbsoluteSurvey>());
            Assert.Equal(5, doc.NumberOfPages);
            Assert.Equal(ReportPageKind.BuildResults, doc.Pages[1].Kind);
            Assert.Equal(ReportPageKind.Chart, doc.Pages[3].Kind);
            Assert.Equal(ChartViewMode.TwoDTop, doc.Pages[3].ChartView);
        }
    }
}
