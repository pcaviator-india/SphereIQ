using EzyPlanData;

namespace SphereIQ.Reports
{
    public sealed partial class HoleChartData
    {
        public SetSideTrackResult SetSideTrack(AbsoluteSurvey survey, bool visible)
        {
            ChartSeries s = GetOrCreate("SideTrack");
            s.Points.Clear();
            if (survey != null)
            {
                s.Points.Add(ChartPoint.FromSurvey(survey));
                s.IsSet = true;
                s.Visible = visible;
                RaiseChanged();
                return SetSideTrackResult.Set;
            }
            s.IsSet = false;
            s.Visible = false;
            return SetSideTrackResult.NullParameter;
        }

        public SetSurveyAtTargetTVDResult SetSurveyAtTargetTVD(AbsoluteSurvey survey, bool visible)
        {
            ChartSeries s = GetOrCreate("SurveyAtTargetTVD");
            if (survey != null)
            {
                if (InRange((double)survey.Northing) && InRange((double)survey.Easting))
                {
                    s.Points.Clear();
                    s.Points.Add(ChartPoint.FromSurvey(survey));
                    s.IsSet = true;
                    s.Visible = visible;
                    RaiseChanged();
                    return SetSurveyAtTargetTVDResult.Set;
                }
                s.Points.Clear();
                s.IsSet = false;
                s.Visible = false;
                return SetSurveyAtTargetTVDResult.OutOfRange;
            }
            s.Points.Clear();
            s.IsSet = false;
            s.Visible = false;
            return SetSurveyAtTargetTVDResult.ArgumentNull;
        }

        public void SetDistanceFromTarget(AbsoluteSurvey survey, Target target, bool visible)
        {
            ChartSeries s = GetOrCreate("DistanceFromTarget");
            s.Points.Clear();
            if (survey != null && target != null)
            {
                s.Points.Add(ChartPoint.FromSurvey(survey));
                s.Points.Add(ChartPoint.FromTarget(target));
                s.IsSet = true;
                s.Visible = visible;
                RaiseChanged();
            }
            else
            {
                s.IsSet = false;
                s.Visible = false;
            }
        }

        public void ClearChartData()
        {
            foreach (ChartSeries s in Series.Values)
            {
                s.Clear();
            }
        }

        /// <summary>
        /// Projects a 3D point into the 2D plane for the given view mode:
        /// Top = Easting/Northing, Easting profile = Easting/TVD,
        /// Northing profile = Northing/TVD. Y is positive-down (depth).
        /// </summary>
        public static void Project(ChartPoint p, ChartViewMode mode, out double x, out double y)
        {
            switch (mode)
            {
                case ChartViewMode.TwoDTop:
                    x = p.X; y = p.Z; break;
                case ChartViewMode.TwoDEasting:
                    x = p.X; y = p.Y; break;
                case ChartViewMode.TwoDNorthing:
                    x = p.Z; y = p.Y; break;
                default:
                    x = p.X; y = p.Y; break;
            }
        }
    }
}
