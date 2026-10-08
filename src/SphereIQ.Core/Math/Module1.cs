using System;
using System.Diagnostics;
using EzyPlanData;
using Microsoft.VisualBasic.CompilerServices;

namespace EzyPlanMath;

[StandardModule]
internal sealed class Module1
{
	public static void main()
	{
		Math.ThreeDWellPlan threeDWellPlan = new Math.ThreeDWellPlan();
		SurveyList surveyList = new SurveyList();
		SurveyList.ImportCSVFilename_ErrorsEnum importCSVFilename_ErrorsEnum = default;
		SurveyList surveyList2 = new SurveyList();
		importCSVFilename_ErrorsEnum = surveyList.ImportCSV("C:\\ScotMc.csv", new ImportSurveyMapping("Test Map", 3, 2, 1, setHeader: false));
		if (importCSVFilename_ErrorsEnum == SurveyList.ImportCSVFilename_ErrorsEnum.NoError)
		{
			foreach (Survey item in surveyList)
			{
				Console.WriteLine(item.ToString());
			}
		}
		else
		{
			Console.WriteLine("Error: " + importCSVFilename_ErrorsEnum);
		}
		Console.WriteLine();
		Console.WriteLine();
		Console.WriteLine("Press any key to continue!");
		Console.ReadKey();
		Collar collar = new Collar();
		Target target = new Target();
		collar.SetNorthing(0m);
		collar.SetEasting(0m);
		target.SetNorthing(4m);
		target.SetEasting(-4m);
		target.SetTVD(425.3m);
		double CollarToTargetInc = default;
		double CollarToTargetAzi = default;
		threeDWellPlan.CollarToTarget(collar, target, ref CollarToTargetInc, ref CollarToTargetAzi);
		Console.WriteLine();
		Console.WriteLine("CollarNorth = " + Conversions.ToString(collar.Northing) + " CollarEast = " + Conversions.ToString(collar.Easting));
		Console.WriteLine("TargetNorth = " + Conversions.ToString(target.Northing) + " TargetEast = " + Conversions.ToString(target.Easting) + " TargetTVD = " + Conversions.ToString(target.TVD));
		Console.WriteLine();
		Console.WriteLine("Press any key to continue!");
		Console.ReadKey();
		AbsoluteSurveyList result = new AbsoluteSurveyList();
		threeDWellPlan.AbsoluteSurvey(collar, surveyList, ref result);
		Console.WriteLine("Processed Survey");
		Console.WriteLine();
		int num = 1;
		BuildAndHoldResults result2;
		double plotdistance;
		AbsoluteSurvey lastSurvey;
		double value = default;
		checked
		{
			foreach (AbsoluteSurvey item2 in result)
			{
				Console.WriteLine(Conversions.ToString(num) + " " + item2.ToString());
				num++;
			}
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Press any key to continue!");
			Console.ReadKey();
			result2 = new BuildAndHoldResults();
			AbsoluteSurvey absoluteSurvey = new AbsoluteSurvey();
			double num2 = System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Northing, collar.Northing)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Easting, collar.Easting)), 2.0));
			plotdistance = ((!(num2 > 20000.0)) ? 5.0 : ((double)(int)System.Math.Round(num2 / 1000.0)));
			AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
			int index = result.Count() - 1;
			lastSurvey = result.RecordAt(index);
			decimal result3 = new decimal(value);
			MinimumDogLegToTarget_Errors minimumDogLegToTarget_Errors = threeDWellPlan.MinimumDogLegToTarget(lastSurvey, target, ref result3, ref result2, plotdistance);
			value = Convert.ToDouble(result3);
			if (minimumDogLegToTarget_Errors == MinimumDogLegToTarget_Errors.NoError)
			{
				Console.WriteLine();
				Console.WriteLine("Minimum dog leg to target results: ");
				Console.WriteLine("Minimum DogLeg required: " + Conversions.ToString(System.Math.Round(value, 2)) + " / 30m");
				Console.WriteLine("EndOfHoleSurvey: " + result2.EndOfBuildSurvey.ToString());
				Console.WriteLine("Distance: " + result2.NaviDistance);
				Console.WriteLine("Start: " + result2.NaviStartMVD);
				Console.WriteLine("Stop: " + result2.NaviStopMVD);
				Console.WriteLine("ToolFace Setting: " + result2.ToolFaceSetting + " degrees off the high side");
				Console.WriteLine("Wedge setting to Target: " + result2.WedgeSetting + " degrees.");
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine("Press any key to continue!");
				Console.ReadKey();
			}
			else
			{
				result3 = new decimal(value);
				MinimumDogLegToTarget_Errors minimumDogLegToTarget_Errors2 = threeDWellPlan.MinimumDogLegToTarget(lastSurvey, target, ref result3, ref result2, plotdistance);
				value = Convert.ToDouble(result3);
				if (minimumDogLegToTarget_Errors2 == MinimumDogLegToTarget_Errors.InsufficientTVD)
				{
					Console.WriteLine();
					Console.WriteLine("WARNING!");
					Console.WriteLine();
					Console.WriteLine("Minimum dog leg to target CANNOT be achieved!");
					Console.WriteLine();
					Console.WriteLine("Minimum dog leg to target TVD results: ");
					Console.WriteLine("Minimum DogLeg required: " + Conversions.ToString(System.Math.Round(value, 2)) + " / 30m");
					Console.WriteLine("EndOfHoleSurvey: " + result2.EndOfBuildSurvey.ToString());
					Console.WriteLine("Distance: " + result2.NaviDistance);
					Console.WriteLine("Start: " + result2.NaviStartMVD);
					Console.WriteLine("Stop: " + result2.NaviStopMVD);
					Console.WriteLine("ToolFace Setting: " + result2.ToolFaceSetting + " degrees off the high side");
					Console.WriteLine("Wedge setting to Target: " + result2.WedgeSetting + " degrees.");
					Console.WriteLine();
					Console.WriteLine();
					Console.WriteLine("Press any key to continue!");
					Console.ReadKey();
				}
				else
				{
					result3 = new decimal(value);
					MinimumDogLegToTarget_Errors minimumDogLegToTarget_Errors3 = threeDWellPlan.MinimumDogLegToTarget(lastSurvey, target, ref result3, ref result2, plotdistance);
					value = Convert.ToDouble(result3);
					if (minimumDogLegToTarget_Errors3 == MinimumDogLegToTarget_Errors.TargetCannotBeAchieved)
					{
						goto IL_061e;
					}
				}
			}
			Console.WriteLine();
			Console.WriteLine("Min DL plot data: ");
			Console.WriteLine();
			foreach (AbsoluteSurvey projectedSurveyDatum in result2.ProjectedSurveyData)
			{
				Console.WriteLine(projectedSurveyDatum.ToString());
			}
			Console.WriteLine("Press any key to continue!");
			Console.WriteLine();
			Console.ReadKey();
			goto IL_061e;
		}
		IL_0a9f:
		Console.WriteLine();
		Console.WriteLine("Build plot data: ");
		Console.WriteLine();
		BuildAndHoldResults results;
		foreach (AbsoluteSurvey projectedSurveyDatum2 in results.ProjectedSurveyData)
		{
			Console.WriteLine(projectedSurveyDatum2.ToString());
		}
		Console.WriteLine("Press any key to continue!");
		Console.WriteLine();
		Console.ReadKey();
		goto IL_0b04;
		IL_0bf0:
		RelativePosition result7 = default;
		checked
		{
			decimal num3;
			do
			{
				Console.WriteLine("This is to caculate the planned hole survey at any MD:");
				Console.WriteLine();
				Console.Write("Enter a depth less than " + Conversions.ToString(surveyList.RecordAt(surveyList.Count() - 1).MD) + " :");
				num3 = Conversions.ToDecimal(Console.ReadLine());
			}
			while (threeDWellPlan.CheckMDValue(result, Convert.ToDouble(num3)) == CheckMDValue_Errors.CheckMDValue);
			if (threeDWellPlan.CheckMDValue(result, Convert.ToDouble(num3)) == CheckMDValue_Errors.NoError)
			{
			}
			Console.WriteLine();
			AbsoluteSurvey result4 = new AbsoluteSurvey();
			threeDWellPlan.PlannedSurveyAtMD(result.RecordAt(0), target, num3, ref result4);
			Console.WriteLine("Planned hole resutls at: " + num3);
			Console.WriteLine("End of hole at selected MD: " + result4.ToString());
			Console.WriteLine("Press any key to continue!");
			Console.WriteLine();
			Console.ReadKey();
			decimal num4;
			AbsoluteSurvey surveyAbove;
			AbsoluteSurvey surveyBelow;
			AbsoluteSurvey extrapolatedresult;
			int result5 = default;
			int SurveyAboveIndex = default;
			int SurveyBelowIndex = default;
			do
			{
				Console.WriteLine("This is to calculate any point in the actual drill hole at any MD:");
				Console.WriteLine();
				Console.Write("Enter a depth less than " + Conversions.ToString(surveyList.RecordAt(surveyList.Count() - 1).MD) + " :");
				num4 = Conversions.ToDecimal(Console.ReadLine());
				Console.WriteLine();
				surveyAbove = new AbsoluteSurvey();
				surveyBelow = new AbsoluteSurvey();
				extrapolatedresult = new AbsoluteSurvey();
				if (threeDWellPlan.CheckIfSurveyExists_MD(result, Convert.ToDouble(num4), ref result5, ref SurveyAboveIndex, ref SurveyBelowIndex, ref extrapolatedresult) == CheckIfSurveyExistsMD_Errors.SurveyExists)
				{
					Console.WriteLine("Actual hole resutls at: " + num4);
					Console.WriteLine("Actual end of hole at selected MD: " + extrapolatedresult.ToString());
					Console.WriteLine();
					break;
				}
				if (threeDWellPlan.CheckIfSurveyExists_MD(result, Convert.ToDouble(num4), ref result5, ref SurveyAboveIndex, ref SurveyBelowIndex, ref extrapolatedresult) != CheckIfSurveyExistsMD_Errors.NoSurveyExists)
				{
					break;
				}
				surveyAbove = result.RecordAt(SurveyAboveIndex);
				surveyBelow = result.RecordAt(SurveyBelowIndex);
				if (threeDWellPlan.ExtrapolateBetweenSurveys_MD(surveyAbove, surveyBelow, num4, ref extrapolatedresult) == ExtrapolateBetweenSurveys_Errors.NoError)
				{
					Console.WriteLine("Actual hole resutls at: " + num4);
					Console.WriteLine("Actual end of hole at selected MD: " + extrapolatedresult.ToString());
					Console.WriteLine();
					break;
				}
			}
			while (threeDWellPlan.ExtrapolateBetweenSurveys_MD(surveyAbove, surveyBelow, num4, ref extrapolatedresult) == ExtrapolateBetweenSurveys_Errors.InvalidMD);
			Console.WriteLine();
			Console.WriteLine("Press any key to continue!");
			Console.WriteLine();
			Console.ReadKey();
			Console.WriteLine();
			Console.WriteLine("This is to calculate any point at TVD in the actual drill hole:");
			Console.WriteLine();
			Console.Write("Enter a depth less than " + Conversions.ToString(surveyList.RecordAt(surveyList.Count() - 1).MD) + " :");
			decimal num5 = Conversions.ToDecimal(Console.ReadLine());
			AbsoluteSurvey absoluteSurvey2 = new AbsoluteSurvey();
			AbsoluteSurvey absoluteSurvey3 = new AbsoluteSurvey();
			AbsoluteSurvey extrapolatedresult2 = new AbsoluteSurvey();
			Console.WriteLine();
			int result6 = default;
			int SurveyAboveIndex2 = default;
			int SurveyBelowIndex2 = default;
			if (threeDWellPlan.CheckIfSurveyExists_TVD(result, Convert.ToDouble(num5), ref result6, ref SurveyAboveIndex2, ref SurveyBelowIndex2, ref extrapolatedresult2) == CheckIfSurveyExistsTVD_Errors.SurveyExists)
			{
				Console.WriteLine("Actual hole resutls at: " + num5);
				Console.WriteLine("Actual end of hole at selected TVD: " + extrapolatedresult2.ToString());
				Console.WriteLine();
			}
			else if (threeDWellPlan.CheckIfSurveyExists_TVD(result, Convert.ToDouble(num5), ref result6, ref SurveyAboveIndex2, ref SurveyBelowIndex2, ref extrapolatedresult2) == CheckIfSurveyExistsTVD_Errors.NoSurveyExists)
			{
				absoluteSurvey2 = result.RecordAt(SurveyAboveIndex2);
				absoluteSurvey3 = result.RecordAt(SurveyBelowIndex2);
				double surveyAboveDip = Convert.ToDouble(result.RecordAt(SurveyAboveIndex2).Dip);
				double surveyBelowDip = Convert.ToDouble(result.RecordAt(SurveyBelowIndex2).Dip);
				double num6 = threeDWellPlan.ConvertTVDToMD(Convert.ToDouble(decimal.Subtract(num5, absoluteSurvey2.TVD)), surveyAboveDip, surveyBelowDip);
				threeDWellPlan.ExtrapolateBetweenSurveys_TVD(absoluteSurvey2, absoluteSurvey3, num5, ref extrapolatedresult2);
				Console.WriteLine("Actual hole resutls at: " + num5);
				Console.WriteLine("Actual end of hole at selected TVD: " + extrapolatedresult2.ToString());
				Console.WriteLine();
			}
			Console.WriteLine("Press any key to continue!");
			Console.WriteLine();
			Console.ReadKey();
			decimal num7;
			do
			{
				Console.Write("Enter a depth at which you would like to comapre Actual vs' planned:");
				num7 = Conversions.ToDecimal(Console.ReadLine());
				Console.WriteLine();
				int num8 = result.Count();
			}
			while (decimal.Compare(num7, result.RecordAt(result.Count() - 1).MD) > 0);
			bool flag = false;
			int num9 = result.Count() - 1;
			int num10 = 0;
			int num12 = default;
			while (true)
			{
				if (num10 <= num9)
				{
					int value2 = Convert.ToInt32(result.RecordAt(num10).MD);
					if (decimal.Compare(new decimal(value2), num7) != 0)
					{
						num10++;
						continue;
					}
					int index2 = num10;
					extrapolatedresult = result.RecordAt(index2);
				}
				else
				{
					if (!flag)
					{
						int num11 = result.Count() - 1;
						for (num10 = 0; num10 <= num11; num10++)
						{
							if (decimal.Compare(result.RecordAt(num10).MD, num4) < 0)
							{
								decimal mD = result.RecordAt(num10).MD;
								num12 = num10;
							}
						}
						surveyAbove = result.RecordAt(num12);
						surveyBelow = result.RecordAt(num12 + 1);
					}
					if (threeDWellPlan.ExtrapolateBetweenSurveys_MD(surveyAbove, surveyBelow, num7, ref extrapolatedresult) != ExtrapolateBetweenSurveys_Errors.NoError)
					{
						if (threeDWellPlan.ExtrapolateBetweenSurveys_MD(surveyAbove, surveyBelow, num7, ref extrapolatedresult) != ExtrapolateBetweenSurveys_Errors.InvalidMD)
						{
						}
						break;
					}
				}
				threeDWellPlan.PlannedSurveyAtMD(result.RecordAt(0), target, num7, ref result4);
				Console.WriteLine("Planned hole resutls at: " + num7);
				Console.WriteLine("End of hole at selected MD: " + result4.ToString());
				Console.WriteLine();
				Console.WriteLine("Actual hole resutls at: " + num7);
				Console.WriteLine("Actual end of hole at selected MD: " + extrapolatedresult.ToString());
				Console.WriteLine();
				Console.WriteLine("Planned vs Actual results:");
				Console.WriteLine();
				threeDWellPlan.DistanceFromStraightPathPlan(result4, extrapolatedresult, ref result7);
				Console.WriteLine(result7.ToString() + " horizontal departure.");
				break;
			}
			Console.WriteLine("Press any key to continue!");
			Console.WriteLine();
			Console.ReadKey();
			return;
		}
		IL_0b04:
		AbsoluteSurvey result8 = new AbsoluteSurvey();
		checked
		{
			if (threeDWellPlan.StraightPathToTargetTVD(target, result.RecordAt(result.Count() - 1), ref result8) == StraightPathToTargetTVD_Errors.NoError)
			{
				RelativePosition result9 = new RelativePosition();
				threeDWellPlan.DistanceFromTarget(target, result8, ref result9);
				Console.WriteLine();
				Console.WriteLine("Test Straigh Path to TVD Resutls: ");
				Console.WriteLine("End of hole survey :" + result8.ToString());
				Console.WriteLine("Resulting distance from target :" + result9.ToString());
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine("Press any key to continue!");
				Console.WriteLine();
				Console.ReadKey();
			}
			else if (threeDWellPlan.StraightPathToTargetTVD(target, result.RecordAt(result.Count() - 1), ref result8) == StraightPathToTargetTVD_Errors.TargetTVDHasAlreadyBeenReached)
			{
				Console.WriteLine("Test Straigh Path to TVD Resutls: ");
				Console.WriteLine();
				Console.WriteLine("'Target TVD has already been passed!'");
				Console.WriteLine();
				Console.WriteLine("Press any key to continue!");
				Console.ReadKey();
				goto IL_0bf0;
			}
			result7 = new RelativePosition();
			Console.WriteLine();
			goto IL_0bf0;
		}
		IL_061e:
		checked
		{
			Console.Write("Enter a Wedge depth less than " + Conversions.ToString(surveyList.RecordAt(surveyList.Count() - 1).MD) + " :");
			decimal num13 = Conversions.ToDecimal(Console.ReadLine());
			Console.Write("Enter a dogleg value between 3 and 7: ");
			double num14 = Conversions.ToDouble(Console.ReadLine());
			Stopwatch stopwatch = new Stopwatch();
			results = new BuildAndHoldResults();
			double num15 = System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Northing, collar.Northing)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Easting, collar.Easting)), 2.0));
			double buildExtrapCD = ((!(num15 > 20000.0)) ? 5.0 : ((double)(int)System.Math.Round(num15 / 1000.0)));
			if (threeDWellPlan.BuildToTarget(num14, result.RecordAt(result.Count() - 1), target, ref results, buildExtrapCD) == BuildToTarget_Errors.NoError)
			{
				stopwatch.Start();
				Console.WriteLine();
				Console.WriteLine("Build and hold test results: ");
				Console.WriteLine("DogLeg: " + System.Math.Round(num14, 2));
				Console.WriteLine("EndOfBuildSurvey: " + results.EndOfBuildSurvey.ToString());
				Console.WriteLine("EndOfHoleSurvey: " + results.EndOfHoleSurvey.ToString());
				Console.WriteLine("NaviDistance: " + results.NaviDistance);
				Console.WriteLine("NaviStart: " + results.NaviStartMVD);
				Console.WriteLine("NaviStop: " + results.NaviStopMVD);
				Console.WriteLine("ToolFace Setting: " + results.ToolFaceSetting + " degrees off the high side");
				Console.WriteLine("Hold distance: " + results.StraightDistance);
				Console.WriteLine("Wedge setting to Target: " + results.WedgeSetting + " degrees.");
				stopwatch.Stop();
				Console.WriteLine("The Build and Hold processing took " + stopwatch.ElapsedMilliseconds + " milliseconds");
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine("Press any key to continue!");
				Console.WriteLine();
				Console.ReadKey();
				goto IL_0a9f;
			}
			if (threeDWellPlan.BuildToTarget(num14, result.RecordAt(result.Count() - 1), target, ref results, buildExtrapCD) == BuildToTarget_Errors.InsufficientTVD)
			{
				Console.WriteLine();
				Console.WriteLine("'Build to Target cannot be achieved due to available TVD!'");
				Console.WriteLine();
				Console.WriteLine("Press any key to continue!");
				Console.ReadKey();
			}
			else if (threeDWellPlan.BuildToTarget(num14, result.RecordAt(result.Count() - 1), target, ref results, buildExtrapCD) == BuildToTarget_Errors.DogLegTooSmall)
			{
				decimal result3 = new decimal(value);
				MinimumDogLegToTarget_Errors minimumDogLegToTarget_Errors4 = threeDWellPlan.MinimumDogLegToTarget(lastSurvey, target, ref result3, ref result2, plotdistance);
				value = Convert.ToDouble(result3);
				if (minimumDogLegToTarget_Errors4 != MinimumDogLegToTarget_Errors.NoError)
				{
					goto IL_0a9f;
				}
				Console.WriteLine();
				Console.WriteLine("Build and hold test results: ");
				Console.WriteLine("DogLeg: " + Conversions.ToString(System.Math.Round(value, 2)));
				Console.WriteLine("EndOfHoleSurvey: " + result2.EndOfBuildSurvey.ToString());
				Console.WriteLine("NaviDistance: " + result2.NaviDistance);
				Console.WriteLine("NaviStart: " + result2.NaviStartMVD);
				Console.WriteLine("NaviStop: " + result2.NaviStopMVD);
				Console.WriteLine("ToolFace Setting: " + result2.ToolFaceSetting + " degrees off the high side");
				Console.WriteLine("Wedge setting to Target: " + result2.WedgeSetting + " degrees.");
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine("Press any key to continue!");
				Console.ReadKey();
				foreach (AbsoluteSurvey projectedSurveyDatum3 in result2.ProjectedSurveyData)
				{
					Console.WriteLine(projectedSurveyDatum3.ToString());
				}
			}
			else if (threeDWellPlan.BuildToTarget(num14, result.RecordAt(result.Count() - 1), target, ref results, buildExtrapCD) != BuildToTarget_Errors.TargetCannotBeAchived)
			{
				goto IL_0a9f;
			}
			goto IL_0b04;
		}
	}
}
