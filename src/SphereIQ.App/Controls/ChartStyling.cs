using OxyPlot;
using OxyPlot.Axes;
using SphereIQ.Reports;
namespace SphereIQ.App.Controls
{
    /// <summary>
    /// Per-series colours/strokes and the axis projection for the hole chart,
    /// mirroring the Nevron series styling on <c>EzyNavHoleChart</c>.
    /// </summary>
    internal static class ChartStyling
    {
        /// <summary>
        /// Projects a ChartPoint (X=Easting, Y=TVD, Z=Northing) into plot space.
        /// Plan view plots Easting vs Northing; section views plot a horizontal
        /// axis against TVD (depth), which is rendered top-down.
        /// </summary>
        public static DataPoint Project(ChartPoint p, bool plan, ChartViewMode mode)
        {
            if (plan)
            {
                return new DataPoint(p.Z, p.X);
            }
            if (mode == ChartViewMode.TwoDEasting)
            {
                return new DataPoint(p.X, p.Y);
            }
            return new DataPoint(p.Z, p.Y);
        }

        public static LinearAxis HorizontalAxis(bool plan, ChartViewMode mode)
        {
            return new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = plan ? "Northing" : (mode == ChartViewMode.TwoDEasting ? "Easting" : "Northing"),
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromArgb(40, 0, 0, 0)
            };
        }

        public static LinearAxis VerticalAxis(bool plan)
        {
            return new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = plan ? "Easting" : "TVD",
                StartPosition = plan ? 0 : 1,
                EndPosition = plan ? 1 : 0,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColor.FromArgb(40, 0, 0, 0)
            };
        }

        public static OxyColor Color(string name)
        {
            switch (name)
            {
                case "Collar": return OxyColors.DarkGreen;
                case "Target": return OxyColors.Red;
                case "HolePlan": return OxyColors.Gray;
                case "SurveyData": return OxyColors.Blue;
                case "Build": return OxyColors.LimeGreen;
                case "StraightPathToTarget": return OxyColors.Orange;
                case "StraightPath": return OxyColors.Purple;
                case "SelectedSurveys": return OxyColors.Cyan;
                case "Extrapolated": return OxyColors.DeepPink;
                case "SideTrack": return OxyColors.Magenta;
                case "SurveyAtTargetTVD": return OxyColors.Gold;
                case "DistanceFromTarget": return OxyColors.OrangeRed;
                case "UserExtrapolated": return OxyColors.SlateBlue;
                default: return OxyColors.Black;
            }
        }

        public static double Stroke(string name)
        {
            switch (name)
            {
                case "HolePlan": return 1.5;
                case "StraightPathToTarget": return 1.5;
                case "DistanceFromTarget": return 1.0;
                default: return 2.0;
            }
        }

        public static LineStyle Style(string name)
        {
            switch (name)
            {
                case "HolePlan":
                case "StraightPathToTarget":
                    return LineStyle.Dash;
                default:
                    return LineStyle.Solid;
            }
        }
    }
}
