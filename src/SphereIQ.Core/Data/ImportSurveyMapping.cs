using System;

namespace EzyPlanData;

[Serializable]
public class ImportSurveyMapping
{
	private string m_name;

	private int m_azimuthIndex;

	private int m_dipIndex;

	private int m_mdIndex;

	private bool m_header;

	public string Name
	{
		get
		{
			return m_name;
		}
		set
		{
			m_name = value;
		}
	}

	public bool Header
	{
		get
		{
			return m_header;
		}
		set
		{
			m_header = value;
		}
	}

	public int Azimuth
	{
		get
		{
			return m_azimuthIndex;
		}
		set
		{
			if (value >= 0)
			{
				m_azimuthIndex = value;
				return;
			}
			throw new ArgumentOutOfRangeException("Azimuth_Index", "The index value is invalid");
		}
	}

	public int Dip
	{
		get
		{
			return m_dipIndex;
		}
		set
		{
			if (value >= 0)
			{
				m_dipIndex = value;
				return;
			}
			throw new ArgumentOutOfRangeException("Dip_Index", "The index value is invalid");
		}
	}

	public int MD
	{
		get
		{
			return m_mdIndex;
		}
		set
		{
			if (value >= 0)
			{
				m_mdIndex = value;
				return;
			}
			throw new ArgumentOutOfRangeException("MD_Index", "The index value is invalid");
		}
	}

	public ImportSurveyMapping()
	{
	}

	public ImportSurveyMapping(string setName, int setAzimuthIndex, int setDipIndex, int setMDIndex, bool setHeader)
		: this()
	{
		Set(setName, setAzimuthIndex, setDipIndex, setMDIndex, setHeader);
	}

	public ImportSurveyMapping(ImportSurveyMapping original)
		: this()
	{
		Copy(original);
	}

	public void Set(string setName, int setAzimuthIndex, int setDipIndex, int setMDIndex, bool setHeader)
	{
		Name = setName;
		Azimuth = setAzimuthIndex;
		Dip = setDipIndex;
		MD = setMDIndex;
		Header = setHeader;
	}

	public void Copy(ImportSurveyMapping original)
	{
		Name = original.Name;
		Azimuth = original.Azimuth;
		Dip = original.Dip;
		MD = original.MD;
		Header = original.Header;
	}

	public override string ToString()
	{
		return Name + ", " + MD + ", " + Azimuth + ", " + Dip + ", " + Header;
	}
}
