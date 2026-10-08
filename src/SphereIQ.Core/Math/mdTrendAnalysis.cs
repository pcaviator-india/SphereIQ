using System;
using Microsoft.VisualBasic.CompilerServices;

namespace EzyPlanMath;

[StandardModule]
internal sealed class mdTrendAnalysis
{
	public static decimal[,] FiveMeterTrend = new decimal[1, 3];

	public static decimal[,] Trend = new decimal[2, 6];

	public static decimal B2;

	public static decimal DogLeg2;

	public static decimal LSMD;

	public static decimal LSInc;

	public static decimal LSAzi;

	public static decimal LSTVD;

	public static decimal LSNth;

	public static decimal LSEst;

	public static decimal BuildMD;

	public static decimal BuildInc;

	public static decimal BuildAzi;

	public static decimal BuildTVD;

	public static decimal BuildNth;

	public static decimal BuildEst;

	public static decimal BuildTF;

	public static decimal TgtTVD;

	public static decimal TgtNth;

	public static decimal TgtEst;

	public static decimal ArcLen;

	public static decimal LSMD1;

	public static decimal LSInc1;

	public static decimal LSAzi1;

	public static decimal LSTVD1;

	public static decimal LSNth1;

	public static decimal LSEst1;

	public static decimal TrendTVD(decimal Zn, decimal Z)
	{
		return decimal.Subtract(Zn, Z);
	}

	public static decimal IncDiff(decimal Xn, decimal X)
	{
		return decimal.Subtract(Xn, X);
	}

	public static decimal IncDiffByTVD(decimal IncDiff, decimal TrendTVD)
	{
		return decimal.Divide(IncDiff, TrendTVD);
	}

	public static decimal AziDiff(decimal AziN, decimal Azi)
	{
		return decimal.Subtract(AziN, Azi);
	}

	public static decimal AziDiffByTVD(decimal AziDiff, decimal TrendTVD)
	{
		return decimal.Divide(AziDiff, TrendTVD);
	}

	public static decimal RemainingTVD(decimal TVDn, decimal TVD)
	{
		return decimal.Subtract(TVDn, TVD);
	}

	public static decimal EOBX(decimal X, decimal IncDiffByTVD, decimal RemainingTVD)
	{
		return decimal.Add(X, decimal.Multiply(IncDiffByTVD, RemainingTVD));
	}

	public static decimal EOBAzi(decimal X, decimal AziDiffByTVD, decimal RemainingTVD)
	{
		return decimal.Add(X, decimal.Multiply(AziDiffByTVD, RemainingTVD));
	}

	public static decimal B(decimal ChangeInAzimuth, decimal Xn, decimal X)
	{
		decimal value = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(ChangeInAzimuth)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(Xn)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(X)) + System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(X)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(Xn)));
		return new decimal(180.0 / System.Math.PI * System.Math.Acos(Convert.ToDouble(value)));
	}

	public static decimal BTrend(decimal Dogleg, decimal CourseLength)
	{
		return B2 = decimal.Divide(decimal.Multiply(Dogleg, CourseLength), 30m);
	}

	public static decimal CourseLength(decimal RemainingTVD, decimal EOBX, decimal Xn)
	{
		return new decimal(Convert.ToDouble(RemainingTVD) / (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(Xn)) + System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(EOBX))) * 2.0);
	}

	public static decimal DogLeg(decimal B, decimal CourseLength)
	{
		return DogLeg2 = decimal.Multiply(decimal.Divide(B, CourseLength), 30m);
	}

	public static decimal TF(decimal EOBX, decimal EOBAzi, decimal B)
	{
		decimal value = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(EOBX)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(EOBAzi)) / System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(B)));
		return new decimal(180.0 / System.Math.PI * System.Math.Asin(Convert.ToDouble(value)));
	}

	public static decimal R(decimal DogLeg)
	{
		return new decimal(180.0 / System.Math.PI * Convert.ToDouble(decimal.Divide(1m, decimal.Divide(DogLeg, 30m))));
	}

	public static decimal AziAtTVD(decimal RemainingTVD, decimal AziDiffByTVD)
	{
		return decimal.Multiply(AziDiffByTVD, RemainingTVD);
	}

	public static decimal EOBNorth(decimal NorthN, decimal CourseLength, decimal Xn, decimal AziN, decimal EOBX, decimal EOBAzi)
	{
		return new decimal(Convert.ToDouble(NorthN) + Convert.ToDouble(decimal.Divide(CourseLength, 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(Xn)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(AziN)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(EOBX)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(EOBAzi))));
	}

	public static decimal EOBEast(decimal EastN, decimal CourseLength, decimal Xn, decimal AziN, decimal EOBX, decimal EOBAzi)
	{
		return new decimal(Convert.ToDouble(EastN) + Convert.ToDouble(decimal.Divide(CourseLength, 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(Xn)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(AziN)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(EOBX)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(EOBAzi))));
	}

	public static decimal EOBTVD(decimal TVDn, decimal CourseLength, decimal Xn, decimal EOBX)
	{
		return new decimal(Convert.ToDouble(TVDn) + Convert.ToDouble(decimal.Divide(CourseLength, 2m)) * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(Xn)) + System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(EOBX))));
	}

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
}
