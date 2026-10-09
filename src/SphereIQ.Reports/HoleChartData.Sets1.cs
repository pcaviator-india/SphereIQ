using System.Collections.Generic;
using EzyPlanData;

namespace SphereIQ.Reports
{
    public sealed partial class HoleChartData
    {
        public void SetHolePlan(AbsoluteSurvey start, Target target, bool visible)
        {
            ChartSeries s = GetOrCreate("HolePlan");
            s.Points.Clear();
            if (start != null && target != null)
            {
                s.Points.Add(ChartPoint.FromSurvey(start));
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

        public SetSurveyDataResult SetSurveyData(AbsoluteSurveyList surveyList, bool visible)
        {
            ChartSeries s = GetOrCreate("SurveyData");
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
                return SetSurveyDataResult.Set;
            }
            s.IsSet = false;
            s.Visible = false;
            return SetSurveyDataResult.NullSurvey;
        }

        public SetSelectedSurveysResult SetSelectedSurveys(IEnumerable<AbsoluteSurvey> surveys, bool visible)
        {
            ChartSeries s = GetOrCreate("SelectedSurveys");
            s.Points.Clear();
            if (surveys != null)
            {
                foreach (AbsoluteSurvey survey in surveys)
                {
                    s.Points.Add(ChartPoint.FromSurvey(survey));
                }
                s.IsSet = true;
                s.Visible = visible;
                RaiseChanged();
                return SetSelectedSurveysResult.Set;
            }
            s.IsSet = false;
            s.Visible = false;
            return SetSelectedSurveysResult.NullParameter;
        }

        public SetExtrapolatedResult SetExtrapolated(IEnumerable<AbsoluteSurvey> surveys, bool visible)
        {
            ChartSeries s = GetOrCreate("Extrapolated");
            s.Points.Clear();
            if (surveys != null)
            {
                foreach (AbsoluteSurvey survey in surveys)
                {
                    s.Points.Add(ChartPoint.FromSurvey(survey));
                }
                s.IsSet = true;
                s.Visible = visible;
                RaiseChanged();
                return SetExtrapolatedResult.Set;
            }
            s.IsSet = false;
            s.Visible = false;
            return SetExtrapolatedResult.NullSurvey;
        }
    }
}
