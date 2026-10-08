namespace EzyPlanData;

public class TrendAnalysisResults : Report
{
	private decimal m_angleChange;

	private decimal m_angleChangeOver;

	private AzimuthTrend m_azimuthTrend;

	private decimal m_buildRate;

	private decimal m_dogLeg;

	private AbsoluteSurveyList m_extrapolatedSurveyData;

	private InclinationTrend m_inclinationTrend;

	private decimal m_turnRate;

	private decimal m_toolFaceSetting;

	public decimal BuildRate
	{
		get
		{
			return m_buildRate;
		}
		set
		{
			if (m_buildRate != value)
			{
				m_buildRate = value;
			}
		}
	}

	public decimal TurnRate
	{
		get
		{
			return m_turnRate;
		}
		set
		{
			if (m_turnRate != value)
			{
				m_turnRate = value;
			}
		}
	}

	public decimal DogLegSeverity
	{
		get
		{
			return m_dogLeg;
		}
		set
		{
			if (m_dogLeg != value)
			{
				m_dogLeg = value;
			}
		}
	}

	public decimal AngleChange
	{
		get
		{
			return m_angleChange;
		}
		set
		{
			if (m_angleChange != value)
			{
				m_angleChange = value;
			}
		}
	}

	public decimal AngleChangeOver
	{
		get
		{
			return m_angleChangeOver;
		}
		set
		{
			if (m_angleChangeOver != value)
			{
				m_angleChangeOver = value;
			}
		}
	}

	public InclinationTrend InclinationTrend
	{
		get
		{
			return m_inclinationTrend;
		}
		set
		{
			if (m_inclinationTrend != value)
			{
				m_inclinationTrend = value;
			}
		}
	}

	public AzimuthTrend AzimuthTrend
	{
		get
		{
			return m_azimuthTrend;
		}
		set
		{
			if (m_azimuthTrend != value)
			{
				m_azimuthTrend = value;
			}
		}
	}

	public AbsoluteSurveyList ExtrapolatedSurveyData
	{
		get
		{
			return m_extrapolatedSurveyData;
		}
		set
		{
			if (value != null)
			{
				m_extrapolatedSurveyData = new AbsoluteSurveyList(value);
			}
			else
			{
				m_extrapolatedSurveyData = new AbsoluteSurveyList();
			}
		}
	}

	public decimal ToolFaceSetting
	{
		get
		{
			return m_toolFaceSetting;
		}
		set
		{
			m_toolFaceSetting = value;
		}
	}

	public TrendAnalysisResults()
	{
		m_extrapolatedSurveyData = new AbsoluteSurveyList();
	}

	public TrendAnalysisResults(decimal setToolFaceSetting, decimal setAngleChange, decimal setAngleChagneOver, AzimuthTrend setAzimuthTrend, decimal setBuildRate, decimal setDogLeg, decimal setTurnRate, AbsoluteSurveyList setExtrapolatedSurveyData, InclinationTrend setInclinationTrend)
		: this()
	{
		Set(setToolFaceSetting, setAngleChange, setAngleChagneOver, setAzimuthTrend, setBuildRate, setDogLeg, setTurnRate, setExtrapolatedSurveyData, setInclinationTrend);
	}

	public TrendAnalysisResults(TrendAnalysisResults original)
		: this()
	{
		Copy(original);
	}

	public void Set(decimal setToolFaceSetting, decimal setAngleChange, decimal setAngleChagneOver, AzimuthTrend setAzimuthTrend, decimal setBuildRate, decimal setDogLeg, decimal setTurnRate, AbsoluteSurveyList setExtrapolatedSurveyData, InclinationTrend setInclinationTrend)
	{
		AngleChange = setAngleChange;
		AngleChangeOver = setAngleChagneOver;
		AzimuthTrend = setAzimuthTrend;
		BuildRate = setBuildRate;
		DogLegSeverity = setDogLeg;
		TurnRate = setTurnRate;
		ExtrapolatedSurveyData = setExtrapolatedSurveyData;
		InclinationTrend = setInclinationTrend;
		ToolFaceSetting = setToolFaceSetting;
	}

	public void Copy(TrendAnalysisResults original)
	{
		if (original != null)
		{
			AngleChange = original.AngleChange;
			AngleChangeOver = original.AngleChangeOver;
			AzimuthTrend = original.AzimuthTrend;
			BuildRate = original.BuildRate;
			DogLegSeverity = original.DogLegSeverity;
			InclinationTrend = original.InclinationTrend;
			TurnRate = original.TurnRate;
			ExtrapolatedSurveyData = original.ExtrapolatedSurveyData;
			ToolFaceSetting = original.ToolFaceSetting;
		}
	}

	public override string ToString()
	{
		string text = null;
		if (ExtrapolatedSurveyData != null)
		{
			text = text + ExtrapolatedSurveyData.ToString() + ", ";
		}
		return ToolFaceSetting + ", " + AngleChange + ", " + AngleChangeOver + ", " + AzimuthTrend.ToString() + ", " + BuildRate + ", " + DogLegSeverity + ", , " + InclinationTrend.ToString() + ", " + TurnRate + text;
	}
}
