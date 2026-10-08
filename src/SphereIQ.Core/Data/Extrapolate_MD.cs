namespace EzyPlanData;

public class Extrapolate_MD
{
	private decimal m_MD;

	private decimal m_dogLeg;

	private decimal m_toolfaceSetting;

	public decimal MD
	{
		get
		{
			return m_MD;
		}
		set
		{
			m_MD = value;
		}
	}

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

	public decimal ToolfaceSetting
	{
		get
		{
			return m_toolfaceSetting;
		}
		set
		{
			m_toolfaceSetting = value;
		}
	}

	public Extrapolate_MD()
	{
	}

	public Extrapolate_MD(decimal md, decimal dogLeg, decimal toolfaceSetting)
		: this()
	{
		MD = md;
		DogLeg = dogLeg;
		ToolfaceSetting = toolfaceSetting;
	}

	public Extrapolate_MD(ref Extrapolate_MD original)
		: this()
	{
		Copy(ref original);
	}

	public void Copy(ref Extrapolate_MD original)
	{
		MD = original.MD;
		DogLeg = original.DogLeg;
		ToolfaceSetting = original.ToolfaceSetting;
	}

	public override string ToString()
	{
		return "MD: " + MD.ToString() + ", Dog Leg: " + DogLeg.ToString() + ", ToolFaceSetting: " + ToolfaceSetting;
	}
}
