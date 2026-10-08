using System.Diagnostics;

namespace EzyPlanMath;

public class CalulatedSurveyList
{
	private decimal TVDValue;

	private decimal CourseNValue;

	private decimal CourseEValue;

	private decimal DogLegValue;

	public decimal TVD
	{
		get
		{
			return TVDValue;
		}
		set
		{
			TVDValue = value;
		}
	}

	public decimal CourseN
	{
		get
		{
			return CourseNValue;
		}
		set
		{
			CourseNValue = value;
		}
	}

	public decimal CourseE
	{
		get
		{
			return CourseEValue;
		}
		set
		{
			CourseEValue = value;
		}
	}

	public decimal DogLeg
	{
		get
		{
			return DogLegValue;
		}
		set
		{
			DogLegValue = value;
		}
	}

	[DebuggerNonUserCode]
	public CalulatedSurveyList()
	{
	}
}
