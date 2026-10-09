using System;
using EzyPlanData;
using EzyPlanStorage;
using SphereIQ.Reports;

namespace SphereIQ.App
{
    /// <summary>
    /// Shared session: the open hole file + status + chart model.
    /// Replaces the ref-chains passed through every Procedure_* constructor.
    /// </summary>
    public sealed class AppSession
    {
        public DrillHoleFile HoleFile { get; } = new DrillHoleFile();

        public DrillHoleStatus HoleStatus { get; } = new DrillHoleStatus();

        public HoleChartData Chart { get; } = new HoleChartData();

        public string CurrentFilePath { get; set; } = "";

        public string PreviousFilePath { get; set; } = "";

        public string ProductVersion { get; set; } = "1.0";

        public event EventHandler FileChanged;

        public void NotifyFileChanged()
        {
            if (FileChanged != null)
            {
                FileChanged(this, EventArgs.Empty);
            }
        }
    }
}
