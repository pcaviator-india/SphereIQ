namespace EzyPlanData;

public class BuildAndHoldResults : Report
{
	private AbsoluteSurvey m_endOfBuildSurvey;

	private AbsoluteSurvey m_endOfHoleSurvey;

	private AbsoluteSurveyList m_projectedSurveyData;

	private decimal m_naviDistance;

	private decimal m_straightDistance;

	private decimal m_naviStopMVD;

	private decimal m_naviStartMVD;

	private decimal m_ToolFaceSetting;

	private decimal m_dogLeg;

	private decimal m_wedgeSetting;

	private bool m_buildBelowTarget;

	public decimal DogLeg
	{
		get
		{
			return m_dogLeg;
		}
		set
		{
			m_dogLeg = value;
		}
	}

	public decimal ToolFaceSetting
	{
		get
		{
			return m_ToolFaceSetting;
		}
		set
		{
			m_ToolFaceSetting = value;
		}
	}

	public decimal NaviStartMVD
	{
		get
		{
			return m_naviStartMVD;
		}
		set
		{
			m_naviStartMVD = value;
		}
	}

	public decimal NaviStopMVD
	{
		get
		{
			return m_naviStopMVD;
		}
		set
		{
			m_naviStopMVD = value;
		}
	}

	public decimal NaviDistance
	{
		get
		{
			return m_naviDistance;
		}
		set
		{
			m_naviDistance = value;
		}
	}

	public decimal StraightDistance
	{
		get
		{
			return m_straightDistance;
		}
		set
		{
			m_straightDistance = value;
		}
	}

	public AbsoluteSurvey EndOfBuildSurvey
	{
		get
		{
			return m_endOfBuildSurvey;
		}
		set
		{
			if (value != null)
			{
				m_endOfBuildSurvey = new AbsoluteSurvey(value);
			}
			else
			{
				m_endOfBuildSurvey = null;
			}
		}
	}

	public AbsoluteSurvey EndOfHoleSurvey
	{
		get
		{
			return m_endOfHoleSurvey;
		}
		set
		{
			if (value != null)
			{
				m_endOfHoleSurvey = new AbsoluteSurvey(value);
			}
			else
			{
				m_endOfHoleSurvey = null;
			}
		}
	}

	public AbsoluteSurveyList ProjectedSurveyData
	{
		get
		{
			return m_projectedSurveyData;
		}
		set
		{
			if (value != null)
			{
				m_projectedSurveyData = new AbsoluteSurveyList(value);
			}
			else
			{
				m_projectedSurveyData = null;
			}
		}
	}

	public decimal WedgeSetting
	{
		get
		{
			return m_wedgeSetting;
		}
		set
		{
			m_wedgeSetting = value;
		}
	}

	public bool BuildBelowTarget
	{
		get
		{
			return m_buildBelowTarget;
		}
		set
		{
			m_buildBelowTarget = value;
		}
	}

	public BuildAndHoldResults()
	{
		m_buildBelowTarget = false;
	}

	public BuildAndHoldResults(AbsoluteSurvey setEndOfBuildSurvey, AbsoluteSurvey setEndOfHoleSurvey, decimal setNaviDistance, decimal setStraightDistance, decimal setNaviStartMVD, decimal setNaviStopMVD, AbsoluteSurveyList setProjectedSurveyData, decimal setToolFaceSetting, decimal setWedgeSetting, bool setBuildBelowTarget)
		: this()
	{
		Set(setEndOfBuildSurvey, setEndOfHoleSurvey, setNaviDistance, setStraightDistance, setNaviStartMVD, setNaviStopMVD, setProjectedSurveyData, setToolFaceSetting, setWedgeSetting, setBuildBelowTarget);
	}

	public BuildAndHoldResults(BuildAndHoldResults original)
		: this()
	{
		Copy(original);
	}

	public void Set(AbsoluteSurvey setEndOfBuildSurvey, AbsoluteSurvey setEndOfHoleSurvey, decimal setNaviDistance, decimal setStraightDistance, decimal setNaviStartMVD, decimal setNaviStopMVD, AbsoluteSurveyList setProjectedSurveyData, decimal setToolFaceSetting, decimal setWedgeSetting, bool setBuildBelowTarget)
	{
		EndOfBuildSurvey = setEndOfBuildSurvey;
		EndOfHoleSurvey = setEndOfHoleSurvey;
		NaviDistance = setNaviDistance;
		NaviStartMVD = setNaviStartMVD;
		NaviStopMVD = setNaviStopMVD;
		ToolFaceSetting = setToolFaceSetting;
		StraightDistance = setStraightDistance;
		ProjectedSurveyData = setProjectedSurveyData;
		WedgeSetting = setWedgeSetting;
		BuildBelowTarget = setBuildBelowTarget;
	}

	public void Copy(BuildAndHoldResults original)
	{
		EndOfBuildSurvey = original.EndOfBuildSurvey;
		EndOfHoleSurvey = original.EndOfHoleSurvey;
		NaviDistance = original.NaviDistance;
		NaviStartMVD = original.NaviStartMVD;
		NaviStopMVD = original.NaviStopMVD;
		ToolFaceSetting = original.ToolFaceSetting;
		StraightDistance = original.StraightDistance;
		ProjectedSurveyData = original.ProjectedSurveyData;
		WedgeSetting = original.WedgeSetting;
		m_buildBelowTarget = original.m_buildBelowTarget;
	}

	public override string ToString()
	{
		string text = null;
		if (EndOfBuildSurvey != null)
		{
			text = string.Concat(text, EndOfBuildSurvey, ", ");
		}
		if (EndOfHoleSurvey != null)
		{
			text = string.Concat(text, EndOfHoleSurvey, ", ");
		}
		return NaviDistance + ", " + StraightDistance + ", " + NaviStartMVD + ", " + NaviStopMVD + ", " + ToolFaceSetting + ", " + WedgeSetting + ", " + BuildBelowTarget + ", " + text;
	}

	public string ToStringSummary()
	{
		return "DogLeg: " + DogLeg + ", Navi Distance: " + NaviDistance + ", Straight Distance: " + StraightDistance + ", Below Target: " + BuildBelowTarget;
	}
}
