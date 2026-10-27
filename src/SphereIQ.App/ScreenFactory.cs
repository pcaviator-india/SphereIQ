using System;

namespace SphereIQ.App
{
    /// <summary>
    /// Maps ScreenId -> screen. Welcome is implemented; the rest return
    /// placeholders until their slices are ported, so navigation never
    /// dead-ends while work proceeds slice by slice.
    /// </summary>
    public static class ScreenFactory
    {
        public static IScreen Create(ScreenId id, AppSession session, string filePath)
        {
            switch (id)
            {
                case ScreenId.Welcome:
                    return new Screens.WelcomeScreen();
                case ScreenId.OpenHoleFile:
                    return new Screens.OpenHoleFileScreen();
                case ScreenId.CreateNewHole:
                    return new Screens.CreateNewHoleScreen();
                case ScreenId.HolePlanning:
                    return new Screens.DrillHolePlanningScreen(session);
                case ScreenId.HoleManagement:
                    return new Screens.DrillHoleManagementScreen(session);
                case ScreenId.EditSurveyData:
                    return new Screens.EditSurveyDataScreen(session);
                case ScreenId.BuildToTarget:
                    return new Screens.BuildToTargetScreen(session);
                case ScreenId.TrendAnalysis:
                    return new Screens.TrendAnalysisScreen(session);
                case ScreenId.Extrapolate:
                    return new Screens.ExtrapolateScreen(session);
                default:
                    return new Screens.PlaceholderScreen(id);
            }
        }
    }
}
