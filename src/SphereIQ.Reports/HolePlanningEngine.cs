using System;
using EzyNavMath;
using EzyPlanData;
using EzyPlanStorage;

namespace SphereIQ.Reports
{
    /// <summary>
    /// Minimum-dogleg result state, ported verbatim from
    /// <c>Body_DrillHolePlanning.MinDLStateEnum</c> (order matters for parity).
    /// </summary>
    public enum MinDLState
    {
        NotCalculated = 0,
        TargetNotAchieved = 1,
        NormalDogleg = 2,
        SevereDogleg = 3,
        ExtremeDogleg = 4,
        Error = 5
    }

    /// <summary>
    /// Dogleg severity buckets, ported from <c>Body_DrillHolePlanning.DoglegColourCodes</c>.
    /// </summary>
    public enum DoglegColourCode
    {
        Normal,
        Severe,
        Extreme
    }

    /// <summary>
    /// Portable, UI-free port of <c>Body_DrillHolePlanning.DisplayCalculatedData</c>
    /// plus its MinDL / chart / field helpers. Fills a <see cref="DrillHoleStatus"/>
    /// and a <see cref="HoleChartData"/> exactly as the WinForms screen did, so the
    /// Avalonia screen (and the report engine) share one calculation.
    /// </summary>
    public sealed class HolePlanningEngine
    {
        public decimal SevereDL { get; set; }
        public decimal ExtremeDL { get; set; }

        public HolePlanningEngine()
            : this(3m, 7m)
        {
        }

        public HolePlanningEngine(decimal severeDL, decimal extremeDL)
        {
            SevereDL = severeDL;
            ExtremeDL = extremeDL;
        }

        /// <summary>Human-readable message produced by the last <see cref="Recalculate"/> call.</summary>
        public string Message { get; private set; }

        /// <summary>
        /// Recomputes every field/chart series for the current hole file.
        /// Mirrors <c>DisplayCalculatedData</c> 1:1, including the
        /// mother/daughter/daughter-no-sidetrack branching.
        /// </summary>
        public DrillHoleStatus Recalculate(DrillHoleFile holeFile)
        {
            if (holeFile == null)
            {
                throw new ArgumentNullException("holeFile");
            }

            Message = null;
            DrillHoleStatus status = new DrillHoleStatus();
            HoleChartData chart = new HoleChartData();

            Collar collar = holeFile.Collar;
            Target target = holeFile.Target;
            SurveyList surveyList = holeFile.SurveyList;

            AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
            RelativePosition distance = new RelativePosition();
            AbsoluteSurvey surveyAtTargetTVD = null;
            RelativePosition distanceFromTarget = null;
            AbsoluteSurvey endOfHoleSurvey = null;
            RelativePosition straightPathDistance = null;
            AbsoluteSurvey extrapolatedSurvey = null;

            NavMath navMath = new NavMath();
            navMath.AbsoluteSurvey(collar, surveyList, ref absoluteSurveyList);

            AbsoluteSurvey lastSurvey = absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1);
            navMath.DistanceFromTarget(target, lastSurvey, ref distance);

            bool isDaughterSurvey = holeFile.DrillHoleType == DrillHoleFile.DrillHoleTypeEnum.daughterHole;
            if (isDaughterSurvey)
            {
                navMath.SideTrackHole(absoluteSurveyList, holeFile.SideTrackMD, out extrapolatedSurvey);
            }

            bool buildVisible;
            if (lastSurvey.TVD >= target.TVD)
            {
                buildVisible = false;
                navMath.DistanceFromTargetAtTargetTVD(target, absoluteSurveyList.List, out surveyAtTargetTVD, out distanceFromTarget);
                MinDLToTarget_Reset(status);
            }
            else
            {
                endOfHoleSurvey = new AbsoluteSurvey();
                straightPathDistance = new RelativePosition();
                NavMath.StraightPathToTargetTVDEnum rc = navMath.StraightPathToTargetTVD(target, lastSurvey, ref endOfHoleSurvey);
                if (rc == NavMath.StraightPathToTargetTVDEnum.NoError)
                {
                    navMath.DistanceFromTarget(target, endOfHoleSurvey, ref straightPathDistance);
                }
                else
                {
                    endOfHoleSurvey = null;
                    straightPathDistance = null;
                    Message = "Straight path to target TVD could not be calculated: " + NavMath.ToHumanReadable(rc);
                }
                MinDLToTarget_Attempt(status, lastSurvey, target);
                buildVisible = true;
            }

            if (isDaughterSurvey)
            {
                BuildChart(chart, collar, target, absoluteSurveyList, extrapolatedSurvey,
                    buildVisible ? endOfHoleSurvey : null, buildVisible ? null : surveyAtTargetTVD, displayStandardPlan: false);
            }
            else if (holeFile.DrillHoleType == DrillHoleFile.DrillHoleTypeEnum.motherHole)
            {
                BuildChart(chart, collar, target, absoluteSurveyList, null,
                    buildVisible ? endOfHoleSurvey : null, buildVisible ? null : surveyAtTargetTVD, displayStandardPlan: true);
            }
            else
            {
                BuildChart(chart, collar, target, absoluteSurveyList, null,
                    buildVisible ? endOfHoleSurvey : null, buildVisible ? null : surveyAtTargetTVD, displayStandardPlan: false);
            }

            status.AbsoluteSurveyList = absoluteSurveyList;
            status.EndOfHoleSurvey = lastSurvey;
            status.StraightPathSurvey = endOfHoleSurvey;
            status.SidetrackSurvey = extrapolatedSurvey;
            status.SurveyAtTargetTVD = surveyAtTargetTVD;
            status.CurrentDistanceFromTarget = distance;
            status.StraightPathDistance = straightPathDistance;
            status.DistanceFromTargetAtTargetTVD = distanceFromTarget;

            return status;
        }

        // Ported from Body_DrillHolePlanning.MinDLToTarget_Attempt / MinDLToTarget_Reset /
        // SetMinDLState / GetDoglegColourCode (UI label writes replaced with DrillHoleStatus fields).
        private void MinDLToTarget_Reset(DrillHoleStatus status)
        {
            SetMinDLState(status, MinDLState.NotCalculated, "Not calculated.", pathBelowTargetTVD: false);
        }

        private void MinDLToTarget_Attempt(DrillHoleStatus status, AbsoluteSurvey buildStart, Target target)
        {
            if (buildStart == null || target == null)
            {
                MinDLToTarget_Reset(status);
                return;
            }

            NavMath navMath = new NavMath();
            decimal dogLeg = 0m;
            BuildAndHoldResults buildAndHold = new BuildAndHoldResults();
            switch (navMath.MinimumDogLegToTarget(buildStart, target, ref dogLeg, out buildAndHold))
            {
                case NavMath.MinimumDogLegEnum.NoError:
                {
                    string text = Math.Round(dogLeg, 2, MidpointRounding.AwayFromZero).ToString();
                    switch (GetDoglegColourCode(dogLeg))
                    {
                        case DoglegColourCode.Extreme:
                            SetMinDLState(status, MinDLState.ExtremeDogleg, text, buildAndHold.BuildBelowTarget);
                            break;
                        case DoglegColourCode.Severe:
                            SetMinDLState(status, MinDLState.SevereDogleg, text, buildAndHold.BuildBelowTarget);
                            break;
                        default:
                            SetMinDLState(status, MinDLState.NormalDogleg, text, buildAndHold.BuildBelowTarget);
                            break;
                    }
                    break;
                }
                case NavMath.MinimumDogLegEnum.InsufficientTVD:
                    SetMinDLState(status, MinDLState.TargetNotAchieved, "Insufficient TVD!", pathBelowTargetTVD: false);
                    break;
                case NavMath.MinimumDogLegEnum.TargetCannotBeAchieved:
                    SetMinDLState(status, MinDLState.TargetNotAchieved, "Cannot achieve target!", pathBelowTargetTVD: false);
                    break;
                default:
                    SetMinDLState(status, MinDLState.Error, "Unknown error!", pathBelowTargetTVD: false);
                    break;
            }
        }

        private void SetMinDLState(DrillHoleStatus status, MinDLState state, string text, bool pathBelowTargetTVD)
        {
            if (pathBelowTargetTVD)
            {
                text += " (Path goes below target)";
            }
            switch (state)
            {
                case MinDLState.NotCalculated:
                    status.MinimumDLStateString = text;
                    status.MinimumDLSeverity = MinDLSeverity.Warning;
                    break;
                case MinDLState.NormalDogleg:
                    status.MinimumDLStateString = text;
                    status.MinimumDLSeverity = MinDLSeverity.Ok;
                    break;
                case MinDLState.SevereDogleg:
                    status.MinimumDLStateString = text;
                    status.MinimumDLSeverity = MinDLSeverity.Warning;
                    break;
                case MinDLState.ExtremeDogleg:
                    status.MinimumDLStateString = text;
                    status.MinimumDLSeverity = MinDLSeverity.Severe;
                    break;
                default:
                    status.MinimumDLStateString = text;
                    status.MinimumDLSeverity = MinDLSeverity.Severe;
                    break;
            }
        }

        private DoglegColourCode GetDoglegColourCode(decimal dogLeg)
        {
            if (dogLeg >= ExtremeDL)
            {
                return DoglegColourCode.Extreme;
            }
            if (dogLeg >= SevereDL)
            {
                return DoglegColourCode.Severe;
            }
            return DoglegColourCode.Normal;
        }

        // Ported from Body_DrillHolePlanning.DisplayChartData (Popup warnings become Message notes).
        private void BuildChart(HoleChartData chart, Collar collar, Target target,
            AbsoluteSurveyList surveyData, AbsoluteSurvey sidetrackStart,
            AbsoluteSurvey straightPathSurvey, AbsoluteSurvey surveyAtTargetTVD, bool displayStandardPlan)
        {
            chart.ClearChartData();
            chart.SetTarget(target, visible: true);
            chart.SetCollar(collar, visible: true);
            chart.SetSurveyData(surveyData, visible: true);

            if (sidetrackStart != null)
            {
                chart.SetHolePlan(sidetrackStart, target, visible: true);
                chart.SetSideTrack(sidetrackStart, visible: true);
            }
            else if (displayStandardPlan)
            {
                chart.SetHolePlan(collar, target, visible: true);
            }

            if (straightPathSurvey != null)
            {
                AbsoluteSurvey last = surveyData.RecordAt(surveyData.Count() - 1);
                SetStraightPathResult rc = chart.SetStraightPath(last, straightPathSurvey, visible: true);
                if (rc != SetStraightPathResult.Set)
                {
                    Message = "Chart warning: Straight path to target could not be charted. " + rc + ".";
                }
            }

            if (surveyAtTargetTVD != null)
            {
                chart.SetSurveyAtTargetTVD(surveyAtTargetTVD, visible: true);
                chart.SetDistanceFromTarget(surveyAtTargetTVD, target, visible: true);
            }
        }
    }
}
