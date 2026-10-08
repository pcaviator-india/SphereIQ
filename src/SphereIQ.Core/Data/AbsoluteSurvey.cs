using System;

namespace EzyPlanData;

[Serializable]
public class AbsoluteSurvey : Survey, IComparable
{
	private decimal m_tvd;

	private decimal m_northing;

	private decimal m_easting;

	private decimal m_dogleg;

	public decimal TVD
	{
		get
		{
			return m_tvd;
		}
		set
		{
			m_tvd = value;
		}
	}

	public decimal Northing
	{
		get
		{
			return m_northing;
		}
		set
		{
			m_northing = value;
		}
	}

	public decimal Easting
	{
		get
		{
			return m_easting;
		}
		set
		{
			m_easting = value;
		}
	}

	public decimal DL
	{
		get
		{
			return m_dogleg;
		}
		set
		{
			m_dogleg = value;
		}
	}

	public AbsoluteSurvey()
	{
	}

	public AbsoluteSurvey(decimal setMD, decimal setDip, decimal setAzimuth, decimal setTVD, decimal setNorthing, decimal setEasting, decimal setDL)
		: this()
	{
		Set(setMD, setDip, setAzimuth, setTVD, setNorthing, setEasting, setDL);
	}

	public AbsoluteSurvey(Survey setSurvey, decimal setTVD, decimal setNorthing, decimal setEasting, decimal setDL)
		: this()
	{
		if (setSurvey != null)
		{
			Set(setSurvey.MD, setSurvey.Dip, setSurvey.Azimuth, setTVD, setNorthing, setEasting, setDL);
		}
		else
		{
			Set(0m, 0m, 0m, setTVD, setNorthing, setEasting, setDL);
		}
	}

	public AbsoluteSurvey(AbsoluteSurvey original)
		: this()
	{
		Copy(original);
	}

	public void Set(decimal setMD, decimal setDip, decimal setAzimuth, decimal setTVD, decimal setNorthing, decimal setEasting, decimal setDL)
	{
		MD = setMD;
		Azimuth = setAzimuth;
		Dip = setDip;
		TVD = setTVD;
		Northing = setNorthing;
		Easting = setEasting;
		DL = setDL;
	}

	public void Copy(AbsoluteSurvey original)
	{
		MD = original.MD;
		Azimuth = original.Azimuth;
		Dip = original.Dip;
		Northing = original.Northing;
		Easting = original.Easting;
		DL = original.DL;
		TVD = original.TVD;
	}

	public override string ToString()
	{
		string text = null;
		text = text + "MD:" + MD;
		text = text + ", Azi:" + Azimuth;
		text = text + ", Dip:" + Dip;
		text = text + ", TVD:" + TVD;
		text = text + ", North:" + Northing;
		text = text + ", East:" + Easting;
		return text + ", DL:" + DL;
	}

	public int CompareTo(object obj)
	{
		if (obj is AbsoluteSurvey)
		{
			AbsoluteSurvey absoluteSurvey = (AbsoluteSurvey)obj;
			return MD.CompareTo(absoluteSurvey.MD);
		}
		throw new ArgumentException("object is not a AbsoluteSurvey");
	}
}
