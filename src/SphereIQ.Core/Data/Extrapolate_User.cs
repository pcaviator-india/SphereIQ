namespace EzyPlanData;

public class Extrapolate_User
{
	public enum SetBuildRateEnum
	{
		Set,
		InvalidBuildRate
	}

	public enum SetDistanceEnum
	{
		Set,
		InvalidDistance
	}

	public enum SetTurnRateEnum
	{
		Set,
		InvalidTurnRate
	}

	private decimal m_buildRate;

	private decimal m_distance;

	private decimal m_turnRate;

	public decimal BuildRate
	{
		get
		{
			return m_buildRate;
		}
		set
		{
			m_buildRate = value;
		}
	}

	public decimal Distance
	{
		get
		{
			return m_distance;
		}
		set
		{
			m_distance = value;
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
			m_turnRate = value;
		}
	}

	public Extrapolate_User()
	{
	}

	public Extrapolate_User(decimal buildRate, decimal turnRate, decimal distance)
		: this()
	{
		BuildRate = buildRate;
		Distance = distance;
		TurnRate = turnRate;
	}

	public Extrapolate_User(ref Extrapolate_User original)
		: this()
	{
		Copy(ref original);
	}

	public SetBuildRateEnum SetBuildRate(decimal value)
	{
		m_buildRate = value;
		return SetBuildRateEnum.Set;
	}

	public static string ToHumanReadable(SetBuildRateEnum value)
	{
		return value switch
		{
			SetBuildRateEnum.Set => "Operation completed successfully", 
			SetBuildRateEnum.InvalidBuildRate => "The build value is invalid", 
			_ => "Unknown error", 
		};
	}

	public SetDistanceEnum SetDistance(decimal value)
	{
		m_distance = value;
		return SetDistanceEnum.Set;
	}

	public static string ToHumanReadable(SetDistanceEnum value)
	{
		return value switch
		{
			SetDistanceEnum.Set => "Operation completed succesfully", 
			SetDistanceEnum.InvalidDistance => "The distance value is invalid", 
			_ => "Unknown Error", 
		};
	}

	public SetTurnRateEnum SetTurnRate(decimal value)
	{
		m_turnRate = value;
		return SetTurnRateEnum.Set;
	}

	public static string ToHumanReadable(SetTurnRateEnum value)
	{
		return value switch
		{
			SetTurnRateEnum.Set => "Operation completed successfully", 
			SetTurnRateEnum.InvalidTurnRate => "The turn rate value is invalid", 
			_ => "Unknown Error", 
		};
	}

	public void Copy(ref Extrapolate_User original)
	{
		BuildRate = original.BuildRate;
		TurnRate = original.TurnRate;
		Distance = original.Distance;
	}

	public override string ToString()
	{
		return "Build Rate: " + BuildRate + ", Turn Rate: " + TurnRate + ", Distance: " + Distance;
	}
}
