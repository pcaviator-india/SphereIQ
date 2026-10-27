using System;
using System.ComponentModel;
using System.Globalization;
using EzyPlanData;

namespace SphereIQ.App.Screens
{
    /// <summary>
    /// Grid row for the Edit Survey Data screen.
    /// <para>
    /// Wraps an <see cref="AbsoluteSurvey"/> because the legacy screen bound the
    /// grid to <c>AbsoluteSurveyList.List</c> (a
    /// <c>SortableBindingList&lt;AbsoluteSurvey&gt;</c>) whose items raise no
    /// change notification for TVD/Northing/Easting/DL. Avalonia's DataGrid has
    /// no WinForms <c>IBindingList</c> shim, so the row must implement
    /// <see cref="INotifyPropertyChanged"/> itself for the grid to refresh after
    /// an edit.
    /// </para>
    /// <para>
    /// Column order and the N2 format match the legacy
    /// <c>dataGridView1.DefaultCellStyle.Format = "N2"</c> and the
    /// <c>AbsoluteSurveyList.Data</c> name order (MD, Dip, Azimuth, TVD,
    /// Northing, Easting, DL). Cell 6 (DL) is the severity-key column, exactly as
    /// <c>UpdateDataGrid</c> read <c>item.Cells[6]</c>.
    /// </para>
    /// </summary>
    public sealed class SurveyRow : INotifyPropertyChanged
    {
        private readonly AbsoluteSurvey _survey;
        private readonly decimal _severeDL;
        private readonly decimal _extremeDL;

        public SurveyRow(AbsoluteSurvey survey, decimal severeDL, decimal extremeDL)
        {
            _survey = survey;
            _severeDL = severeDL;
            _extremeDL = extremeDL;
        }

        /// <summary>The underlying absolute survey; null until the grid rebuilds.</summary>
        public AbsoluteSurvey Survey { get { return _survey; } }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Cell 0 - measured depth. Also the identity key for Remove.</summary>
        public string MD { get { return Num(_survey.MD); } }

        public string Dip { get { return Num(_survey.Dip); } }

        public string Azimuth { get { return Num(_survey.Azimuth); } }

        public string TVD { get { return Num(_survey.TVD); } }

        public string Northing { get { return Num(_survey.Northing); } }

        public string Easting { get { return Num(_survey.Easting); } }

        /// <summary>Cell 6 - dogleg severity. Drives the row background colour.</summary>
        public string DL { get { return Num(_survey.DL); } }

        /// <summary>
        /// Severity bucket for this row, mirroring
        /// <c>Body_EditSurveyData.UpdateDataGrid</c>: extreme first, then severe,
        /// then no highlight.
        /// </summary>
        public SurveySeverity Severity
        {
            get
            {
                if (_survey.DL >= _extremeDL)
                {
                    return SurveySeverity.Extreme;
                }
                if (_survey.DL >= _severeDL)
                {
                    return SurveySeverity.Severe;
                }
                return SurveySeverity.Normal;
            }
        }

        public void Refresh()
        {
            string[] names = new string[] { "MD", "Dip", "Azimuth", "TVD", "Northing", "Easting", "DL", "Severity" };
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                foreach (string name in names)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
        }

        /// <summary>
        /// N2 formatting. Uses InvariantCulture so the grid is identical on every
        /// locale - the legacy format string had no explicit culture, which made
        /// European decimal commas round-trip inconsistently.
        /// </summary>
        private static string Num(decimal value)
        {
            return value.ToString("N2", CultureInfo.InvariantCulture);
        }
    }

    /// <summary>Dogleg severity buckets behind <see cref="SurveyRow.Severity"/>.</summary>
    public enum SurveySeverity
    {
        Normal,
        Severe,
        Extreme
    }
}
