using System;
using EzyPlanData;
using Microsoft.VisualBasic.CompilerServices;

namespace EzyPlanMath;

[StandardModule]
internal sealed class mdFiveMeterBuildPlot
{
	public static int TargetQuadrant;

	private const double RadPerDeg = System.Math.PI / 180.0;

	private const double DegPerRad = 180.0 / System.Math.PI;

	public static AbsoluteSurvey Point9MinDL(double MD, double Radius, double N4, double E4, double V4, double Xr, double Br, double Yr, double Xj, double Bj, double Yj, double P1MD, int CurrentQuadrant)
	{
		double num = MD / Radius;
		double num2 = Radius * System.Math.Cos(num);
		double num3 = Radius * System.Math.Sin(num);
		double value = N4 + num2 * Xr + num3 * Xj;
		double value2 = E4 + num2 * Br + num3 * Bj;
		double value3 = V4 + num2 * Yr + num3 * Yj;
		double num4 = System.Math.Sin(num) * -1.0 * Xr + System.Math.Cos(num) * Xj;
		double num5 = System.Math.Sin(num) * -1.0 * Br + System.Math.Cos(num) * Bj;
		double d = System.Math.Sin(num) * -1.0 * Yr + System.Math.Cos(num) * Yj;
		double value4 = 180.0 / System.Math.PI * System.Math.Acos(d);
		double num6 = 180.0 / System.Math.PI * System.Math.Atan(num5 / num4);
		if ((TargetQuadrant == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 1))
		{
			num6 = num6;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 1))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 2))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 2))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 3))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 3))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 4))
		{
			num6 = num6;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 4))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrant == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 1))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 1))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 3))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 3))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrant == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 4))
		{
			num6 = num6;
		}
		else if ((TargetQuadrant == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 4))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 1))
		{
			num6 = num6;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 1))
		{
			num6 = 90.0 + num6;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 2))
		{
			num6 = 90.0 + num6;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 2))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 3))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 3))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 4))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 4))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrant == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 1))
		{
			num6 = num6;
		}
		else if ((TargetQuadrant == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 1))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 2))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 2))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 3))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrant == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 3))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrant == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrant == 4))
		{
			num6 = num6;
		}
		else if ((TargetQuadrant == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrant == 4))
		{
			num6 = 360.0 + num6;
		}
		AbsoluteSurvey absoluteSurvey = new AbsoluteSurvey();
		absoluteSurvey.MD = new decimal(System.Math.Round(P1MD + MD, 2));
		absoluteSurvey.Dip = new decimal(System.Math.Round(value4, 2));
		absoluteSurvey.Azimuth = new decimal(System.Math.Round(num6, 2));
		absoluteSurvey.Northing = new decimal(System.Math.Round(value, 2));
		absoluteSurvey.Easting = new decimal(System.Math.Round(value2, 2));
		absoluteSurvey.TVD = new decimal(System.Math.Round(value3, 2));
		return absoluteSurvey;
	}

	public static AbsoluteSurvey Point9(double MD, double Radius, double N4, double E4, double V4, double Xr, double Br, double Yr, double Xj, double Bj, double Yj, double P1MD, int CurrentQuadrant)
	{
		double num = MD / Radius;
		double num2 = Radius * System.Math.Cos(num);
		double num3 = Radius * System.Math.Sin(num);
		double value = N4 + num2 * Xr + num3 * Xj;
		double value2 = E4 + num2 * Br + num3 * Bj;
		double value3 = V4 + num2 * Yr + num3 * Yj;
		double num4 = System.Math.Sin(num) * -1.0 * Xr + System.Math.Cos(num) * Xj;
		double num5 = System.Math.Sin(num) * -1.0 * Br + System.Math.Cos(num) * Bj;
		double d = System.Math.Sin(num) * -1.0 * Yr + System.Math.Cos(num) * Yj;
		double num6 = 180.0 / System.Math.PI * System.Math.Acos(d);
		if (num6 > 90.0)
		{
			num6 -= 90.0;
		}
		double num7 = 180.0 / System.Math.PI * System.Math.Atan(num5 / num4);
		double value4 = default;
		if ((TargetQuadrant == 1) & (System.Math.Sign(num7) == 1) & (CurrentQuadrant == 1))
		{
			value4 = num7;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num7) == -1) & (CurrentQuadrant == 1))
		{
			value4 = 180.0 + num7;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num7) == 1) & (CurrentQuadrant == 2))
		{
			value4 = 90.0 + num7;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num7) == -1) & (CurrentQuadrant == 2))
		{
			value4 = 180.0 + num7;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num7) == 1) & (CurrentQuadrant == 3))
		{
			value4 = 180.0 + num7;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num7) == -1) & (CurrentQuadrant == 3))
		{
			value4 = 270.0 + num7;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num7) == 1) & (CurrentQuadrant == 4))
		{
			value4 = num7;
		}
		else if ((TargetQuadrant == 1) & (System.Math.Sign(num7) == -1) & (CurrentQuadrant == 4))
		{
			value4 = 360.0 + num7;
		}
		else if ((TargetQuadrant == 2) & (System.Math.Sign(num7) == 1) & (CurrentQuadrant == 1))
		{
			value4 = 90.0 + num7;
		}
		else if ((TargetQuadrant == 2) & (System.Math.Sign(num7) == -1) & (CurrentQuadrant == 1))
		{
			value4 = 90.0 + num7;
		}
		else if ((TargetQuadrant == 2) & (System.Math.Sign(num7) == 1) & (CurrentQuadrant == 2))
		{
			value4 = 90.0 + num7;
		}
		else if ((TargetQuadrant == 2) & (System.Math.Sign(num7) == -1) & (CurrentQuadrant == 2))
		{
			value4 = 180.0 + num7;
		}
		else if ((TargetQuadrant == 2) & (System.Math.Sign(num7) == 1) & (CurrentQuadrant == 3))
		{
			value4 = 180.0 + num7;
		}
		else if ((TargetQuadrant == 2) & (System.Math.Sign(num7) == -1) & (CurrentQuadrant == 3))
		{
			value4 = 270.0 + num7;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num7) == 1) & (CurrentQuadrant == 1))
		{
			value4 = num7;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num7) == -1) & (CurrentQuadrant == 1))
		{
			value4 = 90.0 + num7;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num7) == 1) & (CurrentQuadrant == 2))
		{
			value4 = 90.0 + num7;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num7) == -1) & (CurrentQuadrant == 2))
		{
			value4 = 180.0 + num7;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num7) == 1) & (CurrentQuadrant == 3))
		{
			value4 = 180.0 + num7;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num7) == -1) & (CurrentQuadrant == 3))
		{
			value4 = 180.0 + num7;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num7) == 1) & (CurrentQuadrant == 4))
		{
			value4 = 180.0 + num7;
		}
		else if ((TargetQuadrant == 3) & (System.Math.Sign(num7) == -1) & (CurrentQuadrant == 4))
		{
			value4 = 360.0 + num7;
		}
		AbsoluteSurvey absoluteSurvey = new AbsoluteSurvey();
		absoluteSurvey.MD = new decimal(System.Math.Round(P1MD + MD, 2));
		absoluteSurvey.Dip = new decimal(System.Math.Round(num6, 2));
		absoluteSurvey.Azimuth = new decimal(System.Math.Round(value4, 2));
		absoluteSurvey.Northing = new decimal(System.Math.Round(value, 2));
		absoluteSurvey.Easting = new decimal(System.Math.Round(value2, 2));
		absoluteSurvey.TVD = new decimal(System.Math.Round(value3, 2));
		return absoluteSurvey;
	}

	public static int CurrentQuadrant(double CurrentAzi)
	{
		if ((CurrentAzi >= 0.0) & (CurrentAzi <= 90.0))
		{
			return 1;
		}
		if ((CurrentAzi >= 90.0) & (CurrentAzi <= 180.0))
		{
			return 2;
		}
		if ((CurrentAzi >= 180.0) & (CurrentAzi <= 270.0))
		{
			return 3;
		}
		if ((CurrentAzi >= 270.0) & (CurrentAzi <= 360.0))
		{
			return 4;
		}
		int result = default;
		return result;
	}

	public static decimal TargetInc1(decimal N3, decimal N5, decimal E3, decimal E5, decimal V3, decimal V5)
	{
		decimal d = decimal.Subtract(V3, V5);
		decimal d2 = new decimal(System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(decimal.Subtract(N3, N5)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(E3, E5)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(V3, V5)), 2.0)));
		return new decimal(180.0 / System.Math.PI * System.Math.Acos(Convert.ToDouble(decimal.Divide(d, d2))));
	}

	public static decimal TFSetting3(decimal inc1, decimal Inc2, decimal B, decimal AziChange)
	{
		if ((decimal.Compare(AziChange, 0m) == 0) & (System.Math.Sign(decimal.Subtract(Inc2, inc1)) == 1))
		{
			return 0m;
		}
		if ((decimal.Compare(AziChange, 0m) == 0) & (System.Math.Sign(decimal.Subtract(Inc2, inc1)) == -1))
		{
			return 180m;
		}
		decimal value = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(Inc2)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(AziChange)) / System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(B)));
		decimal num = new decimal(System.Math.Asin(Convert.ToDouble(value)) * (180.0 / System.Math.PI));
		decimal num2 = new decimal(System.Math.Asin(Convert.ToDouble(value)) * (180.0 / System.Math.PI));
		if (System.Math.Sign(num2) == -1)
		{
			return decimal.Add(360m, num2);
		}
		return num2;
	}
}
