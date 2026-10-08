using System;
using System.Diagnostics;
using EzyPlanData;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace EzyPlanMath;

public class Math
{
	public class ThreeDWellPlan
	{
		private const double RadPerDeg = System.Math.PI / 180.0;

		private const double DegPerRad = 180.0 / System.Math.PI;

		private decimal MinDLLengthOfD;

		private decimal MinDLRadius;

		private double MinimumDogLegToTargetIn30Meters;

		private double Radius;

		private double VRemainder;

		[DebuggerNonUserCode]
		public ThreeDWellPlan()
		{
		}

		public AbsoluteSurvey_Errors AbsoluteSurvey(Collar collar, SurveyList surveyList, ref AbsoluteSurveyList result)
		{
			checked
			{
				AbsoluteSurvey_Errors result2;
				try
				{
					int num = 0;
					int num2 = surveyList.Count();
					double num4 = default;
					decimal num6 = default;
					decimal num8 = default;
					decimal num12 = default;
					decimal d3 = default;
					foreach (Survey survey3 in surveyList)
					{
						if (num == 0)
						{
							Survey survey = surveyList.RecordAt(num);
							Survey survey2 = surveyList.RecordAt(num + 1);
							result.Add(new AbsoluteSurvey(survey3, Conversions.ToDecimal(Strings.Format(0, "#0.00#")), collar.Northing, collar.Easting, Conversions.ToDecimal(Strings.Format(0, "#0.00#"))));
						}
						else if ((num > 0) & (num <= num2 - 2))
						{
							Survey survey = surveyList.RecordAt(num - 1);
							Survey survey2 = surveyList.RecordAt(num);
							decimal d = decimal.Subtract(survey2.MD, survey.MD);
							double num3 = System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(survey.Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(survey2.Azimuth, survey.Azimuth))));
							num3 = 180.0 / System.Math.PI * num3;
							double value = num3 / Convert.ToDouble(decimal.Divide(d, 30m));
							if (num3 == 0.0)
							{
								num4 = 1.0;
							}
							else if (num3 > 0.0)
							{
								num4 = 2.0 / (System.Math.PI / 180.0 * num3) * System.Math.Tan(System.Math.PI / 180.0 * num3 / 2.0);
							}
							double num5 = Convert.ToDouble(decimal.Divide(d, 2m)) * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(survey.Dip)) + System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Dip))) * num4;
							num6 = new decimal(Convert.ToDouble(num6) + num5);
							decimal setTVD = num6;
							decimal num7 = new decimal(Convert.ToDouble(decimal.Divide(d, 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(survey.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Azimuth))) * num4);
							num8 = decimal.Add(num8, num7);
							decimal num9 = decimal.Add(num7, collar.Northing);
							decimal num10 = decimal.Add(collar.Northing, num8);
							decimal setNorthing = decimal.Add(result.RecordAt(num - 1).Northing, num7);
							decimal num11 = new decimal(Convert.ToDouble(decimal.Divide(d, 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Azimuth))) * num4);
							decimal d2 = num11;
							num12 = decimal.Add(num12, num11);
							decimal num13 = decimal.Add(d2, collar.Easting);
							decimal num14 = decimal.Add(collar.Easting, num12);
							decimal setEasting = num14;
							result.Add(new AbsoluteSurvey(survey3, setTVD, setNorthing, setEasting, new decimal(value)));
						}
						else if ((num > 0) & (num > num2 - 2))
						{
							Survey survey = surveyList.RecordAt(num2 - 2);
							Survey survey2 = surveyList.RecordAt(num2 - 1);
							decimal d = decimal.Subtract(survey2.MD, survey.MD);
							double num5 = Convert.ToDouble(decimal.Divide(d, 2m)) * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(survey.Dip)) + System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Dip)));
							num6 = new decimal(Convert.ToDouble(num6) + num5);
							decimal setTVD = num6;
							decimal num7 = new decimal(Convert.ToDouble(decimal.Divide(d, 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(survey.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Azimuth))));
							num8 = decimal.Add(num8, num7);
							decimal num9 = decimal.Add(d3, collar.Northing);
							decimal num10 = decimal.Add(collar.Northing, num8);
							decimal setNorthing = num10;
							decimal num11 = new decimal(Convert.ToDouble(decimal.Divide(d, 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Azimuth))));
							decimal d2 = num11;
							num12 = decimal.Add(num12, num11);
							decimal num13 = decimal.Add(d2, collar.Easting);
							decimal num14 = decimal.Add(collar.Easting, num12);
							decimal setEasting = num14;
							double num3 = System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(survey.Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey2.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(survey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(survey2.Azimuth, survey.Azimuth))));
							num3 = 180.0 / System.Math.PI * num3;
							double value = num3 / Convert.ToDouble(decimal.Divide(d, 30m));
							result.Add(new AbsoluteSurvey(survey3, setTVD, setNorthing, setEasting, new decimal(value)));
						}
						num++;
					}
					int num15 = result.Count() - 1;
					for (int i = 0; i <= num15; i++)
					{
						result.RecordAt(i).Easting = System.Math.Round(result.RecordAt(i).Easting, 2);
						result.RecordAt(i).Northing = System.Math.Round(result.RecordAt(i).Northing, 2);
						result.RecordAt(i).TVD = System.Math.Round(result.RecordAt(i).TVD, 2);
						result.RecordAt(i).DL = System.Math.Round(result.RecordAt(i).DL, 2);
					}
					result2 = AbsoluteSurvey_Errors.NoError;
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					result2 = AbsoluteSurvey_Errors.FunctionInputParameterError;
					ProjectData.ClearProjectError();
				}
				return result2;
			}
		}

		public MinimumDogLegToTarget_Errors MinimumDogLegToTargetTest(AbsoluteSurvey lastSurvey, Target target, ref decimal result, ref BuildAndHoldResults result1, double Plotdistance)
		{
			MinimumDogLegToTarget_Errors result2 = default;
			return result2;
		}

		public MinimumDogLegToTarget_Errors MinimumDogLegToTarget(AbsoluteSurvey lastSurvey, Target target, ref decimal result, ref BuildAndHoldResults result1, double Plotdistance)
		{
			bool setBuildBelowTarget = false;
			if (decimal.Compare(lastSurvey.TVD, target.TVD) > 0)
			{
				return MinimumDogLegToTarget_Errors.TargetTVDHasAlreadyBeenReached;
			}
			int try0018_dispatch = -1;
			checked
			{
				double currentAzi = default;
				double num = default;
				double num2 = default;
				double num3 = default;
				double num4 = default;
				double num5 = default;
				double num6 = default;
				double num7 = default;
				double num8 = default;
				double num10 = default;
				double a = default;
				MinimumDogLegToTarget_Errors result2;
				while (true)
				{
					try
					{
						/*Note: ILSpy has introduced the following switch to emulate a goto from catch-block to try-block*/;
						switch (try0018_dispatch)
						{
						default:
						{
							AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
							currentAzi = Convert.ToDouble(lastSurvey.Azimuth);
							num = System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Northing, lastSurvey.Northing)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Easting, lastSurvey.Easting)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.TVD, lastSurvey.TVD)), 2.0));
							num2 = System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth));
							num3 = System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth));
							num4 = System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip));
							num5 = Convert.ToDouble(decimal.Subtract(target.Northing, lastSurvey.Northing)) / num;
							num6 = Convert.ToDouble(decimal.Subtract(target.Easting, lastSurvey.Easting)) / num;
							num7 = Convert.ToDouble(decimal.Subtract(target.TVD, lastSurvey.TVD)) / num;
							num8 = 180.0 / System.Math.PI * System.Math.Acos(num2 * num5 + num3 * num6 + num4 * num7);
							double num9 = num / 2.0;
							Radius = num9 / System.Math.Sin(System.Math.PI / 180.0 * num8);
							num10 = Radius * (2.0 * (System.Math.PI / 180.0 * num8));
							double num11 = 2.0 * num8 / num10 * 30.0;
							MinimumDogLegToTargetIn30Meters = 180.0 / System.Math.PI * (30.0 / Radius);
							result = new decimal(MinimumDogLegToTargetIn30Meters);
							break;
						}
						case 561:
							break;
						}
						double num21;
						double num22;
						double num23;
						double num29;
						double num30;
						double num31;
						double num32;
						double num33;
						double num34;
						double cosXk;
						double cosBk;
						while (true)
						{
							MinDLRadius = new decimal(180.0 / System.Math.PI * (1.0 / (MinimumDogLegToTargetIn30Meters / 30.0)));
							double num12 = System.Math.Round(Convert.ToDouble(MinDLRadius) / System.Math.Sin(System.Math.PI / 180.0 * num8), 5);
							double num13 = Convert.ToDouble(MinDLRadius) / System.Math.Tan(System.Math.PI / 180.0 * num8);
							MinDLLengthOfD = new decimal(System.Math.Sqrt(System.Math.Pow(num, 2.0) + System.Math.Pow(Convert.ToDouble(MinDLRadius), 2.0) - 2.0 * num * Convert.ToDouble(MinDLRadius) * System.Math.Sin(System.Math.PI / 180.0 * num8)));
							double num14 = System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(MinDLLengthOfD), 2.0) - System.Math.Pow(Convert.ToDouble(MinDLRadius), 2.0));
							double num15 = num12 * num5 + Convert.ToDouble(lastSurvey.Northing);
							double num16 = num12 * num6 + Convert.ToDouble(lastSurvey.Easting);
							double num17 = num12 * num7 + Convert.ToDouble(lastSurvey.TVD);
							double num18 = num2 * -1.0;
							double num19 = num3 * -1.0;
							double num20 = num4 * -1.0;
							num21 = num13 * num18 + num15;
							num22 = num13 * num19 + num16;
							num23 = num13 * num20 + num17;
							double num24 = 180.0 / System.Math.PI * System.Math.Acos(Convert.ToDouble(decimal.Divide(MinDLRadius, MinDLLengthOfD)));
							double num25 = 180.0 / System.Math.PI * System.Math.Acos((System.Math.Pow(Convert.ToDouble(MinDLRadius), 2.0) + System.Math.Pow(Convert.ToDouble(MinDLLengthOfD), 2.0) - System.Math.Pow(num, 2.0)) / Convert.ToDouble(decimal.Multiply(decimal.Multiply(2m, MinDLRadius), MinDLLengthOfD)));
							double num26 = ((System.Math.Sign(num13) != -1) ? (num25 - num24) : (360.0 - num25 - num24));
							double num27 = Convert.ToDouble(MinDLRadius) * System.Math.Cos(System.Math.PI / 180.0 * num26);
							double num28 = Convert.ToDouble(MinDLRadius) * System.Math.Sin(System.Math.PI / 180.0 * num26);
							num29 = (Convert.ToDouble(lastSurvey.Northing) - num21) / Convert.ToDouble(MinDLRadius);
							num30 = (Convert.ToDouble(lastSurvey.Easting) - num22) / Convert.ToDouble(MinDLRadius);
							num31 = (Convert.ToDouble(lastSurvey.TVD) - num23) / Convert.ToDouble(MinDLRadius);
							num32 = num21 + num27 * num29 + num28 * num2;
							num33 = num22 + num27 * num30 + num28 * num3;
							num34 = num23 + num27 * num31 + num28 * num4;
							cosXk = (Convert.ToDouble(target.Northing) - num32) / num14;
							cosBk = (Convert.ToDouble(target.Easting) - num33) / num14;
							double num35 = (Convert.ToDouble(target.TVD) - num34) / num14;
							VRemainder = Convert.ToDouble(target.TVD) - num34;
							if (System.Math.Sign(VRemainder) != -1)
							{
								break;
							}
							if ((decimal.Compare(MinDLLengthOfD, MinDLRadius) < 0) | (decimal.Compare(MinDLLengthOfD, MinDLRadius) == 0))
							{
								MinimumDogLegToTargetIn30Meters += 1E-10;
								continue;
							}
							setBuildBelowTarget = true;
							break;
						}
						double num36 = System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(target.Northing) - num32, 2.0) + System.Math.Pow(Convert.ToDouble(target.Easting) - num33, 2.0) + System.Math.Pow(Convert.ToDouble(target.TVD) - num34, 2.0));
						double num37 = 180.0 / System.Math.PI * System.Math.Acos(VRemainder / num36);
						int TargetQuadrant = (int)System.Math.Round(a);
						double num38 = mdTrendPlot.FinalTargetAzimuth(cosXk, cosBk, ref TargetQuadrant);
						a = TargetQuadrant;
						double num39 = num38;
						double num40 = System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * num37) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) + System.Math.Sin(System.Math.PI / 180.0 * num37) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * (num39 - Convert.ToDouble(lastSurvey.Azimuth))));
						double num41 = 180.0 / System.Math.PI * num40;
						double num42 = System.Math.Round(num41 / (MinimumDogLegToTargetIn30Meters / 30.0), 2);
						double num43 = mdTrendPlot.FinalToolFaceSetting(lastSurvey, num37, num39, num41);
						bool highSide = mdTrendPlot.HighSide(num43);
						int num44 = mdTrendPlot.CurrentQuadrant(currentAzi);
						AbsoluteSurvey absoluteSurvey = mdTrendPlot.TrendPlotLastSurvey(num10, Convert.ToDouble(MinDLRadius), num21, num22, num23, num2, num3, num4, num29, num30, num31, Convert.ToDouble(result), Convert.ToDouble(lastSurvey.MD), (int)System.Math.Round(a), num44, currentAzi);
						double num45 = Convert.ToDouble(lastSurvey.MD);
						double num46 = Convert.ToDouble(absoluteSurvey.MD);
						double mD = 0.0;
						double num47 = num46 - num45;
						decimal value = new decimal(System.Math.Floor(num47 / Plotdistance));
						AbsoluteSurveyList Result = new AbsoluteSurveyList();
						if (decimal.Compare(lastSurvey.Dip, 0m) == 0)
						{
							double endMD = Convert.ToDouble(absoluteSurvey.MD);
							double overallDogLeg = Convert.ToDouble(result);
							bool MaxDip = false;
							mdTrendPlot.ExtrapolateIntervalSurveysMD(endMD, num43, overallDogLeg, 1.0, lastSurvey, ref Result, ref MaxDip, ExtrapToDir: false, IncIsZero: false, DirIsZero: false, ExtrapToInc: false, 0.0, MinDL: true, 0.0, 0.0, 0.0, 0.0, Build: false);
						}
						else
						{
							mdTrendPlot.TrendPlotTest(mD, Convert.ToDouble(MinDLRadius), num21, num22, num23, num2, num3, num4, num29, num30, num31, Convert.ToDouble(result), Convert.ToDouble(lastSurvey.MD), (int)System.Math.Round(a), num44, currentAzi, highSide, Convert.ToInt32(value), Convert.ToDouble(lastSurvey.Dip), ref Result, lastSurvey, Plotdistance);
							num44 = mdTrendPlot.CurrentQuadrant(Convert.ToDouble(Result.LastRecord().Azimuth));
							mD = num10;
							absoluteSurvey = mdTrendPlot.TrendPlotLastSurvey(mD, Convert.ToDouble(MinDLRadius), num21, num22, num23, num2, num3, num4, num29, num30, num31, Convert.ToDouble(result), Convert.ToDouble(lastSurvey.MD), (int)System.Math.Round(a), num44, Convert.ToDouble(Result.LastRecord().Azimuth));
							Result.Add(absoluteSurvey);
							int num48 = Result.Count() - 2;
							for (int i = 1; i <= num48; i++)
							{
								Result.RecordAt(i).DL = new decimal(180.0 / System.Math.PI * System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(Result.RecordAt(i).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(Result.RecordAt(i - 1).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(Result.RecordAt(i).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(Result.RecordAt(i - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(Result.RecordAt(i).Azimuth, Result.RecordAt(i - 1).Azimuth)))) / Convert.ToDouble(decimal.Subtract(Result.RecordAt(i).MD, Result.RecordAt(i - 1).MD)) * 30.0);
							}
							int num49 = Result.Count() - 1;
							for (int j = 0; j <= num49; j++)
							{
								Result.RecordAt(j).Azimuth = System.Math.Round(Result.RecordAt(j).Azimuth, 2);
								Result.RecordAt(j).Dip = System.Math.Round(Result.RecordAt(j).Dip, 2);
								Result.RecordAt(j).DL = System.Math.Round(Result.RecordAt(j).DL, 2);
								Result.RecordAt(j).Easting = System.Math.Round(Result.RecordAt(j).Easting, 2);
								Result.RecordAt(j).MD = System.Math.Round(Result.RecordAt(j).MD, 2);
								Result.RecordAt(j).Northing = System.Math.Round(Result.RecordAt(j).Northing, 2);
								Result.RecordAt(j).TVD = System.Math.Round(Result.RecordAt(j).TVD, 2);
							}
							AbsoluteSurvey absoluteSurvey2 = new AbsoluteSurvey(new decimal(System.Math.Round(Convert.ToDouble(lastSurvey.MD) + num10, 2)), Result.LastRecord().Dip, Result.LastRecord().Azimuth, new decimal(System.Math.Round(num34, 2)), new decimal(System.Math.Round(num32, 2)), new decimal(System.Math.Round(num33, 2)), System.Math.Round(result, 2));
							int num50 = Result.Count() - 1;
							for (int k = 1; k <= num50; k++)
							{
								Result.RecordAt(k).DL = System.Math.Round(Result.RecordAt(k).DL, 2);
							}
							Result.Remove(0);
						}
						double value2 = System.Math.Round(mdMiscSurveyVariables.WedgeAzi(target, lastSurvey), 2);
						BuildAndHoldResults buildAndHoldResults = new BuildAndHoldResults(Result.LastRecord(), absoluteSurvey, new decimal(System.Math.Round(num10, 2)), 0m, System.Math.Round(lastSurvey.MD, 2), new decimal(System.Math.Round(Convert.ToDouble(lastSurvey.MD) + num10, 2)), Result, new decimal(System.Math.Round(num43, 2)), new decimal(value2), setBuildBelowTarget);
						result1 = buildAndHoldResults;
						result2 = MinimumDogLegToTarget_Errors.NoError;
					}
					catch (Exception ex)
					{
						ProjectData.SetProjectError(ex);
						Exception ex2 = ex;
						if ((decimal.Compare(MinDLLengthOfD, MinDLRadius) < 0) | (decimal.Compare(MinDLLengthOfD, MinDLRadius) == 0))
						{
							MinimumDogLegToTargetIn30Meters += 1E-10;
							ProjectData.ClearProjectError();
							try0018_dispatch = 561;
							continue;
						}
						Interaction.MsgBox(ex2.ToString(), MsgBoxStyle.Information, "Error");
						result2 = MinimumDogLegToTarget_Errors.TargetCannotBeAchieved;
						ProjectData.ClearProjectError();
					}
					break;
				}
				return result2;
			}
		}

		public BuildToTarget_Errors BuildToTarget(double dogLeg, AbsoluteSurvey lastSurvey, Target target, ref BuildAndHoldResults results, double BuildExtrapCD)
		{
			bool setBuildBelowTarget = false;
			if (decimal.Compare(lastSurvey.TVD, target.TVD) > 0)
			{
				return BuildToTarget_Errors.TargetCannotBeAchived;
			}
			checked
			{
				BuildToTarget_Errors result;
				try
				{
					AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
					double currentAzi = Convert.ToDouble(lastSurvey.Azimuth);
					double num = 180.0 / System.Math.PI * (1.0 / (dogLeg / 30.0));
					double num2 = System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth));
					double num3 = System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth));
					double num4 = System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip));
					double num5 = System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Northing, lastSurvey.Northing)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Easting, lastSurvey.Easting)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.TVD, lastSurvey.TVD)), 2.0));
					double num6 = Convert.ToDouble(decimal.Subtract(target.Northing, lastSurvey.Northing)) / num5;
					double num7 = Convert.ToDouble(decimal.Subtract(target.Easting, lastSurvey.Easting)) / num5;
					double num8 = Convert.ToDouble(decimal.Subtract(target.TVD, lastSurvey.TVD)) / num5;
					double num9 = 180.0 / System.Math.PI * System.Math.Acos(num2 * num6 + num3 * num7 + num4 * num8);
					double num10 = num / System.Math.Sin(System.Math.PI / 180.0 * num9);
					double num11 = num / System.Math.Tan(System.Math.PI / 180.0 * num9);
					double num12 = System.Math.Sqrt(System.Math.Pow(num5, 2.0) + System.Math.Pow(num, 2.0) - 2.0 * num5 * num * System.Math.Sin(System.Math.PI / 180.0 * num9));
					double num13 = System.Math.Sqrt(System.Math.Pow(num12, 2.0) - System.Math.Pow(num, 2.0));
					double num14 = num10 * num6 + Convert.ToDouble(lastSurvey.Northing);
					double num15 = num10 * num7 + Convert.ToDouble(lastSurvey.Easting);
					double num16 = num10 * num8 + Convert.ToDouble(lastSurvey.TVD);
					double num17 = num2 * -1.0;
					double num18 = num3 * -1.0;
					double num19 = num4 * -1.0;
					double num20 = num11 * num17 + num14;
					double num21 = num11 * num18 + num15;
					double num22 = num11 * num19 + num16;
					double num23 = 180.0 / System.Math.PI * System.Math.Acos(num / num12);
					double num24 = 180.0 / System.Math.PI * System.Math.Acos((System.Math.Pow(num, 2.0) + System.Math.Pow(num12, 2.0) - System.Math.Pow(num5, 2.0)) / (2.0 * num * num12));
					double num25 = ((System.Math.Sign(num11) != -1) ? (num24 - num23) : (360.0 - num24 - num23));
					double num26 = (Convert.ToDouble(lastSurvey.Northing) - num20) / num;
					double num27 = (Convert.ToDouble(lastSurvey.Easting) - num21) / num;
					double num28 = (Convert.ToDouble(lastSurvey.TVD) - num22) / num;
					double num29 = num * System.Math.Cos(System.Math.PI / 180.0 * num25);
					double num30 = num * System.Math.Sin(System.Math.PI / 180.0 * num25);
					double num31 = num20 + num29 * num26 + num30 * num2;
					double num32 = num21 + num29 * num27 + num30 * num3;
					double num33 = num22 + num29 * num28 + num30 * num4;
					double cosXk = (Convert.ToDouble(target.Northing) - num31) / num13;
					double cosBk = (Convert.ToDouble(target.Easting) - num32) / num13;
					double d = (Convert.ToDouble(target.TVD) - num33) / num13;
					double num34 = 180.0 / System.Math.PI * System.Math.Acos(d);
					decimal num35 = new decimal(Convert.ToDouble(target.TVD) - num33);
					if (!((System.Math.Sign(num35) == 1) & Versioned.IsNumeric(num35)) && ((System.Math.Sign(num35) == -1) | !Versioned.IsNumeric(num35)))
					{
						setBuildBelowTarget = true;
					}
					double a = default;
					int TargetQuadrant = (int)System.Math.Round(a);
					double num36 = mdTrendPlot.FinalTargetAzimuth(cosXk, cosBk, ref TargetQuadrant);
					a = TargetQuadrant;
					double num37 = num36;
					double num38 = System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * num34) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) + System.Math.Sin(System.Math.PI / 180.0 * num34) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * (num37 - Convert.ToDouble(lastSurvey.Azimuth))));
					double num39 = 180.0 / System.Math.PI * num38;
					double num40 = num39 / (dogLeg / 30.0);
					double num41 = mdTrendPlot.FinalToolFaceSetting(lastSurvey, num34, num37, num39);
					bool highSide = default;
					if (num41 < 180.0)
					{
						highSide = true;
					}
					else if (num41 > 180.0)
					{
						highSide = false;
					}
					int num42 = mdFiveMeterBuildPlot.CurrentQuadrant(currentAzi);
					AbsoluteSurvey absoluteSurvey = mdTrendPlot.TrendPlotLastSurvey(num40, num, num20, num21, num22, num2, num3, num4, num26, num27, num28, dogLeg, Convert.ToDouble(lastSurvey.MD), (int)System.Math.Round(a), num42, currentAzi);
					double num43 = Convert.ToDouble(lastSurvey.MD);
					double num44 = Convert.ToDouble(absoluteSurvey.MD);
					double mD = 0.0;
					double num45 = num44 - num43;
					decimal value = new decimal(System.Math.Floor(num45 / BuildExtrapCD));
					AbsoluteSurveyList Result = new AbsoluteSurveyList();
					if (decimal.Compare(lastSurvey.Dip, 0m) == 0)
					{
						double endMD = Convert.ToDouble(absoluteSurvey.MD);
						bool MaxDip = false;
						mdTrendPlot.ExtrapolateIntervalSurveysMD(endMD, num41, dogLeg, BuildExtrapCD, lastSurvey, ref Result, ref MaxDip, ExtrapToDir: false, IncIsZero: false, DirIsZero: false, ExtrapToInc: false, 0.0, MinDL: false, 0.0, 0.0, 0.0, 0.0, Build: true);
						double num46 = System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Northing, absoluteSurvey.Northing)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Easting, absoluteSurvey.Easting)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.TVD, absoluteSurvey.TVD)), 2.0));
						double num47 = num46 * System.Math.Cos(System.Math.PI / 180.0 * num34);
						double value2 = Convert.ToDouble(absoluteSurvey.MD) + num46;
						double value3 = Convert.ToDouble(absoluteSurvey.TVD) + num47;
						double value4 = Convert.ToDouble(absoluteSurvey.Northing) + num46 * (System.Math.Sin(System.Math.PI / 180.0 * num34) * System.Math.Cos(System.Math.PI / 180.0 * num37));
						double value5 = Convert.ToDouble(absoluteSurvey.Easting) + num46 * (System.Math.Sin(System.Math.PI / 180.0 * num34) * System.Math.Sin(System.Math.PI / 180.0 * num37));
						AbsoluteSurvey setEndOfHoleSurvey = new AbsoluteSurvey(new decimal(System.Math.Round(value2, 2)), new decimal(System.Math.Round(num34, 2)), new decimal(System.Math.Round(num37, 2)), new decimal(System.Math.Round(value3, 2)), new decimal(System.Math.Round(value4, 2)), new decimal(System.Math.Round(value5, 2)), 0m);
						double value6 = System.Math.Round(mdMiscSurveyVariables.WedgeAzi(target, lastSurvey), 2);
						Result.Remove(0);
						BuildAndHoldResults buildAndHoldResults = new BuildAndHoldResults(absoluteSurvey, setEndOfHoleSurvey, new decimal(System.Math.Round(num40, 2)), new decimal(System.Math.Round(num46, 2)), System.Math.Round(lastSurvey.MD, 2), new decimal(System.Math.Round(Convert.ToDouble(lastSurvey.MD) + num40, 2)), Result, new decimal(System.Math.Round(num41, 2)), new decimal(value6), setBuildBelowTarget);
						results = buildAndHoldResults;
						result = BuildToTarget_Errors.NoError;
					}
					else
					{
						mdTrendPlot.TrendPlotTest(mD, num, num20, num21, num22, num2, num3, num4, num26, num27, num28, num39, Convert.ToDouble(lastSurvey.MD), (int)System.Math.Round(a), num42, currentAzi, highSide, Convert.ToInt32(value), Convert.ToDouble(lastSurvey.Dip), ref Result, lastSurvey, BuildExtrapCD);
						num42 = mdFiveMeterBuildPlot.CurrentQuadrant(Convert.ToDouble(Result.LastRecord().Azimuth));
						mD = num40;
						absoluteSurvey = mdTrendPlot.TrendPlotLastSurvey(mD, num, num20, num21, num22, num2, num3, num4, num26, num27, num28, dogLeg, Convert.ToDouble(lastSurvey.MD), (int)System.Math.Round(a), num42, currentAzi);
						Result.Add(absoluteSurvey);
						double num46 = System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Northing, absoluteSurvey.Northing)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Easting, absoluteSurvey.Easting)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.TVD, absoluteSurvey.TVD)), 2.0));
						double num47 = num46 * System.Math.Cos(System.Math.PI / 180.0 * num34);
						double value2 = Convert.ToDouble(absoluteSurvey.MD) + num46;
						double value3 = Convert.ToDouble(absoluteSurvey.TVD) + num47;
						double value4 = Convert.ToDouble(absoluteSurvey.Northing) + num46 * (System.Math.Sin(System.Math.PI / 180.0 * num34) * System.Math.Cos(System.Math.PI / 180.0 * num37));
						double value5 = Convert.ToDouble(absoluteSurvey.Easting) + num46 * (System.Math.Sin(System.Math.PI / 180.0 * num34) * System.Math.Sin(System.Math.PI / 180.0 * num37));
						AbsoluteSurvey setEndOfHoleSurvey = new AbsoluteSurvey(new decimal(System.Math.Round(value2, 2)), new decimal(System.Math.Round(num34, 2)), new decimal(System.Math.Round(num37, 2)), new decimal(System.Math.Round(value3, 2)), new decimal(System.Math.Round(value4, 2)), new decimal(System.Math.Round(value5, 2)), 0m);
						double value6 = System.Math.Round(mdMiscSurveyVariables.WedgeAzi(target, lastSurvey), 2);
						decimal limit = new decimal(Result.Count() - 2);
						decimal num48 = 1m;
						while (ObjectFlowControl.ForLoopControl.ForNextCheckDec(num48, limit, 1m))
						{
							Result.RecordAt(Convert.ToInt32(num48)).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(Result.RecordAt(Convert.ToInt32(num48)).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(Result.RecordAt(Convert.ToInt32(decimal.Subtract(num48, 1m))).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(Result.RecordAt(Convert.ToInt32(num48)).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(Result.RecordAt(Convert.ToInt32(decimal.Subtract(num48, 1m))).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(Result.RecordAt(Convert.ToInt32(num48)).Azimuth, Result.RecordAt(Convert.ToInt32(decimal.Subtract(num48, 1m))).Azimuth)))));
							Result.RecordAt(Convert.ToInt32(num48)).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(Result.RecordAt(Convert.ToInt32(num48)).DL) / Convert.ToDouble(decimal.Subtract(Result.RecordAt(Convert.ToInt32(num48)).MD, Result.RecordAt(Convert.ToInt32(decimal.Subtract(num48, 1m))).MD)) * 30.0);
							num48 = decimal.Add(num48, 1m);
						}
						int num49 = Result.Count() - 1;
						for (int i = 0; i <= num49; i++)
						{
							Result.RecordAt(i).Azimuth = System.Math.Round(Result.RecordAt(i).Azimuth, 2);
							Result.RecordAt(i).Dip = System.Math.Round(Result.RecordAt(i).Dip, 2);
							Result.RecordAt(i).DL = System.Math.Round(Result.RecordAt(i).DL, 2);
							Result.RecordAt(i).Easting = System.Math.Round(Result.RecordAt(i).Easting, 2);
							Result.RecordAt(i).MD = System.Math.Round(Result.RecordAt(i).MD, 2);
							Result.RecordAt(i).Northing = System.Math.Round(Result.RecordAt(i).Northing, 2);
							Result.RecordAt(i).TVD = System.Math.Round(Result.RecordAt(i).TVD, 2);
						}
						Result.Remove(0);
						BuildAndHoldResults buildAndHoldResults = new BuildAndHoldResults(absoluteSurvey, setEndOfHoleSurvey, new decimal(System.Math.Round(num40, 2)), new decimal(System.Math.Round(num46, 2)), System.Math.Round(lastSurvey.MD, 2), new decimal(System.Math.Round(Convert.ToDouble(lastSurvey.MD) + num40, 2)), Result, new decimal(System.Math.Round(num41, 2)), new decimal(value6), setBuildBelowTarget);
						results = buildAndHoldResults;
						result = BuildToTarget_Errors.NoError;
					}
				}
				catch (OverflowException ex)
				{
					ProjectData.SetProjectError(ex);
					OverflowException ex2 = ex;
					result = BuildToTarget_Errors.DogLegTooSmall;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}

		public StraightPathToTargetTVD_Errors StraightPathToTargetTVD(Target target, AbsoluteSurvey lastSurvey, ref AbsoluteSurvey result)
		{
			StraightPathToTargetTVD_Errors result2;
			try
			{
				decimal num = decimal.Subtract(target.TVD, lastSurvey.TVD);
				if (System.Math.Sign(num) == -1)
				{
					result2 = StraightPathToTargetTVD_Errors.TargetTVDHasAlreadyBeenReached;
				}
				else
				{
					decimal d = new decimal(Convert.ToDouble(num) / System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)));
					decimal d2 = decimal.Add(d, lastSurvey.MD);
					decimal d3 = decimal.Add(lastSurvey.TVD, num);
					decimal d4 = new decimal(Convert.ToDouble(lastSurvey.Northing) + Convert.ToDouble(decimal.Divide(d, 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth))));
					decimal d5 = new decimal(Convert.ToDouble(lastSurvey.Easting) + Convert.ToDouble(decimal.Divide(d, 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth))));
					AbsoluteSurvey absoluteSurvey = new AbsoluteSurvey(System.Math.Round(d2, 2), System.Math.Round(lastSurvey.Dip, 2), System.Math.Round(lastSurvey.Azimuth, 2), System.Math.Round(d3, 2), System.Math.Round(d4, 2), System.Math.Round(d5, 2), 0m);
					result = absoluteSurvey;
					result2 = StraightPathToTargetTVD_Errors.NoError;
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				result2 = StraightPathToTargetTVD_Errors.TargetTVDHasAlreadyBeenReached;
				ProjectData.ClearProjectError();
			}
			return result2;
		}

		public DistanceFromTarget_Errors DistanceFromTarget(Target target, AbsoluteSurvey survey, ref RelativePosition result)
		{
			DistanceFromTarget_Errors result2;
			try
			{
				decimal num = decimal.Subtract(survey.Northing, target.Northing);
				NorthingDirectionEnum setNorthingDirection = default;
				decimal d = default;
				if (System.Math.Sign(num) == -1)
				{
					setNorthingDirection = NorthingDirectionEnum.South;
					d = decimal.Multiply(num, -1m);
				}
				else if (System.Math.Sign(num) == 1)
				{
					setNorthingDirection = NorthingDirectionEnum.North;
					d = num;
				}
				decimal num2 = decimal.Subtract(survey.Easting, target.Easting);
				EastingDirectionEnum setEastingDirection = default;
				decimal d2 = default;
				if (System.Math.Sign(num2) == -1)
				{
					setEastingDirection = EastingDirectionEnum.West;
					d2 = decimal.Multiply(num2, -1m);
				}
				else if (System.Math.Sign(num2) == 1)
				{
					setEastingDirection = EastingDirectionEnum.East;
					d2 = num2;
				}
				decimal num3 = decimal.Subtract(survey.TVD, target.TVD);
				VerticalDirectionEnum setVerticalDirection = default;
				decimal d3 = default;
				if (System.Math.Sign(num3) == 0)
				{
					setVerticalDirection = VerticalDirectionEnum.Null;
					d3 = num3;
				}
				else if (System.Math.Sign(num3) == -1)
				{
					setVerticalDirection = VerticalDirectionEnum.Above;
					d3 = decimal.Multiply(num3, -1m);
				}
				else if (System.Math.Sign(num3) == 1)
				{
					setVerticalDirection = VerticalDirectionEnum.Below;
					d3 = num3;
				}
				decimal d4 = new decimal(System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(num), 2.0) + System.Math.Pow(Convert.ToDouble(num2), 2.0)));
				result = new RelativePosition(System.Math.Round(d, 2), setNorthingDirection, System.Math.Round(d2, 2), setEastingDirection, System.Math.Round(d3, 2), setVerticalDirection, System.Math.Round(d4, 2));
				result2 = DistanceFromTarget_Errors.NoError;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				result2 = DistanceFromTarget_Errors.FunctionInputParameterError;
				ProjectData.ClearProjectError();
			}
			return result2;
		}

		public CheckMDValue_Errors CheckMDValue(AbsoluteSurveyList AbsoluteSurveyList, double depth)
		{
			CheckMDValue_Errors result;
			try
			{
				result = ((!(depth > Convert.ToDouble(AbsoluteSurveyList.RecordAt(checked(AbsoluteSurveyList.Count() - 1)).MD))) ? CheckMDValue_Errors.NoError : CheckMDValue_Errors.CheckMDValue);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				result = CheckMDValue_Errors.NoError;
				ProjectData.ClearProjectError();
			}
			return result;
		}

		public CheckIfSurveyExistsMD_Errors CheckIfSurveyExists_MD(AbsoluteSurveyList AbsoluteSurveyList, double depth, ref int result, ref int SurveyAboveIndex, ref int SurveyBelowIndex, ref AbsoluteSurvey extrapolatedresult)
		{
			checked
			{
				CheckIfSurveyExistsMD_Errors result2 = default;
				try
				{
					int num = AbsoluteSurveyList.Count();
					bool flag = false;
					int num2 = AbsoluteSurveyList.Count() - 1;
					int num3 = 0;
					int num6 = default;
					while (true)
					{
						if (num3 <= num2)
						{
							int num4 = Convert.ToInt32(AbsoluteSurveyList.RecordAt(num3).MD);
							if ((double)num4 == depth)
							{
								result = num3;
								extrapolatedresult = AbsoluteSurveyList.RecordAt(num3);
								flag = true;
								result2 = CheckIfSurveyExistsMD_Errors.SurveyExists;
								break;
							}
							num3++;
							continue;
						}
						if (flag)
						{
							break;
						}
						int num5 = AbsoluteSurveyList.Count() - 1;
						for (num3 = 0; num3 <= num5; num3++)
						{
							if (Convert.ToDouble(AbsoluteSurveyList.RecordAt(num3).MD) < depth)
							{
								decimal mD = AbsoluteSurveyList.RecordAt(num3).MD;
								num6 = num3;
							}
						}
						SurveyAboveIndex = num6;
						SurveyBelowIndex = num6 + 1;
						result2 = CheckIfSurveyExistsMD_Errors.NoSurveyExists;
						break;
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					result2 = CheckIfSurveyExistsMD_Errors.AnError;
					ProjectData.ClearProjectError();
				}
				return result2;
			}
		}

		public ExtrapolateBetweenSurveys_Errors ExtrapolateBetweenSurveys_MD(AbsoluteSurvey surveyAbove, AbsoluteSurvey surveyBelow, decimal depth, ref AbsoluteSurvey result)
		{
			ExtrapolateBetweenSurveys_Errors result2;
			try
			{
				decimal d = decimal.Subtract(depth, surveyAbove.MD);
				decimal d2 = decimal.Subtract(surveyBelow.MD, surveyAbove.MD);
				decimal num = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Azimuth)));
				decimal num2 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Azimuth)));
				decimal num3 = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Dip)));
				decimal d3 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyBelow.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(surveyBelow.Azimuth)));
				decimal d4 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyBelow.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyBelow.Azimuth)));
				decimal d5 = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(surveyBelow.Dip)));
				decimal num4 = decimal.Add(decimal.Multiply(decimal.Subtract(1m, decimal.Divide(d, d2)), num), decimal.Multiply(decimal.Divide(d, d2), d3));
				decimal num5 = decimal.Add(decimal.Multiply(decimal.Subtract(1m, decimal.Divide(d, d2)), num2), decimal.Multiply(decimal.Divide(d, d2), d4));
				decimal num6 = decimal.Add(decimal.Multiply(decimal.Subtract(1m, decimal.Divide(d, d2)), num3), decimal.Multiply(decimal.Divide(d, d2), d5));
				decimal num7 = decimal.Multiply(decimal.Divide(d, 2m), decimal.Add(num, num4));
				decimal num8 = decimal.Multiply(decimal.Divide(d, 2m), decimal.Add(num2, num5));
				decimal num9 = decimal.Multiply(decimal.Divide(d, 2m), decimal.Add(num3, num6));
				double value = Convert.ToDouble(decimal.Subtract(surveyBelow.Northing, surveyAbove.Northing));
				double value2 = Convert.ToDouble(decimal.Subtract(surveyBelow.Easting, surveyAbove.Easting));
				int num10 = default;
				if ((System.Math.Sign(value) == 1) & (System.Math.Sign(value2) == 1))
				{
					num10 = 1;
				}
				else if ((System.Math.Sign(value) == 1) & (System.Math.Sign(value2) == -1))
				{
					num10 = 2;
				}
				else if ((System.Math.Sign(value) == -1) & (System.Math.Sign(value2) == -1))
				{
					num10 = 3;
				}
				else if ((System.Math.Sign(value) == -1) & (System.Math.Sign(value2) == 1))
				{
					num10 = 4;
				}
				int num11 = mdTrendPlot.CurrentQuadrant(Convert.ToDouble(surveyAbove.Azimuth));
				double d6 = Convert.ToDouble(decimal.Divide(num5, num4));
				double num12 = 180.0 / System.Math.PI * System.Math.Atan(d6);
				decimal num13 = default;
				if ((num10 == 1) & (System.Math.Sign(num12) == 1) & (num11 == 1))
				{
					num13 = new decimal(num12);
				}
				else if ((num10 == 1) & (System.Math.Sign(num12) == -1) & (num11 == 1))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 1) & (System.Math.Sign(num12) == 1) & (num11 == 2))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 1) & (System.Math.Sign(num12) == -1) & (num11 == 2))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 1) & (System.Math.Sign(num12) == 1) & (num11 == 3))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 1) & (System.Math.Sign(num12) == -1) & (num11 == 3))
				{
					num13 = new decimal(270.0 + num12);
				}
				else if ((num10 == 1) & (System.Math.Sign(num12) == 1) & (num11 == 4))
				{
					num13 = new decimal(num12);
				}
				else if ((num10 == 1) & (System.Math.Sign(num12) == -1) & (num11 == 4))
				{
					num13 = new decimal(360.0 + num12);
				}
				else if ((num10 == 2) & (System.Math.Sign(num12) == -1) & (num11 == 2))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 2) & (System.Math.Sign(num12) == -1) & (num11 == 3))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 2) & (System.Math.Sign(num12) == 1) & (num11 == 3))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 2) & (System.Math.Sign(num12) == 1) & (num11 == 4))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 2) & (System.Math.Sign(num12) == -1) & (num11 == 4))
				{
					num13 = new decimal(360.0 + num12);
				}
				else if ((num10 == 3) & (System.Math.Sign(num12) == 1) & (num11 == 1))
				{
					num13 = new decimal(num12);
				}
				else if ((num10 == 3) & (System.Math.Sign(num12) == -1) & (num11 == 1))
				{
					num13 = new decimal(90.0 + num12);
				}
				else if ((num10 == 3) & (System.Math.Sign(num12) == 1) & (num11 == 2))
				{
					num13 = new decimal(90.0 + num12);
				}
				else if ((num10 == 3) & (System.Math.Sign(num12) == -1) & (num11 == 2))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 3) & (System.Math.Sign(num12) == 1) & (num11 == 3))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 3) & (System.Math.Sign(num12) == -1) & (num11 == 3))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 3) & (System.Math.Sign(num12) == 1) & (num11 == 4))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 3) & (System.Math.Sign(num12) == -1) & (num11 == 4))
				{
					num13 = new decimal(360.0 + num12);
				}
				else if ((num10 == 4) & (System.Math.Sign(num12) == 1) & (num11 == 1))
				{
					num13 = new decimal(num12);
				}
				else if ((num10 == 4) & (System.Math.Sign(num12) == -1) & (num11 == 1))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 4) & (System.Math.Sign(num12) == -1) & (num11 == 2))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 4) & (System.Math.Sign(num12) == 1) & (num11 == 3))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 4) & (System.Math.Sign(num12) == -1) & (num11 == 3))
				{
					num13 = new decimal(180.0 + num12);
				}
				else if ((num10 == 4) & (System.Math.Sign(num12) == 1) & (num11 == 4))
				{
					num13 = new decimal(num12);
				}
				else if ((num10 == 4) & (System.Math.Sign(num12) == -1) & (num11 == 4))
				{
					num13 = new decimal(360.0 + num12);
				}
				double d7 = System.Math.Pow(System.Math.Pow(Convert.ToDouble(num4), 2.0) + System.Math.Pow(Convert.ToDouble(num5), 2.0), 0.5) / Convert.ToDouble(num6);
				decimal num14 = new decimal(180.0 / System.Math.PI * System.Math.Atan(d7));
				double num15 = System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(num14)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(num14)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(num13, surveyAbove.Azimuth))));
				double num16 = 180.0 / System.Math.PI * num15;
				if (num16 == 0.0)
				{
					num7 = new decimal(Convert.ToDouble(surveyAbove.Northing) + Convert.ToDouble(decimal.Divide(decimal.Subtract(depth, surveyAbove.MD), 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(num14)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(num13))));
					num8 = new decimal(Convert.ToDouble(surveyAbove.Easting) + Convert.ToDouble(decimal.Divide(decimal.Subtract(depth, surveyAbove.MD), 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(num14)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(num13))));
					num9 = new decimal(Convert.ToDouble(decimal.Divide(decimal.Subtract(depth, surveyAbove.MD), 2m)) * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Dip)) + System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(num14))));
				}
				else
				{
					double num17 = 2.0 / (System.Math.PI / 180.0 * num16) * System.Math.Tan(System.Math.PI / 180.0 * num16 / 2.0);
					num7 = new decimal(Convert.ToDouble(surveyAbove.Northing) + Convert.ToDouble(decimal.Multiply(decimal.Divide(d, 2m), decimal.Add(num, num4))) * num17);
					num8 = new decimal(Convert.ToDouble(surveyAbove.Easting) + Convert.ToDouble(decimal.Multiply(decimal.Divide(d, 2m), decimal.Add(num2, num5))) * num17);
					num9 = new decimal(Convert.ToDouble(surveyAbove.TVD) + Convert.ToDouble(decimal.Multiply(decimal.Divide(d, 2m), decimal.Add(num3, num6))) * num17);
				}
				AbsoluteSurvey absoluteSurvey = new AbsoluteSurvey(System.Math.Round(depth, 2), System.Math.Round(num14, 2), System.Math.Round(num13, 2), System.Math.Round(num9, 2), System.Math.Round(num7, 2), System.Math.Round(num8, 2), 0m);
				result = absoluteSurvey;
				result2 = ExtrapolateBetweenSurveys_Errors.NoError;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				result2 = ExtrapolateBetweenSurveys_Errors.InvalidMD;
				ProjectData.ClearProjectError();
			}
			return result2;
		}

		public double ConvertTVDToMD(double TVD, double SurveyAboveDip, double SurveyBelowDip)
		{
			return System.Math.Round(2.0 * (TVD / (System.Math.Cos(System.Math.PI / 180.0 * SurveyAboveDip) + System.Math.Cos(System.Math.PI / 180.0 * SurveyBelowDip))), 7);
		}

		public CheckIfSurveyExistsTVD_Errors CheckIfSurveyExists_TVD(AbsoluteSurveyList AbsoluteSurveyList, double depth, ref int result, ref int SurveyAboveIndex, ref int SurveyBelowIndex, ref AbsoluteSurvey extrapolatedresult)
		{
			checked
			{
				CheckIfSurveyExistsTVD_Errors result2 = default;
				try
				{
					int num = AbsoluteSurveyList.Count();
					bool flag = false;
					int num2 = AbsoluteSurveyList.Count() - 1;
					int num3 = 0;
					int num6 = default;
					while (true)
					{
						if (num3 <= num2)
						{
							double num4 = Convert.ToDouble(AbsoluteSurveyList.RecordAt(num3).TVD);
							if (num4 == depth)
							{
								result = num3;
								extrapolatedresult = AbsoluteSurveyList.RecordAt(num3);
								flag = true;
								result2 = CheckIfSurveyExistsTVD_Errors.SurveyExists;
								break;
							}
							num3++;
							continue;
						}
						if (flag)
						{
							break;
						}
						int num5 = AbsoluteSurveyList.Count() - 1;
						for (num3 = 0; num3 <= num5; num3++)
						{
							if (Convert.ToDouble(AbsoluteSurveyList.RecordAt(num3).TVD) < depth)
							{
								decimal tVD = AbsoluteSurveyList.RecordAt(num3).TVD;
								num6 = num3;
							}
						}
						SurveyAboveIndex = num6;
						SurveyBelowIndex = num6 + 1;
						result2 = CheckIfSurveyExistsTVD_Errors.NoSurveyExists;
						break;
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					result2 = CheckIfSurveyExistsTVD_Errors.AnError;
					ProjectData.ClearProjectError();
				}
				return result2;
			}
		}

		public ExtrapolateBetweenSurveys_TVD_Errors ExtrapolateBetweenSurveys_TVD(AbsoluteSurvey surveyAbove, AbsoluteSurvey surveyBelow, decimal depth, ref AbsoluteSurvey result)
		{
			double num = Convert.ToDouble(decimal.Subtract(depth, surveyAbove.TVD));
			double num2 = System.Math.Round(2.0 * (num / (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Dip)) + System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(surveyBelow.Dip)))), 7);
			ExtrapolateBetweenSurveys_TVD_Errors result2;
			try
			{
				decimal d = new decimal(num2);
				decimal d2 = decimal.Subtract(surveyBelow.MD, surveyAbove.MD);
				decimal num3 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Azimuth)));
				decimal num4 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Azimuth)));
				decimal num5 = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(surveyAbove.Dip)));
				decimal d3 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyBelow.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(surveyBelow.Azimuth)));
				decimal d4 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyBelow.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(surveyBelow.Azimuth)));
				decimal d5 = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(surveyBelow.Dip)));
				decimal num6 = decimal.Add(decimal.Multiply(decimal.Subtract(1m, decimal.Divide(d, d2)), num3), decimal.Multiply(decimal.Divide(d, d2), d3));
				decimal num7 = decimal.Add(decimal.Multiply(decimal.Subtract(1m, decimal.Divide(d, d2)), num4), decimal.Multiply(decimal.Divide(d, d2), d4));
				decimal num8 = decimal.Add(decimal.Multiply(decimal.Subtract(1m, decimal.Divide(d, d2)), num5), decimal.Multiply(decimal.Divide(d, d2), d5));
				decimal d6 = decimal.Add(surveyAbove.Northing, decimal.Multiply(decimal.Divide(d, 2m), decimal.Add(num3, num6)));
				decimal d7 = decimal.Add(surveyAbove.Easting, decimal.Multiply(decimal.Divide(d, 2m), decimal.Add(num4, num7)));
				decimal d8 = decimal.Add(surveyAbove.TVD, decimal.Multiply(decimal.Divide(d, 2m), decimal.Add(num5, num8)));
				double d9 = Convert.ToDouble(decimal.Divide(num7, num6));
				double num9 = 180.0 / System.Math.PI * System.Math.Atan(d9);
				double value = Convert.ToDouble(decimal.Subtract(surveyBelow.Northing, surveyAbove.Northing));
				double value2 = Convert.ToDouble(decimal.Subtract(surveyBelow.Easting, surveyAbove.Easting));
				int num10 = default;
				if ((System.Math.Sign(value) == 1) & (System.Math.Sign(value2) == 1))
				{
					num10 = 1;
				}
				else if ((System.Math.Sign(value) == 1) & (System.Math.Sign(value2) == -1))
				{
					num10 = 2;
				}
				else if ((System.Math.Sign(value) == -1) & (System.Math.Sign(value2) == -1))
				{
					num10 = 3;
				}
				else if ((System.Math.Sign(value) == -1) & (System.Math.Sign(value2) == 1))
				{
					num10 = 4;
				}
				int num11 = mdTrendPlot.CurrentQuadrant(Convert.ToDouble(surveyAbove.Azimuth));
				d9 = Convert.ToDouble(decimal.Divide(num7, num6));
				num9 = 180.0 / System.Math.PI * System.Math.Atan(d9);
				decimal d10 = default;
				if ((num10 == 1) & (System.Math.Sign(num9) == 1) & (num11 == 1))
				{
					d10 = new decimal(num9);
				}
				else if ((num10 == 1) & (System.Math.Sign(num9) == -1) & (num11 == 1))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 1) & (System.Math.Sign(num9) == 1) & (num11 == 2))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 1) & (System.Math.Sign(num9) == -1) & (num11 == 2))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 1) & (System.Math.Sign(num9) == 1) & (num11 == 3))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 1) & (System.Math.Sign(num9) == -1) & (num11 == 3))
				{
					d10 = new decimal(270.0 + num9);
				}
				else if ((num10 == 1) & (System.Math.Sign(num9) == 1) & (num11 == 4))
				{
					d10 = new decimal(num9);
				}
				else if ((num10 == 1) & (System.Math.Sign(num9) == -1) & (num11 == 4))
				{
					d10 = new decimal(360.0 + num9);
				}
				else if ((num10 == 2) & (System.Math.Sign(num9) == -1) & (num11 == 2))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 2) & (System.Math.Sign(num9) == -1) & (num11 == 3))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 2) & (System.Math.Sign(num9) == 1) & (num11 == 3))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 2) & (System.Math.Sign(num9) == 1) & (num11 == 4))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 2) & (System.Math.Sign(num9) == -1) & (num11 == 4))
				{
					d10 = new decimal(360.0 + num9);
				}
				else if ((num10 == 3) & (System.Math.Sign(num9) == 1) & (num11 == 1))
				{
					d10 = new decimal(num9);
				}
				else if ((num10 == 3) & (System.Math.Sign(num9) == -1) & (num11 == 1))
				{
					d10 = new decimal(90.0 + num9);
				}
				else if ((num10 == 3) & (System.Math.Sign(num9) == 1) & (num11 == 2))
				{
					d10 = new decimal(90.0 + num9);
				}
				else if ((num10 == 3) & (System.Math.Sign(num9) == -1) & (num11 == 2))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 3) & (System.Math.Sign(num9) == 1) & (num11 == 3))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 3) & (System.Math.Sign(num9) == -1) & (num11 == 3))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 3) & (System.Math.Sign(num9) == 1) & (num11 == 4))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 3) & (System.Math.Sign(num9) == -1) & (num11 == 4))
				{
					d10 = new decimal(360.0 + num9);
				}
				else if ((num10 == 4) & (System.Math.Sign(num9) == 1) & (num11 == 1))
				{
					d10 = new decimal(num9);
				}
				else if ((num10 == 4) & (System.Math.Sign(num9) == -1) & (num11 == 1))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 4) & (System.Math.Sign(num9) == -1) & (num11 == 2))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 4) & (System.Math.Sign(num9) == 1) & (num11 == 3))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 4) & (System.Math.Sign(num9) == -1) & (num11 == 3))
				{
					d10 = new decimal(180.0 + num9);
				}
				else if ((num10 == 4) & (System.Math.Sign(num9) == 1) & (num11 == 4))
				{
					d10 = new decimal(num9);
				}
				else if ((num10 == 4) & (System.Math.Sign(num9) == -1) & (num11 == 4))
				{
					d10 = new decimal(360.0 + num9);
				}
				double d11 = System.Math.Pow(System.Math.Pow(Convert.ToDouble(num6), 2.0) + System.Math.Pow(Convert.ToDouble(num7), 2.0), 0.5) / Convert.ToDouble(num8);
				decimal d12 = new decimal(180.0 / System.Math.PI * System.Math.Atan(d11));
				decimal d13 = new decimal(Convert.ToDouble(surveyAbove.MD) + num2);
				AbsoluteSurvey absoluteSurvey = new AbsoluteSurvey(System.Math.Round(d13, 2), System.Math.Round(d12, 2), System.Math.Round(d10, 2), System.Math.Round(d8, 2), System.Math.Round(d6, 2), System.Math.Round(d7, 2), 0m);
				result = absoluteSurvey;
				result2 = ExtrapolateBetweenSurveys_TVD_Errors.NoError;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				result2 = ExtrapolateBetweenSurveys_TVD_Errors.InvalidTVD;
				ProjectData.ClearProjectError();
			}
			return result2;
		}

		public PlannedSurveyAtMD_Errors PlannedSurveyAtMD(AbsoluteSurvey collar, Target target, decimal md, ref AbsoluteSurvey result)
		{
			PlannedSurveyAtMD_Errors result2;
			try
			{
				decimal num = new decimal(System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Northing, collar.Northing)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.Easting, collar.Easting)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(target.TVD, 0m)), 2.0)));
				decimal d = decimal.Subtract(target.Northing, collar.Northing);
				decimal d2 = decimal.Subtract(target.Easting, collar.Easting);
				decimal value = new decimal(System.Math.Atan(Convert.ToDouble(decimal.Divide(d2, d))));
				value = new decimal(180.0 / System.Math.PI * Convert.ToDouble(value));
				if (System.Math.Sign(value) == -1)
				{
					value = decimal.Add(360m, value);
				}
				decimal num2 = new decimal(180.0 / System.Math.PI * System.Math.Acos(Convert.ToDouble(decimal.Divide(target.TVD, num))));
				decimal d3 = num;
				decimal num3 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(num2)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(value)));
				decimal num4 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(num2)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(value)));
				decimal num5 = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(num2)));
				decimal d4 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(num2)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(value)));
				decimal d5 = new decimal(System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(num2)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(value)));
				decimal d6 = new decimal(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(num2)));
				decimal d7 = decimal.Add(decimal.Multiply(decimal.Subtract(1m, decimal.Divide(md, d3)), num3), decimal.Multiply(decimal.Divide(md, d3), d4));
				decimal d8 = decimal.Add(decimal.Multiply(decimal.Subtract(1m, decimal.Divide(md, d3)), num4), decimal.Multiply(decimal.Divide(md, d3), d5));
				decimal d9 = decimal.Add(decimal.Multiply(decimal.Subtract(1m, decimal.Divide(md, d3)), num5), decimal.Multiply(decimal.Divide(md, d3), d6));
				decimal d10 = decimal.Add(collar.Northing, decimal.Multiply(decimal.Divide(md, 2m), decimal.Add(num3, d7)));
				decimal d11 = decimal.Add(collar.Easting, decimal.Multiply(decimal.Divide(md, 2m), decimal.Add(num4, d8)));
				decimal d12 = decimal.Add(collar.TVD, decimal.Multiply(decimal.Divide(md, 2m), decimal.Add(num5, d9)));
				AbsoluteSurvey absoluteSurvey = new AbsoluteSurvey(System.Math.Round(md, 2), System.Math.Round(num2, 2), System.Math.Round(value, 2), System.Math.Round(d12, 2), System.Math.Round(d10, 2), System.Math.Round(d11, 2), 0m);
				result = absoluteSurvey;
				result2 = PlannedSurveyAtMD_Errors.NoError;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				result2 = PlannedSurveyAtMD_Errors.CheckMDValue;
				ProjectData.ClearProjectError();
			}
			return result2;
		}

		public DistanceFromPlan_Errors DistanceFromStraightPathPlan(AbsoluteSurvey PlannedSurvey, AbsoluteSurvey survey, ref RelativePosition result)
		{
			DistanceFromPlan_Errors result2;
			try
			{
				RelativePosition relativePosition = new RelativePosition();
				relativePosition.NorthingDistance = decimal.Subtract(PlannedSurvey.Northing, survey.Northing);
				decimal d = default;
				if (System.Math.Sign(relativePosition.NorthingDistance) == -1)
				{
					relativePosition.NorthingDirection = NorthingDirectionEnum.South;
					d = decimal.Multiply(relativePosition.NorthingDistance, -1m);
				}
				else if (System.Math.Sign(relativePosition.NorthingDistance) == 1)
				{
					relativePosition.NorthingDirection = NorthingDirectionEnum.North;
					d = relativePosition.NorthingDistance;
				}
				else if (System.Math.Sign(relativePosition.NorthingDistance) == 0)
				{
					relativePosition.NorthingDirection = NorthingDirectionEnum.Null;
					d = relativePosition.NorthingDistance;
				}
				relativePosition.EastingDistance = decimal.Subtract(PlannedSurvey.Easting, survey.Easting);
				decimal d2 = default;
				if (System.Math.Sign(relativePosition.EastingDistance) == -1)
				{
					relativePosition.EastingDirection = EastingDirectionEnum.West;
					d2 = decimal.Multiply(relativePosition.EastingDistance, -1m);
				}
				else if (System.Math.Sign(relativePosition.EastingDistance) == 1)
				{
					relativePosition.EastingDirection = EastingDirectionEnum.East;
					d2 = relativePosition.EastingDistance;
				}
				else if (System.Math.Sign(relativePosition.EastingDistance) == 0)
				{
					relativePosition.EastingDirection = EastingDirectionEnum.Null;
					d2 = relativePosition.EastingDistance;
				}
				relativePosition.HorizontalDeparture = new decimal(System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(decimal.Subtract(PlannedSurvey.Northing, survey.Northing)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(PlannedSurvey.Easting, survey.Easting)), 2.0)));
				relativePosition.VerticalDistance = decimal.Subtract(PlannedSurvey.TVD, survey.TVD);
				decimal d3 = default;
				if (System.Math.Sign(relativePosition.VerticalDistance) == -1)
				{
					relativePosition.VerticalDirection = VerticalDirectionEnum.Above;
					d3 = decimal.Multiply(relativePosition.VerticalDistance, -1m);
				}
				else if (System.Math.Sign(relativePosition.VerticalDistance) == 1)
				{
					relativePosition.VerticalDirection = VerticalDirectionEnum.Below;
					d3 = relativePosition.VerticalDistance;
				}
				else if (System.Math.Sign(relativePosition.VerticalDistance) == 0)
				{
					relativePosition.VerticalDirection = VerticalDirectionEnum.Null;
					d3 = relativePosition.VerticalDistance;
				}
				relativePosition = new RelativePosition(System.Math.Round(d, 2), relativePosition.NorthingDirection, System.Math.Round(d2, 2), relativePosition.EastingDirection, System.Math.Round(d3, 2), relativePosition.VerticalDirection, System.Math.Round(relativePosition.HorizontalDeparture, 2));
				result = relativePosition;
				result2 = DistanceFromPlan_Errors.NoError;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				result2 = DistanceFromPlan_Errors.AnError;
				ProjectData.ClearProjectError();
			}
			return result2;
		}

		public TrendAnalysis_Errors TrendAnalysisTest(Target Target, AbsoluteSurveyList selectedSurveyData, ref TrendAnalysisResults results, AbsoluteSurvey TrendLastSurvey, double interval)
		{
			if (System.Math.Sign(interval) == -1)
			{
				return TrendAnalysis_Errors.InvalidInterval;
			}
			AbsoluteSurvey absoluteSurvey = new AbsoluteSurvey();
			AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
			AbsoluteSurveyList absoluteSurveyList2 = new AbsoluteSurveyList();
			checked
			{
				int index = selectedSurveyData.Count() - 1;
				double num = System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.LastRecord().Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.RecordAt(0).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.LastRecord().Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.RecordAt(0).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(selectedSurveyData.LastRecord().Azimuth, selectedSurveyData.RecordAt(0).Azimuth))));
				double dogLeg = 180.0 / System.Math.PI * num;
				double value = 180.0 / System.Math.PI * (num / Convert.ToDouble(decimal.Subtract(selectedSurveyData.LastRecord().MD, selectedSurveyData.RecordAt(0).MD)) * 30.0);
				double d = (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.RecordAt(0).Dip)) * System.Math.Cos(num) - System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.LastRecord().Dip))) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.RecordAt(0).Dip)) * System.Math.Sin(num));
				d = System.Math.Acos(d) * (180.0 / System.Math.PI) + 180.0;
				d = mdTrendPlot.FinalToolFaceSetting(selectedSurveyData.RecordAt(0), Convert.ToDouble(selectedSurveyData.LastRecord().Dip), Convert.ToDouble(selectedSurveyData.LastRecord().Azimuth), dogLeg);
				TrendAnalysis_Errors result2;
				try
				{
					decimal num2 = new decimal(selectedSurveyData.Count() - 1);
					decimal num3 = decimal.Subtract(selectedSurveyData.LastRecord().MD, selectedSurveyData.RecordAt(0).MD);
					decimal num4 = decimal.Subtract(selectedSurveyData.LastRecord().TVD, selectedSurveyData.RecordAt(0).TVD);
					decimal num5 = decimal.Subtract(selectedSurveyData.LastRecord().Dip, selectedSurveyData.RecordAt(0).Dip);
					double num6 = Convert.ToDouble(decimal.Divide(num5, num3));
					double value2 = num6 * 30.0;
					object obj = decimal.Divide(num5, num4);
					double num7 = Convert.ToDouble(decimal.Subtract(selectedSurveyData.LastRecord().Azimuth, selectedSurveyData.RecordAt(0).Azimuth));
					double num8 = num7 / Convert.ToDouble(num3);
					double value3 = num8 * 30.0;
					object obj2 = num7 / Convert.ToDouble(num4);
					double num9 = Convert.ToDouble(decimal.Subtract(Target.TVD, TrendLastSurvey.TVD));
					AzimuthTrend setAzimuthTrend = ((System.Math.Sign(num7) == -1) ? AzimuthTrend.South : ((System.Math.Sign(num7) != 1) ? AzimuthTrend.NoTrend : AzimuthTrend.North));
					InclinationTrend setInclinationTrend = ((System.Math.Sign(num5) == -1) ? InclinationTrend.Drop : ((System.Math.Sign(num5) != 1) ? InclinationTrend.NoTrend : InclinationTrend.Build));
					AbsoluteSurvey result = new AbsoluteSurvey();
					if ((decimal.Compare(num5, 0m) == 0) & (num7 == 0.0))
					{
						StraightPathToTargetTVD(Target, selectedSurveyData.RecordAt(index), ref result);
						absoluteSurvey = new AbsoluteSurvey(System.Math.Round(result.MD, 2), System.Math.Round(result.Dip, 2), System.Math.Round(result.Azimuth, 2), System.Math.Round(result.TVD, 2), System.Math.Round(result.Northing, 2), System.Math.Round(result.Easting, 2), System.Math.Round(result.DL, 2));
						absoluteSurveyList.Add(absoluteSurvey);
						double value4 = default;
						results = new TrendAnalysisResults(0m, 0m, new decimal(System.Math.Round(value4, 2)), AzimuthTrend.NoTrend, 0m, new decimal(System.Math.Round(num, 2)), 0m, absoluteSurveyList, InclinationTrend.NoTrend);
						result2 = TrendAnalysis_Errors.NoError;
					}
					else
					{
						double num10 = Convert.ToDouble(TrendLastSurvey.Dip);
						double num11 = Convert.ToDouble(TrendLastSurvey.Azimuth);
						double num12 = num6;
						double num13 = num8;
						int num14 = 1;
						int num15 = 1;
						double num16 = 0.0;
						double num17 = 0.0;
						num14 = 0;
						double num18 = Convert.ToDouble(TrendLastSurvey.TVD);
						absoluteSurveyList.Add(new AbsoluteSurvey(TrendLastSurvey.MD, TrendLastSurvey.Dip, TrendLastSurvey.Azimuth, TrendLastSurvey.TVD, TrendLastSurvey.Northing, TrendLastSurvey.Easting, 0m));
						double num19 = 180.0 / System.Math.PI * (num / Convert.ToDouble(decimal.Subtract(selectedSurveyData.LastRecord().MD, selectedSurveyData.RecordAt(0).MD)));
						double num22 = default;
						double num24 = default;
						double num25 = default;
						double num23 = default;
						double num31 = default;
						while (true)
						{
							int num26;
							double num27;
							double num29;
							double num34;
							double num30;
							double num32;
							double value7;
							double num35;
							double value8;
							double num36;
							double value9;
							double num37;
							double value10;
							double value11;
							if (!(num18 > Convert.ToDouble(Target.TVD)))
							{
								double num20 = num6 * (double)num15;
								double num21 = num8 * (double)num15;
								num22 = num19 * (double)num15;
								num17 = Convert.ToDouble(absoluteSurveyList.RecordAt(0).Dip) + num20;
								double value5;
								double value6;
								if ((num17 > 90.0) | (num17 < -90.0))
								{
									num23 = Convert.ToDouble(absoluteSurveyList.RecordAt(0).Azimuth) + num21;
									if (num23 > 360.0)
									{
										num23 = mdTrendPlot.FixRange(num23, 0.0, 360.0);
									}
									else if (System.Math.Sign(num24) == -1)
									{
										num23 = Convert.ToDouble(decimal.Subtract(decimal.Add(absoluteSurveyList.RecordAt(0).Azimuth, 180m), 360m)) + num24;
									}
									num18 += ((double)num15 - num25) / 2.0 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num17));
									value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Northing) + Convert.ToDouble(decimal.Divide(decimal.Subtract(decimal.Add(new decimal(num15), TrendLastSurvey.MD), absoluteSurveyList.RecordAt(num15 - 1).MD), 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num17) * System.Math.Cos(System.Math.PI / 180.0 * num23));
									value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Easting) + Convert.ToDouble(decimal.Divide(decimal.Subtract(decimal.Add(new decimal(num15), TrendLastSurvey.MD), absoluteSurveyList.RecordAt(num15 - 1).MD), 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num17) * System.Math.Sin(System.Math.PI / 180.0 * num23));
									absoluteSurveyList.Add(new AbsoluteSurvey(decimal.Add(new decimal(num15), TrendLastSurvey.MD), new decimal(num17), new decimal(num23), new decimal(num18), new decimal(value5), new decimal(value6), 0m));
									results = new TrendAnalysisResults(new decimal(System.Math.Round(d, 2)), new decimal(System.Math.Round(num19 * 30.0, 2)), new decimal(System.Math.Round(num22, 2)), setAzimuthTrend, new decimal(System.Math.Round(value2, 2)), 0m, new decimal(System.Math.Round(value3, 2)), absoluteSurveyList, setInclinationTrend);
									num26 = absoluteSurveyList.Count();
									num27 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num26 - 1).TVD, absoluteSurveyList.RecordAt(num26 - 2).TVD));
									double num28 = ((System.Math.Sign(num17) != -1) ? Convert.ToDouble(decimal.Subtract(90m, absoluteSurveyList.RecordAt(num26 - 2).Dip)) : Convert.ToDouble(decimal.Subtract(-90m, absoluteSurveyList.RecordAt(num26 - 2).Dip)));
									num29 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num26 - 1).MD, absoluteSurveyList.RecordAt(num26 - 2).MD));
									num30 = num / 30.0 * (num31 - Convert.ToDouble(TrendLastSurvey.MD));
									num32 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num26 - 1).Dip, absoluteSurveyList.RecordAt(num26 - 2).Dip));
									double num33 = num28 / num32;
									num34 = num29 * num33;
									num31 = Convert.ToDouble(absoluteSurveyList.RecordAt(num26 - 2).MD) + num33;
									value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(num26 - 2).Dip) + num32 * num33;
									num35 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num26 - 1).Azimuth, absoluteSurveyList.RecordAt(num26 - 2).Azimuth));
									value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(num26 - 2).Azimuth) + num35 * num33;
									num36 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num26 - 1).Northing, absoluteSurveyList.RecordAt(num26 - 2).Northing));
									value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(num26 - 2).Northing) + num36 * num33;
									num37 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num26 - 1).Easting, absoluteSurveyList.RecordAt(num26 - 2).Easting));
									value10 = Convert.ToDouble(absoluteSurveyList.RecordAt(num26 - 2).Easting) + num37 * num33;
									value11 = Convert.ToDouble(absoluteSurveyList.RecordAt(num26 - 2).TVD) + num27 * num33;
									results.ExtrapolatedSurveyData.LastRecord().MD = new decimal(System.Math.Round(num31, 2));
									results.ExtrapolatedSurveyData.LastRecord().Azimuth = new decimal(System.Math.Round(value8, 2));
									results.ExtrapolatedSurveyData.LastRecord().Dip = new decimal(System.Math.Round(value7, 2));
									results.ExtrapolatedSurveyData.LastRecord().TVD = new decimal(System.Math.Round(value11, 2));
									results.ExtrapolatedSurveyData.LastRecord().Northing = new decimal(System.Math.Round(value9, 2));
									results.ExtrapolatedSurveyData.LastRecord().Easting = new decimal(System.Math.Round(value10, 2));
									if (System.Math.Sign(num17) == -1)
									{
										int num38 = num26 - 1;
										for (int i = 0; i <= num38; i++)
										{
											if (System.Math.Sign(results.ExtrapolatedSurveyData.RecordAt(i).Dip) == -1)
											{
												results.ExtrapolatedSurveyData.RecordAt(i).Dip = decimal.Multiply(results.ExtrapolatedSurveyData.RecordAt(i).Dip, -1m);
											}
										}
									}
									if (interval != 1.0)
									{
										if (interval > num31 - Convert.ToDouble(TrendLastSurvey.MD))
										{
											result2 = TrendAnalysis_Errors.InvalidInterval;
											break;
										}
										absoluteSurveyList = mdTrendPlot.ExtrapolateIntervalSurveys(TrendLastSurvey, results.ExtrapolatedSurveyData, interval);
									}
									int num39 = absoluteSurveyList.Count() - 1;
									for (int j = 1; j <= num39; j++)
									{
										absoluteSurveyList.RecordAt(j).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(j).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(j - 1).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(j).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(j - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(j).Azimuth, absoluteSurveyList.RecordAt(j - 1).Azimuth)))));
										absoluteSurveyList.RecordAt(j).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(absoluteSurveyList.RecordAt(j).DL));
									}
									int num40 = absoluteSurveyList.Count() - 1;
									for (int k = 0; k <= num40; k++)
									{
										absoluteSurveyList.RecordAt(k).Dip = System.Math.Round(absoluteSurveyList.RecordAt(k).Dip, 2);
										absoluteSurveyList.RecordAt(k).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(k).Azimuth, 2);
										absoluteSurveyList.RecordAt(k).DL = System.Math.Round(absoluteSurveyList.RecordAt(k).DL, 2);
										absoluteSurveyList.RecordAt(k).Easting = System.Math.Round(absoluteSurveyList.RecordAt(k).Easting, 2);
										absoluteSurveyList.RecordAt(k).MD = System.Math.Round(absoluteSurveyList.RecordAt(k).MD, 2);
										absoluteSurveyList.RecordAt(k).Northing = System.Math.Round(absoluteSurveyList.RecordAt(k).Northing, 2);
										absoluteSurveyList.RecordAt(k).TVD = System.Math.Round(absoluteSurveyList.RecordAt(k).TVD, 2);
									}
									results.ExtrapolatedSurveyData.Remove(0);
									results.ExtrapolatedSurveyData = absoluteSurveyList;
									result2 = TrendAnalysis_Errors.TargetTVDCannotBeReached;
									break;
								}
								num23 = Convert.ToDouble(absoluteSurveyList.RecordAt(0).Azimuth) + num21;
								if (num23 > 360.0)
								{
									num23 = mdTrendPlot.FixRange(num23, 0.0, 360.0);
								}
								else if (System.Math.Sign(num24) == -1)
								{
									num23 = Convert.ToDouble(decimal.Subtract(decimal.Add(absoluteSurveyList.RecordAt(0).Azimuth, 180m), 360m)) + num24;
								}
								num18 += ((double)num15 - num25) / 2.0 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Dip)) + System.Math.Cos(System.Math.PI / 180.0 * num17));
								value5 = Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Northing) + Convert.ToDouble(decimal.Divide(decimal.Subtract(decimal.Add(new decimal(num15), TrendLastSurvey.MD), absoluteSurveyList.RecordAt(num15 - 1).MD), 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num17) * System.Math.Cos(System.Math.PI / 180.0 * num23));
								value6 = Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Easting) + Convert.ToDouble(decimal.Divide(decimal.Subtract(decimal.Add(new decimal(num15), TrendLastSurvey.MD), absoluteSurveyList.RecordAt(num15 - 1).MD), 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(num15 - 1).Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * num17) * System.Math.Sin(System.Math.PI / 180.0 * num23));
								absoluteSurveyList.Add(new AbsoluteSurvey(decimal.Add(new decimal(num15), TrendLastSurvey.MD), new decimal(num17), new decimal(num23), new decimal(num18), new decimal(value5), new decimal(value6), 0m));
								num25++;
								num14++;
								num15++;
								continue;
							}
							double num41 = 2.0 / (System.Math.PI / 180.0 * num16) * System.Math.Tan(System.Math.PI / 180.0 * num16 / 2.0);
							num26 = absoluteSurveyList.Count();
							num27 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num26 - 1).TVD, absoluteSurveyList.RecordAt(num26 - 2).TVD));
							double num42 = Convert.ToDouble(decimal.Subtract(Target.TVD, absoluteSurveyList.RecordAt(num26 - 2).TVD));
							double num43 = num42 / num27;
							num29 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num26 - 1).MD, absoluteSurveyList.RecordAt(num26 - 2).MD));
							num34 = num29 * num43;
							num31 = Convert.ToDouble(absoluteSurveyList.RecordAt(num26 - 2).MD) + num43;
							num30 = num / 30.0 * (num31 - Convert.ToDouble(TrendLastSurvey.MD));
							num32 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num26 - 1).Dip, absoluteSurveyList.RecordAt(num26 - 2).Dip));
							value7 = Convert.ToDouble(absoluteSurveyList.RecordAt(num26 - 2).Dip) + num32 * num43;
							num35 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num26 - 1).Azimuth, absoluteSurveyList.RecordAt(num26 - 2).Azimuth));
							value8 = Convert.ToDouble(absoluteSurveyList.RecordAt(num26 - 2).Azimuth) + num32 * num43;
							num36 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num26 - 1).Northing, absoluteSurveyList.RecordAt(num26 - 2).Northing));
							value9 = Convert.ToDouble(absoluteSurveyList.RecordAt(num26 - 2).Northing) + num36 * num43;
							num37 = Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(num26 - 1).Easting, absoluteSurveyList.RecordAt(num26 - 2).Easting));
							value10 = Convert.ToDouble(absoluteSurveyList.RecordAt(num26 - 2).Easting) + num37 * num43;
							value11 = Convert.ToDouble(absoluteSurveyList.RecordAt(num26 - 2).TVD) + num27 * num43;
							absoluteSurveyList.LastRecord().MD = new decimal(System.Math.Round(num31, 2));
							absoluteSurveyList.LastRecord().Azimuth = new decimal(System.Math.Round(value8, 2));
							absoluteSurveyList.LastRecord().Dip = new decimal(System.Math.Round(value7, 2));
							absoluteSurveyList.LastRecord().TVD = new decimal(System.Math.Round(value11, 2));
							absoluteSurveyList.LastRecord().Northing = new decimal(System.Math.Round(value9, 2));
							absoluteSurveyList.LastRecord().Easting = new decimal(System.Math.Round(value10, 2));
							int num44 = absoluteSurveyList.Count() - 1;
							for (int l = 1; l <= num44; l++)
							{
								absoluteSurveyList.RecordAt(l).DL = new decimal(System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(l).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(l - 1).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(l).Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(absoluteSurveyList.RecordAt(l - 1).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(absoluteSurveyList.RecordAt(l).Azimuth, absoluteSurveyList.RecordAt(l - 1).Azimuth)))));
								absoluteSurveyList.RecordAt(l).DL = new decimal(180.0 / System.Math.PI * Convert.ToDouble(absoluteSurveyList.RecordAt(l).DL) * 30.0);
							}
							int num45 = absoluteSurveyList.Count() - 1;
							for (int m = 0; m <= num45; m++)
							{
								absoluteSurveyList.RecordAt(m).Azimuth = System.Math.Round(absoluteSurveyList.RecordAt(m).Azimuth, 2);
								absoluteSurveyList.RecordAt(m).Dip = System.Math.Round(absoluteSurveyList.RecordAt(m).Dip, 2);
								absoluteSurveyList.RecordAt(m).DL = System.Math.Round(absoluteSurveyList.RecordAt(m).DL, 2);
								absoluteSurveyList.RecordAt(m).Easting = System.Math.Round(absoluteSurveyList.RecordAt(m).Easting, 2);
								absoluteSurveyList.RecordAt(m).MD = System.Math.Round(absoluteSurveyList.RecordAt(m).MD, 2);
								absoluteSurveyList.RecordAt(m).Northing = System.Math.Round(absoluteSurveyList.RecordAt(m).Northing, 2);
								absoluteSurveyList.RecordAt(m).TVD = System.Math.Round(absoluteSurveyList.RecordAt(m).TVD, 2);
							}
							if (interval != 1.0)
							{
								if (interval > num31 - Convert.ToDouble(TrendLastSurvey.MD))
								{
									result2 = TrendAnalysis_Errors.InvalidInterval;
									break;
								}
								absoluteSurveyList = mdTrendPlot.ExtrapolateIntervalSurveys(TrendLastSurvey, absoluteSurveyList, interval);
							}
							d = mdTrendPlot.FinalToolFaceSetting(TrendLastSurvey, num17, num23, num22);
							absoluteSurveyList.Remove(0);
							results = new TrendAnalysisResults(new decimal(System.Math.Round(d, 2)), new decimal(System.Math.Round(num19 * 30.0, 2)), new decimal(System.Math.Round(num22, 2)), setAzimuthTrend, new decimal(System.Math.Round(value2, 2)), new decimal(System.Math.Round(value, 2)), new decimal(System.Math.Round(value3, 2)), absoluteSurveyList, setInclinationTrend);
							result2 = TrendAnalysis_Errors.NoError;
							break;
						}
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					result2 = TrendAnalysis_Errors.TargetTVDCannotBeReached;
					ProjectData.ClearProjectError();
				}
				return result2;
			}
		}

		public TrendAnalysis_Errors TrendAnalysis(Target Target, AbsoluteSurveyList selectedSurveyData, ref TrendAnalysisResults results, AbsoluteSurvey TrendLastSurvey, double interval)
		{
			if (System.Math.Sign(interval) == -1)
			{
				return TrendAnalysis_Errors.InvalidInterval;
			}
			AbsoluteSurvey absoluteSurvey = new AbsoluteSurvey();
			AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
			AbsoluteSurveyList absoluteSurveyList2 = new AbsoluteSurveyList();
			checked
			{
				int num = selectedSurveyData.Count() - 1;
				double num2 = System.Math.Acos(System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.LastRecord().Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.RecordAt(0).Dip)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.LastRecord().Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.RecordAt(0).Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(decimal.Subtract(selectedSurveyData.LastRecord().Azimuth, selectedSurveyData.RecordAt(0).Azimuth))));
				double num3 = 180.0 / System.Math.PI * num2;
				double num4 = 180.0 / System.Math.PI * (num2 / Convert.ToDouble(decimal.Subtract(selectedSurveyData.LastRecord().MD, selectedSurveyData.RecordAt(0).MD)) * 30.0);
				double d = (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.RecordAt(0).Dip)) * System.Math.Cos(num3) - System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.LastRecord().Dip))) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(selectedSurveyData.RecordAt(0).Dip)) * System.Math.Sin(num3));
				d = System.Math.Acos(d) * (180.0 / System.Math.PI) + 180.0;
				d = mdTrendPlot.FinalToolFaceSetting(selectedSurveyData.RecordAt(0), Convert.ToDouble(selectedSurveyData.LastRecord().Dip), Convert.ToDouble(selectedSurveyData.LastRecord().Azimuth), num3);
				decimal num5 = new decimal(selectedSurveyData.Count() - 1);
				decimal num6 = decimal.Subtract(selectedSurveyData.LastRecord().MD, selectedSurveyData.RecordAt(0).MD);
				decimal num7 = decimal.Subtract(selectedSurveyData.LastRecord().TVD, selectedSurveyData.RecordAt(0).TVD);
				decimal num8 = decimal.Subtract(selectedSurveyData.LastRecord().Dip, selectedSurveyData.RecordAt(0).Dip);
				InclinationTrend setInclinationTrend = ((System.Math.Sign(num8) == -1) ? InclinationTrend.Drop : ((System.Math.Sign(num8) != 1) ? InclinationTrend.NoTrend : InclinationTrend.Build));
				double num9 = Convert.ToDouble(decimal.Divide(num8, num6));
				double value = num9 * 30.0;
				object obj = decimal.Divide(num8, num7);
				double num10 = Convert.ToDouble(decimal.Subtract(selectedSurveyData.LastRecord().Azimuth, selectedSurveyData.RecordAt(0).Azimuth));
				AzimuthTrend setAzimuthTrend = ((System.Math.Sign(num10) == -1) ? AzimuthTrend.South : ((System.Math.Sign(num10) != 1) ? AzimuthTrend.NoTrend : AzimuthTrend.North));
				double num11 = num10 / Convert.ToDouble(num6);
				double value2 = num11 * 30.0;
				object obj2 = num10 / Convert.ToDouble(num7);
				double num12 = Convert.ToDouble(decimal.Subtract(Target.TVD, TrendLastSurvey.TVD));
				TrendAnalysis_Errors result;
				try
				{
					AbsoluteSurveyList results2 = new AbsoluteSurveyList();
					bool MaxDip = default;
					results2 = mdTrendPlot.ExtrapolateSurveysTrendAnalysis(num4, TVDCannotBeReached: false, Target, num9, num11, interval, TrendLastSurvey, results2, d, ref MaxDip);
					if (!MaxDip)
					{
						results = new TrendAnalysisResults(new decimal(System.Math.Round(d, 2)), new decimal(System.Math.Round(num4, 2)), System.Math.Round(results.AngleChange, 2), setAzimuthTrend, new decimal(System.Math.Round(value, 2)), new decimal(System.Math.Round(num4, 2)), new decimal(System.Math.Round(value2, 2)), results2, setInclinationTrend);
						result = TrendAnalysis_Errors.NoError;
					}
					else
					{
						results = new TrendAnalysisResults(new decimal(System.Math.Round(d, 2)), new decimal(System.Math.Round(num4, 2)), System.Math.Round(results.AngleChange, 2), setAzimuthTrend, new decimal(System.Math.Round(value, 2)), new decimal(System.Math.Round(num4, 2)), new decimal(System.Math.Round(value2, 2)), results2, setInclinationTrend);
						result = TrendAnalysis_Errors.TargetTVDCannotBeReached;
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					result = TrendAnalysis_Errors.TargetTVDCannotBeReached;
					ProjectData.ClearProjectError();
				}
				return result;
			}
		}

		public Extrapolate_MD_Errors ExtrapolateToMD(double MD, double DogLeg, double ToolFace, AbsoluteSurvey LastSurvey, ref AbsoluteSurveyList results, ref ExtrapolatedAbsoluteSurvey result, double Interval)
		{
			if (MD > 24000.0)
			{
				return Extrapolate_MD_Errors.DepthTooGreat;
			}
			if ((System.Math.Sign(MD) == -1) | (MD <= Convert.ToDouble(LastSurvey.MD)))
			{
				return Extrapolate_MD_Errors.InvalidMD;
			}
			if (System.Math.Sign(Interval) == -1)
			{
				return Extrapolate_MD_Errors.InvalidInterval;
			}
			if (Interval > MD - Convert.ToDouble(LastSurvey.MD))
			{
				Interval = MD - Convert.ToDouble(LastSurvey.MD);
			}
			if ((DogLeg == 0.0) | (DogLeg > 30.0) | (System.Math.Sign(DogLeg) == -1))
			{
				return Extrapolate_MD_Errors.InavlidDogLeg;
			}
			if ((System.Math.Sign(ToolFace) == -1) | (ToolFace < 0.0) | (ToolFace > 360.0))
			{
				return Extrapolate_MD_Errors.InvalidToolFace;
			}
			bool MaxDip = false;
			Extrapolate_MD_Errors result2;
			try
			{
				results = mdTrendPlot.ExtrapolateIntervalSurveysMD(MD, ToolFace, DogLeg, Interval, LastSurvey, ref results, ref MaxDip, ExtrapToDir: false, IncIsZero: false, DirIsZero: false, ExtrapToInc: false, 0.0, MinDL: false, 0.0, 0.0, 0.0, 0.0, Build: false);
				result = new ExtrapolatedAbsoluteSurvey(System.Math.Round(results.LastRecord().MD, 2), System.Math.Round(results.LastRecord().Dip, 2), System.Math.Round(results.LastRecord().Azimuth, 2), System.Math.Round(results.LastRecord().TVD, 2), System.Math.Round(results.LastRecord().Northing, 2), System.Math.Round(results.LastRecord().Easting, 2), new decimal(System.Math.Round(DogLeg, 2)), new decimal(System.Math.Round(ToolFace, 2)), results);
				result2 = (MaxDip ? Extrapolate_MD_Errors.MaximumDipReachedBeforeDepth : Extrapolate_MD_Errors.NoError);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				if (Operators.CompareString(ex2.Message, "Depth too great", TextCompare: false) == 0)
				{
					result2 = Extrapolate_MD_Errors.DepthTooGreat;
					ProjectData.ClearProjectError();
				}
				else if (Operators.CompareString(ex2.Message, "Inavalid dogleg", TextCompare: false) == 0)
				{
					result2 = Extrapolate_MD_Errors.InavlidDogLeg;
					ProjectData.ClearProjectError();
				}
				else
				{
					result2 = Extrapolate_MD_Errors.PathCannotBeAchieved;
					ProjectData.ClearProjectError();
				}
			}
			return result2;
		}

		public Extrapolate_TVD_Errors ExtrapolateToTVD(double TVD, double DogLeg, double ToolFace, AbsoluteSurvey LastSurvey, ref AbsoluteSurveyList resultList, ref ExtrapolatedAbsoluteSurvey result, double Interval)
		{
			if (System.Math.Sign(Interval) == -1)
			{
				return Extrapolate_TVD_Errors.InvalidInterval;
			}
			if ((DogLeg > 30.0) | (System.Math.Sign(DogLeg) == -1) | (DogLeg == 0.0))
			{
				return Extrapolate_TVD_Errors.InvalidDogLeg;
			}
			if ((System.Math.Sign(TVD) == -1) | (TVD <= Convert.ToDouble(LastSurvey.TVD)))
			{
				return Extrapolate_TVD_Errors.InvalidTVD;
			}
			if ((System.Math.Sign(ToolFace) == -1) | (ToolFace < 0.0) | (ToolFace > 360.0))
			{
				return Extrapolate_TVD_Errors.InvalidToolFace;
			}
			bool MaxDip = false;
			bool MaxMD = false;
			Extrapolate_TVD_Errors result2;
			try
			{
				resultList = mdTrendPlot.ExtrapolateIntervalSurveysTVD(TVD, ToolFace, DogLeg, Interval, LastSurvey, ref resultList, ref MaxDip, ref MaxMD);
				result = new ExtrapolatedAbsoluteSurvey(System.Math.Round(resultList.LastRecord().MD, 2), System.Math.Round(resultList.LastRecord().Dip, 2), System.Math.Round(resultList.LastRecord().Azimuth, 2), System.Math.Round(resultList.LastRecord().TVD, 2), System.Math.Round(resultList.LastRecord().Northing, 2), System.Math.Round(resultList.LastRecord().Easting, 2), new decimal(System.Math.Round(DogLeg, 2)), new decimal(System.Math.Round(ToolFace, 2)), resultList);
				result2 = (MaxDip ? Extrapolate_TVD_Errors.MaximumDipReachedBeforeDepth : ((!MaxMD) ? Extrapolate_TVD_Errors.NoError : Extrapolate_TVD_Errors.MaxMDReachedBeforeTVD));
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				result2 = Extrapolate_TVD_Errors.PathCannotBeAchieved;
				ProjectData.ClearProjectError();
			}
			return result2;
		}

		public Extrapolate_Inc_Errors ExtrapolateToInclination(double Inc, double DogLeg, double ToolFace, AbsoluteSurvey LastSurvey, ref AbsoluteSurveyList resultList, ref ExtrapolatedAbsoluteSurvey result, double Interval)
		{
			bool MaxMD = false;
			if ((System.Math.Sign(Inc) == -1) | (Inc < 0.0) | (Inc >= 90.0))
			{
				return Extrapolate_Inc_Errors.InvalidInclination;
			}
			if ((Inc > Convert.ToDouble(LastSurvey.Dip)) & ((ToolFace < 270.01) & (ToolFace > 90.0)) & (decimal.Compare(LastSurvey.Dip, 0m) != 0))
			{
				return Extrapolate_Inc_Errors.CannotAchieveIncWithTFS;
			}
			if ((Inc < Convert.ToDouble(LastSurvey.Dip)) & ((ToolFace < 90.0) | (ToolFace > 270.01)) & (decimal.Compare(LastSurvey.Dip, 0m) != 0))
			{
				return Extrapolate_Inc_Errors.CannotAchieveIncWithTFS;
			}
			if (Inc == Convert.ToDouble(LastSurvey.Dip))
			{
				return Extrapolate_Inc_Errors.IncSameAsPrevious;
			}
			if (System.Math.Sign(Interval) == -1)
			{
				return Extrapolate_Inc_Errors.InvalidInterval;
			}
			if ((DogLeg > 30.0) | (System.Math.Sign(DogLeg) == -1) | (DogLeg == 0.0))
			{
				return Extrapolate_Inc_Errors.InvalidDogLeg;
			}
			if ((System.Math.Sign(ToolFace) == -1) | (ToolFace < 0.0) | (ToolFace > 360.0))
			{
				return Extrapolate_Inc_Errors.InvalidToolFace;
			}
			double num = (Inc - Convert.ToDouble(LastSurvey.Dip)) / (DogLeg / 30.0);
			if (System.Math.Sign(num) == -1)
			{
				num = Convert.ToDouble(LastSurvey.MD) + num * -1.0;
			}
			else
			{
				num = Convert.ToDouble(LastSurvey.MD) + num;
			}
			Extrapolate_Inc_Errors result2;
			try
			{
				resultList = mdTrendPlot.ExtrapolateIntervalSurveysInc(Inc, ToolFace, DogLeg, Interval, LastSurvey, ref resultList, ref MaxMD);
				result = new ExtrapolatedAbsoluteSurvey(System.Math.Round(resultList.LastRecord().MD, 2), System.Math.Round(resultList.LastRecord().Dip, 2), System.Math.Round(resultList.LastRecord().Azimuth, 2), System.Math.Round(resultList.LastRecord().TVD, 2), System.Math.Round(resultList.LastRecord().Northing, 2), System.Math.Round(resultList.LastRecord().Easting, 2), new decimal(System.Math.Round(DogLeg, 2)), new decimal(System.Math.Round(ToolFace, 2)), resultList);
				result2 = ((!MaxMD) ? Extrapolate_Inc_Errors.NoError : Extrapolate_Inc_Errors.MaximumMDReachedBeforeInclination);
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				if (Operators.CompareString(ex2.Message, "Invalid tool face", TextCompare: false) == 0)
				{
					result2 = Extrapolate_Inc_Errors.CannotAchieveIncWithTFS;
					ProjectData.ClearProjectError();
				}
				else
				{
					result2 = Extrapolate_Inc_Errors.PathCannotBeAchieved;
					ProjectData.ClearProjectError();
				}
			}
			return result2;
		}

		public Extrapolate_Dir_Errors ExtrapolateToDirection(double Inc, double Direction, double DL, AbsoluteSurvey LastSurvey, ref AbsoluteSurveyList Results, ref ExtrapolatedAbsoluteSurvey result, double Interval)
		{
			if (Direction == 360.0)
			{
				Direction = 0.0;
			}
			if (System.Math.Sign(Interval) == -1)
			{
				return Extrapolate_Dir_Errors.InvalidInterval;
			}
			if ((Inc == Convert.ToDouble(LastSurvey.Dip)) & (Direction == Convert.ToDouble(LastSurvey.Azimuth)))
			{
				return Extrapolate_Dir_Errors.AlreadyThere;
			}
			if ((DL > 30.0) | (System.Math.Sign(DL) == -1) | (DL == 0.0))
			{
				return Extrapolate_Dir_Errors.InvalidDogLeg;
			}
			if ((Direction == Convert.ToDouble(LastSurvey.Azimuth)) & (Inc == Convert.ToDouble(LastSurvey.Dip)))
			{
				return Extrapolate_Dir_Errors.AlreadyThere;
			}
			if ((System.Math.Sign(Direction) == -1) | (Direction < 0.0) | (Direction > 360.0))
			{
				return Extrapolate_Dir_Errors.InvalidAzimuth;
			}
			if ((System.Math.Sign(Inc) == -1) | (Inc < 0.0) | (Inc >= 90.0))
			{
				return Extrapolate_Dir_Errors.InvalidIncliation;
			}
			Extrapolate_Dir_Errors result2;
			try
			{
				double num = 180.0 / System.Math.PI * (1.0 / (DL / 30.0));
				double d = System.Math.Cos(System.Math.PI / 180.0 * (Inc - Convert.ToDouble(LastSurvey.Dip))) - System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Inc) * (1.0 - System.Math.Cos(System.Math.PI / 180.0 * (Direction - Convert.ToDouble(LastSurvey.Azimuth))));
				d = 180.0 / System.Math.PI * System.Math.Acos(d);
				double num2 = System.Math.PI / 180.0 * d * num;
				double num3 = Convert.ToDouble(LastSurvey.MD) + num2;
				double value;
				double num5;
				double num6;
				double num7;
				double num8;
				bool dirIsZero = default;
				bool incIsZero = default;
				if (num3 > 24000.0)
				{
					result2 = Extrapolate_Dir_Errors.DepthTooGreatPickHigherDogLeg;
				}
				else
				{
					value = Convert.ToDouble(LastSurvey.MD) + num2;
					double num4 = 2.0 / (System.Math.PI / 180.0 * d) * System.Math.Tan(System.Math.PI / 180.0 * d / 2.0);
					num5 = Convert.ToDouble(LastSurvey.TVD) + num2 / 2.0 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) + System.Math.Cos(System.Math.PI / 180.0 * Inc)) * num4;
					num6 = Convert.ToDouble(LastSurvey.Northing) + num2 / 2.0 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Inc) * System.Math.Cos(System.Math.PI / 180.0 * Direction)) * num4;
					num7 = Convert.ToDouble(LastSurvey.Easting) + num2 / 2.0 * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Inc) * System.Math.Sin(System.Math.PI / 180.0 * Direction)) * num4;
					if (decimal.Compare(LastSurvey.Dip, 0m) == 0)
					{
						num8 = Direction;
					}
					else
					{
						double d2 = (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * d) - System.Math.Cos(System.Math.PI / 180.0 * Inc)) / (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(LastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * d));
						d2 = 180.0 / System.Math.PI * System.Math.Acos(d2);
						num8 = mdTrendPlot.FinalToolFaceSetting(LastSurvey, Inc, Direction, d);
						if ((num8 == 0.0) | (num8 == 180.0))
						{
							if (Inc < Convert.ToDouble(LastSurvey.Dip))
							{
								num8 = 180.0;
							}
							else if ((Inc > Convert.ToDouble(LastSurvey.Dip)) & (Direction == Convert.ToDouble(LastSurvey.Azimuth)))
							{
								num8 = 0.0;
							}
							else if ((Inc > Convert.ToDouble(LastSurvey.Dip)) & (Direction == Convert.ToDouble(decimal.Add(LastSurvey.Azimuth, 180m))))
							{
								num8 = 180.0;
							}
							else if ((Inc > Convert.ToDouble(LastSurvey.Dip)) & (Direction == Convert.ToDouble(decimal.Subtract(LastSurvey.Azimuth, 180m))))
							{
								num8 = 180.0;
							}
						}
					}
					if ((Direction == 0.0) | (Direction == 360.0))
					{
						dirIsZero = true;
					}
					if (Inc != 0.0)
					{
						goto IL_0512;
					}
					if (decimal.Compare(LastSurvey.Dip, 0m) != 0)
					{
						incIsZero = true;
						dirIsZero = true;
						goto IL_0512;
					}
					result2 = Extrapolate_Dir_Errors.NoDirectionWhenPathVertical;
				}
				goto end_IL_00e1;
				IL_0512:
				double toolFace = num8;
				bool MaxDip = false;
				Results = mdTrendPlot.ExtrapolateIntervalSurveysMD(num3, toolFace, DL, Interval, LastSurvey, ref Results, ref MaxDip, ExtrapToDir: true, incIsZero, dirIsZero, ExtrapToInc: false, Inc, MinDL: false, Direction, num5, num6, num7, Build: false);
				result = new ExtrapolatedAbsoluteSurvey(new decimal(System.Math.Round(value, 2)), new decimal(System.Math.Round(Inc, 2)), new decimal(System.Math.Round(Direction, 2)), new decimal(System.Math.Round(num5, 2)), new decimal(System.Math.Round(num6, 2)), new decimal(System.Math.Round(num7, 2)), new decimal(System.Math.Round(DL, 2)), new decimal(System.Math.Round(num8, 2)));
				result2 = Extrapolate_Dir_Errors.NoError;
				end_IL_00e1:;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				result2 = Extrapolate_Dir_Errors.PathCannotBeAchieved;
				ProjectData.ClearProjectError();
			}
			return result2;
		}

		public Extrapolate_StraightPath_Errors ExtrapolateToStraightPath(double Depth, AbsoluteSurvey lastSurvey, ref AbsoluteSurvey result, ref AbsoluteSurveyList Results, bool MD, double Interval)
		{
			Extrapolate_StraightPath_Errors result2;
			try
			{
				AbsoluteSurvey absoluteSurvey = new AbsoluteSurvey();
				if ((System.Math.Sign(Depth) == -1) | (Depth <= 0.0))
				{
					result2 = Extrapolate_StraightPath_Errors.InvalidDepth;
				}
				else if (MD)
				{
					if (Depth < Convert.ToDouble(lastSurvey.MD))
					{
						result2 = Extrapolate_StraightPath_Errors.MDLessThanCurrent;
					}
					else if (0 - (MD ? 1 : 0) > 24000)
					{
						result2 = Extrapolate_StraightPath_Errors.MDDepthTooGreat;
					}
					else
					{
						decimal num = new decimal((Depth - Convert.ToDouble(lastSurvey.MD)) / 2.0 * (System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) + System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip))));
						if (System.Math.Sign(num) != -1)
						{
							decimal d = new decimal(Convert.ToDouble(num) / System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)));
							decimal d2 = decimal.Add(d, lastSurvey.MD);
							decimal d3 = decimal.Add(lastSurvey.TVD, num);
							decimal d4 = new decimal(Convert.ToDouble(lastSurvey.Northing) + Convert.ToDouble(decimal.Divide(d, 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth))));
							decimal d5 = new decimal(Convert.ToDouble(lastSurvey.Easting) + Convert.ToDouble(decimal.Divide(d, 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth))));
							absoluteSurvey = new AbsoluteSurvey(System.Math.Round(d2, 2), System.Math.Round(lastSurvey.Dip, 2), System.Math.Round(lastSurvey.Azimuth, 2), System.Math.Round(d3, 2), System.Math.Round(d4, 2), System.Math.Round(d5, 2), 0m);
							double endMD = Convert.ToDouble(absoluteSurvey.MD);
							bool MaxDip = false;
							Results = mdTrendPlot.ExtrapolateIntervalSurveysMD(endMD, 0.0, 0.0, Interval, lastSurvey, ref Results, ref MaxDip, ExtrapToDir: false, IncIsZero: false, DirIsZero: false, ExtrapToInc: false, 0.0, MinDL: false, 0.0, 0.0, 0.0, 0.0, Build: false);
							goto IL_0522;
						}
						result2 = Extrapolate_StraightPath_Errors.TargetTVDHasAlreadyBeenReached;
					}
				}
				else
				{
					if (MD)
					{
						goto IL_0522;
					}
					decimal num = new decimal(Depth - Convert.ToDouble(lastSurvey.TVD));
					if (System.Math.Sign(num) == -1)
					{
						result2 = Extrapolate_StraightPath_Errors.TargetTVDHasAlreadyBeenReached;
					}
					else
					{
						decimal d = new decimal(Convert.ToDouble(num) / System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)));
						decimal d2 = decimal.Add(d, lastSurvey.MD);
						if (decimal.Compare(d2, 24000m) <= 0)
						{
							decimal d3 = decimal.Add(lastSurvey.TVD, num);
							decimal d4 = new decimal(Convert.ToDouble(lastSurvey.Northing) + Convert.ToDouble(decimal.Divide(d, 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Cos(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth))));
							decimal d5 = new decimal(Convert.ToDouble(lastSurvey.Easting) + Convert.ToDouble(decimal.Divide(d, 2m)) * (System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth)) + System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Dip)) * System.Math.Sin(System.Math.PI / 180.0 * Convert.ToDouble(lastSurvey.Azimuth))));
							absoluteSurvey = new AbsoluteSurvey(System.Math.Round(d2, 2), System.Math.Round(lastSurvey.Dip, 2), System.Math.Round(lastSurvey.Azimuth, 2), System.Math.Round(d3, 2), System.Math.Round(d4, 2), System.Math.Round(d5, 2), 0m);
							double endMD2 = Convert.ToDouble(absoluteSurvey.MD);
							bool MaxDip = false;
							Results = mdTrendPlot.ExtrapolateIntervalSurveysMD(endMD2, 0.0, 0.0, Interval, lastSurvey, ref Results, ref MaxDip, ExtrapToDir: false, IncIsZero: false, DirIsZero: false, ExtrapToInc: false, 0.0, MinDL: false, 0.0, 0.0, 0.0, 0.0, Build: false);
							goto IL_0522;
						}
						result2 = Extrapolate_StraightPath_Errors.MDDepthTooGreat;
					}
				}
				goto end_IL_0000;
				IL_0522:
				result = absoluteSurvey;
				result2 = Extrapolate_StraightPath_Errors.NoError;
				end_IL_0000:;
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				result2 = Extrapolate_StraightPath_Errors.TargetTVDHasAlreadyBeenReached;
				ProjectData.ClearProjectError();
			}
			return result2;
		}

		public CollarToTarget_Errors CollarToTarget(Collar Collar, Target Target, ref double CollarToTargetInc, ref double CollarToTargetAzi)
		{
			double num = System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(decimal.Subtract(Target.Easting, Collar.Easting)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(Target.Northing, Collar.Northing)), 2.0) + System.Math.Pow(Convert.ToDouble(Target.TVD), 2.0));
			double num2 = System.Math.Sqrt(System.Math.Pow(Convert.ToDouble(decimal.Subtract(Target.Easting, Collar.Easting)), 2.0) + System.Math.Pow(Convert.ToDouble(decimal.Subtract(Target.Northing, Collar.Northing)), 2.0));
			double num3 = System.Math.Acos(Convert.ToDouble(Target.TVD) / num);
			num3 *= 180.0 / System.Math.PI;
			double num4 = Convert.ToDouble(decimal.Subtract(Target.Easting, Collar.Easting));
			double num5 = Convert.ToDouble(decimal.Subtract(Target.Northing, Collar.Northing));
			if ((System.Math.Sign(num4) == 1) & (System.Math.Sign(num5) == 1))
			{
				int num6 = 1;
				double num7 = System.Math.Atan(num4 / num5);
				num7 *= 180.0 / System.Math.PI;
			}
			else if ((System.Math.Sign(num4) == 1) & (System.Math.Sign(num5) == -1))
			{
				int num6 = 2;
				double num7 = System.Math.Atan(num4 / num5);
				num7 *= 180.0 / System.Math.PI;
				num7 = 180.0 + num7;
			}
			else if ((System.Math.Sign(num4) == -1) & (System.Math.Sign(num5) == -1))
			{
				int num6 = 3;
				double num7 = System.Math.Atan(num4 / num5);
				num7 *= 180.0 / System.Math.PI;
				num7 = 180.0 + num7;
			}
			else if ((System.Math.Sign(num4) == -1) & (System.Math.Sign(num5) == 1))
			{
				int num6 = 4;
				double num7 = System.Math.Atan(num4 / num5);
				num7 *= 180.0 / System.Math.PI;
				num7 = 360.0 + num7;
			}
			double num8 = 7.0;
			CollarToTarget_Errors result = default;
			return result;
		}
	}

	[DebuggerNonUserCode]
	public Math()
	{
	}
}
