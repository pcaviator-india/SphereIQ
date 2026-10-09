using System;

namespace SphereIQ.App
{
    /// <summary>
    /// Workflow navigation targets (= the legacy FinishedEventArgs states).
    /// The MainWindow shows one screen at a time; each screen raises
    /// Navigate with its desired destination.
    /// </summary>
    public enum ScreenId
    {
        Welcome,
        HoleManagement,
        CreateNewHole,
        OpenHoleFile,
        HolePlanning,
        EditSurveyData,
        Import,
        BuildToTarget,
        TrendAnalysis,
        Extrapolate,
        EditDetails,
        SideTrackHole,
        Reports,
        Exit
    }

    public sealed class NavigateEventArgs : EventArgs
    {
        public ScreenId Destination { get; }
        public string FilePath { get; }

        public NavigateEventArgs(ScreenId destination, string filePath = "")
        {
            Destination = destination;
            FilePath = filePath ?? "";
        }
    }
}
