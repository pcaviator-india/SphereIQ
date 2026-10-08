using System;

namespace EzyPlanData;

[Serializable]
public class Collar
{
	public enum SetEnum
	{
		Set,
		EastingOutOfRange,
		NorthingOutOfRange,
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

	private decimal m_easting;

	private decimal m_northing;

	public decimal Easting => m_easting;

	public decimal Northing => m_northing;

	public Collar()
	{
	}

	public Collar(decimal setEasting, decimal setNorthing)
		: this()
	{
		Set(setEasting, setNorthing);
	}

	public Collar(Collar original)
		: this()
	{
		Copy(original);
	}

	public SetEnum Set(decimal setEasting, decimal setNorthing)
	{
		decimal easting = m_easting;
		decimal northing = m_northing;
		switch (SetEasting(setEasting))
		{
		case SetEastingEnum.OutOfRange:
			SetFailed(easting, northing);
			return SetEnum.EastingOutOfRange;
		default:
			SetFailed(easting, northing);
			return SetEnum.UnknownError;
		case SetEastingEnum.Set:
			switch (SetNorthing(setNorthing))
			{
			case SetNorthingEnum.Set:
				return SetEnum.Set;
			case SetNorthingEnum.OutOfRange:
				SetFailed(easting, northing);
				return SetEnum.NorthingOutOfRange;
			default:
				SetFailed(easting, northing);
				return SetEnum.UnknownError;
			}
		}
	}

	private void SetFailed(decimal easting, decimal northing)
	{
		m_easting = easting;
		m_northing = northing;
	}

	public static string ToHumanReadable(SetEnum error)
	{
		return error switch
		{
			SetEnum.Set => "Target Set", 
			SetEnum.EastingOutOfRange => ToHumanReadable(SetEastingEnum.OutOfRange), 
			SetEnum.NorthingOutOfRange => ToHumanReadable(SetNorthingEnum.OutOfRange), 
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
			SetNorthingEnum.OutOfRange => "Invalid Northing - Northings must be greater than or equal to -11000000 and less than or equal to 11000000.", 
			_ => "Unknown error setting northing", 
		};
	}

	public void Copy(Collar original)
	{
		m_easting = original.Easting;
		m_northing = original.Northing;
	}

	public override string ToString()
	{
		return "North:" + Northing + ", East:" + Easting;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		try
		{
			return Equals((Collar)obj);
		}
		catch (InvalidCastException)
		{
			return false;
		}
	}

	public bool Equals(Collar comp)
	{
		if (comp == null)
		{
			return false;
		}
		if (Easting == comp.Easting)
		{
			return Northing == comp.Northing;
		}
		return false;
	}
}
