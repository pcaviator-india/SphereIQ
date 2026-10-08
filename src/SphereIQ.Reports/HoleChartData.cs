using System.Collections.Generic;
using EzyPlanData;

namespace SphereIQ.Reports
{
    public enum ChartView { Top, EastingProfile, NorthingProfile, Side }
    public sealed class ChartSeries
    {
        public string Name { get; set; }
        public bool Visible { get; set; }
        public List<ChartPoint> Points { get; } = new List<ChartPoint>();
    }
    public struct ChartPoint
    {
        public double X; public double Y; public double Z;
        public ChartPoint(double x, double y) { X = x; Y = y; Z = 0; }
        public ChartPoint(double x, double y, double z) { X = x; Y = y; Z = z; }
    }
    public sealed class HoleChartData
    {
        public Dictionary<string, ChartSeries> Series { get; } = new Dictionary<string, ChartSeries>();
        public ChartSeries GetOrCreate(string name)
        {
            ChartSeries s; if (!Series.TryGetValue(name, out s)) { s = new ChartSeries { Name = name, Visible = true }; Series[name] = s; }
            return s;
        }
        public void SetCollar(Collar c, bool visible)
        { ChartSeries s = GetOrCreate("Collar"); s.Visible = visible; s.Points.Clear(); if (c != null) s.Points.Add(new ChartPoint((double)c.Easting, (double)c.Northing)); }
        public void SetTarget(Target t, bool visible)
        { ChartSeries s = GetOrCreate("Target"); s.Visible = visible; s.Points.Clear(); if (t != null) s.Points.Add(new ChartPoint((double)t.Easting, (double)t.Northing, (double)t.TVD)); }
        public void SetSurveyData(IEnumerable<AbsoluteSurvey> surveys, bool visible)
        { ChartSeries s = GetOrCreate("SurveyData"); s.Visible = visible; s.Points.Clear(); if (surveys != null) foreach (AbsoluteSurvey a in surveys) s.Points.Add(new ChartPoint((double)a.Easting, (double)a.Northing, (double)a.TVD)); }
        public void SetExtrapolated(IEnumerable<ExtrapolatedAbsoluteSurvey> surveys, bool visible)
        { ChartSeries s = GetOrCreate("Extrapolated"); s.Visible = visible; s.Points.Clear(); if (surveys != null) foreach (ExtrapolatedAbsoluteSurvey a in surveys) s.Points.Add(new ChartPoint((double)a.Easting, (double)a.Northing, (double)a.TVD)); }
        public void SetStraightPath(AbsoluteSurvey from, AbsoluteSurvey to, bool visible)
        { ChartSeries s = GetOrCreate("StraightPath"); s.Visible = visible; s.Points.Clear(); if (from != null) s.Points.Add(new ChartPoint((double)from.Easting, (double)from.Northing, (double)from.TVD)); if (to != null) s.Points.Add(new ChartPoint((double)to.Easting, (double)to.Northing, (double)to.TVD)); }
    }
}
