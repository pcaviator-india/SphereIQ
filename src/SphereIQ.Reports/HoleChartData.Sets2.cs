using EzyPlanData;

namespace SphereIQ.Reports
{
    public sealed partial class HoleChartData
    {
        public SetExtrapolatedResult SetExtrapolated(ExtrapolatedSurveyList extrapolatedList, bool visible)
        {
            ChartSeries u = GetOrCreate("UserExtrapolated");
            u.Points.Clear();
            if (extrapolatedList != null)
            {
                foreach (ExtrapolatedAbsoluteSurvey item in extrapolatedList.List)
                {
                    u.Points.Add(new ChartPoint((double)item.Easting, (double)item.TVD, (double)item.Northing));
                }
                u.IsSet = true;
                u.Visible = visible;
                SetExtrapolated(extrapolatedList.GetFullExtrapolatedPath().List, true);
                RaiseChanged();
                return SetExtrapolatedResult.Set;
            }
            SetExtrapolated((System.Collections.Generic.IEnumerable<AbsoluteSurvey>)null, false);
            u.IsSet = false;
            u.Visible = false;
            return SetExtrapolatedResult.NullSurvey;
        }

        public void SetBuild(AbsoluteSurveyList surveyList, bool visible)
        {
            ChartSeries s = GetOrCreate("Build");
            s.Points.Clear();
            if (surveyList != null)
            {
                foreach (AbsoluteSurvey survey in surveyList)
                {
                    s.Points.Add(ChartPoint.FromSurvey(survey));
                }
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

        private static void AddSegment(ChartSeries s, AbsoluteSurvey from, AbsoluteSurvey to)
        {
            s.Points.Clear();
            s.Points.Add(ChartPoint.FromSurvey(from));
            s.Points.Add(ChartPoint.FromSurvey(to));
        }

        public void SetStraightPathToTarget(AbsoluteSurvey from, AbsoluteSurvey to, bool visible)
        {
            ChartSeries s = GetOrCreate("StraightPathToTarget");
            s.Points.Clear();
            if (from != null && to != null)
            {
                AddSegment(s, from, to);
                s.IsSet = true;
                s.Visible = visible;
            }
            else
            {
                s.IsSet = false;
                s.Visible = false;
            }
        }

        public SetStraightPathResult SetStraightPath(AbsoluteSurvey from, AbsoluteSurvey to, bool visible)
        {
            ChartSeries s = GetOrCreate("StraightPath");
            if (from != null && to != null)
            {
                if (InRange((double)from.Northing) && InRange((double)to.Easting))
                {
                    AddSegment(s, from, to);
                    s.IsSet = true;
                    s.Visible = visible;
                    return SetStraightPathResult.Set;
                }
                s.Points.Clear();
                s.IsSet = false;
                s.Visible = false;
                return SetStraightPathResult.OutOfRange;
            }
            s.Points.Clear();
            s.IsSet = false;
            s.Visible = false;
            return SetStraightPathResult.ArgumentNull;
        }
    }
}
