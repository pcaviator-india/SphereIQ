namespace EzyPlanData
{
    public class DrillHoleStatus
    {
        private AbsoluteSurvey m_endOfHoleSurvey;
        private AbsoluteSurvey m_straightPathSurvey;
        private AbsoluteSurvey m_surveyAtTargetTVD;
        private RelativePosition m_currentDistance;
        private RelativePosition m_straightPathDistance;
        private RelativePosition m_distanceFromTargetAtTargetTVD;
        private AbsoluteSurveyList m_absoluteSurveyList;
        private AbsoluteSurvey m_sidetrackSurvey;
        private string m_minDLStateString;
        private MinDLSeverity m_minDLSeverity;
        private SeverityColor m_minDlStateColor;

        public AbsoluteSurvey EndOfHoleSurvey
        {
            get { return m_endOfHoleSurvey; }
            set { m_endOfHoleSurvey = value; }
        }

        public AbsoluteSurvey StraightPathSurvey
        {
            get { return m_straightPathSurvey; }
            set { m_straightPathSurvey = value; }
        }

        public AbsoluteSurvey SurveyAtTargetTVD
        {
            get { return m_surveyAtTargetTVD; }
            set { m_surveyAtTargetTVD = value; }
        }

        public RelativePosition CurrentDistanceFromTarget
        {
            get { return m_currentDistance; }
            set { m_currentDistance = value; }
        }

        public RelativePosition StraightPathDistance
        {
            get { return m_straightPathDistance; }
            set { m_straightPathDistance = value; }
        }

        public RelativePosition DistanceFromTargetAtTargetTVD
        {
            get { return m_distanceFromTargetAtTargetTVD; }
            set { m_distanceFromTargetAtTargetTVD = value; }
        }

        public AbsoluteSurveyList AbsoluteSurveyList
        {
            get { return m_absoluteSurveyList; }
            set { m_absoluteSurveyList = value; }
        }

        public string MinimumDLStateString
        {
            get { return m_minDLStateString; }
            set { m_minDLStateString = value; }
        }

        public SeverityColor MinimumDLStateColor
        {
            get { return m_minDlStateColor; }
            set
            {
                m_minDlStateColor = value;
                m_minDLSeverity = Classify(value);
            }
        }

        public MinDLSeverity MinimumDLSeverity
        {
            get { return m_minDLSeverity; }
            set
            {
                m_minDLSeverity = value;
                m_minDlStateColor = SeverityColor.FromSeverity(value);
            }
        }

        public AbsoluteSurvey SidetrackSurvey
        {
            get { return m_sidetrackSurvey; }
            set { m_sidetrackSurvey = value; }
        }

        public void Reset()
        {
            m_endOfHoleSurvey = null;
            m_straightPathSurvey = null;
            m_surveyAtTargetTVD = null;
            m_currentDistance = null;
            m_straightPathDistance = null;
            m_distanceFromTargetAtTargetTVD = null;
            m_absoluteSurveyList = null;
            m_minDLStateString = "";
            m_sidetrackSurvey = null;
            m_minDLSeverity = MinDLSeverity.Ok;
            m_minDlStateColor = SeverityColor.FromSeverity(MinDLSeverity.Ok);
        }

        private static MinDLSeverity Classify(SeverityColor color)
        {
            if (color.R == 255 && color.G == 0 && color.B == 0)
            {
                return MinDLSeverity.Severe;
            }
            if (color.R == 255 && color.G == 165 && color.B == 0)
            {
                return MinDLSeverity.Warning;
            }
            return MinDLSeverity.Ok;
        }
    }
}
