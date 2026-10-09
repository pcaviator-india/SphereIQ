using System.Collections.Generic;
using EzyPlanData;

namespace SphereIQ.Reports
{
    /// <summary>
    /// Typed builders mirroring each legacy Report_* template constructor.
    /// Each captures the same data the WinForms template displayed; the
    /// Avalonia/PDF renderers (Phase 3) lay it out natively.
    /// </summary>
    public sealed partial class ReportDocument
    {
        public void AppendHoleIdPage(string pageName, string holeId, string rigId, string location, string area, string client)
        {
            Pages.Add(new ReportPage
            {
                PageName = pageName,
                Kind = ReportPageKind.HoleId,
                Title = holeId,
                Subtitle = rigId,
                Rows = new List<string[]>
                {
                    new string[] { "Location", location ?? "" },
                    new string[] { "Area", area ?? "" },
                    new string[] { "Client", client ?? "" }
                }
            });
        }

        public void AppendHoleStatusDetailPage(string pageName, AbsoluteSurvey atTargetTVD, AbsoluteSurvey endOfHole, AbsoluteSurvey sidetrack, AbsoluteSurvey straightPathSurvey, RelativePosition currentDist, RelativePosition distAtTargetTVD, RelativePosition straightPathDist, string minDLText, MinDLSeverity severity)
        {
            Pages.Add(new ReportPage
            {
                PageName = pageName,
                Kind = ReportPageKind.HoleStatus,
                HoleStatusText = minDLText,
                HoleStatusSeverity = severity,
                SurveyBlocks = new List<SurveyBlock>
                {
                    new SurveyBlock { Heading = "Survey at Target TVD", Survey = atTargetTVD },
                    new SurveyBlock { Heading = "End of Hole", Survey = endOfHole },
                    new SurveyBlock { Heading = "Sidetrack", Survey = sidetrack },
                    new SurveyBlock { Heading = "Straight Path", Survey = straightPathSurvey }
                },
                DistanceBlocks = new List<DistanceBlock>
                {
                    new DistanceBlock { Heading = "Current Distance", Distance = currentDist },
                    new DistanceBlock { Heading = "Distance at Target TVD", Distance = distAtTargetTVD },
                    new DistanceBlock { Heading = "Straight Path Distance", Distance = straightPathDist }
                }
            });
        }

        public void AppendBuildResultsPage(string pageName, BuildAndHoldResults build)
        {
            Pages.Add(new ReportPage { PageName = pageName, Kind = ReportPageKind.BuildResults, BuildResults = build });
        }

        public void AppendExtrapolatePage(string pageName, AbsoluteSurvey atEnd, AbsoluteSurvey straightEnd, AbsoluteSurvey atTargetTVD, RelativePosition extraDist, RelativePosition distAtTVD, RelativePosition straightDist)
        {
            Pages.Add(new ReportPage
            {
                PageName = pageName,
                Kind = ReportPageKind.Extrapolate,
                SurveyBlocks = new List<SurveyBlock>
                {
                    new SurveyBlock { Heading = "End of Extrapolation", Survey = atEnd },
                    new SurveyBlock { Heading = "End of Straight Path", Survey = straightEnd },
                    new SurveyBlock { Heading = "Extrapolated at Target TVD", Survey = atTargetTVD }
                },
                DistanceBlocks = new List<DistanceBlock>
                {
                    new DistanceBlock { Heading = "Extrapolated Distance", Distance = extraDist },
                    new DistanceBlock { Heading = "Distance at Target TVD", Distance = distAtTVD },
                    new DistanceBlock { Heading = "Straight Path Distance", Distance = straightDist }
                }
            });
        }

        public void AppendTrendAnalysisPage(string pageName, TrendAnalysisResults results, RelativePosition extraDist)
        {
            Pages.Add(new ReportPage
            {
                PageName = pageName,
                Kind = ReportPageKind.TrendAnalysis,
                TrendResults = results,
                DistanceBlocks = new List<DistanceBlock>
                {
                    new DistanceBlock { Heading = "Extrapolated Distance", Distance = extraDist }
                }
            });
        }

        public void AppendChartPage(string pageName, HoleChartData chart, ChartViewMode view)
        {
            Pages.Add(new ReportPage { PageName = pageName, Kind = ReportPageKind.Chart, Chart = chart, ChartView = view });
        }

        public void AppendExtrapolatedPage(string pageName, IEnumerable<ExtrapolatedAbsoluteSurvey> surveys)
        {
            string[] cols = new string[] { "MD", "Dip", "Azi", "TVD", "Northing", "Easting", "DL", "Toolface" };
            List<string[]> rows = new List<string[]>();
            foreach (ExtrapolatedAbsoluteSurvey s in surveys)
                rows.Add(new string[] { s.MD.ToString(), s.Dip.ToString(), s.Azimuth.ToString(), s.TVD.ToString(), s.Northing.ToString(), s.Easting.ToString(), s.DL.ToString(), s.Toolface.ToString() });
            AppendTablePage(pageName, cols, rows);
        }
    }
}
