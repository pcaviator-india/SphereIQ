using System;
using EzyPlanData;
using Microsoft.VisualBasic.CompilerServices;

namespace EzyPlanMath;

[StandardModule]
internal sealed class mdMiscSurveyVariables
{
	private const double RadPerDeg = System.Math.PI / 180.0;

	private const double DegPerRad = 180.0 / System.Math.PI;

	public static double MiscCalcs(Target Target, AbsoluteSurvey LastSurvey, AbsoluteSurvey CollarSurvey)
	{
		double num = Convert.ToDouble(decimal.Subtract(Target.Northing, CollarSurvey.Northing));
		double num2 = Convert.ToDouble(decimal.Subtract(Target.Easting, CollarSurvey.Easting));
		double num3 = Convert.ToDouble(decimal.Subtract(LastSurvey.Northing, CollarSurvey.Northing));
		double num4 = Convert.ToDouble(decimal.Subtract(LastSurvey.Easting, CollarSurvey.Easting));
		double num5 = System.Math.Atan(num2 / num);
		double num6 = System.Math.Sqrt(System.Math.Pow(num2, 2.0) + System.Math.Pow(num, 2.0));
		double num7 = Convert.ToDouble(LastSurvey.MD);
		double num8 = Convert.ToDouble(LastSurvey.Dip);
		double num9 = Convert.ToDouble(LastSurvey.Azimuth);
		double num10 = Convert.ToDouble(decimal.Subtract(LastSurvey.Northing, CollarSurvey.Northing));
		double num11 = Convert.ToDouble(decimal.Subtract(LastSurvey.Easting, CollarSurvey.Easting));
		double num12 = Convert.ToDouble(LastSurvey.Northing);
		double num13 = Convert.ToDouble(LastSurvey.Easting);
		double num14 = Convert.ToDouble(LastSurvey.TVD);
		double num15 = System.Math.Sqrt(System.Math.Pow(num4, 2.0) + System.Math.Pow(num3, 2.0));
		double num16 = System.Math.Atan(num4 / num3);
		double num17 = num16 - num5;
		double num18 = num15 * System.Math.Cos(num17);
		double num19 = num18 * System.Math.Tan(num17);
		double num20 = num6 - num18;
		double a = System.Math.Atan(num19 / num20);
		double num21 = num19 / System.Math.Sin(a);
		double num22 = Convert.ToDouble(Target.Northing) - num12;
		double num23 = Convert.ToDouble(Target.Easting) - num13;
		double num24 = System.Math.Atan(num23 / num22);
		double result = ((System.Math.Sign(num24) != -1) ? (180.0 / System.Math.PI * num24) : (180.0 / System.Math.PI * num24 + 360.0));
		double num25 = System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Azimuth);
		double num26 = num25 + num24;
		return result;
	}

	public static double WedgeAzi(Target Target, AbsoluteSurvey LastSurvey)
	{
		double num = Convert.ToDouble(LastSurvey.Northing);
		double num2 = Convert.ToDouble(LastSurvey.Easting);
		double num3 = Convert.ToDouble(Target.Northing) - num;
		double num4 = Convert.ToDouble(Target.Easting) - num2;
		double num5 = System.Math.Atan(num4 / num3);
		double result = ((System.Math.Sign(num5) != -1) ? (180.0 / System.Math.PI * num5) : (180.0 / System.Math.PI * num5 + 360.0));
		double num6 = System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Azimuth);
		double num7 = num6 + num5;
		return result;
	}

	public static double MD(double Ediff, double NDiff, double TVDDiff)
	{
		double x = System.Math.Sqrt(System.Math.Pow(Ediff, 2.0) + System.Math.Pow(NDiff, 2.0) + System.Math.Pow(TVDDiff, 2.0));
		double num = System.Math.Pow(x, 2.0) / (2.0 * System.Math.Pow(System.Math.Pow(x, 2.0) - System.Math.Pow(TVDDiff, 2.0), 0.5));
		double num2 = 2.0 * System.Math.Atan(System.Math.Pow(System.Math.Pow(x, 2.0) - System.Math.Pow(TVDDiff, 2.0), 0.5) / TVDDiff);
		double num3 = num * num2;
		return num2;
	}
}
