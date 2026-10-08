namespace EzyPlanData;

public class RelativePosition
{
	private decimal m_verticalDistance;

	private decimal m_eastingDistance;

	private decimal m_northingDistance;

	private decimal m_horizontalDeparture;

	private NorthingDirectionEnum m_northingDirection;

	private EastingDirectionEnum m_eastingDirection;

	private VerticalDirectionEnum m_verticalDirection;

	public decimal NorthingDistance
	{
		get
		{
			return m_northingDistance;
		}
		set
		{
			m_northingDistance = value;
		}
	}

	public decimal EastingDistance
	{
		get
		{
			return m_eastingDistance;
		}
		set
		{
			m_eastingDistance = value;
		}
	}

	public decimal VerticalDistance
	{
		get
		{
			return m_verticalDistance;
		}
		set
		{
			m_verticalDistance = value;
		}
	}

	public decimal HorizontalDeparture
	{
		get
		{
			return m_horizontalDeparture;
		}
		set
		{
			m_horizontalDeparture = value;
		}
	}

	public NorthingDirectionEnum NorthingDirection
	{
		get
		{
			return m_northingDirection;
		}
		set
		{
			m_northingDirection = value;
		}
	}

	public EastingDirectionEnum EastingDirection
	{
		get
		{
			return m_eastingDirection;
		}
		set
		{
			m_eastingDirection = value;
		}
	}

	public VerticalDirectionEnum VerticalDirection
	{
		get
		{
			return m_verticalDirection;
		}
		set
		{
			m_verticalDirection = value;
		}
	}

	public string VerticalString => VerticalDirection switch
	{
		VerticalDirectionEnum.Null => "Same TVD", 
		VerticalDirectionEnum.Above => VerticalDistance + " Above", 
		VerticalDirectionEnum.Below => VerticalDistance + " Below", 
		_ => "Error", 
	};

	public string HorizontalString => HorizontalDeparture + " HD";

	public string EastingString => EastingDirection switch
	{
		EastingDirectionEnum.Null => "Same Easting", 
		EastingDirectionEnum.East => EastingDistance + " East", 
		EastingDirectionEnum.West => EastingDistance + " West", 
		_ => "Error", 
	};

	public string NorthingString => NorthingDirection switch
	{
		NorthingDirectionEnum.Null => "Same Northing", 
		NorthingDirectionEnum.North => NorthingDistance + " North", 
		NorthingDirectionEnum.South => NorthingDistance + " South", 
		_ => "Error", 
	};

	public RelativePosition()
	{
	}

	public RelativePosition(decimal setNorthingDistance, NorthingDirectionEnum setNorthingDirection, decimal setEastingDistance, EastingDirectionEnum setEastingDirection, decimal setVerticalDistance, VerticalDirectionEnum setVerticalDirection, decimal setHorizontalDeparture)
		: this()
	{
		Set(setNorthingDistance, setNorthingDirection, setEastingDistance, setEastingDirection, setVerticalDistance, setVerticalDirection, setHorizontalDeparture);
	}

	public RelativePosition(RelativePosition original)
		: this()
	{
		Copy(original);
	}

	public void Set(decimal setNorthingDistance, NorthingDirectionEnum setNorthingDirection, decimal setEastingDistance, EastingDirectionEnum setEastingDirection, decimal setVerticalDistance, VerticalDirectionEnum setVerticalDirection, decimal setHorizontalDeparture)
	{
		NorthingDistance = setNorthingDistance;
		NorthingDirection = setNorthingDirection;
		EastingDistance = setEastingDistance;
		EastingDirection = setEastingDirection;
		VerticalDistance = setVerticalDistance;
		VerticalDirection = setVerticalDirection;
		HorizontalDeparture = setHorizontalDeparture;
	}

	public void Copy(RelativePosition original)
	{
		VerticalDistance = original.VerticalDistance;
		VerticalDirection = original.VerticalDirection;
		EastingDistance = original.EastingDistance;
		EastingDirection = original.EastingDirection;
		NorthingDistance = original.NorthingDistance;
		NorthingDirection = original.NorthingDirection;
		HorizontalDeparture = original.HorizontalDeparture;
	}

	public override string ToString()
	{
		return VerticalDistance + ", " + VerticalDirection.ToString() + ", " + NorthingDistance + ", " + NorthingDirection.ToString() + ", " + EastingDistance + ", " + EastingDirection.ToString() + ", " + HorizontalDeparture;
	}
}
