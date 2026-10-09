using System;
using Avalonia.Controls;
using Avalonia.Layout;
using OxyPlot;
using OxyPlotPlotView = OxyPlot.Avalonia.PlotView;
using OxyPlot.Series;
using SphereIQ.Reports;

namespace SphereIQ.App.Controls
{
    /// <summary>
    /// OxyPlot-backed renderer for <see cref="HoleChartData"/> — the direct
    /// replacement for the Nevron <c>NChartControl</c> in
    /// <c>EzyNavHoleChart</c>. Keeps the same 13 named series, the same
    /// set/visibility semantics and the same coordinate convention
    /// (ChartPoint X=Easting, Y=TVD depth-down, Z=Northing).
    /// The 3D view has no OxyPlot equivalent, so it falls back to the plan
    /// view instead of dropping data.
    /// </summary>
    public sealed class ChartHost : UserControl
    {
        private readonly OxyPlotPlotView _plotView;
        private readonly PlotModel _model;
        private HoleChartData _chartData;

        public HoleChartData ChartData
        {
            get { return _chartData; }
            set
            {
                if (ReferenceEquals(_chartData, value))
                {
                    return;
                }
                if (_chartData != null)
                {
                    _chartData.SeriesDataChanged -= OnSeriesDataChanged;
                }
                _chartData = value;
                if (_chartData != null)
                {
                    _chartData.SeriesDataChanged += OnSeriesDataChanged;
                }
                Rebuild();
            }
        }

        public ChartHost()
        {
            _model = new PlotModel();
            _plotView = new OxyPlotPlotView
            {
                Model = _model,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch
            };
            Grid grid = new Grid();
            grid.Children.Add(_plotView);
            Content = grid;
        }

        private void OnSeriesDataChanged(object sender, EventArgs e)
        {
            Rebuild();
        }

        /// <summary>Forces a redraw after mutating ChartData in place.</summary>
        public void Refresh()
        {
            Rebuild();
        }

        private void Rebuild()
        {
            if (_chartData == null)
            {
                _model.Series.Clear();
                _model.Axes.Clear();
                _model.InvalidatePlot(true);
                return;
            }
            ChartViewMode mode = _chartData.ViewMode;
            if (mode == ChartViewMode.ThreeD)
            {
                mode = ChartViewMode.TwoDTop;
            }
            bool plan = mode == ChartViewMode.TwoDTop;
            _model.Series.Clear();
            _model.Axes.Clear();
            foreach (ChartSeries s in _chartData.Series.Values)
            {
                if (!s.Displayed || s.Points.Count == 0)
                {
                    continue;
                }
                OxyPlot.OxyColor color = ChartStyling.Color(s.Name);
                if (s.Kind == ChartSeriesKind.Line)
                {
                    OxyPlot.Series.LineSeries line = new OxyPlot.Series.LineSeries
                    {
                        Title = s.Name,
                        Color = color,
                        StrokeThickness = ChartStyling.Stroke(s.Name),
                        LineStyle = ChartStyling.Style(s.Name),
                        MarkerType = MarkerType.None
                    };
                    foreach (ChartPoint p in s.Points)
                    {
                        line.Points.Add(ChartStyling.Project(p, plan, mode));
                    }
                    _model.Series.Add(line);
                }
                else
                {
                    ScatterSeries scatter = new ScatterSeries
                    {
                        Title = s.Name,
                        MarkerFill = color,
                        MarkerStroke = color,
                        MarkerSize = 4,
                        MarkerType = MarkerType.Circle
                    };
                    foreach (ChartPoint p in s.Points)
                    {
                        DataPoint dp = ChartStyling.Project(p, plan, mode);
                        scatter.Points.Add(new ScatterPoint(dp.X, dp.Y));
                    }
                    _model.Series.Add(scatter);
                }
            }
            _model.Axes.Add(ChartStyling.HorizontalAxis(plan, mode));
            _model.Axes.Add(ChartStyling.VerticalAxis(plan));
            _model.InvalidatePlot(true);
        }
    }
}
