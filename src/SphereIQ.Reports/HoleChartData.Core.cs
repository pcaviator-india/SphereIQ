using System;
using System.Collections.Generic;
using EzyPlanData;

namespace SphereIQ.Reports
{
    /// <summary>
    /// Portable replacement for the Nevron-backed <c>EzyNavHoleChart</c> state.
    /// Same 13 named series, same Set*/visibility/set-flag semantics, same
    /// coordinate convention (X=Easting, Y=TVD/depth-down, Z=Northing).
    /// Rendering is left to the UI layer (OxyPlot in Phase 3).
    /// </summary>
    public sealed partial class HoleChartData
    {
        public const double CoordinateLimit = 20000000.0;

        public Dictionary<string, ChartSeries> Series { get; } = new Dictionary<string, ChartSeries>();

        public ChartViewMode ViewMode { get; set; } = ChartViewMode.ThreeD;

        public event EventHandler SeriesDataChanged;

        public HoleChartData()
        {
            Add("Collar", ChartSeriesKind.Point);
            Add("Target", ChartSeriesKind.Point);
            Add("HolePlan", ChartSeriesKind.Line);
            Add("SurveyData", ChartSeriesKind.Point);
            Add("Build", ChartSeriesKind.Point);
            Add("StraightPathToTarget", ChartSeriesKind.Line);
            Add("StraightPath", ChartSeriesKind.Line);
            Add("SelectedSurveys", ChartSeriesKind.Point);
            Add("Extrapolated", ChartSeriesKind.Point);
            Add("SideTrack", ChartSeriesKind.Point);
            Add("SurveyAtTargetTVD", ChartSeriesKind.Point);
            Add("DistanceFromTarget", ChartSeriesKind.Line);
            Add("UserExtrapolated", ChartSeriesKind.Point);
        }

        private void Add(string name, ChartSeriesKind kind)
        {
            Series[name] = new ChartSeries { Name = name, Kind = kind };
        }

        public ChartSeries GetOrCreate(string name)
        {
            ChartSeries s;
            if (!Series.TryGetValue(name, out s))
            {
                s = new ChartSeries { Name = name, Kind = ChartSeriesKind.Point, Visible = true };
                Series[name] = s;
            }
            return s;
        }

        private void RaiseChanged()
        {
            if (SeriesDataChanged != null)
            {
                SeriesDataChanged(this, EventArgs.Empty);
            }
        }

        private static bool InRange(double v)
        {
            return v <= CoordinateLimit && v >= -CoordinateLimit;
        }

        public void SetCollar(Collar collar, bool visible)
        {
            ChartSeries s = GetOrCreate("Collar");
            s.Points.Clear();
            if (collar != null)
            {
                s.Points.Add(ChartPoint.FromCollar(collar));
                s.IsSet = true;
                s.Visible = visible;
            }
            else
            {
                s.IsSet = false;
                s.Visible = false;
            }
            RaiseChanged();
        }

        public void SetTarget(Target target, bool visible)
        {
            ChartSeries s = GetOrCreate("Target");
            s.Points.Clear();
            if (target != null)
            {
                s.Points.Add(ChartPoint.FromTarget(target));
                s.IsSet = true;
                s.Visible = visible;
            }
            else
            {
                s.IsSet = false;
                s.Visible = false;
            }
            RaiseChanged();
        }

        public void SetHolePlan(Collar collar, Target target, bool visible)
        {
            ChartSeries s = GetOrCreate("HolePlan");
            s.Points.Clear();
            if (collar != null && target != null)
            {
                s.Points.Add(ChartPoint.FromCollar(collar));
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
    }
}
