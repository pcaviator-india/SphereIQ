using System;
using System.ComponentModel;

namespace EzyPlanData;

[Serializable]
public class Survey
{
	public enum SetMDEnum
	{
		ValueSet,
		LessThanZero,
		OutOfUpperBound
	}

	public enum SetDipEnum
	{
		ValueSet,
		NotValidDip,
		HorizontalDip
	}

	public enum SetAzimuthEnum
	{
		ValueSet,
		NotValidAzimuth
	}

	private decimal m_measuredDepth;

	private decimal m_dip;

	private decimal m_azimuth;

	public decimal MD
	{
		get
		{
			return m_measuredDepth;
		}
		set
		{
			if (value != m_measuredDepth)
			{
				m_measuredDepth = value;
			}
		}
	}

	public decimal Dip
	{
		get
		{
			return m_dip;
		}
		set
		{
			if (value != m_dip)
			{
				m_dip = value;
			}
		}
	}

	public decimal Azimuth
	{
		get
		{
			return m_azimuth;
		}
		set
		{
			if (value != m_azimuth)
			{
				m_azimuth = value;
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public Survey()
	{
	}

	public Survey(decimal setMD, decimal setDip, decimal setAzimuth)
		: this()
	{
		Set(setMD, setDip, setAzimuth);
	}

	public Survey(Survey original)
		: this()
	{
		Copy(original);
	}

	protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
	{
		if (PropertyChanged != null)
		{
			PropertyChanged(this, e);
		}
	}

	public void Set(decimal setMD, decimal setDip, decimal setAzimuth)
	{
		MD = setMD;
		Dip = setDip;
		Azimuth = setAzimuth;
	}

	public SetMDEnum SetMD(decimal MD)
	{
		if (MD >= 0m)
		{
			if (MD <= 24000m)
			{
				m_measuredDepth = MD;
				OnPropertyChanged(new PropertyChangedEventArgs("MD"));
				return SetMDEnum.ValueSet;
			}
			return SetMDEnum.OutOfUpperBound;
		}
		return SetMDEnum.LessThanZero;
	}

	public static string ToHumanReadable(SetMDEnum value)
	{
		return value switch
		{
			SetMDEnum.ValueSet => "Operation completed successfully", 
			SetMDEnum.LessThanZero => "Invalid MD - Must be greater than or equal to 0.", 
			SetMDEnum.OutOfUpperBound => "Invalid MD - Must be less than or equal to 24000.", 
			_ => "Unexpected error", 
		};
	}

	public SetDipEnum SetDip(decimal dip)
	{
		if ((dip >= -90m) & (dip <= 90m))
		{
			if (Math.Sign(dip) == -1)
			{
				dip += 90m;
			}
			if (dip < 90m)
			{
				m_dip = dip;
				OnPropertyChanged(new PropertyChangedEventArgs("Dip"));
				return SetDipEnum.ValueSet;
			}
			return SetDipEnum.HorizontalDip;
		}
		return SetDipEnum.NotValidDip;
	}

	public static string ToHumanReadable(SetDipEnum value)
	{
		return value switch
		{
			SetDipEnum.ValueSet => "Operation completed successfully", 
			SetDipEnum.NotValidDip => "Invalid Dip - Dip must be in the range 0 to 90 or 0 to -90", 
			SetDipEnum.HorizontalDip => "Invlaid Dip - Dip can't be horizontal", 
			_ => "Unexpected error", 
		};
	}

	public SetAzimuthEnum SetAzimuth(decimal azimuth)
	{
		if ((azimuth >= 0m) & (azimuth <= 360m))
		{
			m_azimuth = azimuth;
			OnPropertyChanged(new PropertyChangedEventArgs("Azimuth"));
			return SetAzimuthEnum.ValueSet;
		}
		return SetAzimuthEnum.NotValidAzimuth;
	}

	public static string ToHumanReadable(SetAzimuthEnum value)
	{
		return value switch
		{
			SetAzimuthEnum.ValueSet => "Operation completed successfully", 
			SetAzimuthEnum.NotValidAzimuth => "Invalid Azimuth - Azimuth must be between 0 and 360 inclusive", 
			_ => "Unexpected error", 
		};
	}

	public void Copy(Survey original)
	{
		Azimuth = original.Azimuth;
		Dip = original.Dip;
		MD = original.MD;
	}

	public override string ToString()
	{
		return MD + ", " + Dip + ", " + Azimuth;
	}
}
