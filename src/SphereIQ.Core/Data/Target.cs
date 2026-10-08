using System;

namespace EzyPlanData;

[Serializable]
public class Target
{
	public enum SetEnum
	{
		Set,
		EastingOutOfRange,
		NorthingOutOfRange,
		TVDOutOfRange,
		UnknownError
	}

	public enum SetEastingEnum
	{
		Set,
		OutOfRange
	}

	public enum SetNorthingEnum
	{
		Set,
		OutOfRange
	}

	public enum SetTVDEnum
	{
		Set,
		OutOfRange
	}

	private decimal m_easting;

	private decimal m_northing;

	private decimal m_TVD;

	public decimal Easting => m_easting;

	public decimal Northing => m_northing;

	public decimal TVD => m_TVD;

	public Target()
	{
	}

	public Target(decimal setEasting, decimal setNorthing, decimal setTVD)
		: this()
	{
		Set(setEasting, setNorthing, setTVD);
	}

	public Target(Target original)
		: this()
	{
		Copy(original);
	}

	public SetEnum Set(decimal setEasting, decimal setNorthing, decimal setTVD)
	{
		decimal easting = m_easting;
		decimal northing = m_northing;
		decimal tVD = m_TVD;
		switch (SetEasting(setEasting))
		{
		case SetEastingEnum.OutOfRange:
			SetFailed(easting, northing, tVD);
			return SetEnum.EastingOutOfRange;
		default:
			SetFailed(easting, northing, tVD);
			return SetEnum.UnknownError;
		case SetEastingEnum.Set:
			switch (SetNorthing(setNorthing))
			{
			case SetNorthingEnum.OutOfRange:
				SetFailed(easting, northing, tVD);
				return SetEnum.NorthingOutOfRange;
			default:
				SetFailed(easting, northing, tVD);
				return SetEnum.UnknownError;
			case SetNorthingEnum.Set:
				switch (SetTVD(setTVD))
				{
				case SetTVDEnum.Set:
					return SetEnum.Set;
				case SetTVDEnum.OutOfRange:
					SetFailed(easting, northing, tVD);
					return SetEnum.TVDOutOfRange;
				default:
					SetFailed(easting, northing, tVD);
					return SetEnum.UnknownError;
				}
			}
		}
	}

	private void SetFailed(decimal easting, decimal northing, decimal tvd)
	{
		m_easting = easting;
		m_northing = northing;
		m_TVD = tvd;
	}

	public static string ToHumanReadable(SetEnum error)
	{
		return error switch
		{
			SetEnum.Set => "Target Set", 
			SetEnum.EastingOutOfRange => ToHumanReadable(SetEastingEnum.OutOfRange), 
			SetEnum.NorthingOutOfRange => ToHumanReadable(SetNorthingEnum.OutOfRange), 
			SetEnum.TVDOutOfRange => ToHumanReadable(SetTVDEnum.OutOfRange), 
			_ => "Unknown Error", 
		};
	}

	public SetEastingEnum SetEasting(decimal easting)
	{
		if ((easting >= -1000000m) & (easting <= 1000000m))
		{
			m_easting = easting;
			return SetEastingEnum.Set;
		}
		return SetEastingEnum.OutOfRange;
	}

	public static string ToHumanReadable(SetEastingEnum error)
	{
		return error switch
		{
			SetEastingEnum.Set => "Value set", 
			SetEastingEnum.OutOfRange => "Invalid Easting - Eastings must be greater than or equal to -1000000 and less than or equal to 1000000", 
			_ => "Unknown error setting easting", 
		};
	}

	public SetNorthingEnum SetNorthing(decimal northing)
	{
		if ((northing >= -11000000m) & (northing <= 11000000m))
		{
			m_northing = northing;
			return SetNorthingEnum.Set;
		}
		return SetNorthingEnum.OutOfRange;
	}

	public static string ToHumanReadable(SetNorthingEnum error)
	{
		return error switch
		{
			SetNorthingEnum.Set => "Value set", 
			SetNorthingEnum.OutOfRange => "Invalid Northing - Northings must be greater than or equal to -11000000 and less than or equal to 11000000", 
			_ => "Unknown error setting northing", 
		};
	}

	public SetTVDEnum SetTVD(decimal tvd)
	{
		if ((tvd > 0m) & (tvd <= 24000m))
		{
			m_TVD = tvd;
			return SetTVDEnum.Set;
		}
		return SetTVDEnum.OutOfRange;
	}

	public static string ToHumanReadable(SetTVDEnum error)
	{
		return error switch
		{
			SetTVDEnum.Set => "Value set", 
			SetTVDEnum.OutOfRange => "Invalid TVD - TVD must be greater than 0 and less than or equal to 24000", 
			_ => "Unknown error setting TVD", 
		};
	}

	public void Copy(Target original)
	{
		m_easting = original.Easting;
		m_northing = original.Northing;
		m_TVD = original.TVD;
	}

	public override string ToString()
	{
		return "TVD:" + TVD + ", East:" + Easting + ", North:" + Northing;
	}

	public bool Equals(Target comp)
	{
		if (comp == null)
		{
			return false;
		}
		if (Easting == comp.Easting && Northing == comp.Northing)
		{
			return TVD == comp.TVD;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		try
		{
			return Equals((Target)obj);
		}
		catch (InvalidCastException)
		{
			return false;
		}
	}
}
