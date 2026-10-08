using System;
using EzyPlanData;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace EzyPlanMath;

[StandardModule]
internal sealed class mdTrendPlot
{
	private const double RadPerDeg = System.Math.PI / 180.0;

	private const double DegPerRad = 180.0 / System.Math.PI;

	public static double T1N(double P1Inc, double P1Azi)
	{
		return System.Math.Sin(System.Math.PI / 180.0 * P1Inc) * System.Math.Cos(System.Math.PI / 180.0 * P1Azi);
	}

	public static double T1E(double P1Inc, double P1Azi)
	{
		return System.Math.Sin(System.Math.PI / 180.0 * P1Inc) * System.Math.Sin(System.Math.PI / 180.0 * P1Azi);
	}

	public static double T1V(double P1Inc)
	{
		return System.Math.Cos(System.Math.PI / 180.0 * P1Inc);
	}

	public static double T2N(double P2Inc, double P2Azi)
	{
		return System.Math.Sin(System.Math.PI / 180.0 * P2Inc) * System.Math.Cos(System.Math.PI / 180.0 * P2Azi);
	}

	public static double T2E(double P2Inc, double P2Azi)
	{
		return System.Math.Sin(System.Math.PI / 180.0 * P2Inc) * System.Math.Sin(System.Math.PI / 180.0 * P2Azi);
	}

	public static double T2V(double P2Inc)
	{
		return System.Math.Cos(System.Math.PI / 180.0 * P2Inc);
	}

	public static double TN(double CD, double CMD, double T1N, double T2N)
	{
		return (1.0 - CD / CMD) * T1N + CD / CMD * T2N;
	}

	public static double TE(double CD, double CMD, double T1E, double T2E)
	{
		return (1.0 - CD / CMD) * T1E + CD / CMD * T2E;
	}

	public static double TV(double CD, double CMD, double T1V, double T2V)
	{
		return (1.0 - CD / CMD) * T1V + CD / CMD * T2V;
	}

	public static double PN(double P1N, double CD, double T1N, double TN)
	{
		return P1N + CD / 2.0 * (T1N + TN);
	}

	public static double PE(double P1E, double CD, double T1E, double TE)
	{
		return P1E + CD / 2.0 * (T1E + TE);
	}

	public static double PV(double P1V, double CD, double T1V, double TV)
	{
		return P1V + CD / 2.0 * (T1V + TV);
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

	public static int CurrentQuadrantTnTe(double TN, double TE)
	{
		if ((System.Math.Sign(TN) == 1) & (System.Math.Sign(TE) == 1))
		{
			return 1;
		}
		if ((System.Math.Sign(TN) == -1) & (System.Math.Sign(TE) == 1))
		{
			return 2;
		}
		if ((System.Math.Sign(TN) == -1) & (System.Math.Sign(TE) == -1))
		{
			return 3;
		}
		if ((System.Math.Sign(TN) == 1) & (System.Math.Sign(TE) == -1))
		{
			return 4;
		}
		int result = default;
		return result;
	}

	public static int CurrentQuadrantAzi(double Azi)
	{
		if ((Azi > 0.0) & (Azi < 90.0))
		{
			return 1;
		}
		if ((Azi > 90.0) & (Azi < 180.0))
		{
			return 2;
		}
		if ((Azi > 180.0) & (Azi < 270.0))
		{
			return 3;
		}
		if ((Azi > 270.0) & (Azi < 360.0))
		{
			return 4;
		}
		int result = default;
		return result;
	}

	public static AbsoluteSurvey MitchellTrendPlot(double MD, double Radius, double N4, double E4, double V4, double CosXj, double CosBj, double CosYj, double CosXr, double CosBr, double CosYr, double MinDl, double LastsurveyMD, int TargetQuadrantQ, double CurrentQuadrantQ, double CurrentAzi)
	{
		double num = 180.0 / System.Math.PI * MD / Radius;
		double num2 = Radius * System.Math.Cos(System.Math.PI / 180.0 * num);
		double num3 = Radius * System.Math.Sin(System.Math.PI / 180.0 * num);
		double value = N4 + num2 * CosXr + num3 * CosXj;
		double value2 = E4 + num2 * CosBr + num3 * CosBj;
		double value3 = V4 + num2 * CosYr + num3 * CosYj;
		double num4 = (0.0 - System.Math.Sin(System.Math.PI / 180.0 * num)) * CosXr + System.Math.Cos(System.Math.PI / 180.0 * num) * CosXj;
		double num5 = (0.0 - System.Math.Sin(System.Math.PI / 180.0 * num)) * CosBr + System.Math.Cos(System.Math.PI / 180.0 * num) * CosBj;
		double d = (0.0 - System.Math.Sin(System.Math.PI / 180.0 * num)) * CosYr + System.Math.Cos(System.Math.PI / 180.0 * num) * CosYj;
		double value4 = 180.0 / System.Math.PI * System.Math.Acos(d);
		double num6 = 180.0 / System.Math.PI * System.Math.Atan(num5 / num4);
		if ((mdFiveMeterBuildPlot.TargetQuadrant == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 2.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 270.0 + num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = 90.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 2.0))
		{
			num6 = 90.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 360.0 + num6;
		}
		return new AbsoluteSurvey(new decimal(System.Math.Round(LastsurveyMD + MD, 2)), new decimal(System.Math.Round(value4, 2)), new decimal(System.Math.Round(num6, 2)), new decimal(System.Math.Round(value3, 2)), new decimal(System.Math.Round(value, 2)), new decimal(System.Math.Round(value2, 2)), new decimal(System.Math.Round(MinDl, 2)));
	}

	public static object TrendPlotTest(double MD, double Radius, double N4, double E4, double V4, double CosXj, double CosBj, double CosYj, double CosXr, double CosBr, double CosYr, double MinDl, double LastsurveyMD, int TargetQuadrantQ, double CurrentQuadrantQ, double CurrentAzi, bool HighSide, int ArcLength, double CurrentInc, ref AbsoluteSurveyList Result3, AbsoluteSurvey LastSurvey, double ExtrapCD)
	{
		checked
		{
			double[,] array = new double[ArcLength + 1, 2];
			array[0, 0] = Convert.ToDouble(LastSurvey.Azimuth);
			array[0, 1] = Convert.ToDouble(LastSurvey.Dip);
			double num = MD / Radius;
			for (int i = 0; i <= ArcLength; i++)
			{
				num = 180.0 / System.Math.PI * MD / Radius;
				double num2 = Radius * System.Math.Cos(System.Math.PI / 180.0 * num);
				double num3 = Radius * System.Math.Sin(System.Math.PI / 180.0 * num);
				double value = N4 + num2 * CosXr + num3 * CosXj;
				double value2 = E4 + num2 * CosBr + num3 * CosBj;
				double value3 = V4 + num2 * CosYr + num3 * CosYj;
				double num4 = (0.0 - System.Math.Sin(System.Math.PI / 180.0 * num)) * CosXr + System.Math.Cos(System.Math.PI / 180.0 * num) * CosXj;
				double num5 = (0.0 - System.Math.Sin(System.Math.PI / 180.0 * num)) * CosBr + System.Math.Cos(System.Math.PI / 180.0 * num) * CosBj;
				double d = (0.0 - System.Math.Sin(System.Math.PI / 180.0 * num)) * CosYr + System.Math.Cos(System.Math.PI / 180.0 * num) * CosYj;
				if ((System.Math.Sign(num4) == 1) & (System.Math.Sign(num5) == 1))
				{
					CurrentQuadrantQ = 1.0;
				}
				else if ((System.Math.Sign(num4) == -1) & (System.Math.Sign(num5) == 1))
				{
					CurrentQuadrantQ = 2.0;
				}
				else if ((System.Math.Sign(num4) == -1) & (System.Math.Sign(num5) == -1))
				{
					CurrentQuadrantQ = 3.0;
				}
				else if ((System.Math.Sign(num4) == 1) & (System.Math.Sign(num5) == -1))
				{
					CurrentQuadrantQ = 4.0;
				}
				double value4 = 180.0 / System.Math.PI * System.Math.Acos(d);
				double num6 = 180.0 / System.Math.PI * System.Math.Atan(num5 / num4);
				if (HighSide)
				{
					if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 270.0 + num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 360.0 + num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 270.0 + num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 360.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 360.0 + num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 360.0 + num6;
					}
				}
				if (!HighSide)
				{
					if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = 360.0 + num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 270.0 + num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 360.0 + num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 360.0 - num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 360.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = 360.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 90.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 360.0 + num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
					{
						num6 = 360.0 + num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 180.0 + num6;
					}
					else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
					{
						num6 = 360.0 + num6;
					}
				}
				AbsoluteSurvey element = new AbsoluteSurvey(new decimal(LastsurveyMD + MD), new decimal(value4), new decimal(num6), new decimal(value3), new decimal(value), new decimal(value2), 0m);
				Result3.Add(element);
				MD += ExtrapCD;
			}
			object result = default;
			return result;
		}
	}

	public static AbsoluteSurvey TrendPlotLastSurvey(double MD, double Radius, double N4, double E4, double V4, double CosXj, double CosBj, double CosYj, double CosXr, double CosBr, double CosYr, double Dl, double LastsurveyMD, int TargetQuadrantQ, double CurrentQuadrantQ, double CurrentAzi)
	{
		double num = 180.0 / System.Math.PI * MD / Radius;
		double num2 = Radius * System.Math.Cos(System.Math.PI / 180.0 * num);
		double num3 = Radius * System.Math.Sin(System.Math.PI / 180.0 * num);
		double value = N4 + num2 * CosXr + num3 * CosXj;
		double value2 = E4 + num2 * CosBr + num3 * CosBj;
		double value3 = V4 + num2 * CosYr + num3 * CosYj;
		double num4 = (0.0 - System.Math.Sin(System.Math.PI / 180.0 * num)) * CosXr + System.Math.Cos(System.Math.PI / 180.0 * num) * CosXj;
		double num5 = (0.0 - System.Math.Sin(System.Math.PI / 180.0 * num)) * CosBr + System.Math.Cos(System.Math.PI / 180.0 * num) * CosBj;
		double d = (0.0 - System.Math.Sin(System.Math.PI / 180.0 * num)) * CosYr + System.Math.Cos(System.Math.PI / 180.0 * num) * CosYj;
		double value4 = 180.0 / System.Math.PI * System.Math.Acos(d);
		double num6 = 180.0 / System.Math.PI * System.Math.Atan(num5 / num4);
		if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 2.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 270.0 + num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = num6;
		}
		else if ((TargetQuadrantQ == 1) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 2) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = 90.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 2.0))
		{
			num6 = 90.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 3) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 1.0))
		{
			num6 = 360.0 + num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 2.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 3.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == 1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 180.0 + num6;
		}
		else if ((TargetQuadrantQ == 4) & (System.Math.Sign(num6) == -1) & (CurrentQuadrantQ == 4.0))
		{
			num6 = 360.0 + num6;
		}
		return new AbsoluteSurvey(new decimal(System.Math.Round(LastsurveyMD + MD, 2)), new decimal(System.Math.Round(value4, 2)), new decimal(System.Math.Round(num6, 2)), new decimal(System.Math.Round(value3, 2)), new decimal(System.Math.Round(value, 2)), new decimal(System.Math.Round(value2, 2)), new decimal(Dl));
	}

	public static double FinalToolFaceSetting(AbsoluteSurvey LastSurvey, double TargetInc, double TargetAzimuth, double DogLeg)
	{
		if (decimal.Compare(LastSurvey.Dip, 0m) == 0)
		{
			return TargetAzimuth;
		}
		double d = (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * DogLeg) - System.Math.Cos(System.Math.PI / 180.0 * TargetInc)) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * DogLeg));
		double num = System.Math.Acos(d) * (180.0 / System.Math.PI);
		if ((System.Math.Sign(TargetAzimuth - Convert.ToDouble(LastSurvey.Azimuth)) == 1) & (TargetAzimuth - Convert.ToDouble(LastSurvey.Azimuth) < 180.0))
		{
			return num;
		}
		if ((System.Math.Sign(TargetAzimuth - Convert.ToDouble(LastSurvey.Azimuth)) == 1) & (TargetAzimuth - Convert.ToDouble(LastSurvey.Azimuth) > 180.0))
		{
			return 360.0 - num;
		}
		if ((System.Math.Sign(TargetAzimuth - Convert.ToDouble(LastSurvey.Azimuth)) == -1) & (TargetAzimuth - Convert.ToDouble(LastSurvey.Azimuth) > -180.0))
		{
			return 360.0 - num;
		}
		if ((System.Math.Sign(TargetAzimuth - Convert.ToDouble(LastSurvey.Azimuth)) == -1) & (TargetAzimuth - Convert.ToDouble(LastSurvey.Azimuth) < -180.0))
		{
			return num;
		}
		double result = default;
		return result;
	}

	public static double FinalTargetAzimuth(double CosXk, double CosBk, ref int TargetQuadrant)
	{
		double num = 180.0 / System.Math.PI * System.Math.Atan(CosBk / CosXk);
		if ((System.Math.Sign(CosXk) == 1) & (System.Math.Sign(CosBk) == 1))
		{
			TargetQuadrant = 1;
		}
		else if ((System.Math.Sign(CosXk) == -1) & (System.Math.Sign(CosBk) == 1))
		{
			TargetQuadrant = 2;
		}
		else if ((System.Math.Sign(CosXk) == -1) & (System.Math.Sign(CosBk) == -1))
		{
			TargetQuadrant = 3;
		}
		else if ((System.Math.Sign(CosXk) == 1) & (System.Math.Sign(CosBk) == -1))
		{
			TargetQuadrant = 4;
		}
		if (num == 0.0)
		{
			Interaction.MsgBox("The target can no longer be achieved.", MsgBoxStyle.OkOnly, "Too far gone");
		}
		else
		{
			if ((TargetQuadrant == 1) & (System.Math.Sign(num) == 1))
			{
				return num;
			}
			if ((TargetQuadrant == 1) & (System.Math.Sign(num) == -1))
			{
				return 180.0 + num;
			}
			if ((TargetQuadrant == 2) & (System.Math.Sign(num) == 1))
			{
				return 180.0 + num;
			}
			if ((TargetQuadrant == 2) & (System.Math.Sign(num) == -1))
			{
				return 180.0 + num;
			}
			if ((TargetQuadrant == 3) & (System.Math.Sign(num) == 1))
			{
				return 180.0 + num;
			}
			if ((TargetQuadrant == 3) & (System.Math.Sign(num) == -1))
			{
				return 180.0 + num;
			}
			if ((TargetQuadrant == 4) & (System.Math.Sign(num) == 1))
			{
				return 180.0 + num;
			}
			if ((TargetQuadrant == 4) & (System.Math.Sign(num) == -1))
			{
				return 360.0 + num;
			}
		}
		double result = default;
		return result;
	}

	public static bool HighSide(double ToolfaceSetting)
	{
		if (ToolfaceSetting < 180.0)
		{
			return true;
		}
		if (ToolfaceSetting > 180.0)
		{
			return false;
		}
		bool result = default;
		return result;
	}

	public static double FixRange(double origValue, double rangeMin, double rangeMax)
	{
		double num = origValue - rangeMin;
		double num2 = rangeMax - rangeMin;
		return (num % num2 + num2) % num2 + rangeMin;
	}

	public static AbsoluteSurveyList ExtrapolateIntervalSurveys(AbsoluteSurvey LastSurvey, AbsoluteSurveyList results, double interval)
	{
		AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
		absoluteSurveyList.Add(results.RecordAt(0));
		double num = 1.0;
		checked
		{
			double limit = results.Count() - 1;
			int num12 = default;
			int num13 = default;
			decimal num15 = default;
			for (double num2 = 0.0; ObjectFlowControl.ForLoopControl.ForNextCheckR8(num2, limit, interval); num2 += interval)
			{
				decimal value = decimal.Subtract(results.LastRecord().MD, LastSurvey.MD);
				decimal num3 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Azimuth)));
				decimal num4 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Azimuth)));
				decimal num5 = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)));
				decimal value2 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(results.LastRecord().Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(results.LastRecord().Azimuth)));
				decimal value3 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(results.LastRecord().Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(results.LastRecord().Azimuth)));
				decimal value4 = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(results.LastRecord().Dip)));
				decimal num6 = new decimal((1.0 - num / Convert.ToDouble(value)) * Convert.ToDouble(num3) + num / Convert.ToDouble(value) * Convert.ToDouble(value2));
				decimal num7 = new decimal((1.0 - num / Convert.ToDouble(value)) * Convert.ToDouble(num4) + num / Convert.ToDouble(value) * Convert.ToDouble(value3));
				decimal num8 = new decimal((1.0 - num / Convert.ToDouble(value)) * Convert.ToDouble(num5) + num / Convert.ToDouble(value) * Convert.ToDouble(value4));
				decimal num9 = new decimal(num / 2.0 * Convert.ToDouble(decimal.Add(num3, num6)));
				decimal num10 = new decimal(num / 2.0 * Convert.ToDouble(decimal.Add(num4, num7)));
				decimal num11 = new decimal(num / 2.0 * Convert.ToDouble(decimal.Add(num5, num8)));
				double value5 = Convert.ToDouble(decimal.Subtract(results.LastRecord().Northing, LastSurvey.Northing));
				double value6 = Convert.ToDouble(decimal.Subtract(results.LastRecord().Easting, LastSurvey.Easting));
				if ((System.Math.Sign(value5) == 1) & (System.Math.Sign(value6) == 1))
				{
					num12 = 1;
				}
				else if ((System.Math.Sign(value5) == 1) & (System.Math.Sign(value6) == -1))
				{
					num12 = 2;
				}
				else if ((System.Math.Sign(value5) == -1) & (System.Math.Sign(value6) == -1))
				{
					num12 = 3;
				}
				else if ((System.Math.Sign(value5) == -1) & (System.Math.Sign(value6) == 1))
				{
					num12 = 4;
				}
				if ((System.Math.Sign(num6) == 1) & (System.Math.Sign(num7) == 1))
				{
					num13 = 1;
				}
				else if ((System.Math.Sign(num6) == -1) & (System.Math.Sign(num7) == 1))
				{
					num13 = 2;
				}
				else if ((System.Math.Sign(num6) == -1) & (System.Math.Sign(num7) == -1))
				{
					num13 = 3;
				}
				else if ((System.Math.Sign(num6) == 1) & (System.Math.Sign(num7) == -1))
				{
					num13 = 4;
				}
				double d = Convert.ToDouble(decimal.Divide(num7, num6));
				double num14 = 180.0 / System.Math.PI * System.Math.Atan(d);
				if ((num12 == 1) & (System.Math.Sign(num14) == 1) & (num13 == 1))
				{
					num15 = new decimal(num14);
				}
				else if ((num12 == 1) & (System.Math.Sign(num14) == -1) & (num13 == 1))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 1) & (System.Math.Sign(num14) == 1) & (num13 == 2))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 1) & (System.Math.Sign(num14) == -1) & (num13 == 2))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 1) & (System.Math.Sign(num14) == 1) & (num13 == 3))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 1) & (System.Math.Sign(num14) == -1) & (num13 == 3))
				{
					num15 = new decimal(270.0 + num14);
				}
				else if ((num12 == 1) & (System.Math.Sign(num14) == 1) & (num13 == 4))
				{
					num15 = new decimal(num14);
				}
				else if ((num12 == 1) & (System.Math.Sign(num14) == -1) & (num13 == 4))
				{
					num15 = new decimal(360.0 + num14);
				}
				else if ((num12 == 2) & (System.Math.Sign(num14) == 1) & (num13 == 1))
				{
					num15 = new decimal(num14);
				}
				else if ((num12 == 2) & (System.Math.Sign(num14) == -1) & (num13 == 1))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 2) & (System.Math.Sign(num14) == -1) & (num13 == 2))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 2) & (System.Math.Sign(num14) == -1) & (num13 == 3))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 2) & (System.Math.Sign(num14) == 1) & (num13 == 3))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 2) & (System.Math.Sign(num14) == 1) & (num13 == 4))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 2) & (System.Math.Sign(num14) == -1) & (num13 == 4))
				{
					num15 = new decimal(360.0 + num14);
				}
				else if ((num12 == 3) & (System.Math.Sign(num14) == 1) & (num13 == 1))
				{
					num15 = new decimal(num14);
				}
				else if ((num12 == 3) & (System.Math.Sign(num14) == -1) & (num13 == 1))
				{
					num15 = new decimal(90.0 + num14);
				}
				else if ((num12 == 3) & (System.Math.Sign(num14) == 1) & (num13 == 2))
				{
					num15 = new decimal(90.0 + num14);
				}
				else if ((num12 == 3) & (System.Math.Sign(num14) == -1) & (num13 == 2))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 3) & (System.Math.Sign(num14) == 1) & (num13 == 3))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 3) & (System.Math.Sign(num14) == -1) & (num13 == 3))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 3) & (System.Math.Sign(num14) == 1) & (num13 == 4))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 3) & (System.Math.Sign(num14) == -1) & (num13 == 4))
				{
					num15 = new decimal(360.0 + num14);
				}
				else if ((num12 == 4) & (System.Math.Sign(num14) == 1) & (num13 == 1))
				{
					num15 = new decimal(num14);
				}
				else if ((num12 == 4) & (System.Math.Sign(num14) == -1) & (num13 == 1))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 4) & (System.Math.Sign(num14) == -1) & (num13 == 2))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 4) & (System.Math.Sign(num14) == 1) & (num13 == 3))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 4) & (System.Math.Sign(num14) == -1) & (num13 == 3))
				{
					num15 = new decimal(180.0 + num14);
				}
				else if ((num12 == 4) & (System.Math.Sign(num14) == 1) & (num13 == 4))
				{
					num15 = new decimal(num14);
				}
				else if ((num12 == 4) & (System.Math.Sign(num14) == -1) & (num13 == 4))
				{
					num15 = new decimal(360.0 + num14);
				}
				double d2 = System.Math.Pow(System.Math.Pow(Convert.ToDouble(num6), 2.0) + System.Math.Pow(Convert.ToDouble(num7), 2.0), 0.5) / Convert.ToDouble(num8);
				decimal num16 = new decimal(180.0 / System.Math.PI * System.Math.Atan(d2));
				if (System.Math.Sign(num8) == -1)
				{
					num16 = decimal.Add(180m, num16);
				}
				double num17 = System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(num16)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(num16)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(num15, LastSurvey.Azimuth))));
				double num18 = 180.0 / System.Math.PI * num17;
				double num19 = 2.0 / (System.Math.PI / 180.0 * num18) * System.Math.Tan(System.Math.PI / 180.0 * num18 / 2.0);
				num9 = new decimal(Convert.ToDouble(LastSurvey.Northing) + num / 2.0 * Convert.ToDouble(decimal.Add(num3, num6)) * num19);
				num10 = new decimal(Convert.ToDouble(LastSurvey.Easting) + num / 2.0 * Convert.ToDouble(decimal.Add(num4, num7)) * num19);
				num11 = new decimal(Convert.ToDouble(LastSurvey.TVD) + num / 2.0 * Convert.ToDouble(decimal.Add(num5, num8)) * num19);
				AbsoluteSurvey element = new AbsoluteSurvey(new decimal(Convert.ToDouble(LastSurvey.MD) + num), num16, num15, num11, num9, num10, 0m);
				absoluteSurveyList.Add(element);
				num += interval;
			}
			if (decimal.Compare(absoluteSurveyList.LastRecord().MD, results.LastRecord().MD) > 0)
			{
				absoluteSurveyList.LastRecord().MD = results.LastRecord().MD;
				absoluteSurveyList.LastRecord().Dip = results.LastRecord().Dip;
				absoluteSurveyList.LastRecord().Azimuth = results.LastRecord().Azimuth;
				absoluteSurveyList.LastRecord().TVD = results.LastRecord().TVD;
				absoluteSurveyList.LastRecord().Northing = results.LastRecord().Northing;
				absoluteSurveyList.LastRecord().Easting = results.LastRecord().Easting;
			}
			if (decimal.Compare(results.LastRecord().MD, absoluteSurveyList.LastRecord().MD) > 0)
			{
				absoluteSurveyList.Add(results.RecordAt(results.Count() - 1));
			}
			absoluteSurveyList.RecordAt(0).DL = new decimal(180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(0).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(0).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(0).Azimuth, LastSurvey.Azimuth)))));
			decimal limit2 = new decimal(absoluteSurveyList.Count() - 1);
			decimal num20 = 1m;
			while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num20, limit2, 1m))
			{
				absoluteSurveyList.RecordAt(Convert.ToInt32(num20)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num20)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num20, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num20)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num20, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num20)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num20, 1m))).Azimuth)))));
				absoluteSurveyList.RecordAt(Convert.ToInt32(num20)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num20)).DL));
				num20 = decimal.Add(num20, 1m);
			}
			int num21 = absoluteSurveyList.Count() - 1;
			for (int i = 1; i <= num21; i++)
			{
				absoluteSurveyList.RecordAt(i).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(i).Azimuth, 2);
				absoluteSurveyList.RecordAt(i).Dip = System.Math.Round(absoluteSurveyList.RecordAt(i).Dip, 2);
				absoluteSurveyList.RecordAt(i).DL = System.Math.Round(absoluteSurveyList.RecordAt(i).DL, 2);
				absoluteSurveyList.RecordAt(i).Easting = System.Math.Round(absoluteSurveyList.RecordAt(i).Easting, 2);
				absoluteSurveyList.RecordAt(i).MD = System.Math.Round(absoluteSurveyList.RecordAt(i).MD, 2);
				absoluteSurveyList.RecordAt(i).Northing = System.Math.Round(absoluteSurveyList.RecordAt(i).Northing, 2);
				absoluteSurveyList.RecordAt(i).TVD = System.Math.Round(absoluteSurveyList.RecordAt(i).TVD, 2);
			}
			return absoluteSurveyList;
		}
	}

	public static AbsoluteSurveyList ExtrapolateIntervalSurveysTest(bool TVDCannotBeReached, Target Target, double TestIncDiffByMD, double TestAziDiffByMD, double interval, AbsoluteSurvey LastSurvey, AbsoluteSurveyList Results)
	{
		double num = Convert.ToDouble(LastSurvey.TVD);
		checked
		{
			int num2 = (int)System.Math.Round(interval);
			int num3 = 1;
			AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
			absoluteSurveyList.Add(new AbsoluteSurvey(LastSurvey.MD, LastSurvey.Dip, LastSurvey.Azimuth, LastSurvey.TVD, LastSurvey.Northing, LastSurvey.Easting, 0m));
			int num4 = Results.Count() - 1;
			int num5 = (int)System.Math.Round(interval);
			int num6 = num4;
			double num11 = default;
			for (int i = 1; ((num5 >> 31) ^ i) <= ((num5 >> 31) ^ num6); i += num5)
			{
				double num7 = TestIncDiffByMD * (double)num2;
				double num8 = TestAziDiffByMD * (double)num2;
				double num9 = Convert.ToDouble(absoluteSurveyList.RecordAt(0).Dip) + num7;
				double num10 = Convert.ToDouble(absoluteSurveyList.RecordAt(0).Azimuth) + num8;
				if (num10 > 360.0)
				{
					num10 = FixRange(num10, 0.0, 360.0);
				}
				else if (System.Math.Sign(num10) == -1)
				{
					num10 = 360.0 + num10;
				}
				num += ((double)num2 - num11) / 2.0 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num3 - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num9));
				double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num3 - 1).Northing) + Convert.ToDouble(decimal.Divide(decimal.Subtract(decimal.Add(new decimal(num2), LastSurvey.MD), absoluteSurveyList.RecordAt(num3 - 1).MD), 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num3 - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num3 - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num9) * System.Math.Cos(System.Math.PI / 180.0 * num10));
				double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num3 - 1).Easting) + Convert.ToDouble(decimal.Divide(decimal.Subtract(decimal.Add(new decimal(num2), LastSurvey.MD), absoluteSurveyList.RecordAt(num3 - 1).MD), 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num3 - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num3 - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num9) * System.Math.Sin(System.Math.PI / 180.0 * num10));
				absoluteSurveyList.Add(new AbsoluteSurvey(decimal.Add(new decimal(num2), LastSurvey.MD), new decimal(num9), new decimal(num10), new decimal(num), new decimal(value), new decimal(value2), 0m));
				num3++;
				num11 += interval;
				num2 = (int)System.Math.Round((double)num2 + interval);
			}
			int num12 = absoluteSurveyList.Count() - 1;
			for (int j = 0; j <= num12; j++)
			{
				if (System.Math.Sign(absoluteSurveyList.RecordAt(j).Dip) == -1)
				{
					absoluteSurveyList.RecordAt(j).Dip = decimal.Multiply(absoluteSurveyList.RecordAt(j).Dip, -1m);
				}
			}
			int num13 = absoluteSurveyList.Count();
			double value3 = default;
			double value4 = default;
			double value5 = default;
			double value6 = default;
			double value7 = default;
			double value8 = default;
			if (TVDCannotBeReached)
			{
				double num14 = 89.99 - Convert.ToDouble(absoluteSurveyList.RecordAt(num13 - 2).Dip);
				double num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num13 - 1).MD, absoluteSurveyList.RecordAt(num13 - 2).MD));
				double num16 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num13 - 1).Dip, absoluteSurveyList.RecordAt(num13 - 2).Dip));
				double num17 = num14 / num16;
				double num18 = num15 * num17;
				value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num13 - 2).MD) + num17;
				value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(num13 - 2).Dip) + num16 * num17;
				double num19 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num13 - 1).Azimuth, absoluteSurveyList.RecordAt(num13 - 2).Azimuth));
				value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(num13 - 2).Azimuth) + num19 * num17;
				double num20 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num13 - 1).Northing, absoluteSurveyList.RecordAt(num13 - 2).Northing));
				if (System.Math.Sign(num20) == -1)
				{
					num20 *= -1.0;
				}
				value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(num13 - 2).Northing) + num20 * num17;
				double num21 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num13 - 1).Easting, absoluteSurveyList.RecordAt(num13 - 2).Easting));
				if (System.Math.Sign(num21) == -1)
				{
					num21 *= -1.0;
				}
				value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(num13 - 2).Easting) + num21 * num17;
				double num22 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num13 - 1).TVD, absoluteSurveyList.RecordAt(num13 - 2).TVD));
				if (System.Math.Sign(num22) == -1)
				{
					num22 *= -1.0;
				}
				value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(num13 - 2).TVD) + num22 * num17;
			}
			else if (!TVDCannotBeReached)
			{
				double num22 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num13 - 1).TVD, absoluteSurveyList.RecordAt(num13 - 2).TVD));
				double num23 = Convert.ToDouble(decimal.Subtract(Target.TVD, absoluteSurveyList.RecordAt(num13 - 2).TVD));
				double num24 = num23 / num22;
				double num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num13 - 1).MD, absoluteSurveyList.RecordAt(num13 - 2).MD));
				double num18 = num15 * num24;
				value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num13 - 2).MD) + num24;
				double num16 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num13 - 1).Dip, absoluteSurveyList.RecordAt(num13 - 2).Dip));
				value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(num13 - 2).Dip) + num16 * num24;
				double num19 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num13 - 1).Azimuth, absoluteSurveyList.RecordAt(num13 - 2).Azimuth));
				value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(num13 - 2).Azimuth) + num19 * num24;
				double num20 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num13 - 1).Northing, absoluteSurveyList.RecordAt(num13 - 2).Northing));
				value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(num13 - 2).Northing) + num20 * num24;
				double num21 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num13 - 1).Easting, absoluteSurveyList.RecordAt(num13 - 2).Easting));
				value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(num13 - 2).Easting) + num21 * num24;
				value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(num13 - 2).TVD) + num22 * num24;
			}
			absoluteSurveyList.LastRecord().MD = new decimal(System.Math.Round(value3, 2));
			absoluteSurveyList.LastRecord().Azimuth = new decimal(System.Math.Round(value5, 2));
			absoluteSurveyList.LastRecord().Dip = new decimal(System.Math.Round(value4, 2));
			absoluteSurveyList.LastRecord().TVD = new decimal(System.Math.Round(value8, 2));
			absoluteSurveyList.LastRecord().Northing = new decimal(System.Math.Round(value6, 2));
			absoluteSurveyList.LastRecord().Easting = new decimal(System.Math.Round(value7, 2));
			decimal limit = new decimal(absoluteSurveyList.Count() - 1);
			decimal num25 = 1m;
			while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num25, limit, 1m))
			{
				absoluteSurveyList.RecordAt(Convert.ToInt32(num25)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num25)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num25, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num25)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num25, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num25)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num25, 1m))).Azimuth)))));
				absoluteSurveyList.RecordAt(Convert.ToInt32(num25)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num25)).DL));
				num25 = decimal.Add(num25, 1m);
			}
			int num26 = absoluteSurveyList.Count() - 1;
			for (int k = 1; k <= num26; k++)
			{
				absoluteSurveyList.RecordAt(k).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(k).Azimuth, 2);
				absoluteSurveyList.RecordAt(k).Dip = System.Math.Round(absoluteSurveyList.RecordAt(k).Dip, 2);
				absoluteSurveyList.RecordAt(k).DL = System.Math.Round(absoluteSurveyList.RecordAt(k).DL, 2);
				absoluteSurveyList.RecordAt(k).Easting = System.Math.Round(absoluteSurveyList.RecordAt(k).Easting, 2);
				absoluteSurveyList.RecordAt(k).MD = System.Math.Round(absoluteSurveyList.RecordAt(k).MD, 2);
				absoluteSurveyList.RecordAt(k).Northing = System.Math.Round(absoluteSurveyList.RecordAt(k).Northing, 2);
				absoluteSurveyList.RecordAt(k).TVD = System.Math.Round(absoluteSurveyList.RecordAt(k).TVD, 2);
			}
			return absoluteSurveyList;
		}
	}

	public static AbsoluteSurveyList ExtrapolateIntervalSurveysDir(double EndInc, double EndDir, double DogLeg, double interval, AbsoluteSurvey FirstSurvey, ref AbsoluteSurveyList ResultList, double Curve, ref bool MaxDip)
	{
		AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
		absoluteSurveyList.Add(FirstSurvey);
		double d = System.Math.Cos(System.Math.PI / 180.0 * (EndInc - Convert.ToDouble(FirstSurvey.Dip))) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * EndInc) * (1.0 - System.Math.Cos(System.Math.PI / 180.0 * (EndDir - Convert.ToDouble(FirstSurvey.Azimuth))));
		d = 180.0 / System.Math.PI * System.Math.Acos(d);
		int num = 1;
		object left = DogLeg / 30.0;
		double num2 = 1.0;
		double num3 = Convert.ToDouble(FirstSurvey.MD);
		double num4 = Convert.ToDouble(FirstSurvey.Azimuth);
		double d2 = (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * d) - System.Math.Cos(System.Math.PI / 180.0 * EndInc)) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * d));
		d2 = 180.0 / System.Math.PI * System.Math.Acos(d2);
		d2 = FinalToolFaceSetting(FirstSurvey, EndInc, EndDir, d);
		checked
		{
			double num8 = default;
			if ((d2 == 0.0) | (d2 == 180.0))
			{
				if (EndInc < Convert.ToDouble(FirstSurvey.Dip))
				{
					d2 = 180.0;
				}
				else if ((EndInc > Convert.ToDouble(FirstSurvey.Dip)) & (EndDir == Convert.ToDouble(FirstSurvey.Azimuth)))
				{
					d2 = 0.0;
				}
				else if ((EndInc > Convert.ToDouble(FirstSurvey.Dip)) & (EndDir == Convert.ToDouble(decimal.Add(FirstSurvey.Azimuth, 180m))))
				{
					d2 = 180.0;
				}
				else if ((EndInc > Convert.ToDouble(FirstSurvey.Dip)) & (EndDir == Convert.ToDouble(decimal.Subtract(FirstSurvey.Azimuth, 180m))))
				{
					d2 = 180.0;
				}
				int num5 = (int)System.Math.Round(Curve);
				for (int i = 1; i <= num5; i++)
				{
					num3++;
					double num6 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
					double num7 = num6 - Convert.ToDouble(FirstSurvey.Dip);
					num8 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num7) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num7) * System.Math.Cos(System.Math.PI / 180.0 * d2));
					num4 = Convert.ToDouble(FirstSurvey.Azimuth) + 180.0 / System.Math.PI * System.Math.Atan(System.Math.Tan(System.Math.PI / 180.0 * num7) * System.Math.Sin(System.Math.PI / 180.0 * d2) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num7) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * d2)));
					if ((d2 == 180.0) & (num7 > Convert.ToDouble(FirstSurvey.Dip)))
					{
						num4 = Convert.ToDouble(decimal.Add(FirstSurvey.Azimuth, 180m));
						num4 = FixRange(num4, 360.0, 0.0);
					}
					else if (d2 == 0.0)
					{
						num4 = num4;
					}
					double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num8));
					double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num8) * System.Math.Cos(System.Math.PI / 180.0 * num4));
					double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num8) * System.Math.Sin(System.Math.PI / 180.0 * num4));
					absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num8), new decimal(num4), new decimal(value), new decimal(value2), new decimal(value3), 0m));
					num2++;
					num++;
				}
				if ((num8 > 90.0) | (num8 < -90.0))
				{
					MaxDip = true;
					goto IL_13a2;
				}
			}
			else if ((d2 > 180.0) & (d2 < 360.0))
			{
				int num9 = (int)System.Math.Round(Curve);
				for (int j = 1; j <= num9; j++)
				{
					num3++;
					double num6 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
					double num7 = num6 - Convert.ToDouble(FirstSurvey.Dip);
					num8 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num7) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num7) * System.Math.Cos(System.Math.PI / 180.0 * d2));
					num4 = Convert.ToDouble(FirstSurvey.Azimuth) + 180.0 / System.Math.PI * System.Math.Atan(System.Math.Tan(System.Math.PI / 180.0 * num7) * System.Math.Sin(System.Math.PI / 180.0 * d2) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num7) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * d2)));
					if ((num4 > Convert.ToDouble(FirstSurvey.Azimuth)) & (num4 < 270.0))
					{
						num4 = num4 - 180.0 + 360.0;
					}
					double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num8));
					double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num8) * System.Math.Cos(System.Math.PI / 180.0 * num4));
					double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num8) * System.Math.Sin(System.Math.PI / 180.0 * num4));
					absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num8), new decimal(num4), new decimal(value), new decimal(value2), new decimal(value3), 0m));
					num2++;
					num++;
				}
				if ((num8 > 90.0) | (num8 < -90.0))
				{
					MaxDip = true;
					goto IL_13a2;
				}
			}
			else if ((d2 > 0.0) & (d2 < 180.0))
			{
				int num10 = (int)System.Math.Round(Curve);
				for (int k = 1; k <= num10; k++)
				{
					num3++;
					double num6 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
					double num7 = num6 - Convert.ToDouble(FirstSurvey.Dip);
					num8 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num7) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num7) * System.Math.Cos(System.Math.PI / 180.0 * d2));
					num4 = Convert.ToDouble(FirstSurvey.Azimuth) + 180.0 / System.Math.PI * System.Math.Atan(System.Math.Tan(System.Math.PI / 180.0 * num7) * System.Math.Sin(System.Math.PI / 180.0 * d2) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num7) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * d2)));
					if (num4 < Convert.ToDouble(FirstSurvey.Azimuth))
					{
						num4 += 180.0;
					}
					double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num8));
					double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num8) * System.Math.Cos(System.Math.PI / 180.0 * num4));
					double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num8) * System.Math.Sin(System.Math.PI / 180.0 * num4));
					absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num8), new decimal(num4), new decimal(value), new decimal(value2), new decimal(value3), 0m));
					num2++;
					num++;
				}
				if ((num8 > 90.0) | (num8 < -90.0))
				{
					MaxDip = true;
					goto IL_13a2;
				}
			}
			int num11 = absoluteSurveyList.Count() - 1;
			for (int l = 0; l <= num11; l++)
			{
				if (decimal.Compare(absoluteSurveyList.RecordAt(l).Azimuth, 360m) > 0)
				{
					absoluteSurveyList.RecordAt(l).Azimuth = decimal.Subtract(absoluteSurveyList.RecordAt(l).Azimuth, 360m);
				}
			}
			double num12 = EndInc - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip);
			double num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
			double num14 = num12 / num13;
			double value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num13 * num14;
			double num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
			double value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num14;
			double num16 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
			double value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num16 * num14;
			double num17 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Azimuth, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth));
			double value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth) + num17 * num14;
			double num18 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
			double value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num18 * num14;
			double num19 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
			double value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num19 * num14;
			absoluteSurveyList.LastRecord().MD = new decimal(value5);
			absoluteSurveyList.LastRecord().Azimuth = new decimal(value7);
			absoluteSurveyList.LastRecord().Dip = new decimal(value4);
			absoluteSurveyList.LastRecord().TVD = new decimal(value6);
			absoluteSurveyList.LastRecord().Northing = new decimal(value8);
			absoluteSurveyList.LastRecord().Easting = new decimal(value9);
			if (interval > Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD)))
			{
				interval = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD));
			}
			if (interval > 1.0)
			{
				int num20 = (int)System.Math.Round((double)absoluteSurveyList.Count() - interval);
				int num21 = (int)System.Math.Round(interval);
				int num22 = num20;
				for (int m = 0; ((num21 >> 31) ^ m) <= ((num21 >> 31) ^ num22); m += num21)
				{
					ResultList.Add(absoluteSurveyList.RecordAt(m));
				}
				ResultList.Add(absoluteSurveyList.LastRecord());
			}
			else
			{
				ResultList = absoluteSurveyList;
			}
			decimal limit = new decimal(ResultList.Count() - 1);
			decimal num23 = 1m;
			while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num23, limit, 1m))
			{
				ResultList.RecordAt(Convert.ToInt32(num23)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num23)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num23, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num23)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num23, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(ResultList.RecordAt(Convert.ToInt32(num23)).Azimuth, ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num23, 1m))).Azimuth)))));
				ResultList.RecordAt(Convert.ToInt32(num23)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num23)).DL));
				num23 = decimal.Add(num23, 1m);
			}
			int num24 = ResultList.Count() - 1;
			for (int n = 1; n <= num24; n++)
			{
				ResultList.RecordAt(n).Azimuth = System.Math.Round(ResultList.RecordAt(n).Azimuth, 2);
				ResultList.RecordAt(n).Dip = System.Math.Round(ResultList.RecordAt(n).Dip, 2);
				ResultList.RecordAt(n).DL = System.Math.Round(ResultList.RecordAt(n).DL, 2);
				ResultList.RecordAt(n).Easting = System.Math.Round(ResultList.RecordAt(n).Easting, 2);
				ResultList.RecordAt(n).MD = System.Math.Round(ResultList.RecordAt(n).MD, 2);
				ResultList.RecordAt(n).Northing = System.Math.Round(ResultList.RecordAt(n).Northing, 2);
				ResultList.RecordAt(n).TVD = System.Math.Round(ResultList.RecordAt(n).TVD, 2);
			}
			ResultList.Remove(0);
			return ResultList;
		}
		IL_13a2:
		checked
		{
			int num25 = absoluteSurveyList.Count() - 1;
			for (int num26 = 0; num26 <= num25; num26++)
			{
				if (decimal.Compare(absoluteSurveyList.RecordAt(num26).Azimuth, 360m) > 0)
				{
					absoluteSurveyList.RecordAt(num26).Azimuth = decimal.Subtract(absoluteSurveyList.RecordAt(num26).Azimuth, 360m);
				}
			}
			double num12 = EndInc - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip);
			double num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
			double num14 = num12 / num13;
			double value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num13 * num14;
			double num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
			double value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num14;
			double num16 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
			double value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num16 * num14;
			double value7 = 0.0;
			double num18 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
			double value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num18 * num14;
			double num19 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
			double value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num19 * num14;
			absoluteSurveyList.LastRecord().MD = new decimal(value5);
			absoluteSurveyList.LastRecord().Azimuth = new decimal(value7);
			absoluteSurveyList.LastRecord().Dip = new decimal(value4);
			absoluteSurveyList.LastRecord().TVD = new decimal(value6);
			absoluteSurveyList.LastRecord().Northing = new decimal(value8);
			absoluteSurveyList.LastRecord().Easting = new decimal(value9);
			if (interval > Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD)))
			{
				interval = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD));
			}
			if (interval > 1.0)
			{
				int num27 = (int)System.Math.Round((double)absoluteSurveyList.Count() - interval);
				int num28 = (int)System.Math.Round(interval);
				int num29 = num27;
				for (int num30 = 0; ((num28 >> 31) ^ num30) <= ((num28 >> 31) ^ num29); num30 += num28)
				{
					ResultList.Add(absoluteSurveyList.RecordAt(num30));
				}
				ResultList.Add(absoluteSurveyList.LastRecord());
			}
			else
			{
				ResultList = absoluteSurveyList;
			}
			decimal limit2 = new decimal(ResultList.Count() - 1);
			decimal num31 = 1m;
			while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num31, limit2, 1m))
			{
				ResultList.RecordAt(Convert.ToInt32(num31)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num31)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num31, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num31)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num31, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(ResultList.RecordAt(Convert.ToInt32(num31)).Azimuth, ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num31, 1m))).Azimuth)))));
				ResultList.RecordAt(Convert.ToInt32(num31)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num31)).DL));
				num31 = decimal.Add(num31, 1m);
			}
			int num32 = ResultList.Count() - 1;
			for (int num33 = 1; num33 <= num32; num33++)
			{
				ResultList.RecordAt(num33).Azimuth = System.Math.Round(ResultList.RecordAt(num33).Azimuth, 2);
				ResultList.RecordAt(num33).Dip = System.Math.Round(ResultList.RecordAt(num33).Dip, 2);
				ResultList.RecordAt(num33).DL = System.Math.Round(ResultList.RecordAt(num33).DL, 3);
				ResultList.RecordAt(num33).Easting = System.Math.Round(ResultList.RecordAt(num33).Easting, 2);
				ResultList.RecordAt(num33).MD = System.Math.Round(ResultList.RecordAt(num33).MD, 2);
				ResultList.RecordAt(num33).Northing = System.Math.Round(ResultList.RecordAt(num33).Northing, 2);
				ResultList.RecordAt(num33).TVD = System.Math.Round(ResultList.RecordAt(num33).TVD, 2);
			}
			ResultList.Remove(0);
			return ResultList;
		}
	}

	public static AbsoluteSurveyList ExtrapolateIntervalSurveysTVD(double EndTVD, double ToolFace, double OverallDogLeg, double interval, AbsoluteSurvey FirstSurvey, ref AbsoluteSurveyList ResultList, ref bool MaxDip, ref bool MaxMD)
	{
		AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
		absoluteSurveyList.Add(FirstSurvey);
		int num = 1;
		object left = OverallDogLeg / 30.0;
		double num2 = 1.0;
		double num3 = Convert.ToDouble(FirstSurvey.TVD);
		double num4 = Convert.ToDouble(FirstSurvey.MD);
		checked
		{
			int num8;
			int i;
			double num13;
			double num15;
			double value3;
			double value4;
			double num16;
			double value5;
			double num17;
			double value6;
			double num18;
			double value7;
			double num12;
			double value8;
			while (true)
			{
				if (num3 < EndTVD)
				{
					num4++;
					double num5 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
					double num6 = num5 - Convert.ToDouble(FirstSurvey.Dip);
					double num7 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num6) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
					double d = System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
					d = System.Math.Atan(d);
					d = 180.0 / System.Math.PI * d;
					if (d > 360.0)
					{
						d = FixRange(d, 0.0, 360.0);
					}
					if (ToolFace == 0.0)
					{
						d = Convert.ToDouble(FirstSurvey.Azimuth) + d;
						if (d > 360.0)
						{
							d -= 360.0;
						}
					}
					else if ((ToolFace >= 0.0) & (ToolFace <= 90.0) & (System.Math.Sign(d) == 1))
					{
						d = Convert.ToDouble(FirstSurvey.Azimuth) + d;
						if (d > 360.0)
						{
							d -= 360.0;
						}
					}
					else if ((ToolFace >= 0.0) & (ToolFace <= 90.0) & (System.Math.Sign(d) == -1))
					{
						d = Convert.ToDouble(decimal.Subtract(FirstSurvey.Azimuth, 180m)) + d;
					}
					else if ((ToolFace > 90.0) & (ToolFace <= 180.0) & (System.Math.Sign(d) == -1))
					{
						d = Convert.ToDouble(decimal.Add(FirstSurvey.Azimuth, 180m)) + d;
					}
					else if ((ToolFace > 90.0) & (ToolFace <= 180.0) & (System.Math.Sign(d) == 1))
					{
						d = Convert.ToDouble(FirstSurvey.Azimuth) + d;
						if (d > 360.0)
						{
							d -= 360.0;
						}
					}
					else if ((ToolFace > 180.0) & (ToolFace <= 270.0) & (System.Math.Sign(d) == -1))
					{
						d = Convert.ToDouble(FirstSurvey.Azimuth) + d;
					}
					else if ((ToolFace > 180.0) & (ToolFace <= 270.0) & (System.Math.Sign(d) == 1))
					{
						d = Convert.ToDouble(decimal.Subtract(FirstSurvey.Azimuth, 180m)) + d;
					}
					else if ((ToolFace > 270.0) & (ToolFace <= 360.0) & (System.Math.Sign(d) == -1))
					{
						d = Convert.ToDouble(FirstSurvey.Azimuth) + d;
					}
					else if ((ToolFace > 270.0) & (ToolFace <= 360.0) & (num7 > 90.0))
					{
						d = Convert.ToDouble(decimal.Subtract(FirstSurvey.Azimuth, 180m)) + d;
					}
					if (d > 360.0)
					{
						d -= 360.0;
					}
					num3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num7));
					double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num7) * System.Math.Cos(System.Math.PI / 180.0 * d));
					double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num7) * System.Math.Sin(System.Math.PI / 180.0 * d));
					absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num4), new decimal(num7), new decimal(d), new decimal(num3), new decimal(value), new decimal(value2), 0m));
					if ((num7 > 90.0) | (num7 < -90.0))
					{
						MaxDip = true;
						num8 = absoluteSurveyList.Count() - 1;
						i = 0;
						break;
					}
					if (num4 > 24000.0)
					{
						MaxMD = true;
						int num9 = absoluteSurveyList.Count() - 1;
						for (int j = 0; j <= num9; j++)
						{
							if (System.Math.Sign(absoluteSurveyList.RecordAt(j).Dip) == -1)
							{
								absoluteSurveyList.RecordAt(j).Dip = decimal.Multiply(absoluteSurveyList.RecordAt(j).Dip, -1m);
							}
						}
						decimal limit = new decimal(absoluteSurveyList.Count() - 1);
						decimal num10 = 1m;
						while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num10, limit, 1m))
						{
							absoluteSurveyList.RecordAt(Convert.ToInt32(num10)).DL = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num10)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num10, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num10)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num10, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num10)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num10, 1m))).Azimuth))));
							absoluteSurveyList.RecordAt(Convert.ToInt32(num10)).DL = new decimal(180.0 / System.Math.PI * System.Math.Acos(Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num10)).DL)) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num10)).MD, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num10, 1m))).MD)) * 30.0);
							num10 = decimal.Add(num10, 1m);
						}
						double num11 = Convert.ToDouble(decimal.Subtract(24000m, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
						num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
						num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
						double num14 = num11 / num13;
						value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num14;
						num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
						value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num15 * num14;
						num16 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Azimuth, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth));
						value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth) + num16 * num14;
						num17 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
						value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num17 * num14;
						num18 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
						value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num18 * num14;
						value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num12 * num14;
						absoluteSurveyList.LastRecord().MD = new decimal(System.Math.Round(value3, 2));
						absoluteSurveyList.LastRecord().Azimuth = new decimal(System.Math.Round(value5, 2));
						absoluteSurveyList.LastRecord().Dip = new decimal(System.Math.Round(value4, 2));
						absoluteSurveyList.LastRecord().TVD = new decimal(System.Math.Round(value8, 2));
						absoluteSurveyList.LastRecord().Northing = new decimal(System.Math.Round(value6, 2));
						absoluteSurveyList.LastRecord().Easting = new decimal(System.Math.Round(value7, 2));
						int num19 = absoluteSurveyList.Count() - 1;
						for (int k = 1; k <= num19; k++)
						{
							absoluteSurveyList.RecordAt(k).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(k).Azimuth, 2);
							absoluteSurveyList.RecordAt(k).Dip = System.Math.Round(absoluteSurveyList.RecordAt(k).Dip, 2);
							absoluteSurveyList.RecordAt(k).DL = System.Math.Round(absoluteSurveyList.RecordAt(k).DL, 2);
							absoluteSurveyList.RecordAt(k).Easting = System.Math.Round(absoluteSurveyList.RecordAt(k).Easting, 2);
							absoluteSurveyList.RecordAt(k).MD = System.Math.Round(absoluteSurveyList.RecordAt(k).MD, 2);
							absoluteSurveyList.RecordAt(k).Northing = System.Math.Round(absoluteSurveyList.RecordAt(k).Northing, 2);
							absoluteSurveyList.RecordAt(k).TVD = System.Math.Round(absoluteSurveyList.RecordAt(k).TVD, 2);
						}
						if (interval > 1.0)
						{
							int num20 = (int)System.Math.Round((double)absoluteSurveyList.Count() - (interval - 1.0));
							int num21 = (int)System.Math.Round(interval);
							int num22 = num20;
							for (int l = 0; ((num21 >> 31) ^ l) <= ((num21 >> 31) ^ num22); l += num21)
							{
								ResultList.Add(absoluteSurveyList.RecordAt(l));
							}
							ResultList.Add(absoluteSurveyList.LastRecord());
						}
						else
						{
							ResultList = absoluteSurveyList;
						}
						ResultList.Remove(0);
						return ResultList;
					}
					num2++;
					num++;
					continue;
				}
				int num23 = absoluteSurveyList.Count() - 1;
				for (int m = 0; m <= num23; m++)
				{
					if (System.Math.Sign(absoluteSurveyList.RecordAt(m).Dip) == -1)
					{
						absoluteSurveyList.RecordAt(m).Dip = decimal.Multiply(absoluteSurveyList.RecordAt(m).Dip, -1m);
					}
				}
				decimal limit2 = new decimal(absoluteSurveyList.Count() - 1);
				decimal num24 = 1m;
				while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num24, limit2, 1m))
				{
					absoluteSurveyList.RecordAt(Convert.ToInt32(num24)).DL = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num24)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num24, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num24)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num24, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num24)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num24, 1m))).Azimuth))));
					absoluteSurveyList.RecordAt(Convert.ToInt32(num24)).DL = new decimal(180.0 / System.Math.PI * System.Math.Acos(Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num24)).DL)) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num24)).MD, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num24, 1m))).MD)) * 30.0);
					num24 = decimal.Add(num24, 1m);
				}
				double num25 = EndTVD - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD);
				num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
				num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
				double num26 = num25 / num12;
				value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num26;
				num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
				value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num15 * num26;
				num16 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Azimuth, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth));
				value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth) + num16 * num26;
				num17 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
				value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num17 * num26;
				num18 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
				value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num18 * num26;
				value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num12 * num26;
				absoluteSurveyList.LastRecord().MD = new decimal(value3);
				absoluteSurveyList.LastRecord().Azimuth = new decimal(value5);
				absoluteSurveyList.LastRecord().Dip = new decimal(value4);
				absoluteSurveyList.LastRecord().TVD = new decimal(value8);
				absoluteSurveyList.LastRecord().Northing = new decimal(value6);
				absoluteSurveyList.LastRecord().Easting = new decimal(value7);
				decimal limit3 = new decimal(absoluteSurveyList.Count() - 1);
				decimal num27 = 1m;
				while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num27, limit3, 1m))
				{
					absoluteSurveyList.RecordAt(Convert.ToInt32(num27)).DL = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num27)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num27, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num27)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num27, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num27)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num27, 1m))).Azimuth))));
					absoluteSurveyList.RecordAt(Convert.ToInt32(num27)).DL = new decimal(180.0 / System.Math.PI * System.Math.Acos(Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num27)).DL)) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num27)).MD, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num27, 1m))).MD)) * 30.0);
					num27 = decimal.Add(num27, 1m);
				}
				int num28 = absoluteSurveyList.Count() - 1;
				for (int n = 1; n <= num28; n++)
				{
					absoluteSurveyList.RecordAt(n).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(n).Azimuth, 2);
					absoluteSurveyList.RecordAt(n).Dip = System.Math.Round(absoluteSurveyList.RecordAt(n).Dip, 2);
					absoluteSurveyList.RecordAt(n).DL = System.Math.Round(absoluteSurveyList.RecordAt(n).DL, 2);
					absoluteSurveyList.RecordAt(n).Easting = System.Math.Round(absoluteSurveyList.RecordAt(n).Easting, 2);
					absoluteSurveyList.RecordAt(n).MD = System.Math.Round(absoluteSurveyList.RecordAt(n).MD, 2);
					absoluteSurveyList.RecordAt(n).Northing = System.Math.Round(absoluteSurveyList.RecordAt(n).Northing, 2);
					absoluteSurveyList.RecordAt(n).TVD = System.Math.Round(absoluteSurveyList.RecordAt(n).TVD, 2);
				}
				if (interval > Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD)))
				{
					interval = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD));
				}
				if (interval > 1.0)
				{
					int num29 = (int)System.Math.Round((double)absoluteSurveyList.Count() - (interval - 1.0));
					int num30 = (int)System.Math.Round(interval);
					int num31 = num29;
					for (int num32 = 0; ((num30 >> 31) ^ num32) <= ((num30 >> 31) ^ num31); num32 += num30)
					{
						ResultList.Add(absoluteSurveyList.RecordAt(num32));
					}
					ResultList.Add(absoluteSurveyList.LastRecord());
				}
				else
				{
					ResultList = absoluteSurveyList;
				}
				ResultList.Remove(0);
				return ResultList;
			}
			for (; i <= num8; i++)
			{
				if (System.Math.Sign(absoluteSurveyList.RecordAt(i).Dip) == -1)
				{
					absoluteSurveyList.RecordAt(i).Dip = decimal.Multiply(absoluteSurveyList.RecordAt(i).Dip, -1m);
				}
			}
			decimal limit4 = new decimal(absoluteSurveyList.Count() - 1);
			decimal num33 = 1m;
			while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num33, limit4, 1m))
			{
				absoluteSurveyList.RecordAt(Convert.ToInt32(num33)).DL = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num33)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num33, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num33)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num33, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num33)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num33, 1m))).Azimuth))));
				absoluteSurveyList.RecordAt(Convert.ToInt32(num33)).DL = new decimal(180.0 / System.Math.PI * System.Math.Acos(Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num33)).DL)) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num33)).MD, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num33, 1m))).MD)) * 30.0);
				num33 = decimal.Add(num33, 1m);
			}
			double num34 = 89.99 - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip);
			num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
			num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
			double num35 = num34 / num15;
			value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num35;
			value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num15 * num35;
			num16 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Azimuth, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth));
			value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth) + num16 * num35;
			num17 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
			if (System.Math.Sign(num17) == -1)
			{
				num17 *= -1.0;
			}
			value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num17 * num35;
			num18 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
			if (System.Math.Sign(num18) == -1)
			{
				num18 *= -1.0;
			}
			value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num18 * num35;
			num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
			if (System.Math.Sign(num12) == -1)
			{
				num12 *= -1.0;
			}
			value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num12 * num35;
			absoluteSurveyList.LastRecord().MD = new decimal(System.Math.Round(value3, 2));
			absoluteSurveyList.LastRecord().Azimuth = new decimal(System.Math.Round(value5, 2));
			absoluteSurveyList.LastRecord().Dip = new decimal(System.Math.Round(value4, 2));
			absoluteSurveyList.LastRecord().TVD = new decimal(System.Math.Round(value8, 2));
			absoluteSurveyList.LastRecord().Northing = new decimal(System.Math.Round(value6, 2));
			absoluteSurveyList.LastRecord().Easting = new decimal(System.Math.Round(value7, 2));
			decimal limit5 = new decimal(absoluteSurveyList.Count() - 1);
			decimal num36 = 1m;
			while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num36, limit5, 1m))
			{
				absoluteSurveyList.RecordAt(Convert.ToInt32(num36)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num36)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num36, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num36)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num36, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num36)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num36, 1m))).Azimuth)))));
				absoluteSurveyList.RecordAt(Convert.ToInt32(num36)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num36)).DL));
				num36 = decimal.Add(num36, 1m);
			}
			int num37 = absoluteSurveyList.Count() - 1;
			for (int num38 = 1; num38 <= num37; num38++)
			{
				absoluteSurveyList.RecordAt(num38).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(num38).Azimuth, 2);
				absoluteSurveyList.RecordAt(num38).Dip = System.Math.Round(absoluteSurveyList.RecordAt(num38).Dip, 2);
				absoluteSurveyList.RecordAt(num38).DL = System.Math.Round(absoluteSurveyList.RecordAt(num38).DL, 2);
				absoluteSurveyList.RecordAt(num38).Easting = System.Math.Round(absoluteSurveyList.RecordAt(num38).Easting, 2);
				absoluteSurveyList.RecordAt(num38).MD = System.Math.Round(absoluteSurveyList.RecordAt(num38).MD, 2);
				absoluteSurveyList.RecordAt(num38).Northing = System.Math.Round(absoluteSurveyList.RecordAt(num38).Northing, 2);
				absoluteSurveyList.RecordAt(num38).TVD = System.Math.Round(absoluteSurveyList.RecordAt(num38).TVD, 2);
			}
			if (interval > Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD)))
			{
				interval = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD));
			}
			if (interval > 1.0)
			{
				int num39 = (int)System.Math.Round((double)absoluteSurveyList.Count() - (interval - 1.0));
				int num40 = (int)System.Math.Round(interval);
				int num41 = num39;
				for (int num42 = 0; ((num40 >> 31) ^ num42) <= ((num40 >> 31) ^ num41); num42 += num40)
				{
					ResultList.Add(absoluteSurveyList.RecordAt(num42));
				}
				ResultList.Add(absoluteSurveyList.LastRecord());
			}
			else
			{
				ResultList = absoluteSurveyList;
			}
			ResultList.Remove(0);
			return ResultList;
		}
	}

	public static AbsoluteSurveyList ExtrapolateIntervalSurveysMD(double EndMD, double ToolFace, double OverallDogLeg, double interval, AbsoluteSurvey FirstSurvey, ref AbsoluteSurveyList ResultList, ref bool MaxDip, bool ExtrapToDir, bool IncIsZero, bool DirIsZero, bool ExtrapToInc, double EndInc, bool MinDL, double Endazi, double EndTVD, double EndN, double EndE, bool Build)
	{
		AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
		absoluteSurveyList.Add(FirstSurvey);
		int num = 1;
		object left = OverallDogLeg / 30.0;
		double num2 = 1.0;
		double num3 = Convert.ToDouble(FirstSurvey.MD);
		checked
		{
			double num23;
			double num13;
			double num12;
			double num24;
			double value5;
			double num10;
			double value4;
			double num25;
			double value7;
			double num14;
			double value8;
			double num15;
			double value9;
			double value6;
			double num16;
			if (ExtrapToDir)
			{
				if (IncIsZero)
				{
					ToolFace = 180.0;
					double num4 = default;
					while (!(num4 < 0.0))
					{
						num3++;
						num4 = Conversions.ToDouble(Operators.SubtractObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
						double num5 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
						double num6 = num5 - Convert.ToDouble(FirstSurvey.Dip);
						double num7 = Convert.ToDouble(FirstSurvey.Azimuth) + 180.0 / System.Math.PI * System.Math.Atan(System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace)));
						double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num4));
						double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num4) * System.Math.Cos(System.Math.PI / 180.0 * num7));
						double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num4) * System.Math.Sin(System.Math.PI / 180.0 * num7));
						absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num4), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), new decimal(OverallDogLeg)));
						num2++;
						num++;
					}
					absoluteSurveyList.LastRecord().Azimuth = 0m;
					decimal limit = new decimal(ResultList.Count() - 1);
					decimal num8 = 1m;
					while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num8, limit, 1m))
					{
						ResultList.RecordAt(Convert.ToInt32(num8)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num8)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num8, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num8)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num8, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(ResultList.RecordAt(Convert.ToInt32(num8)).Azimuth, ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num8, 1m))).Azimuth)))));
						ResultList.RecordAt(Convert.ToInt32(num8)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num8)).DL));
						num8 = decimal.Add(num8, 1m);
					}
					double num9 = Convert.ToDouble(decimal.Subtract(0m, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
					num10 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
					double num11 = num9 / num10;
					value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num10 * num11;
					num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
					value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num11;
					num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
					value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num13 * num11;
					value7 = 0.0;
					num14 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
					value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num14 * num11;
					num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
					value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num15 * num11;
					num16 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).DL) * num11;
					absoluteSurveyList.LastRecord().MD = new decimal(value5);
					absoluteSurveyList.LastRecord().Azimuth = new decimal(value7);
					absoluteSurveyList.LastRecord().Dip = new decimal(value4);
					absoluteSurveyList.LastRecord().TVD = new decimal(value6);
					absoluteSurveyList.LastRecord().Northing = new decimal(value8);
					absoluteSurveyList.LastRecord().Easting = new decimal(value9);
					if (decimal.Compare(absoluteSurveyList.LastRecord().MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) == 0)
					{
						absoluteSurveyList.Remove(absoluteSurveyList.Count() - 2);
					}
					if (interval > Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD)))
					{
						interval = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD));
					}
					if (interval > 1.0)
					{
						int num17 = (int)System.Math.Round((double)absoluteSurveyList.Count() - interval);
						int num18 = (int)System.Math.Round(interval);
						int num19 = num17;
						for (int i = 0; ((num18 >> 31) ^ i) <= ((num18 >> 31) ^ num19); i += num18)
						{
							ResultList.Add(absoluteSurveyList.RecordAt(i));
						}
						ResultList.Add(absoluteSurveyList.LastRecord());
					}
					else
					{
						ResultList = absoluteSurveyList;
					}
					int num20 = ResultList.Count() - 1;
					for (int j = 1; j <= num20; j++)
					{
						ResultList.RecordAt(j).Azimuth = System.Math.Round(ResultList.RecordAt(j).Azimuth, 2);
						ResultList.RecordAt(j).Dip = System.Math.Round(ResultList.RecordAt(j).Dip, 2);
						ResultList.RecordAt(j).DL = System.Math.Round(ResultList.RecordAt(j).DL, 3);
						ResultList.RecordAt(j).Easting = System.Math.Round(ResultList.RecordAt(j).Easting, 2);
						ResultList.RecordAt(j).MD = System.Math.Round(ResultList.RecordAt(j).MD, 2);
						ResultList.RecordAt(j).Northing = System.Math.Round(ResultList.RecordAt(j).Northing, 2);
						ResultList.RecordAt(j).TVD = System.Math.Round(ResultList.RecordAt(j).TVD, 2);
					}
					ResultList.Remove(0);
					return ResultList;
				}
				if (DirIsZero)
				{
					while (EndMD > num3)
					{
						num3++;
						double num5 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
						double num6 = num5 - Convert.ToDouble(FirstSurvey.Dip);
						double num21 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num6) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
						double num7 = Convert.ToDouble(FirstSurvey.Azimuth) + 180.0 / System.Math.PI * System.Math.Atan(System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace)));
						double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num21));
						double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Cos(System.Math.PI / 180.0 * num7));
						double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Sin(System.Math.PI / 180.0 * num7));
						absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), new decimal(OverallDogLeg)));
						num2++;
						num++;
					}
					decimal limit2 = new decimal(absoluteSurveyList.Count() - 1);
					decimal num22 = 1m;
					while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num22, limit2, 1m))
					{
						absoluteSurveyList.RecordAt(Convert.ToInt32(num22)).DL = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num22)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num22, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num22)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num22, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num22)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num22, 1m))).Azimuth))));
						absoluteSurveyList.RecordAt(Convert.ToInt32(num22)).DL = new decimal(180.0 / System.Math.PI * System.Math.Acos(Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num22)).DL)) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num22)).MD, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num22, 1m))).MD)) * 30.0);
						num22 = decimal.Add(num22, 1m);
					}
					num23 = EndMD - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD);
					num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
					num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
					num24 = num23 / num12;
					value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num24;
					num10 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
					value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num10 * num24;
					num25 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Azimuth, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth));
					value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth) + num25 * num24;
					num14 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
					value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num14 * num24;
					num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
					value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num15 * num24;
					value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num13 * num24;
					num16 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).DL) * num24;
					absoluteSurveyList.LastRecord().MD = new decimal(System.Math.Round(value5, 2));
					absoluteSurveyList.LastRecord().Azimuth = 0m;
					absoluteSurveyList.LastRecord().Dip = new decimal(System.Math.Round(value4, 2));
					absoluteSurveyList.LastRecord().TVD = new decimal(System.Math.Round(value6, 2));
					absoluteSurveyList.LastRecord().Northing = new decimal(System.Math.Round(value8, 2));
					absoluteSurveyList.LastRecord().Easting = new decimal(System.Math.Round(value9, 2));
					int num26 = absoluteSurveyList.Count() - 1;
					for (int k = 1; k <= num26; k++)
					{
						absoluteSurveyList.RecordAt(k).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(k).Azimuth, 2);
						absoluteSurveyList.RecordAt(k).Dip = System.Math.Round(absoluteSurveyList.RecordAt(k).Dip, 2);
						absoluteSurveyList.RecordAt(k).DL = System.Math.Round(absoluteSurveyList.RecordAt(k).DL, 2);
						absoluteSurveyList.RecordAt(k).Easting = System.Math.Round(absoluteSurveyList.RecordAt(k).Easting, 2);
						absoluteSurveyList.RecordAt(k).MD = System.Math.Round(absoluteSurveyList.RecordAt(k).MD, 2);
						absoluteSurveyList.RecordAt(k).Northing = System.Math.Round(absoluteSurveyList.RecordAt(k).Northing, 2);
						absoluteSurveyList.RecordAt(k).TVD = System.Math.Round(absoluteSurveyList.RecordAt(k).TVD, 2);
					}
					if (interval > 1.0)
					{
						int num27 = (int)System.Math.Round((double)absoluteSurveyList.Count() - (interval - 1.0));
						int num28 = (int)System.Math.Round(interval);
						int num29 = num27;
						for (int l = 0; ((num28 >> 31) ^ l) <= ((num28 >> 31) ^ num29); l += num28)
						{
							ResultList.Add(absoluteSurveyList.RecordAt(l));
						}
						ResultList.Add(absoluteSurveyList.LastRecord());
					}
					else
					{
						ResultList = absoluteSurveyList;
					}
					ResultList.Remove(0);
					return ResultList;
				}
				if (decimal.Compare(FirstSurvey.Dip, 0m) != 0)
				{
					if (decimal.Compare(FirstSurvey.Azimuth, 0m) == 0)
					{
						while (num3 < EndMD)
						{
							num3++;
							double num5 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
							double num6 = num5 - Convert.ToDouble(FirstSurvey.Dip);
							double num21 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num6) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
							double num7 = 360.0 + 180.0 / System.Math.PI * System.Math.Atan(System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace)));
							if (num7 > 360.0)
							{
								num7 = FixRange(num7, 0.0, 360.0);
							}
							double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num5));
							double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num5) * System.Math.Cos(System.Math.PI / 180.0 * num7));
							double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num5) * System.Math.Sin(System.Math.PI / 180.0 * num7));
							absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), new decimal(OverallDogLeg)));
							num2++;
							num++;
						}
						decimal limit3 = new decimal(ResultList.Count() - 1);
						decimal num30 = 1m;
						while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num30, limit3, 1m))
						{
							ResultList.RecordAt(Convert.ToInt32(num30)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num30)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num30, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num30)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num30, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(ResultList.RecordAt(Convert.ToInt32(num30)).Azimuth, ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num30, 1m))).Azimuth)))));
							ResultList.RecordAt(Convert.ToInt32(num30)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num30)).DL));
							num30 = decimal.Add(num30, 1m);
						}
						double num9 = EndInc - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip);
						num10 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
						double num11 = num9 / num10;
						value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num10 * num11;
						double num31 = Endazi - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth);
						num25 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Azimuth, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth));
						double num32 = num31 / num25;
						value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth) + num25 * num32;
						num23 = EndMD - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD);
						num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
						num24 = num23 / num12;
						value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num12 * num24;
						double num33 = EndTVD - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD);
						num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
						double num34 = num33 / num13;
						value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num13 * num34;
						double num35 = EndN - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing);
						num14 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
						double num36 = num35 / num14;
						value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num14 * num36;
						double num37 = EndE - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting);
						num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
						double num38 = num37 / num15;
						value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num15 * num38;
						absoluteSurveyList.LastRecord().MD = new decimal(value5);
						absoluteSurveyList.LastRecord().Azimuth = new decimal(value7);
						absoluteSurveyList.LastRecord().Dip = new decimal(value4);
						absoluteSurveyList.LastRecord().TVD = new decimal(value6);
						absoluteSurveyList.LastRecord().Northing = new decimal(value8);
						absoluteSurveyList.LastRecord().Easting = new decimal(value9);
						int num39 = absoluteSurveyList.Count() - 1;
						for (int m = 1; m <= num39; m++)
						{
							absoluteSurveyList.RecordAt(m).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(m).Azimuth, 2);
							absoluteSurveyList.RecordAt(m).Dip = System.Math.Round(absoluteSurveyList.RecordAt(m).Dip, 2);
							absoluteSurveyList.RecordAt(m).DL = System.Math.Round(absoluteSurveyList.RecordAt(m).DL, 2);
							absoluteSurveyList.RecordAt(m).Easting = System.Math.Round(absoluteSurveyList.RecordAt(m).Easting, 2);
							absoluteSurveyList.RecordAt(m).MD = System.Math.Round(absoluteSurveyList.RecordAt(m).MD, 2);
							absoluteSurveyList.RecordAt(m).Northing = System.Math.Round(absoluteSurveyList.RecordAt(m).Northing, 2);
							absoluteSurveyList.RecordAt(m).TVD = System.Math.Round(absoluteSurveyList.RecordAt(m).TVD, 2);
						}
						if (interval > 1.0)
						{
							int num40 = (int)System.Math.Round((double)absoluteSurveyList.Count() - (interval - 1.0));
							int num41 = (int)System.Math.Round(interval);
							int num42 = num40;
							for (int n = 0; ((num41 >> 31) ^ n) <= ((num41 >> 31) ^ num42); n += num41)
							{
								ResultList.Add(absoluteSurveyList.RecordAt(n));
							}
							ResultList.Add(absoluteSurveyList.LastRecord());
						}
						else
						{
							ResultList = absoluteSurveyList;
						}
						ResultList.Remove(0);
						return ResultList;
					}
					double value10 = default;
					while (true)
					{
						if (num3 < EndMD)
						{
							num3++;
							double num5 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
							double num6 = num5 - Convert.ToDouble(FirstSurvey.Dip);
							double num21 = ((num6 - Convert.ToDouble(FirstSurvey.Dip) != 0.0) ? (180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num6) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace))) : 0.0);
							double num43 = System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip));
							double num44 = System.Math.Cos(System.Math.PI / 180.0 * num21);
							value10 = num44 - num43;
							double num7 = System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
							num7 = System.Math.Atan(num7);
							num7 = 180.0 / System.Math.PI * num7;
							if (num7 > 360.0)
							{
								num7 = FixRange(num7, 0.0, 360.0);
							}
							if (ToolFace == 0.0)
							{
								num7 = Convert.ToDouble(FirstSurvey.Azimuth) + num7;
								if (num7 > 360.0)
								{
									num7 -= 360.0;
								}
							}
							else if ((ToolFace > 0.0) & (ToolFace <= 90.0) & (System.Math.Sign(num7) == 1))
							{
								num7 = Convert.ToDouble(FirstSurvey.Azimuth) + num7;
								if (num7 > 360.0)
								{
									num7 -= 360.0;
								}
							}
							else if ((ToolFace > 0.0) & (ToolFace <= 90.0) & (System.Math.Sign(num7) == -1))
							{
								num7 = Convert.ToDouble(decimal.Subtract(FirstSurvey.Azimuth, 180m)) + num7;
							}
							else if ((ToolFace > 90.0) & (ToolFace <= 180.0) & (System.Math.Sign(num7) == -1))
							{
								num7 = Convert.ToDouble(decimal.Add(FirstSurvey.Azimuth, 180m)) + num7;
							}
							else if ((ToolFace > 90.0) & (ToolFace <= 180.0) & (System.Math.Sign(num7) == 1))
							{
								num7 = Convert.ToDouble(FirstSurvey.Azimuth) + num7;
								if (num7 > 360.0)
								{
									num7 -= 360.0;
								}
							}
							else if ((ToolFace > 180.0) & (ToolFace <= 270.0) & (System.Math.Sign(num7) == -1))
							{
								num7 = Convert.ToDouble(FirstSurvey.Azimuth) + num7;
							}
							else if ((ToolFace > 180.0) & (ToolFace <= 270.0) & (System.Math.Sign(num7) == 1))
							{
								num7 = Convert.ToDouble(decimal.Subtract(FirstSurvey.Azimuth, 180m)) + num7;
							}
							else if ((ToolFace > 270.0) & (ToolFace <= 360.0) & (System.Math.Sign(num7) == -1))
							{
								num7 = Convert.ToDouble(FirstSurvey.Azimuth) + num7;
							}
							else if ((ToolFace > 270.0) & (ToolFace <= 360.0) & (num21 > 90.0))
							{
								num7 = Convert.ToDouble(decimal.Subtract(FirstSurvey.Azimuth, 180m)) + num7;
							}
							if (num7 > 360.0)
							{
								num7 -= 360.0;
							}
							double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num21));
							double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Cos(System.Math.PI / 180.0 * num7));
							double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Sin(System.Math.PI / 180.0 * num7));
							absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), new decimal(OverallDogLeg)));
							if ((num21 > 90.0) | (num21 < -90.0))
							{
								break;
							}
							num2++;
							num++;
							continue;
						}
						decimal limit4 = new decimal(ResultList.Count() - 1);
						decimal num45 = 1m;
						while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num45, limit4, 1m))
						{
							ResultList.RecordAt(Convert.ToInt32(num45)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num45)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num45, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num45)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num45, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(ResultList.RecordAt(Convert.ToInt32(num45)).Azimuth, ResultList.RecordAt(Convert.ToInt32(decimal.Subtract(num45, 1m))).Azimuth)))));
							ResultList.RecordAt(Convert.ToInt32(num45)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(ResultList.RecordAt(Convert.ToInt32(num45)).DL));
							num45 = decimal.Add(num45, 1m);
						}
						double num31 = Endazi - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth);
						if (num31 == 0.0)
						{
							value7 = Endazi;
							double num9 = EndInc - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip);
							num10 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
							double num11 = num9 / num10;
							value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num10 * num11;
							num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
							value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num11;
							num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
							value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num13 * num11;
							num14 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
							value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num14 * num11;
							num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
							value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num15 * num11;
						}
						else if (System.Math.Sign(value10) == -1)
						{
							double num9 = EndInc - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip);
							num10 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
							double num11 = num9 / num10;
							value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num10 * num11;
							num25 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Azimuth, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth));
							double num32 = num31 / num25;
							value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth) + num25 * num32;
							num23 = EndMD - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD);
							num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
							num24 = num23 / num12;
							value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num12 * num24;
							double num33 = EndTVD - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD);
							num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
							double num34 = num33 / num13;
							value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num13 * num34;
							double num35 = EndN - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing);
							num14 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
							double num36 = num35 / num14;
							value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num14 * num36;
							double num37 = EndE - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting);
							num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
							double num38 = num37 / num15;
							value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num15 * num38;
						}
						else
						{
							num25 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Azimuth, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth));
							double num32 = num31 / num25;
							value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth) + num25 * num32;
							num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
							value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num32;
							num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
							value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num13 * num32;
							num10 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
							value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num10 * num32;
							num14 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
							value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num14 * num32;
							num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
							value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num15 * num32;
							num16 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).DL) * num32;
						}
						absoluteSurveyList.LastRecord().MD = new decimal(value5);
						absoluteSurveyList.LastRecord().Azimuth = new decimal(value7);
						absoluteSurveyList.LastRecord().Dip = new decimal(value4);
						absoluteSurveyList.LastRecord().TVD = new decimal(value6);
						absoluteSurveyList.LastRecord().Northing = new decimal(value8);
						absoluteSurveyList.LastRecord().Easting = new decimal(value9);
						if (interval > Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD)))
						{
							interval = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD));
						}
						if (interval > 1.0)
						{
							int num46 = (int)System.Math.Round((double)absoluteSurveyList.Count() - interval);
							int num47 = (int)System.Math.Round(interval);
							int num48 = num46;
							for (int num49 = 0; ((num47 >> 31) ^ num49) <= ((num47 >> 31) ^ num48); num49 += num47)
							{
								ResultList.Add(absoluteSurveyList.RecordAt(num49));
							}
							ResultList.Add(absoluteSurveyList.LastRecord());
						}
						else
						{
							ResultList = absoluteSurveyList;
						}
						int num50 = ResultList.Count() - 1;
						for (int num51 = 1; num51 <= num50; num51++)
						{
							ResultList.RecordAt(num51).Azimuth = System.Math.Round(ResultList.RecordAt(num51).Azimuth, 2);
							ResultList.RecordAt(num51).Dip = System.Math.Round(ResultList.RecordAt(num51).Dip, 2);
							ResultList.RecordAt(num51).DL = System.Math.Round(ResultList.RecordAt(num51).DL, 3);
							ResultList.RecordAt(num51).Easting = System.Math.Round(ResultList.RecordAt(num51).Easting, 2);
							ResultList.RecordAt(num51).MD = System.Math.Round(ResultList.RecordAt(num51).MD, 2);
							ResultList.RecordAt(num51).Northing = System.Math.Round(ResultList.RecordAt(num51).Northing, 2);
							ResultList.RecordAt(num51).TVD = System.Math.Round(ResultList.RecordAt(num51).TVD, 2);
						}
						ResultList.Remove(0);
						return ResultList;
					}
					MaxDip = true;
					goto IL_24cc;
				}
				while (num3 < EndMD)
				{
					num3++;
					double num5 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
					double num6 = num5 - Convert.ToDouble(FirstSurvey.Dip);
					double num21 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num6) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
					double num7 = ToolFace;
					double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num5));
					double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num5) * System.Math.Cos(System.Math.PI / 180.0 * num7));
					double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num5) * System.Math.Sin(System.Math.PI / 180.0 * num7));
					absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), new decimal(OverallDogLeg)));
					num2++;
					num++;
				}
			}
			else if (MinDL)
			{
				while (num3 < EndMD)
				{
					num3++;
					double num5 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
					double num6 = num5 - Convert.ToDouble(FirstSurvey.Dip);
					double num21 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num6) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
					double num7 = ToolFace;
					double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num5));
					double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num5) * System.Math.Cos(System.Math.PI / 180.0 * num7));
					double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num5) * System.Math.Sin(System.Math.PI / 180.0 * num7));
					absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), new decimal(OverallDogLeg)));
					num2++;
					num++;
				}
			}
			else
			{
				if (!Build)
				{
					while (true)
					{
						if (num3 < EndMD)
						{
							num3++;
							double num5 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
							double num6 = num5 - Convert.ToDouble(FirstSurvey.Dip);
							double num21 = ((num6 - Convert.ToDouble(FirstSurvey.Dip) != 0.0) ? (180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num6) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace))) : 0.0);
							double num7 = System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
							num7 = System.Math.Atan(num7);
							num7 = 180.0 / System.Math.PI * num7;
							if (num7 > 360.0)
							{
								num7 = FixRange(num7, 0.0, 360.0);
							}
							if (ToolFace == 0.0)
							{
								num7 = Convert.ToDouble(FirstSurvey.Azimuth) + num7;
								if (num7 > 360.0)
								{
									num7 -= 360.0;
								}
							}
							else if ((ToolFace > 0.0) & (ToolFace <= 90.0) & (System.Math.Sign(num7) == 1))
							{
								num7 = Convert.ToDouble(FirstSurvey.Azimuth) + num7;
								if (num7 > 360.0)
								{
									num7 -= 360.0;
								}
							}
							else if ((ToolFace > 0.0) & (ToolFace <= 90.0) & (System.Math.Sign(num7) == -1))
							{
								num7 = Convert.ToDouble(decimal.Subtract(FirstSurvey.Azimuth, 180m)) + num7;
							}
							else if ((ToolFace > 90.0) & (ToolFace <= 180.0) & (System.Math.Sign(num7) == -1))
							{
								num7 = Convert.ToDouble(decimal.Add(FirstSurvey.Azimuth, 180m)) + num7;
							}
							else if ((ToolFace > 90.0) & (ToolFace <= 180.0) & (System.Math.Sign(num7) == 1))
							{
								num7 = Convert.ToDouble(FirstSurvey.Azimuth) + num7;
								if (num7 > 360.0)
								{
									num7 -= 360.0;
								}
							}
							else if ((ToolFace > 180.0) & (ToolFace <= 270.0) & (System.Math.Sign(num7) == -1))
							{
								num7 = Convert.ToDouble(FirstSurvey.Azimuth) + num7;
							}
							else if ((ToolFace > 180.0) & (ToolFace <= 270.0) & (System.Math.Sign(num7) == 1))
							{
								num7 = Convert.ToDouble(decimal.Subtract(FirstSurvey.Azimuth, 180m)) + num7;
							}
							else if ((ToolFace > 270.0) & (ToolFace <= 360.0) & (System.Math.Sign(num7) == -1))
							{
								num7 = Convert.ToDouble(FirstSurvey.Azimuth) + num7;
							}
							else if ((ToolFace > 270.0) & (ToolFace <= 360.0) & (num21 > 90.0))
							{
								num7 = Convert.ToDouble(decimal.Subtract(FirstSurvey.Azimuth, 180m)) + num7;
							}
							if (num7 > 360.0)
							{
								num7 -= 360.0;
							}
							double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num21));
							double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Cos(System.Math.PI / 180.0 * num7));
							double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Sin(System.Math.PI / 180.0 * num7));
							absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), new decimal(OverallDogLeg)));
							if ((num21 > 90.0) | (num21 < -90.0))
							{
								break;
							}
							num2++;
							num++;
							continue;
						}
						decimal limit5 = new decimal(absoluteSurveyList.Count() - 1);
						decimal num52 = 1m;
						while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num52, limit5, 1m))
						{
							absoluteSurveyList.RecordAt(Convert.ToInt32(num52)).DL = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num52)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num52, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num52)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num52, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num52)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num52, 1m))).Azimuth))));
							absoluteSurveyList.RecordAt(Convert.ToInt32(num52)).DL = new decimal(180.0 / System.Math.PI * System.Math.Acos(Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num52)).DL)) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num52)).MD, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num52, 1m))).MD)) * 30.0);
							num52 = decimal.Add(num52, 1m);
						}
						num23 = EndMD - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD);
						num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
						num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
						num24 = num23 / num12;
						value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num24;
						num10 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
						value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num10 * num24;
						num25 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Azimuth, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth));
						value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth) + num25 * num24;
						num14 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
						value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num14 * num24;
						num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
						value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num15 * num24;
						value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num13 * num24;
						num16 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).DL) * num24;
						absoluteSurveyList.LastRecord().MD = new decimal(System.Math.Round(value5, 2));
						absoluteSurveyList.LastRecord().Azimuth = new decimal(System.Math.Round(value7, 2));
						absoluteSurveyList.LastRecord().Dip = new decimal(System.Math.Round(value4, 2));
						absoluteSurveyList.LastRecord().TVD = new decimal(System.Math.Round(value6, 2));
						absoluteSurveyList.LastRecord().Northing = new decimal(System.Math.Round(value8, 2));
						absoluteSurveyList.LastRecord().Easting = new decimal(System.Math.Round(value9, 2));
						int num53 = absoluteSurveyList.Count() - 1;
						for (int num54 = 1; num54 <= num53; num54++)
						{
							absoluteSurveyList.RecordAt(num54).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(num54).Azimuth, 2);
							absoluteSurveyList.RecordAt(num54).Dip = System.Math.Round(absoluteSurveyList.RecordAt(num54).Dip, 2);
							absoluteSurveyList.RecordAt(num54).DL = System.Math.Round(absoluteSurveyList.RecordAt(num54).DL, 2);
							absoluteSurveyList.RecordAt(num54).Easting = System.Math.Round(absoluteSurveyList.RecordAt(num54).Easting, 2);
							absoluteSurveyList.RecordAt(num54).MD = System.Math.Round(absoluteSurveyList.RecordAt(num54).MD, 2);
							absoluteSurveyList.RecordAt(num54).Northing = System.Math.Round(absoluteSurveyList.RecordAt(num54).Northing, 2);
							absoluteSurveyList.RecordAt(num54).TVD = System.Math.Round(absoluteSurveyList.RecordAt(num54).TVD, 2);
						}
						if (interval > 1.0)
						{
							int num55 = (int)System.Math.Round((double)absoluteSurveyList.Count() - (interval - 1.0));
							int num56 = (int)System.Math.Round(interval);
							int num57 = num55;
							for (int num58 = 0; ((num56 >> 31) ^ num58) <= ((num56 >> 31) ^ num57); num58 += num56)
							{
								ResultList.Add(absoluteSurveyList.RecordAt(num58));
							}
							ResultList.Add(absoluteSurveyList.LastRecord());
						}
						else
						{
							ResultList = absoluteSurveyList;
						}
						ResultList.Remove(0);
						return ResultList;
					}
					MaxDip = true;
					goto IL_24cc;
				}
				while (num3 < EndMD)
				{
					num3++;
					double num5 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
					double num6 = num5 - Convert.ToDouble(FirstSurvey.Dip);
					double num21 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num6) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
					double num7 = ToolFace;
					double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num5));
					double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num5) * System.Math.Cos(System.Math.PI / 180.0 * num7));
					double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num5) * System.Math.Sin(System.Math.PI / 180.0 * num7));
					absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), new decimal(OverallDogLeg)));
					num2++;
					num++;
				}
			}
			decimal limit6 = new decimal(absoluteSurveyList.Count() - 1);
			decimal num59 = 1m;
			while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num59, limit6, 1m))
			{
				absoluteSurveyList.RecordAt(Convert.ToInt32(num59)).DL = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num59)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num59, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num59)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num59, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num59)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num59, 1m))).Azimuth))));
				absoluteSurveyList.RecordAt(Convert.ToInt32(num59)).DL = new decimal(180.0 / System.Math.PI * System.Math.Acos(Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num59)).DL)) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num59)).MD, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num59, 1m))).MD)) * 30.0);
				num59 = decimal.Add(num59, 1m);
			}
			num23 = EndMD - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD);
			num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
			num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
			num24 = num23 / num12;
			value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num24;
			num10 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
			value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num10 * num24;
			num25 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Azimuth, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth));
			value7 = ToolFace;
			num14 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
			value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num14 * num24;
			num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
			value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num15 * num24;
			value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num13 * num24;
			num16 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).DL) * num24;
			absoluteSurveyList.LastRecord().MD = new decimal(System.Math.Round(value5, 2));
			absoluteSurveyList.LastRecord().Azimuth = new decimal(System.Math.Round(value7, 2));
			absoluteSurveyList.LastRecord().Dip = new decimal(System.Math.Round(value4, 2));
			absoluteSurveyList.LastRecord().TVD = new decimal(System.Math.Round(value6, 2));
			absoluteSurveyList.LastRecord().Northing = new decimal(System.Math.Round(value8, 2));
			absoluteSurveyList.LastRecord().Easting = new decimal(System.Math.Round(value9, 2));
			int num60 = absoluteSurveyList.Count() - 1;
			for (int num61 = 1; num61 <= num60; num61++)
			{
				absoluteSurveyList.RecordAt(num61).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(num61).Azimuth, 2);
				absoluteSurveyList.RecordAt(num61).Dip = System.Math.Round(absoluteSurveyList.RecordAt(num61).Dip, 2);
				absoluteSurveyList.RecordAt(num61).DL = System.Math.Round(absoluteSurveyList.RecordAt(num61).DL, 2);
				absoluteSurveyList.RecordAt(num61).Easting = System.Math.Round(absoluteSurveyList.RecordAt(num61).Easting, 2);
				absoluteSurveyList.RecordAt(num61).MD = System.Math.Round(absoluteSurveyList.RecordAt(num61).MD, 2);
				absoluteSurveyList.RecordAt(num61).Northing = System.Math.Round(absoluteSurveyList.RecordAt(num61).Northing, 2);
				absoluteSurveyList.RecordAt(num61).TVD = System.Math.Round(absoluteSurveyList.RecordAt(num61).TVD, 2);
			}
			if (interval > 1.0)
			{
				int num62 = (int)System.Math.Round((double)absoluteSurveyList.Count() - (interval - 1.0));
				int num63 = (int)System.Math.Round(interval);
				int num64 = num62;
				for (int num65 = 0; ((num63 >> 31) ^ num65) <= ((num63 >> 31) ^ num64); num65 += num63)
				{
					ResultList.Add(absoluteSurveyList.RecordAt(num65));
				}
				ResultList.Add(absoluteSurveyList.LastRecord());
			}
			else
			{
				ResultList = absoluteSurveyList;
			}
			ResultList.Remove(0);
			return ResultList;
		}
		IL_24cc:
		checked
		{
			int num66 = absoluteSurveyList.Count() - 1;
			for (int num67 = 0; num67 <= num66; num67++)
			{
				if (System.Math.Sign(absoluteSurveyList.RecordAt(num67).Dip) == -1)
				{
					absoluteSurveyList.RecordAt(num67).Dip = decimal.Multiply(absoluteSurveyList.RecordAt(num67).Dip, -1m);
				}
			}
			decimal limit7 = new decimal(absoluteSurveyList.Count() - 1);
			decimal num68 = 1m;
			while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num68, limit7, 1m))
			{
				absoluteSurveyList.RecordAt(Convert.ToInt32(num68)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num68)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num68, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num68)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num68, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num68)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num68, 1m))).Azimuth)))));
				absoluteSurveyList.RecordAt(Convert.ToInt32(num68)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num68)).DL) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num68)).MD, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num68, 1m))).MD)) * 30.0);
				num68 = decimal.Add(num68, 1m);
			}
			double num9 = 89.99 - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip);
			double num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
			double num10 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
			double num11 = num9 / num10;
			double value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num11;
			double value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num10 * num11;
			double num25 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Azimuth, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth));
			double value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth) + num25 * num11;
			double num14 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
			if (System.Math.Sign(num14) == -1)
			{
				num14 *= -1.0;
			}
			double value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num14 * num11;
			double num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
			if (System.Math.Sign(num15) == -1)
			{
				num15 *= -1.0;
			}
			double value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num15 * num11;
			double num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
			if (System.Math.Sign(num13) == -1)
			{
				num13 *= -1.0;
			}
			double value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num13 * num11;
			double num16 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).DL) * num11;
			absoluteSurveyList.LastRecord().MD = new decimal(System.Math.Round(value5, 2));
			absoluteSurveyList.LastRecord().Azimuth = new decimal(System.Math.Round(value7, 2));
			absoluteSurveyList.LastRecord().Dip = new decimal(System.Math.Round(value4, 2));
			absoluteSurveyList.LastRecord().TVD = new decimal(System.Math.Round(value6, 2));
			absoluteSurveyList.LastRecord().Northing = new decimal(System.Math.Round(value8, 2));
			absoluteSurveyList.LastRecord().Easting = new decimal(System.Math.Round(value9, 2));
			int num69 = absoluteSurveyList.Count() - 1;
			for (int num70 = 1; num70 <= num69; num70++)
			{
				absoluteSurveyList.RecordAt(num70).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(num70).Azimuth, 2);
				absoluteSurveyList.RecordAt(num70).Dip = System.Math.Round(absoluteSurveyList.RecordAt(num70).Dip, 2);
				absoluteSurveyList.RecordAt(num70).DL = System.Math.Round(absoluteSurveyList.RecordAt(num70).DL, 2);
				absoluteSurveyList.RecordAt(num70).Easting = System.Math.Round(absoluteSurveyList.RecordAt(num70).Easting, 2);
				absoluteSurveyList.RecordAt(num70).MD = System.Math.Round(absoluteSurveyList.RecordAt(num70).MD, 2);
				absoluteSurveyList.RecordAt(num70).Northing = System.Math.Round(absoluteSurveyList.RecordAt(num70).Northing, 2);
				absoluteSurveyList.RecordAt(num70).TVD = System.Math.Round(absoluteSurveyList.RecordAt(num70).TVD, 2);
			}
			if (interval > 1.0)
			{
				int num71 = (int)System.Math.Round((double)absoluteSurveyList.Count() - (interval - 1.0));
				int num72 = (int)System.Math.Round(interval);
				int num73 = num71;
				for (int num74 = 0; ((num72 >> 31) ^ num74) <= ((num72 >> 31) ^ num73); num74 += num72)
				{
					ResultList.Add(absoluteSurveyList.RecordAt(num74));
				}
				ResultList.Add(absoluteSurveyList.LastRecord());
			}
			else
			{
				ResultList = absoluteSurveyList;
			}
			ResultList.Remove(0);
			return ResultList;
		}
	}

	public static Extrapolate_StraightPath_Errors ExtrapolateIntervalStraightPath()
	{
		Extrapolate_StraightPath_Errors result = default;
		return result;
	}

	public static AbsoluteSurveyList ExtrapolateSurveysTrendAnalysis(double OverallDogLeg, bool TVDCannotBeReached, Target Target, double TestIncDiffByMD, double TestAziDiffByMD, double interval, AbsoluteSurvey LastSurvey, AbsoluteSurveyList Results, double ToolFace, ref bool MaxDip)
	{
		int num = 1;
		object left = OverallDogLeg / 30.0;
		double num2 = 1.0;
		double num3 = Convert.ToDouble(LastSurvey.MD);
		AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
		AbsoluteSurveyList absoluteSurveyList2 = new AbsoluteSurveyList();
		absoluteSurveyList2.Add(new AbsoluteSurvey(LastSurvey.MD, LastSurvey.Dip, LastSurvey.Azimuth, LastSurvey.TVD, LastSurvey.Northing, LastSurvey.Easting, 0m));
		int num4 = 1;
		int num5 = 1;
		checked
		{
			double num6 = default;
			double num18;
			double num12;
			double value3;
			double num13;
			double value4;
			double num15;
			double value5;
			double num16;
			double value6;
			double num17;
			double value7;
			double value8;
			while (!(num6 > Convert.ToDouble(Target.TVD)))
			{
				num3++;
				double num7 = Conversions.ToDouble(Operators.AddObject(LastSurvey.Dip, Operators.MultiplyObject(left, num2)));
				double num8 = num7 - Convert.ToDouble(LastSurvey.Dip);
				double num9 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num8) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num8) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
				double d = System.Math.Tan(System.Math.PI / 180.0 * num8) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num8) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
				d = System.Math.Atan(d);
				d = 180.0 / System.Math.PI * d;
				if (d > 360.0)
				{
					d = FixRange(d, 0.0, 360.0);
				}
				if (ToolFace == 0.0)
				{
					d = Convert.ToDouble(LastSurvey.Azimuth) + d;
					if (d > 360.0)
					{
						d -= 360.0;
					}
				}
				else if ((ToolFace > 0.0) & (ToolFace <= 90.0) & (System.Math.Sign(d) == 1))
				{
					d = Convert.ToDouble(LastSurvey.Azimuth) + d;
					if (d > 360.0)
					{
						d -= 360.0;
					}
				}
				else if ((ToolFace > 0.0) & (ToolFace <= 90.0) & (System.Math.Sign(d) == -1))
				{
					d = Convert.ToDouble(decimal.Subtract(LastSurvey.Azimuth, 180m)) + d;
				}
				else if ((ToolFace > 90.0) & (ToolFace <= 180.0) & (System.Math.Sign(d) == -1))
				{
					d = Convert.ToDouble(decimal.Add(LastSurvey.Azimuth, 180m)) + d;
				}
				else if ((ToolFace > 90.0) & (ToolFace <= 180.0) & (System.Math.Sign(d) == 1))
				{
					d = Convert.ToDouble(LastSurvey.Azimuth) + d;
					if (d > 360.0)
					{
						d -= 360.0;
					}
				}
				else if ((ToolFace > 180.0) & (ToolFace <= 270.0) & (System.Math.Sign(d) == -1))
				{
					d = Convert.ToDouble(LastSurvey.Azimuth) + d;
				}
				else if ((ToolFace > 180.0) & (ToolFace <= 270.0) & (System.Math.Sign(d) == 1))
				{
					d = Convert.ToDouble(decimal.Subtract(LastSurvey.Azimuth, 180m)) + d;
				}
				else if ((ToolFace > 270.0) & (ToolFace <= 360.0) & (System.Math.Sign(d) == -1))
				{
					d = Convert.ToDouble(LastSurvey.Azimuth) + d;
				}
				else if ((ToolFace > 270.0) & (ToolFace <= 360.0) & (num9 > 90.0))
				{
					d = Convert.ToDouble(decimal.Subtract(LastSurvey.Azimuth, 180m)) + d;
				}
				if (d > 360.0)
				{
					d -= 360.0;
				}
				num6 = Convert.ToDouble(absoluteSurveyList2.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList2.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num9));
				double value = Convert.ToDouble(absoluteSurveyList2.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList2.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList2.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num9) * System.Math.Cos(System.Math.PI / 180.0 * d));
				double value2 = Convert.ToDouble(absoluteSurveyList2.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList2.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList2.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num9) * System.Math.Sin(System.Math.PI / 180.0 * d));
				absoluteSurveyList2.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num9), new decimal(d), new decimal(num6), new decimal(value), new decimal(value2), 0m));
				if ((num9 > 90.0) | (num9 < -90.0))
				{
					MaxDip = true;
					decimal limit = new decimal(absoluteSurveyList2.Count() - 1);
					decimal num10 = 1m;
					while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num10, limit, 1m))
					{
						absoluteSurveyList2.RecordAt(Convert.ToInt32(num10)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList2.RecordAt(Convert.ToInt32(num10)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList2.RecordAt(Convert.ToInt32(decimal.Subtract(num10, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList2.RecordAt(Convert.ToInt32(num10)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList2.RecordAt(Convert.ToInt32(decimal.Subtract(num10, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(Convert.ToInt32(num10)).Azimuth, absoluteSurveyList2.RecordAt(Convert.ToInt32(decimal.Subtract(num10, 1m))).Azimuth)))));
						absoluteSurveyList2.RecordAt(Convert.ToInt32(num10)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(absoluteSurveyList2.RecordAt(Convert.ToInt32(num10)).DL) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(Convert.ToInt32(num10)).MD, absoluteSurveyList2.RecordAt(Convert.ToInt32(decimal.Subtract(num10, 1m))).MD)) * 30.0);
						num10 = decimal.Add(num10, 1m);
					}
					double num11 = 89.99 - Convert.ToDouble(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Dip);
					num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 1).MD, absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).MD));
					num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 1).Dip, absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Dip));
					double num14 = num11 / num13;
					value3 = Convert.ToDouble(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).MD) + num14;
					value4 = Convert.ToDouble(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Dip) + num13 * num14;
					num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 1).Azimuth, absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Azimuth));
					value5 = Convert.ToDouble(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Azimuth) + num15 * num14;
					num16 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 1).Northing, absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Northing));
					if (System.Math.Sign(num16) == -1)
					{
						num16 *= -1.0;
					}
					value6 = Convert.ToDouble(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Northing) + num16 * num14;
					num17 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 1).Easting, absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Easting));
					if (System.Math.Sign(num17) == -1)
					{
						num17 *= -1.0;
					}
					value7 = Convert.ToDouble(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Easting) + num17 * num14;
					num18 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 1).TVD, absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).TVD));
					if (System.Math.Sign(num18) == -1)
					{
						num18 *= -1.0;
					}
					value8 = Convert.ToDouble(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).TVD) + num18 * num14;
					absoluteSurveyList2.LastRecord().MD = new decimal(value3);
					absoluteSurveyList2.LastRecord().Azimuth = new decimal(value5);
					absoluteSurveyList2.LastRecord().Dip = new decimal(value4);
					absoluteSurveyList2.LastRecord().TVD = new decimal(value8);
					absoluteSurveyList2.LastRecord().Northing = new decimal(value6);
					absoluteSurveyList2.LastRecord().Easting = new decimal(value7);
					if (interval > Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.LastRecord().MD, LastSurvey.MD)))
					{
						interval = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.LastRecord().MD, LastSurvey.MD));
					}
					if (interval > 1.0)
					{
						int num19 = (int)System.Math.Round((double)absoluteSurveyList2.Count() - interval);
						int num20 = (int)System.Math.Round(interval);
						int num21 = num19;
						for (int i = 0; ((num20 >> 31) ^ i) <= ((num20 >> 31) ^ num21); i += num20)
						{
							absoluteSurveyList.Add(absoluteSurveyList2.RecordAt(i));
						}
						absoluteSurveyList.Add(absoluteSurveyList2.LastRecord());
					}
					else
					{
						absoluteSurveyList = absoluteSurveyList2;
					}
					int num22 = absoluteSurveyList.Count() - 1;
					for (int j = 1; j <= num22; j++)
					{
						absoluteSurveyList.RecordAt(j).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(j).Azimuth, 2);
						absoluteSurveyList.RecordAt(j).Dip = System.Math.Round(absoluteSurveyList.RecordAt(j).Dip, 2);
						absoluteSurveyList.RecordAt(j).DL = System.Math.Round(absoluteSurveyList.RecordAt(j).DL, 2);
						absoluteSurveyList.RecordAt(j).Easting = System.Math.Round(absoluteSurveyList.RecordAt(j).Easting, 2);
						absoluteSurveyList.RecordAt(j).MD = System.Math.Round(absoluteSurveyList.RecordAt(j).MD, 2);
						absoluteSurveyList.RecordAt(j).Northing = System.Math.Round(absoluteSurveyList.RecordAt(j).Northing, 2);
						absoluteSurveyList.RecordAt(j).TVD = System.Math.Round(absoluteSurveyList.RecordAt(j).TVD, 2);
					}
					int num23 = absoluteSurveyList.Count() - 1;
					for (int k = 0; k <= num23; k++)
					{
						if (decimal.Compare(absoluteSurveyList.RecordAt(k).Azimuth, 360m) > 0)
						{
							absoluteSurveyList.RecordAt(k).Azimuth = decimal.Subtract(absoluteSurveyList.RecordAt(k).Azimuth, 360m);
						}
					}
					absoluteSurveyList.Remove(0);
					return absoluteSurveyList;
				}
				num2++;
				num++;
			}
			decimal limit2 = new decimal(absoluteSurveyList2.Count() - 1);
			decimal num24 = 1m;
			while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num24, limit2, 1m))
			{
				absoluteSurveyList2.RecordAt(Convert.ToInt32(num24)).DL = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList2.RecordAt(Convert.ToInt32(num24)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList2.RecordAt(Convert.ToInt32(decimal.Subtract(num24, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList2.RecordAt(Convert.ToInt32(num24)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList2.RecordAt(Convert.ToInt32(decimal.Subtract(num24, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(Convert.ToInt32(num24)).Azimuth, absoluteSurveyList2.RecordAt(Convert.ToInt32(decimal.Subtract(num24, 1m))).Azimuth))));
				absoluteSurveyList2.RecordAt(Convert.ToInt32(num24)).DL = new decimal(180.0 / System.Math.PI * System.Math.Acos(Convert.ToDouble(absoluteSurveyList2.RecordAt(Convert.ToInt32(num24)).DL)) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(Convert.ToInt32(num24)).MD, absoluteSurveyList2.RecordAt(Convert.ToInt32(decimal.Subtract(num24, 1m))).MD)) * 30.0);
				num24 = decimal.Add(num24, 1m);
			}
			double num25 = Convert.ToDouble(decimal.Subtract(Target.TVD, absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).TVD));
			num18 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 1).TVD, absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).TVD));
			num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 1).MD, absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).MD));
			double num26 = num25 / num18;
			value3 = Convert.ToDouble(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).MD) + num26;
			num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 1).Dip, absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Dip));
			value4 = Convert.ToDouble(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Dip) + num13 * num26;
			num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 1).Azimuth, absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Azimuth));
			value5 = Convert.ToDouble(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Azimuth) + num15 * num26;
			num16 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 1).Northing, absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Northing));
			value6 = Convert.ToDouble(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Northing) + num16 * num26;
			num17 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 1).Easting, absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Easting));
			value7 = Convert.ToDouble(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).Easting) + num17 * num26;
			value8 = Convert.ToDouble(absoluteSurveyList2.RecordAt(absoluteSurveyList2.Count() - 2).TVD) + num18 * num26;
			absoluteSurveyList2.LastRecord().MD = new decimal(value3);
			absoluteSurveyList2.LastRecord().Azimuth = new decimal(value5);
			absoluteSurveyList2.LastRecord().Dip = new decimal(value4);
			absoluteSurveyList2.LastRecord().TVD = new decimal(value8);
			absoluteSurveyList2.LastRecord().Northing = new decimal(value6);
			absoluteSurveyList2.LastRecord().Easting = new decimal(value7);
			int num27 = absoluteSurveyList2.Count() - 1;
			for (int l = 1; l <= num27; l++)
			{
				absoluteSurveyList2.RecordAt(l).Azimuth = System.Math.Round(absoluteSurveyList2.RecordAt(l).Azimuth, 2);
				absoluteSurveyList2.RecordAt(l).Dip = System.Math.Round(absoluteSurveyList2.RecordAt(l).Dip, 2);
				absoluteSurveyList2.RecordAt(l).DL = System.Math.Round(absoluteSurveyList2.RecordAt(l).DL, 2);
				absoluteSurveyList2.RecordAt(l).Easting = System.Math.Round(absoluteSurveyList2.RecordAt(l).Easting, 2);
				absoluteSurveyList2.RecordAt(l).MD = System.Math.Round(absoluteSurveyList2.RecordAt(l).MD, 2);
				absoluteSurveyList2.RecordAt(l).Northing = System.Math.Round(absoluteSurveyList2.RecordAt(l).Northing, 2);
				absoluteSurveyList2.RecordAt(l).TVD = System.Math.Round(absoluteSurveyList2.RecordAt(l).TVD, 2);
			}
			if (interval > Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.LastRecord().MD, LastSurvey.MD)))
			{
				interval = Convert.ToDouble(decimal.Subtract(absoluteSurveyList2.LastRecord().MD, LastSurvey.MD));
			}
			if (interval > 1.0)
			{
				int num28 = (int)System.Math.Round((double)absoluteSurveyList2.Count() - (interval - 1.0));
				int num29 = (int)System.Math.Round(interval);
				int num30 = num28;
				for (int m = 0; ((num29 >> 31) ^ m) <= ((num29 >> 31) ^ num30); m += num29)
				{
					absoluteSurveyList.Add(absoluteSurveyList2.RecordAt(m));
				}
				absoluteSurveyList.Add(absoluteSurveyList2.LastRecord());
			}
			else
			{
				absoluteSurveyList = absoluteSurveyList2;
			}
			absoluteSurveyList.Remove(0);
			return absoluteSurveyList;
		}
	}

	public static AbsoluteSurveyList ExtrapolateIntervalSurveysInc(double EndInc, double ToolFace, double OverallDogLeg, double interval, AbsoluteSurvey FirstSurvey, ref AbsoluteSurveyList ResultList, ref bool MaxMD)
	{
		AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
		absoluteSurveyList.Add(FirstSurvey);
		int num = 1;
		object left = OverallDogLeg / 30.0;
		double num2 = 1.0;
		double num3 = Convert.ToDouble(FirstSurvey.MD);
		object[,] array = new object[1, 7];
		object[,] array2 = new object[1, 7];
		checked
		{
			double num9;
			double num10;
			double num11;
			double value4;
			double num12;
			double value5;
			double num13;
			double value6;
			double value7;
			double num14;
			double value8;
			double num15;
			double value9;
			double num16;
			if (EndInc == 0.0)
			{
				ToolFace = 180.0;
				double num4 = default;
				while (true)
				{
					if (!(num4 < 0.0))
					{
						num3++;
						num4 = Conversions.ToDouble(Operators.SubtractObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
						double num5 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
						double num6 = num5 - Convert.ToDouble(FirstSurvey.Dip);
						double num7 = Convert.ToDouble(FirstSurvey.Azimuth) + 180.0 / System.Math.PI * System.Math.Atan(System.Math.Tan(System.Math.PI / 180.0 * num4) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num4) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace)));
						double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num4));
						double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num4) * System.Math.Cos(System.Math.PI / 180.0 * num7));
						double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num4) * System.Math.Sin(System.Math.PI / 180.0 * num7));
						absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num4), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), 0m));
						if (num3 > 24000.0)
						{
							break;
						}
						num2++;
						num++;
						continue;
					}
					absoluteSurveyList.LastRecord().Azimuth = 0m;
					decimal limit = new decimal(absoluteSurveyList.Count() - 1);
					decimal num8 = 1m;
					while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num8, limit, 1m))
					{
						absoluteSurveyList.RecordAt(Convert.ToInt32(num8)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num8)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num8, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num8)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num8, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num8)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num8, 1m))).Azimuth)))));
						absoluteSurveyList.RecordAt(Convert.ToInt32(num8)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num8)).DL) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num8)).MD, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num8, 1m))).MD)) * 30.0);
						num8 = decimal.Add(num8, 1m);
					}
					num9 = EndInc - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip);
					num10 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
					num11 = num9 / num10;
					value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num10 * num11;
					num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
					value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num11;
					num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
					value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num13 * num11;
					value7 = 0.0;
					num14 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
					value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num14 * num11;
					num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
					value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num15 * num11;
					num16 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).DL) * num11;
					absoluteSurveyList.LastRecord().MD = new decimal(value5);
					absoluteSurveyList.LastRecord().Azimuth = new decimal(value7);
					absoluteSurveyList.LastRecord().Dip = new decimal(value4);
					absoluteSurveyList.LastRecord().TVD = new decimal(value6);
					absoluteSurveyList.LastRecord().Northing = new decimal(value8);
					absoluteSurveyList.LastRecord().Easting = new decimal(value9);
					if (decimal.Compare(absoluteSurveyList.LastRecord().MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) == 0)
					{
						absoluteSurveyList.Remove(absoluteSurveyList.Count() - 2);
					}
					if (interval > Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD)))
					{
						interval = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD));
					}
					if (interval > 1.0)
					{
						int num17 = (int)System.Math.Round((double)absoluteSurveyList.Count() - interval);
						int num18 = (int)System.Math.Round(interval);
						int num19 = num17;
						for (int i = 0; ((num18 >> 31) ^ i) <= ((num18 >> 31) ^ num19); i += num18)
						{
							ResultList.Add(absoluteSurveyList.RecordAt(i));
						}
						ResultList.Add(absoluteSurveyList.LastRecord());
					}
					else
					{
						ResultList = absoluteSurveyList;
					}
					int num20 = ResultList.Count() - 1;
					for (int j = 1; j <= num20; j++)
					{
						ResultList.RecordAt(j).Azimuth = System.Math.Round(ResultList.RecordAt(j).Azimuth, 2);
						ResultList.RecordAt(j).Dip = System.Math.Round(ResultList.RecordAt(j).Dip, 2);
						ResultList.RecordAt(j).DL = System.Math.Round(ResultList.RecordAt(j).DL, 2);
						ResultList.RecordAt(j).Easting = System.Math.Round(ResultList.RecordAt(j).Easting, 2);
						ResultList.RecordAt(j).MD = System.Math.Round(ResultList.RecordAt(j).MD, 2);
						ResultList.RecordAt(j).Northing = System.Math.Round(ResultList.RecordAt(j).Northing, 2);
						ResultList.RecordAt(j).TVD = System.Math.Round(ResultList.RecordAt(j).TVD, 2);
					}
					ResultList.Remove(0);
					return ResultList;
				}
				MaxMD = true;
				goto IL_2895;
			}
			double num21 = default;
			if (decimal.Compare(FirstSurvey.Dip, 0m) == 0)
			{
				while (num21 < EndInc)
				{
					num3++;
					double num5 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
					double num6 = num5 - Convert.ToDouble(FirstSurvey.Dip);
					num21 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num6) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
					double num7 = ToolFace;
					double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num21));
					double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Cos(System.Math.PI / 180.0 * num7));
					double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Sin(System.Math.PI / 180.0 * num7));
					absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), 0m));
					double num22 = System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip));
					double num23 = System.Math.Cos(System.Math.PI / 180.0 * num21);
					double num24 = num23 - num22;
					if (!(num3 > 24000.0))
					{
						num2++;
						num++;
						continue;
					}
					goto IL_064c;
				}
			}
			else if (EndInc > Convert.ToDouble(FirstSurvey.Dip))
			{
				if (ToolFace == 0.0)
				{
					double num25 = 180.0 / System.Math.PI * (1.0 / (OverallDogLeg / 30.0));
					double num6 = EndInc - Convert.ToDouble(FirstSurvey.Dip);
					double num26 = System.Math.PI / 180.0 * num6 * num25;
					double num27;
					double num7;
					double value;
					double value2;
					double value3;
					while (num3 < num26 + Convert.ToDouble(FirstSurvey.MD) - 1.0)
					{
						num3++;
						num27 = OverallDogLeg / 30.0 * num2;
						num21 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num27) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num27) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
						num7 = Convert.ToDouble(FirstSurvey.Azimuth) + 180.0 / System.Math.PI * System.Math.Atan(System.Math.Tan(System.Math.PI / 180.0 * num27) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num27) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace)));
						value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num21));
						value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Cos(System.Math.PI / 180.0 * num7));
						value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Sin(System.Math.PI / 180.0 * num7));
						absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), 0m));
						if (!(num3 > 24000.0))
						{
							num2++;
							num++;
							continue;
						}
						goto IL_09cf;
					}
					num3 = num26 + Convert.ToDouble(FirstSurvey.MD);
					num27 = OverallDogLeg / 30.0 * num26;
					num21 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num27) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num27) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
					num7 = Convert.ToDouble(FirstSurvey.Azimuth) + 180.0 / System.Math.PI * System.Math.Atan(System.Math.Tan(System.Math.PI / 180.0 * num27) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num27) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace)));
					value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + (num3 - Convert.ToDouble(absoluteSurveyList.LastRecord().MD)) / 2.0 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num21));
					value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + (num3 - Convert.ToDouble(absoluteSurveyList.LastRecord().MD)) / 2.0 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Cos(System.Math.PI / 180.0 * num7));
					value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + (num3 - Convert.ToDouble(absoluteSurveyList.LastRecord().MD)) / 2.0 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Sin(System.Math.PI / 180.0 * num7));
					absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), 0m));
					if (num3 > 24000.0)
					{
						MaxMD = true;
						goto IL_2895;
					}
					goto IL_2edb;
				}
				while (num21 < EndInc)
				{
					num3++;
					double num5 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
					double num6 = num5 - Convert.ToDouble(FirstSurvey.Dip);
					num21 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num6) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
					double num7 = Convert.ToDouble(FirstSurvey.Azimuth) + 180.0 / System.Math.PI * System.Math.Atan(System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace)));
					double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num21));
					double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Cos(System.Math.PI / 180.0 * num7));
					double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Sin(System.Math.PI / 180.0 * num7));
					absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), 0m));
					double num22 = System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip));
					double num23 = System.Math.Cos(System.Math.PI / 180.0 * num21);
					double num24 = num23 - num22;
					if (System.Math.Sign(num24) == 1)
					{
						throw new Exception("Invalid tool face");
					}
					if (num21 < Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip))
					{
						throw new Exception("Invalid tool face");
					}
					if (!(num3 > 24000.0))
					{
						num2++;
						num++;
						continue;
					}
					goto IL_10d8;
				}
			}
			else if (EndInc < Convert.ToDouble(FirstSurvey.Dip))
			{
				double num6;
				if (ToolFace == 180.0)
				{
					double num28 = 180.0 / System.Math.PI * (1.0 / (OverallDogLeg / 30.0));
					num6 = (EndInc - Convert.ToDouble(FirstSurvey.Dip)) * -1.0;
					double num29 = System.Math.PI / 180.0 * num6 * num28;
					double num30;
					double num7;
					double value;
					double value2;
					double value3;
					while (num3 < num29 + Convert.ToDouble(FirstSurvey.MD) - 1.0)
					{
						num3++;
						num30 = OverallDogLeg / 30.0 * num2;
						num21 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num30) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num30) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
						num7 = Convert.ToDouble(FirstSurvey.Azimuth) + 180.0 / System.Math.PI * System.Math.Atan(System.Math.Tan(System.Math.PI / 180.0 * num30) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num30) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace)));
						value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num21));
						value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Cos(System.Math.PI / 180.0 * num7));
						value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Sin(System.Math.PI / 180.0 * num7));
						absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), 0m));
						num2++;
						num++;
					}
					num3 = num29 + Convert.ToDouble(FirstSurvey.MD);
					num30 = OverallDogLeg / 30.0 * num29;
					num21 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num30) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num30) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
					num7 = Convert.ToDouble(FirstSurvey.Azimuth) + 180.0 / System.Math.PI * System.Math.Atan(System.Math.Tan(System.Math.PI / 180.0 * num30) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num30) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace)));
					value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + (num3 - Convert.ToDouble(absoluteSurveyList.LastRecord().MD)) / 2.0 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num21));
					value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + (num3 - Convert.ToDouble(absoluteSurveyList.LastRecord().MD)) / 2.0 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Cos(System.Math.PI / 180.0 * num7));
					value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + (num3 - Convert.ToDouble(absoluteSurveyList.LastRecord().MD)) / 2.0 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Sin(System.Math.PI / 180.0 * num7));
					absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), 0m));
					goto IL_2edb;
				}
				num21 = Convert.ToDouble(FirstSurvey.Dip);
				num6 = Conversions.ToDouble(Operators.SubtractObject(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)), FirstSurvey.Dip));
				while (num21 > EndInc)
				{
					double num31 = Convert.ToDouble(FirstSurvey.TVD);
					double num32 = Convert.ToDouble(FirstSurvey.Northing);
					double num33 = Convert.ToDouble(FirstSurvey.Easting);
					num3++;
					double num5 = Conversions.ToDouble(Operators.AddObject(FirstSurvey.Dip, Operators.MultiplyObject(left, num2)));
					num6 = num5 - Convert.ToDouble(FirstSurvey.Dip);
					num21 = 180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * num6) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace));
					double num7 = Convert.ToDouble(FirstSurvey.Azimuth) + 180.0 / System.Math.PI * System.Math.Atan(System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Sin(System.Math.PI / 180.0 * ToolFace) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) + System.Math.Tan(System.Math.PI / 180.0 * num6) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * ToolFace)));
					double value = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).TVD) + 0.5 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num21));
					double value2 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Northing) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Cos(System.Math.PI / 180.0 * num7));
					double value3 = Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Easting) + 0.5 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num21) * System.Math.Sin(System.Math.PI / 180.0 * num7));
					absoluteSurveyList.Add(new AbsoluteSurvey(new decimal(num3), new decimal(num21), new decimal(num7), new decimal(value), new decimal(value2), new decimal(value3), 0m));
					double num22 = System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(FirstSurvey.Dip));
					double num23 = System.Math.Cos(System.Math.PI / 180.0 * num21);
					double num24 = num23 - num22;
					if (System.Math.Sign(num24) == -1)
					{
						throw new Exception("Invalid tool face");
					}
					if (!(num3 > 24000.0))
					{
						num2++;
						num++;
						continue;
					}
					goto IL_1b8b;
				}
			}
			decimal limit2 = new decimal(absoluteSurveyList.Count() - 1);
			decimal num34 = 1m;
			while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num34, limit2, 1m))
			{
				absoluteSurveyList.RecordAt(Convert.ToInt32(num34)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num34)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num34, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num34)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num34, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num34)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num34, 1m))).Azimuth)))));
				absoluteSurveyList.RecordAt(Convert.ToInt32(num34)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num34)).DL) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num34)).MD, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num34, 1m))).MD)) * 30.0);
				num34 = decimal.Add(num34, 1m);
			}
			num9 = EndInc - Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip);
			num10 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
			num11 = num9 / num10;
			value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num10 * num11;
			num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
			value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num11;
			num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
			value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num13 * num11;
			double num35 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Azimuth, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth));
			value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth) + num35 * num11;
			num14 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
			value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num14 * num11;
			num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
			value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num15 * num11;
			num16 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).DL) * num11;
			absoluteSurveyList.LastRecord().MD = new decimal(value5);
			absoluteSurveyList.LastRecord().Azimuth = new decimal(value7);
			absoluteSurveyList.LastRecord().Dip = new decimal(value4);
			absoluteSurveyList.LastRecord().TVD = new decimal(value6);
			absoluteSurveyList.LastRecord().Northing = new decimal(value8);
			absoluteSurveyList.LastRecord().Easting = new decimal(value9);
			if (interval > Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD)))
			{
				interval = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD));
			}
			if (interval > 1.0)
			{
				int num36 = (int)System.Math.Round((double)absoluteSurveyList.Count() - interval);
				int num37 = (int)System.Math.Round(interval);
				int num38 = num36;
				for (int k = 0; ((num37 >> 31) ^ k) <= ((num37 >> 31) ^ num38); k += num37)
				{
					ResultList.Add(absoluteSurveyList.RecordAt(k));
				}
				ResultList.Add(absoluteSurveyList.LastRecord());
			}
			else
			{
				ResultList = absoluteSurveyList;
			}
			int num39 = ResultList.Count() - 1;
			for (int l = 1; l <= num39; l++)
			{
				ResultList.RecordAt(l).Azimuth = System.Math.Round(ResultList.RecordAt(l).Azimuth, 2);
				ResultList.RecordAt(l).Dip = System.Math.Round(ResultList.RecordAt(l).Dip, 2);
				ResultList.RecordAt(l).DL = System.Math.Round(ResultList.RecordAt(l).DL, 2);
				ResultList.RecordAt(l).Easting = System.Math.Round(ResultList.RecordAt(l).Easting, 2);
				ResultList.RecordAt(l).MD = System.Math.Round(ResultList.RecordAt(l).MD, 2);
				ResultList.RecordAt(l).Northing = System.Math.Round(ResultList.RecordAt(l).Northing, 2);
				ResultList.RecordAt(l).TVD = System.Math.Round(ResultList.RecordAt(l).TVD, 2);
			}
			int num40 = ResultList.Count() - 1;
			for (int m = 0; m <= num40; m++)
			{
				if (decimal.Compare(ResultList.RecordAt(m).Azimuth, 360m) > 0)
				{
					ResultList.RecordAt(m).Azimuth = decimal.Subtract(ResultList.RecordAt(m).Azimuth, 360m);
				}
			}
			ResultList.Remove(0);
			return ResultList;
		}
		IL_2895:
		checked
		{
			decimal limit3 = new decimal(absoluteSurveyList.Count() - 1);
			decimal num41 = 1m;
			while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num41, limit3, 1m))
			{
				absoluteSurveyList.RecordAt(Convert.ToInt32(num41)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num41)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num41, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num41)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num41, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num41)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num41, 1m))).Azimuth)))));
				absoluteSurveyList.RecordAt(Convert.ToInt32(num41)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num41)).DL) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num41)).MD, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num41, 1m))).MD)) * 30.0);
				num41 = decimal.Add(num41, 1m);
			}
			double num42 = Convert.ToDouble(decimal.Subtract(24000m, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
			double num13 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).TVD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD));
			double num12 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).MD, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD));
			double num43 = num42 / num12;
			double value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).MD) + num43;
			double num10 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Dip, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip));
			double value4 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Dip) + num10 * num43;
			double num35 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Azimuth, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth));
			double value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Azimuth) + num35 * num43;
			double num14 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Northing, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing));
			double value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Northing) + num14 * num43;
			double num15 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 1).Easting, absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting));
			double value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).Easting) + num15 * num43;
			double value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(absoluteSurveyList.Count() - 2).TVD) + num13 * num43;
			absoluteSurveyList.LastRecord().MD = new decimal(System.Math.Round(value5, 2));
			absoluteSurveyList.LastRecord().Azimuth = new decimal(System.Math.Round(value7, 2));
			absoluteSurveyList.LastRecord().Dip = new decimal(System.Math.Round(value4, 2));
			absoluteSurveyList.LastRecord().TVD = new decimal(System.Math.Round(value6, 2));
			absoluteSurveyList.LastRecord().Northing = new decimal(System.Math.Round(value8, 2));
			absoluteSurveyList.LastRecord().Easting = new decimal(System.Math.Round(value9, 2));
			int num44 = absoluteSurveyList.Count() - 1;
			for (int n = 1; n <= num44; n++)
			{
				absoluteSurveyList.RecordAt(n).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(n).Azimuth, 2);
				absoluteSurveyList.RecordAt(n).Dip = System.Math.Round(absoluteSurveyList.RecordAt(n).Dip, 2);
				absoluteSurveyList.RecordAt(n).DL = System.Math.Round(absoluteSurveyList.RecordAt(n).DL, 2);
				absoluteSurveyList.RecordAt(n).Easting = System.Math.Round(absoluteSurveyList.RecordAt(n).Easting, 2);
				absoluteSurveyList.RecordAt(n).MD = System.Math.Round(absoluteSurveyList.RecordAt(n).MD, 2);
				absoluteSurveyList.RecordAt(n).Northing = System.Math.Round(absoluteSurveyList.RecordAt(n).Northing, 2);
				absoluteSurveyList.RecordAt(n).TVD = System.Math.Round(absoluteSurveyList.RecordAt(n).TVD, 2);
			}
			if (interval > 1.0)
			{
				int num45 = (int)System.Math.Round((double)absoluteSurveyList.Count() - (interval - 1.0));
				int num46 = (int)System.Math.Round(interval);
				int num47 = num45;
				for (int num48 = 0; ((num46 >> 31) ^ num48) <= ((num46 >> 31) ^ num47); num48 += num46)
				{
					ResultList.Add(absoluteSurveyList.RecordAt(num48));
				}
				ResultList.Add(absoluteSurveyList.LastRecord());
			}
			else
			{
				ResultList = absoluteSurveyList;
			}
			ResultList.Remove(0);
			return ResultList;
		}
		IL_10d8:
		MaxMD = true;
		goto IL_2895;
		IL_1b8b:
		MaxMD = true;
		goto IL_2895;
		IL_064c:
		MaxMD = true;
		goto IL_2895;
		IL_09cf:
		MaxMD = true;
		goto IL_2895;
		IL_2edb:
		checked
		{
			decimal limit4 = new decimal(absoluteSurveyList.Count() - 1);
			decimal num49 = 1m;
			while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num49, limit4, 1m))
			{
				absoluteSurveyList.RecordAt(Convert.ToInt32(num49)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num49)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num49, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num49)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num49, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num49)).Azimuth, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num49, 1m))).Azimuth)))));
				absoluteSurveyList.RecordAt(Convert.ToInt32(num49)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(absoluteSurveyList.RecordAt(Convert.ToInt32(num49)).DL) / Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(Convert.ToInt32(num49)).MD, absoluteSurveyList.RecordAt(Convert.ToInt32(decimal.Subtract(num49, 1m))).MD)) * 30.0);
				num49 = decimal.Add(num49, 1m);
			}
			if (interval > Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD)))
			{
				interval = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.LastRecord().MD, FirstSurvey.MD));
			}
			if (interval > 1.0)
			{
				int num50 = (int)System.Math.Round((double)absoluteSurveyList.Count() - interval);
				int num51 = (int)System.Math.Round(interval);
				int num52 = num50;
				for (int num53 = 0; ((num51 >> 31) ^ num53) <= ((num51 >> 31) ^ num52); num53 += num51)
				{
					ResultList.Add(absoluteSurveyList.RecordAt(num53));
				}
				ResultList.Add(absoluteSurveyList.LastRecord());
			}
			else
			{
				ResultList = absoluteSurveyList;
			}
			int num54 = ResultList.Count() - 1;
			for (int num55 = 1; num55 <= num54; num55++)
			{
				ResultList.RecordAt(num55).Azimuth = System.Math.Round(ResultList.RecordAt(num55).Azimuth, 2);
				ResultList.RecordAt(num55).Dip = System.Math.Round(ResultList.RecordAt(num55).Dip, 2);
				ResultList.RecordAt(num55).DL = System.Math.Round(ResultList.RecordAt(num55).DL, 2);
				ResultList.RecordAt(num55).Easting = System.Math.Round(ResultList.RecordAt(num55).Easting, 2);
				ResultList.RecordAt(num55).MD = System.Math.Round(ResultList.RecordAt(num55).MD, 2);
				ResultList.RecordAt(num55).Northing = System.Math.Round(ResultList.RecordAt(num55).Northing, 2);
				ResultList.RecordAt(num55).TVD = System.Math.Round(ResultList.RecordAt(num55).TVD, 2);
			}
			ResultList.Remove(0);
			return ResultList;
		}
	}

	public static Array AnyTriangle(AbsoluteSurvey LastSurvey, double SideC, double SideB, double AngleB, double Dogleg, double Azimuth, double Toolface, ref Array Array)
	{
		double num = SideB * System.Math.Sin(System.Math.PI / 180.0 * AngleB) / SideC;
		if (num > 1.0)
		{
			NewLateBinding.LateIndexSet(Array, new object[3] { 0, 0, 0 }, null);
			NewLateBinding.LateIndexSet(Array, new object[3] { 0, 1, 0 }, null);
			return Array;
		}
		if (Toolface == 180.0)
		{
			NewLateBinding.LateIndexSet(Array, new object[3]
			{
				0,
				0,
				(Convert.ToDouble(LastSurvey.Dip) - SideB) / (Dogleg / 30.0)
			}, null);
			NewLateBinding.LateIndexSet(Array, new object[3] { 0, 1, Azimuth }, null);
			return Array;
		}
		if ((Toolface < 270.0) & (Toolface > 90.0))
		{
			num = System.Math.Asin(num);
			num = 180.0 - 180.0 / System.Math.PI * num;
		}
		else if ((Toolface < 90.0) | (Toolface > 270.0))
		{
			num = System.Math.Asin(num);
			num = 180.0 / System.Math.PI * num;
		}
		double num2 = 180.0 - num - AngleB;
		double num3 = System.Math.Sqrt(System.Math.Pow(SideB, 2.0) + System.Math.Pow(SideC, 2.0) - 2.0 * SideB * SideC * System.Math.Cos(System.Math.PI / 180.0 * num2));
		NewLateBinding.LateIndexSet(Array, new object[3]
		{
			0,
			0,
			num3 / (Dogleg / 30.0)
		}, null);
		NewLateBinding.LateIndexSet(Array, new object[3]
		{
			0,
			1,
			Azimuth + AngleB
		}, null);
		return Array;
	}
}
