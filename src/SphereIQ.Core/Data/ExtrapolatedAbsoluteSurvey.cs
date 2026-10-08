namespace EzyPlanData;

public class ExtrapolatedAbsoluteSurvey : AbsoluteSurvey
{
	private decimal m_toolface;

	private AbsoluteSurveyList m_absoluteSurveyList;

	public decimal Toolface
	{
		get
		{
			return m_toolface;
		}
		set
		{
			m_toolface = value;
		}
	}

	public ExtrapolatedAbsoluteSurvey()
	{
		m_absoluteSurveyList = new AbsoluteSurveyList();
	}

	public ExtrapolatedAbsoluteSurvey(decimal md, decimal dip, decimal azimuth, decimal tvd, decimal northing, decimal easting, decimal dl, decimal toolface, AbsoluteSurveyList absoluteSurveyList)
		: base(md, dip, azimuth, tvd, northing, easting, dl)
	{
		Toolface = toolface;
		SetAbsoluteSurveyList(absoluteSurveyList);
	}

	public ExtrapolatedAbsoluteSurvey(decimal md, decimal dip, decimal azimuth, decimal tvd, decimal northing, decimal easting, decimal dl, decimal toolface)
		: base(md, dip, azimuth, tvd, northing, easting, dl)
	{
		Toolface = toolface;
		SetAbsoluteSurveyList(null);
	}

	public ExtrapolatedAbsoluteSurvey(ExtrapolatedAbsoluteSurvey original)
	{
		Copy(original);
	}

	public ExtrapolatedAbsoluteSurvey(AbsoluteSurvey original)
		: this(original.MD, original.Dip, original.Azimuth, original.TVD, original.Northing, original.Easting, original.DL, -1m, null)
	{
		m_absoluteSurveyList = new AbsoluteSurveyList();
	}

	public AbsoluteSurveyList GetAbsoluteSurveyList()
	{
		return m_absoluteSurveyList;
	}

	public void SetAbsoluteSurveyList(AbsoluteSurveyList value)
	{
		m_absoluteSurveyList = value;
	}

	public void Copy(ExtrapolatedAbsoluteSurvey original)
	{
		Copy((AbsoluteSurvey)original);
		Toolface = original.Toolface;
		SetAbsoluteSurveyList(new AbsoluteSurveyList(original.GetAbsoluteSurveyList()));
	}

	public void Set(decimal md, decimal dip, decimal azimuth, decimal tvd, decimal northing, decimal easting, decimal dl, decimal toolface)
	{
		Set(md, dip, azimuth, tvd, northing, easting, dl, toolface, null);
	}

	public void Set(decimal md, decimal dip, decimal azimuth, decimal tvd, decimal northing, decimal easting, decimal dl, decimal toolface, AbsoluteSurveyList absoluteSurveyList)
	{
		Set(md, dip, azimuth, tvd, northing, easting, dl);
		Toolface = toolface;
		SetAbsoluteSurveyList(absoluteSurveyList);
	}

	public override string ToString()
	{
		return base.ToString() + ", TF:" + Toolface;
	}
}
