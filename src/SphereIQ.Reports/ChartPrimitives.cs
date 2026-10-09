using System.Collections.Generic;
using EzyPlanData;

namespace SphereIQ.Reports
{
    /// <summary>
    /// View modes mirroring Nevron <c>ChartViewModes</c> on EzyNavHoleChart:
    /// TwoDTop / TwoDNorthing / TwoDEasting / ThreeD.
    /// </summary>
    public enum ChartViewMode
    {
        TwoDTop,
        TwoDNorthing,
        TwoDEasting,
        ThreeD
    }

    public enum ChartSeriesKind
    {
        Point,
        Line
    }

    public sealed class ChartSeries
    {
        public string Name { get; set; }
        public ChartSeriesKind Kind { get; set; }
        public bool Visible { get; set; }
        public bool IsSet { get; set; }
        public List<ChartPoint> Points { get; } = new List<ChartPoint>();

        /// <summary>Effective on-screen state: set AND visible (mirrors RefreshChart logic).</summary>
        public bool Displayed => IsSet && Visible;

        public void Clear()
        {
            Points.Clear();
            IsSet = false;
            Visible = false;
        }
    }

    public struct ChartPoint
    {
        /// <summary>Easting.</summary>
        public double X;
        /// <summary>TVD (depth, positive down).</summary>
        public double Y;
        /// <summary>Northing.</summary>
        public double Z;

        public ChartPoint(double x, double y) { X = x; Y = y; Z = 0; }
        public ChartPoint(double x, double y, double z) { X = x; Y = y; Z = z; }

        public static ChartPoint FromCollar(Collar c)
        {
            return new ChartPoint((double)c.Easting, 0.0, (double)c.Northing);
        }

        public static ChartPoint FromTarget(Target t)
        {
            return new ChartPoint((double)t.Easting, (double)t.TVD, (double)t.Northing);
        }

        public static ChartPoint FromSurvey(AbsoluteSurvey s)
        {
            return new ChartPoint((double)s.Easting, (double)s.TVD, (double)s.Northing);
        }
    }

    public enum SetSurveyDataResult { Set, NullSurvey }
    public enum SetSelectedSurveysResult { Set, NullParameter }
    public enum SetExtrapolatedResult { Set, NullSurvey }
    public enum SetStraightPathResult { Set, ArgumentNull, OutOfRange }
    public enum SetSideTrackResult { Set, NullParameter }
    public enum SetSurveyAtTargetTVDResult { Set, ArgumentNull, OutOfRange }
}
