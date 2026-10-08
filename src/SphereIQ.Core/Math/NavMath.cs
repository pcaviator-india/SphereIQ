using System;
using System.Collections.Generic;
using EzyPlanData;
using EzyPlanMath;

namespace EzyNavMath;

public class NavMath
{
	public enum AbsoluteSurveyEnum
	{
		NoError,
		UnknownError,
		FunctionInputParameterError,
		UnknownException
	}

	public enum StraightPathToTargetTVDEnum
	{
		NoError,
		HorizontalPath,
		TargetTVDHasAlreadyBeenReached,
		UnknownError,
		UnknownException
	}

	public enum DistanceFromTargetEnum
	{
		NoError,
		UnknownError,
		FunctionInputParameterError,
		UnknownException
	}

	public enum DistanceFromTargetAtTargetTVDEnum
	{
		Found,
		Extrapolated,
		SurveyNotAtOrBelowTargetTVD,
		InsufficientSurveyData,
		NoSurveyData,
		NullParameter,
		InternalError,
		UnknownError,
		UnknownException
	}

	public enum GetAbsoluteSurveyAtTVDEnum
	{
		Found,
		Extrapolated,
		InsufficientSurveyData,
		InternalError,
		UnknownError,
		UnknownException
	}

	public enum GetAbsoluteSurveyAtMDEnum
	{
		Found,
		Extrapolated,
		InsufficientSurveyData,
		InternalError,
		UnknownError,
		UnknownException
	}

	public enum BuildToTargetEnum
	{
		NoError,
		TargetCannotNotBeAchieved,
		DogLegTooSmall,
		InsufficientTVD,
		UnknownError,
		UnknownException
	}

	public enum BuildToTargetListEnum
	{
		CannotAchieveTarget,
		InsufficientTVD,
		Success,
		UnknownMinDLError,
		UnexpectedMinDLException,
		UnknownBuildError,
		UnexpectedBuildException,
		InternalError
	}

	public enum MinimumDogLegEnum
	{
		NoError,
		TargetCannotBeAchieved,
		InsufficientTVD,
		UnknownError,
		UnknownException
	}

	public enum TrendAnalysisEnum
	{
		Success,
		TrendCannotBeAchieved,
		TargetTVDCannotBeAchieved,
		StraightPath,
		NullParameter,
		NotEnoughSurveys,
		UnknownError,
		UnknownException
	}

	public enum SideTrackHoleEnum
	{
		Found,
		Extrapolated,
		NullParameter,
		DepthNotGreaterThanZero,
		DepthOutsideSurveyRange,
		InsufficientSurveyData,
		NoSurveyData,
		InternalError,
		UnknownExtrapolateBetweenError,
		UnknownExtrapolateBetweenException
	}

	public enum ExtrapolateBetweenMDEnum
	{
		Success,
		NullParameter,
		DepthNotGreaterThanZero,
		DepthNotBetweenSurveys,
		InvalidSurveys,
		InvalidDepth,
		UnknownError,
		UnknownException
	}

	public enum ExtrapolateBetweenTVDEnum
	{
		Success,
		NullParameter,
		DepthNotGreaterThanZero,
		DepthNotBetweenSurveys,
		InvalidSurveys,
		InvalidDepth,
		UnknownError,
		UnknownException
	}

	public enum ExtrapolateToMDResultEnum
	{
		InvalidDogleg,
		InvalidToolface,
		InvalidMD,
		DipTooGreat,
		PathCannotBeAchieved,
		MaximumDipReachedBeforeDepth,
		DepthTooGreat,
		NoError,
		UnknownError,
		UnknownException
	}

	public enum ExtrapolateToTVDResultEnum
	{
		InvalidDogleg,
		InvalidToolface,
		InvalidTVD,
		DipTooGreat,
		PathCannotBeAchieved,
		MaximumDipReachedBeforeDepth,
		MaxMDReachedBeforeTVD,
		DepthTooGreat,
		NoError,
		UnknownError,
		UnknownException
	}

	public enum ExtrapolateToDirectionResultEnum
	{
		InvalidDogleg,
		InvalidDirection,
		InvalidInclination,
		DipTooGreat,
		PathCannotBeAchieved,
		DepthTooGreatPickHigherDogLeg,
		AlreadyThere,
		NoError,
		UnknownError,
		UnknownException
	}

	public enum ExtrapolateToInclinationResultEnum
	{
		InvalidDogleg,
		InvalidInclination,
		InvalidToolface,
		DipTooGreat,
		PathCannotBeAchieved,
		IncSameAsPrevious,
		CannotAchieveIncWithTFS,
		MaximumMDReachedBeforeInclination,
		NoError,
		UnknownError,
		UnknownException
	}

	private EzyPlanMath.Math.ThreeDWellPlan math;

	public NavMath()
	{
		Initialise();
	}

	public void Initialise()
	{
		math = new EzyPlanMath.Math.ThreeDWellPlan();
	}

	public AbsoluteSurveyEnum AbsoluteSurvey(Collar collar, SurveyList surveyList, ref AbsoluteSurveyList absoluteSurveyList)
	{
		try
		{
			return math.AbsoluteSurvey(collar, surveyList, ref absoluteSurveyList) switch
			{
				AbsoluteSurvey_Errors.NoError => AbsoluteSurveyEnum.NoError, 
				AbsoluteSurvey_Errors.FunctionInputParameterError => AbsoluteSurveyEnum.FunctionInputParameterError, 
				_ => AbsoluteSurveyEnum.UnknownError, 
			};
		}
		catch
		{
			return AbsoluteSurveyEnum.UnknownException;
		}
	}

	public static string ToHumanReadable(AbsoluteSurveyEnum error)
	{
		return error switch
		{
			AbsoluteSurveyEnum.NoError => "No Error", 
			AbsoluteSurveyEnum.UnknownError => "An unexpected error occured during the processing of the survey data", 
			AbsoluteSurveyEnum.FunctionInputParameterError => "An unknown internal error occured during the calculations", 
			AbsoluteSurveyEnum.UnknownException => "An unexpected exception occured while processing the survey data", 
			_ => "An unknown error was encountered in the survey data", 
		};
	}

	public StraightPathToTargetTVDEnum StraightPathToTargetTVD(Target target, AbsoluteSurvey lastSurvey, ref AbsoluteSurvey endOfHoleSurvey)
	{
		if (lastSurvey == null)
		{
			throw new ArgumentNullException("lastSurvey", "lastSurvey parameter cannot be null");
		}
		if (target == null)
		{
			throw new ArgumentNullException("target", "Target parameter cannot be null");
		}
		if (endOfHoleSurvey == null)
		{
			throw new ArgumentNullException("endOfHoleSurvey", "endOfHoleSurvey parameter cannot be null");
		}
		if (lastSurvey.Dip < 90m)
		{
			try
			{
				StraightPathToTargetTVD_Errors straightPathToTargetTVD_Errors = math.StraightPathToTargetTVD(target, lastSurvey, ref endOfHoleSurvey);
				if (straightPathToTargetTVD_Errors == StraightPathToTargetTVD_Errors.NoError)
				{
					return StraightPathToTargetTVDEnum.NoError;
				}
				if (straightPathToTargetTVD_Errors == StraightPathToTargetTVD_Errors.TargetTVDHasAlreadyBeenReached)
				{
					return StraightPathToTargetTVDEnum.TargetTVDHasAlreadyBeenReached;
				}
				return StraightPathToTargetTVDEnum.UnknownError;
			}
			catch
			{
				return StraightPathToTargetTVDEnum.UnknownException;
			}
		}
		return StraightPathToTargetTVDEnum.HorizontalPath;
	}

	public static string ToHumanReadable(StraightPathToTargetTVDEnum error)
	{
		return error switch
		{
			StraightPathToTargetTVDEnum.NoError => "No Error", 
			StraightPathToTargetTVDEnum.HorizontalPath => "A straight path projection at 90 degrees will never reach the target depth", 
			StraightPathToTargetTVDEnum.TargetTVDHasAlreadyBeenReached => "The target depth has already been achieved", 
			StraightPathToTargetTVDEnum.UnknownError => "An unknown error occured during the calculations", 
			StraightPathToTargetTVDEnum.UnknownException => "An unknown exception occured during the calculations", 
			_ => "An unknown error occured during the calculations", 
		};
	}

	public DistanceFromTargetEnum DistanceFromTarget(Target target, AbsoluteSurvey survey, ref RelativePosition distance)
	{
		try
		{
			return math.DistanceFromTarget(target, survey, ref distance) switch
			{
				DistanceFromTarget_Errors.NoError => DistanceFromTargetEnum.NoError, 
				DistanceFromTarget_Errors.FunctionInputParameterError => DistanceFromTargetEnum.FunctionInputParameterError, 
				_ => DistanceFromTargetEnum.UnknownError, 
			};
		}
		catch
		{
			return DistanceFromTargetEnum.UnknownException;
		}
	}

	public static string ToHumanReadable(DistanceFromTargetEnum error)
	{
		return error switch
		{
			DistanceFromTargetEnum.NoError => "Operation completed successfully", 
			DistanceFromTargetEnum.UnknownError => "An unknown error occured during the calculations", 
			DistanceFromTargetEnum.UnknownException => "An unknown exception occured during the calculations", 
			DistanceFromTargetEnum.FunctionInputParameterError => "An unknown internal error occured during the calculations", 
			_ => "An unknown error occured during the calculations", 
		};
	}

	public DistanceFromTargetAtTargetTVDEnum DistanceFromTargetAtTargetTVD(Target target, SortableBindingList<AbsoluteSurvey> absoluteSurveyList, out AbsoluteSurvey surveyAtTargetTVD, out RelativePosition distanceFromTarget)
	{
		surveyAtTargetTVD = null;
		distanceFromTarget = null;
		if (absoluteSurveyList == null || target == null)
		{
			return DistanceFromTargetAtTargetTVDEnum.NullParameter;
		}
		if (absoluteSurveyList.Count < 1)
		{
			return DistanceFromTargetAtTargetTVDEnum.NoSurveyData;
		}
		if (absoluteSurveyList[absoluteSurveyList.Count - 1].TVD < target.TVD)
		{
			return DistanceFromTargetAtTargetTVDEnum.SurveyNotAtOrBelowTargetTVD;
		}
		GetAbsoluteSurveyAtTVDEnum absoluteSurveyAtTVD = GetAbsoluteSurveyAtTVD(target.TVD, absoluteSurveyList, out surveyAtTargetTVD);
		if ((absoluteSurveyAtTVD == GetAbsoluteSurveyAtTVDEnum.Extrapolated) | (absoluteSurveyAtTVD == GetAbsoluteSurveyAtTVDEnum.Found))
		{
			DistanceFromTarget(target, surveyAtTargetTVD, ref distanceFromTarget);
			return absoluteSurveyAtTVD switch
			{
				GetAbsoluteSurveyAtTVDEnum.Found => DistanceFromTargetAtTargetTVDEnum.Found, 
				GetAbsoluteSurveyAtTVDEnum.Extrapolated => DistanceFromTargetAtTargetTVDEnum.Extrapolated, 
				_ => DistanceFromTargetAtTargetTVDEnum.InternalError, 
			};
		}
		return absoluteSurveyAtTVD switch
		{
			GetAbsoluteSurveyAtTVDEnum.InsufficientSurveyData => DistanceFromTargetAtTargetTVDEnum.InsufficientSurveyData, 
			GetAbsoluteSurveyAtTVDEnum.InternalError => DistanceFromTargetAtTargetTVDEnum.InternalError, 
			GetAbsoluteSurveyAtTVDEnum.UnknownError => DistanceFromTargetAtTargetTVDEnum.UnknownError, 
			GetAbsoluteSurveyAtTVDEnum.UnknownException => DistanceFromTargetAtTargetTVDEnum.UnknownException, 
			_ => DistanceFromTargetAtTargetTVDEnum.InternalError, 
		};
	}

	public DistanceFromTargetAtTargetTVDEnum DistanceFromTargetAtTargetTVD(Target target, ExtrapolatedSurveyList extrapolatedSurveyList, out AbsoluteSurvey surveyAtTargetTVD, out RelativePosition distanceFromTarget)
	{
		surveyAtTargetTVD = null;
		distanceFromTarget = null;
		if (extrapolatedSurveyList == null || target == null)
		{
			return DistanceFromTargetAtTargetTVDEnum.NullParameter;
		}
		SortableBindingList<AbsoluteSurvey> sortableBindingList = new SortableBindingList<AbsoluteSurvey>();
		sortableBindingList.Add(extrapolatedSurveyList.LastRealSurvey);
		foreach (ExtrapolatedAbsoluteSurvey item in extrapolatedSurveyList.List)
		{
			foreach (AbsoluteSurvey absoluteSurvey in item.GetAbsoluteSurveyList())
			{
				sortableBindingList.Add(absoluteSurvey);
			}
		}
		if (sortableBindingList.Count < 1)
		{
			return DistanceFromTargetAtTargetTVDEnum.NoSurveyData;
		}
		if (sortableBindingList[sortableBindingList.Count - 1].TVD < target.TVD)
		{
			return DistanceFromTargetAtTargetTVDEnum.SurveyNotAtOrBelowTargetTVD;
		}
		GetAbsoluteSurveyAtTVDEnum absoluteSurveyAtTVD = GetAbsoluteSurveyAtTVD(target.TVD, sortableBindingList, out surveyAtTargetTVD);
		if ((absoluteSurveyAtTVD == GetAbsoluteSurveyAtTVDEnum.Extrapolated) | (absoluteSurveyAtTVD == GetAbsoluteSurveyAtTVDEnum.Found))
		{
			DistanceFromTarget(target, surveyAtTargetTVD, ref distanceFromTarget);
			return absoluteSurveyAtTVD switch
			{
				GetAbsoluteSurveyAtTVDEnum.Found => DistanceFromTargetAtTargetTVDEnum.Found, 
				GetAbsoluteSurveyAtTVDEnum.Extrapolated => DistanceFromTargetAtTargetTVDEnum.Extrapolated, 
				_ => DistanceFromTargetAtTargetTVDEnum.InternalError, 
			};
		}
		return absoluteSurveyAtTVD switch
		{
			GetAbsoluteSurveyAtTVDEnum.InsufficientSurveyData => DistanceFromTargetAtTargetTVDEnum.InsufficientSurveyData, 
			GetAbsoluteSurveyAtTVDEnum.InternalError => DistanceFromTargetAtTargetTVDEnum.InternalError, 
			GetAbsoluteSurveyAtTVDEnum.UnknownError => DistanceFromTargetAtTargetTVDEnum.UnknownError, 
			GetAbsoluteSurveyAtTVDEnum.UnknownException => DistanceFromTargetAtTargetTVDEnum.UnknownException, 
			_ => DistanceFromTargetAtTargetTVDEnum.InternalError, 
		};
	}

	public static string ToHumanReadable(DistanceFromTargetAtTargetTVDEnum value)
	{
		return value switch
		{
			DistanceFromTargetAtTargetTVDEnum.Found => "A Survey was found at the target TVD", 
			DistanceFromTargetAtTargetTVDEnum.Extrapolated => "A Survey was extrapolated at the target TVD", 
			DistanceFromTargetAtTargetTVDEnum.NullParameter => "Null parameters", 
			DistanceFromTargetAtTargetTVDEnum.SurveyNotAtOrBelowTargetTVD => "The MD (Measured Depth) must be greater than 0", 
			DistanceFromTargetAtTargetTVDEnum.InsufficientSurveyData => "There are not enough surveys to perfom an extrapolation", 
			DistanceFromTargetAtTargetTVDEnum.NoSurveyData => "There is no survey data", 
			DistanceFromTargetAtTargetTVDEnum.InternalError => "An interanl system error has occured", 
			DistanceFromTargetAtTargetTVDEnum.UnknownError => "An unknown error has occured", 
			_ => "An unknown error has occured", 
		};
	}

	public GetAbsoluteSurveyAtTVDEnum GetAbsoluteSurveyAtTVD(decimal TVD, SortableBindingList<AbsoluteSurvey> absoluteSurveyList, out AbsoluteSurvey surveyAtTVD)
	{
		surveyAtTVD = null;
		AbsoluteSurvey absoluteSurvey = null;
		AbsoluteSurvey absoluteSurvey2 = null;
		foreach (AbsoluteSurvey absoluteSurvey3 in absoluteSurveyList)
		{
			if (absoluteSurvey3.TVD == TVD)
			{
				surveyAtTVD = absoluteSurvey3;
				return GetAbsoluteSurveyAtTVDEnum.Found;
			}
			if (absoluteSurvey3.TVD > TVD)
			{
				if (absoluteSurvey == null)
				{
					return GetAbsoluteSurveyAtTVDEnum.InsufficientSurveyData;
				}
				absoluteSurvey2 = absoluteSurvey3;
				return ExtrapolateBetweenSurveys_TVD(absoluteSurvey, absoluteSurvey2, TVD, out surveyAtTVD) switch
				{
					ExtrapolateBetweenTVDEnum.NullParameter => GetAbsoluteSurveyAtTVDEnum.InternalError, 
					ExtrapolateBetweenTVDEnum.DepthNotGreaterThanZero => GetAbsoluteSurveyAtTVDEnum.InternalError, 
					ExtrapolateBetweenTVDEnum.DepthNotBetweenSurveys => GetAbsoluteSurveyAtTVDEnum.InternalError, 
					ExtrapolateBetweenTVDEnum.InvalidSurveys => GetAbsoluteSurveyAtTVDEnum.InternalError, 
					ExtrapolateBetweenTVDEnum.InvalidDepth => GetAbsoluteSurveyAtTVDEnum.InternalError, 
					ExtrapolateBetweenTVDEnum.UnknownError => GetAbsoluteSurveyAtTVDEnum.UnknownError, 
					ExtrapolateBetweenTVDEnum.UnknownException => GetAbsoluteSurveyAtTVDEnum.UnknownException, 
					ExtrapolateBetweenTVDEnum.Success => GetAbsoluteSurveyAtTVDEnum.Extrapolated, 
					_ => GetAbsoluteSurveyAtTVDEnum.InternalError, 
				};
			}
			absoluteSurvey = absoluteSurvey3;
		}
		return GetAbsoluteSurveyAtTVDEnum.InternalError;
	}

	public GetAbsoluteSurveyAtMDEnum GetAbsoluteSurveyAtMD(decimal MD, SortableBindingList<AbsoluteSurvey> absoluteSurveyList, out AbsoluteSurvey surveyAtMD)
	{
		surveyAtMD = null;
		AbsoluteSurvey absoluteSurvey = null;
		AbsoluteSurvey absoluteSurvey2 = null;
		foreach (AbsoluteSurvey absoluteSurvey3 in absoluteSurveyList)
		{
			if (absoluteSurvey3.MD == MD)
			{
				surveyAtMD = absoluteSurvey3;
				return GetAbsoluteSurveyAtMDEnum.Found;
			}
			if (absoluteSurvey3.MD > MD)
			{
				if (absoluteSurvey == null)
				{
					return GetAbsoluteSurveyAtMDEnum.InsufficientSurveyData;
				}
				absoluteSurvey2 = absoluteSurvey3;
				return ExtrapolateBetweenSurveys_MD(absoluteSurvey, absoluteSurvey2, MD, out surveyAtMD) switch
				{
					ExtrapolateBetweenMDEnum.NullParameter => GetAbsoluteSurveyAtMDEnum.InternalError, 
					ExtrapolateBetweenMDEnum.DepthNotGreaterThanZero => GetAbsoluteSurveyAtMDEnum.InternalError, 
					ExtrapolateBetweenMDEnum.DepthNotBetweenSurveys => GetAbsoluteSurveyAtMDEnum.InternalError, 
					ExtrapolateBetweenMDEnum.InvalidSurveys => GetAbsoluteSurveyAtMDEnum.InternalError, 
					ExtrapolateBetweenMDEnum.InvalidDepth => GetAbsoluteSurveyAtMDEnum.InternalError, 
					ExtrapolateBetweenMDEnum.UnknownError => GetAbsoluteSurveyAtMDEnum.UnknownError, 
					ExtrapolateBetweenMDEnum.UnknownException => GetAbsoluteSurveyAtMDEnum.UnknownException, 
					ExtrapolateBetweenMDEnum.Success => GetAbsoluteSurveyAtMDEnum.Extrapolated, 
					_ => GetAbsoluteSurveyAtMDEnum.UnknownError, 
				};
			}
			absoluteSurvey = absoluteSurvey3;
		}
		return GetAbsoluteSurveyAtMDEnum.InternalError;
	}

	public BuildToTargetEnum BuildToTarget(decimal dogLeg, AbsoluteSurvey lastSurvey, Target target, out BuildAndHoldResults buildResult)
	{
		buildResult = null;
		try
		{
			buildResult = new BuildAndHoldResults();
			BuildToTarget_Errors buildToTarget_Errors = math.BuildToTarget((double)dogLeg, lastSurvey, target, ref buildResult, 5.0);
			buildResult.DogLeg = dogLeg;
			if (buildToTarget_Errors == BuildToTarget_Errors.NoError)
			{
				return BuildToTargetEnum.NoError;
			}
			return buildToTarget_Errors switch
			{
				BuildToTarget_Errors.DogLegTooSmall => BuildToTargetEnum.DogLegTooSmall, 
				BuildToTarget_Errors.InsufficientTVD => BuildToTargetEnum.InsufficientTVD, 
				BuildToTarget_Errors.NoError => BuildToTargetEnum.NoError, 
				BuildToTarget_Errors.TargetCannotBeAchived => BuildToTargetEnum.TargetCannotNotBeAchieved, 
				_ => BuildToTargetEnum.UnknownError, 
			};
		}
		catch
		{
			return BuildToTargetEnum.UnknownException;
		}
	}

	public static string ToHumanReadable(BuildToTargetEnum error)
	{
		return error switch
		{
			BuildToTargetEnum.NoError => "No error", 
			BuildToTargetEnum.UnknownError => "An unknown error has occured", 
			BuildToTargetEnum.UnknownException => "An unknown exception has occured", 
			BuildToTargetEnum.TargetCannotNotBeAchieved => "Target cannot be achieved", 
			BuildToTargetEnum.InsufficientTVD => "There is not enough TVD available to achieve the target", 
			BuildToTargetEnum.DogLegTooSmall => "The dogleg is too small to achieve the target", 
			_ => "An unknown error was encountered in the survey data", 
		};
	}

	public BuildToTargetListEnum BuildToTargetList(ref Target target, ref Collar collar, ref AbsoluteSurvey lastSurvey, decimal multiplesOf, int maxDogLeg, out List<BuildAndHoldResults> buildList)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target", "target cannot be null.");
		}
		if (collar == null)
		{
			throw new ArgumentNullException("collar", "collar cannot be null.");
		}
		if (lastSurvey == null)
		{
			throw new ArgumentNullException("lastSurvey", "lastSurvey cannot be null.");
		}
		if ((multiplesOf > 1m) | (multiplesOf <= 0m))
		{
			throw new ArgumentException("multiplesOf", "multiplesOf must be greater than zero and less than or equal to one.");
		}
		if (maxDogLeg <= 0)
		{
			throw new ArgumentException("maxDogLeg", "maxDogLeg must be greater than 0.");
		}
		NavMath navMath = new NavMath();
		buildList = new List<BuildAndHoldResults>();
		decimal dogLeg = 0m;
		decimal num = 0m;
		BuildAndHoldResults buildAndHold = new BuildAndHoldResults();
		switch (navMath.MinimumDogLegToTarget(lastSurvey, target, ref dogLeg, out buildAndHold))
		{
		case MinimumDogLegEnum.TargetCannotBeAchieved:
			return BuildToTargetListEnum.CannotAchieveTarget;
		case MinimumDogLegEnum.InsufficientTVD:
			return BuildToTargetListEnum.InsufficientTVD;
		case MinimumDogLegEnum.UnknownError:
			return BuildToTargetListEnum.UnknownMinDLError;
		case MinimumDogLegEnum.UnknownException:
			return BuildToTargetListEnum.UnexpectedMinDLException;
		default:
			return BuildToTargetListEnum.UnknownMinDLError;
		case MinimumDogLegEnum.NoError:
		{
			buildList.Add(buildAndHold);
			decimal num2 = Quantization(dogLeg, multiplesOf);
			num = ((!(num2 <= dogLeg)) ? num2 : (num2 + multiplesOf));
			for (decimal num3 = num; num3 <= (decimal)maxDogLeg; num3 += multiplesOf)
			{
				BuildToTargetEnum buildToTargetEnum = navMath.BuildToTarget(num3, lastSurvey, target, out var buildResult);
				if (buildToTargetEnum != BuildToTargetEnum.NoError)
				{
					return buildToTargetEnum switch
					{
						BuildToTargetEnum.NoError => BuildToTargetListEnum.Success, 
						BuildToTargetEnum.TargetCannotNotBeAchieved => BuildToTargetListEnum.InternalError, 
						BuildToTargetEnum.DogLegTooSmall => BuildToTargetListEnum.InternalError, 
						BuildToTargetEnum.InsufficientTVD => BuildToTargetListEnum.InternalError, 
						BuildToTargetEnum.UnknownError => BuildToTargetListEnum.UnknownBuildError, 
						BuildToTargetEnum.UnknownException => BuildToTargetListEnum.UnexpectedBuildException, 
						_ => BuildToTargetListEnum.UnknownBuildError, 
					};
				}
				buildResult.DogLeg = num3;
				buildList.Add(buildResult);
			}
			return BuildToTargetListEnum.Success;
		}
		}
	}

	public static string ToHumanReadable(BuildToTargetListEnum value)
	{
		return value switch
		{
			BuildToTargetListEnum.CannotAchieveTarget => "Target cannot be achieved", 
			BuildToTargetListEnum.Success => "Operation completed successfully", 
			BuildToTargetListEnum.InsufficientTVD => "There is insufficient TVD to achieve the target", 
			BuildToTargetListEnum.UnexpectedMinDLException => "An unexpected exception occured during the Minimum DL calculations", 
			BuildToTargetListEnum.UnknownMinDLError => "An unknown error occured during the Minimum DL calculations", 
			BuildToTargetListEnum.UnexpectedBuildException => "An unexpected exception occured during the Build calculations", 
			BuildToTargetListEnum.UnknownBuildError => "An unknown error occured during the Build calculations", 
			BuildToTargetListEnum.InternalError => "An unexpected internal error occured during the calculations", 
			_ => "An unknown error occured during the calculations", 
		};
	}

	public void BuildToTargetListSummary(ref List<BuildAndHoldResults> buildList, out LinkedList<string[]> summaryList)
	{
		if (buildList == null)
		{
			throw new ArgumentNullException("BuildList.", "BuildList cannot be null.");
		}
		summaryList = new LinkedList<string[]>();
		foreach (BuildAndHoldResults build in buildList)
		{
			string[] value = new string[2]
			{
				build.DogLeg.ToString(),
				build.NaviDistance.ToString()
			};
			summaryList.AddLast(value);
		}
	}

	public decimal Quantization(decimal number, decimal precission)
	{
		if (precission > 0m)
		{
			decimal d = number / precission;
			decimal num = System.Math.Round(d, 0);
			return num * precission;
		}
		throw new ArgumentException("precission", "precission must be greater than zero");
	}

	public MinimumDogLegEnum MinimumDogLegToTarget(AbsoluteSurvey lastSurvey, Target target, ref decimal dogLeg, out BuildAndHoldResults buildAndHold)
	{
		if ((lastSurvey == null) | (target == null))
		{
			throw new ArgumentNullException();
		}
		buildAndHold = new BuildAndHoldResults();
		try
		{
			MinimumDogLegToTarget_Errors minimumDogLegToTarget_Errors = math.MinimumDogLegToTarget(lastSurvey, target, ref dogLeg, ref buildAndHold, 5.0);
			buildAndHold.DogLeg = dogLeg;
			if (minimumDogLegToTarget_Errors == MinimumDogLegToTarget_Errors.NoError)
			{
				return MinimumDogLegEnum.NoError;
			}
			return minimumDogLegToTarget_Errors switch
			{
				MinimumDogLegToTarget_Errors.InsufficientTVD => MinimumDogLegEnum.InsufficientTVD, 
				MinimumDogLegToTarget_Errors.NoError => MinimumDogLegEnum.NoError, 
				MinimumDogLegToTarget_Errors.TargetCannotBeAchieved => MinimumDogLegEnum.TargetCannotBeAchieved, 
				_ => MinimumDogLegEnum.UnknownError, 
			};
		}
		catch
		{
			return MinimumDogLegEnum.UnknownException;
		}
	}

	public static string ToHumanReadable(MinimumDogLegEnum value)
	{
		return value switch
		{
			MinimumDogLegEnum.NoError => "Operation succesfull", 
			MinimumDogLegEnum.TargetCannotBeAchieved => "The target cannot be achieved", 
			MinimumDogLegEnum.InsufficientTVD => "There is not enough TVD available to achieve the target", 
			MinimumDogLegEnum.UnknownError => "Unknown error", 
			MinimumDogLegEnum.UnknownException => "Unknown exception has occured", 
			_ => "Unknown error has occured", 
		};
	}

	public TrendAnalysisEnum TrendAnalysis(Target target, SortableBindingList<AbsoluteSurvey> surveyList, ref TrendAnalysisResults analysisResults, ref AbsoluteSurvey endOfRealHoleSurvey)
	{
		AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
		absoluteSurveyList.List = surveyList;
		analysisResults = new TrendAnalysisResults();
		if ((target != null) & (surveyList != null) & (analysisResults != null))
		{
			if (surveyList.Count >= 2)
			{
				try
				{
					TrendAnalysis_Errors trendAnalysis_Errors = math.TrendAnalysis(target, absoluteSurveyList, ref analysisResults, endOfRealHoleSurvey, 10.0);
					if (absoluteSurveyList.LastRecord().TVD >= target.TVD)
					{
						analysisResults.ExtrapolatedSurveyData = null;
					}
					return trendAnalysis_Errors switch
					{
						TrendAnalysis_Errors.NoError => TrendAnalysisEnum.Success, 
						TrendAnalysis_Errors.TargetTVDCannotBeReached => TrendAnalysisEnum.TargetTVDCannotBeAchieved, 
						TrendAnalysis_Errors.TrendCannotBeAchieved => TrendAnalysisEnum.TrendCannotBeAchieved, 
						TrendAnalysis_Errors.StraightPath => TrendAnalysisEnum.StraightPath, 
						_ => TrendAnalysisEnum.UnknownError, 
					};
				}
				catch (Exception)
				{
					return TrendAnalysisEnum.UnknownException;
				}
			}
			return TrendAnalysisEnum.NotEnoughSurveys;
		}
		return TrendAnalysisEnum.NullParameter;
	}

	public static string ToHumanReadable(TrendAnalysisEnum value)
	{
		return value switch
		{
			TrendAnalysisEnum.Success => "Operation completed successfully", 
			TrendAnalysisEnum.NullParameter => "Null Parameter", 
			TrendAnalysisEnum.NotEnoughSurveys => "Trend analysis is only possible with two or more surveys", 
			TrendAnalysisEnum.UnknownError => "An unknown error occured during the calculations", 
			TrendAnalysisEnum.UnknownException => "An unknown exception occured during the calculations", 
			TrendAnalysisEnum.TrendCannotBeAchieved => "The trend cannot be achieved", 
			TrendAnalysisEnum.TargetTVDCannotBeAchieved => "The target TVD cannot be achieved with the selected trend", 
			TrendAnalysisEnum.StraightPath => "The selected range is a straight path and has no trend", 
			_ => "Internal system error occured", 
		};
	}

	public SideTrackHoleEnum SideTrackHole(AbsoluteSurveyList surveyList, decimal md, out AbsoluteSurvey extrapolatedSurvey)
	{
		extrapolatedSurvey = new AbsoluteSurvey();
		if (surveyList == null)
		{
			return SideTrackHoleEnum.NullParameter;
		}
		if (md <= 0m)
		{
			return SideTrackHoleEnum.DepthNotGreaterThanZero;
		}
		if (surveyList.Count() < 1)
		{
			return SideTrackHoleEnum.NoSurveyData;
		}
		if (md > surveyList.LastRecord().MD)
		{
			return SideTrackHoleEnum.DepthOutsideSurveyRange;
		}
		AbsoluteSurvey absoluteSurvey = null;
		AbsoluteSurvey absoluteSurvey2 = null;
		foreach (AbsoluteSurvey survey in surveyList)
		{
			if (survey.MD == md)
			{
				extrapolatedSurvey = survey;
				return SideTrackHoleEnum.Found;
			}
			if (survey.MD > md)
			{
				if (absoluteSurvey == null)
				{
					return SideTrackHoleEnum.InsufficientSurveyData;
				}
				absoluteSurvey2 = survey;
				return ExtrapolateBetweenSurveys_MD(absoluteSurvey, absoluteSurvey2, md, out extrapolatedSurvey) switch
				{
					ExtrapolateBetweenMDEnum.NullParameter => SideTrackHoleEnum.InternalError, 
					ExtrapolateBetweenMDEnum.DepthNotGreaterThanZero => SideTrackHoleEnum.InternalError, 
					ExtrapolateBetweenMDEnum.DepthNotBetweenSurveys => SideTrackHoleEnum.InternalError, 
					ExtrapolateBetweenMDEnum.InvalidSurveys => SideTrackHoleEnum.InternalError, 
					ExtrapolateBetweenMDEnum.InvalidDepth => SideTrackHoleEnum.InternalError, 
					ExtrapolateBetweenMDEnum.UnknownError => SideTrackHoleEnum.UnknownExtrapolateBetweenError, 
					ExtrapolateBetweenMDEnum.UnknownException => SideTrackHoleEnum.UnknownExtrapolateBetweenException, 
					ExtrapolateBetweenMDEnum.Success => SideTrackHoleEnum.Extrapolated, 
					_ => SideTrackHoleEnum.InternalError, 
				};
			}
			absoluteSurvey = survey;
		}
		return SideTrackHoleEnum.InternalError;
	}

	public static string ToHumanReadable(SideTrackHoleEnum value)
	{
		return value switch
		{
			SideTrackHoleEnum.Found => "A Survey was found at the specified MD", 
			SideTrackHoleEnum.Extrapolated => "A Survey was extrapolated at the specified MD", 
			SideTrackHoleEnum.NullParameter => "Null parameters", 
			SideTrackHoleEnum.DepthNotGreaterThanZero => "The MD (Measured Depth) must be greater than 0", 
			SideTrackHoleEnum.DepthOutsideSurveyRange => "The MD (Measured Depth) does not fall within the survey data", 
			SideTrackHoleEnum.InsufficientSurveyData => "There are not enough surveys to perfom an extrapolation", 
			SideTrackHoleEnum.NoSurveyData => "There is no survey data", 
			SideTrackHoleEnum.InternalError => "An internal program error has occured", 
			SideTrackHoleEnum.UnknownExtrapolateBetweenError => "An unexpected error occured during the extrapolation calculations", 
			SideTrackHoleEnum.UnknownExtrapolateBetweenException => "An unexpected exception occured during the extrapolation calculations", 
			_ => "An unknown error has occured", 
		};
	}

	public ExtrapolateBetweenMDEnum ExtrapolateBetweenSurveys_MD(AbsoluteSurvey surveyAbove, AbsoluteSurvey surveyBelow, decimal md, out AbsoluteSurvey extrapolatedSurvey)
	{
		extrapolatedSurvey = new AbsoluteSurvey();
		if (surveyAbove == null || surveyBelow == null)
		{
			return ExtrapolateBetweenMDEnum.NullParameter;
		}
		if (md <= 0m)
		{
			return ExtrapolateBetweenMDEnum.DepthNotGreaterThanZero;
		}
		if (surveyAbove.MD >= surveyBelow.MD)
		{
			return ExtrapolateBetweenMDEnum.InvalidSurveys;
		}
		if (md <= surveyAbove.MD || md >= surveyBelow.MD)
		{
			return ExtrapolateBetweenMDEnum.DepthNotBetweenSurveys;
		}
		try
		{
			ExtrapolateBetweenSurveys_Errors extrapolateBetweenSurveys_Errors = math.ExtrapolateBetweenSurveys_MD(surveyAbove, surveyBelow, md, ref extrapolatedSurvey);
			if (extrapolateBetweenSurveys_Errors == ExtrapolateBetweenSurveys_Errors.NoError)
			{
				return ExtrapolateBetweenMDEnum.Success;
			}
			extrapolatedSurvey = null;
			if (extrapolateBetweenSurveys_Errors == ExtrapolateBetweenSurveys_Errors.InvalidMD)
			{
				return ExtrapolateBetweenMDEnum.InvalidDepth;
			}
			return ExtrapolateBetweenMDEnum.UnknownError;
		}
		catch
		{
			extrapolatedSurvey = null;
			return ExtrapolateBetweenMDEnum.UnknownException;
		}
	}

	public static string ToHumanReadable(ExtrapolateBetweenMDEnum value)
	{
		return value switch
		{
			ExtrapolateBetweenMDEnum.Success => "Operation completed successfully", 
			ExtrapolateBetweenMDEnum.NullParameter => "Null parameter passed", 
			ExtrapolateBetweenMDEnum.DepthNotGreaterThanZero => "The MD value must be greater than Zero", 
			ExtrapolateBetweenMDEnum.DepthNotBetweenSurveys => "The MD must be between the two surveys", 
			ExtrapolateBetweenMDEnum.InvalidSurveys => "The surveys are not in the correct order", 
			ExtrapolateBetweenMDEnum.InvalidDepth => "The MD value is invalid", 
			ExtrapolateBetweenMDEnum.UnknownError => "An unexpected error error during the calculations", 
			ExtrapolateBetweenMDEnum.UnknownException => "An unexpected exception error during the calculations", 
			_ => "An unknown error occured", 
		};
	}

	public ExtrapolateBetweenTVDEnum ExtrapolateBetweenSurveys_TVD(AbsoluteSurvey surveyAbove, AbsoluteSurvey surveyBelow, decimal tvd, out AbsoluteSurvey extrapolatedSurvey)
	{
		extrapolatedSurvey = new AbsoluteSurvey();
		if (surveyAbove == null || surveyBelow == null)
		{
			return ExtrapolateBetweenTVDEnum.NullParameter;
		}
		if (tvd <= 0m)
		{
			return ExtrapolateBetweenTVDEnum.DepthNotGreaterThanZero;
		}
		if (surveyAbove.TVD >= surveyBelow.TVD)
		{
			return ExtrapolateBetweenTVDEnum.InvalidSurveys;
		}
		if (tvd <= surveyAbove.TVD || tvd >= surveyBelow.TVD)
		{
			return ExtrapolateBetweenTVDEnum.DepthNotBetweenSurveys;
		}
		try
		{
			ExtrapolateBetweenSurveys_TVD_Errors extrapolateBetweenSurveys_TVD_Errors = math.ExtrapolateBetweenSurveys_TVD(surveyAbove, surveyBelow, tvd, ref extrapolatedSurvey);
			if (extrapolateBetweenSurveys_TVD_Errors == ExtrapolateBetweenSurveys_TVD_Errors.NoError)
			{
				return ExtrapolateBetweenTVDEnum.Success;
			}
			extrapolatedSurvey = null;
			return ExtrapolateBetweenTVDEnum.UnknownError;
		}
		catch
		{
			extrapolatedSurvey = null;
			return ExtrapolateBetweenTVDEnum.UnknownException;
		}
	}

	public static string ToHumanReadable(ExtrapolateBetweenTVDEnum value)
	{
		return value switch
		{
			ExtrapolateBetweenTVDEnum.Success => "Operation completed successfully", 
			ExtrapolateBetweenTVDEnum.NullParameter => "Null parameter passed", 
			ExtrapolateBetweenTVDEnum.DepthNotGreaterThanZero => "The TVD value must be greater than Zero", 
			ExtrapolateBetweenTVDEnum.DepthNotBetweenSurveys => "The TVD must be between the two surveys", 
			ExtrapolateBetweenTVDEnum.InvalidSurveys => "The surveys are not in the correct order", 
			ExtrapolateBetweenTVDEnum.InvalidDepth => "The TVD value is invalid", 
			ExtrapolateBetweenTVDEnum.UnknownError => "An unexpected error occurred during the calculations", 
			ExtrapolateBetweenTVDEnum.UnknownException => "An unexpected exception occurred during the calculations", 
			_ => "An unknown error occured", 
		};
	}

	public ExtrapolateToMDResultEnum ExtrapolateToMD(decimal MD, decimal dogleg, decimal toolface, AbsoluteSurvey lastSurvey, out ExtrapolatedAbsoluteSurvey lastExtrapolatedSurvey)
	{
		AbsoluteSurveyList results = new AbsoluteSurveyList();
		lastExtrapolatedSurvey = new ExtrapolatedAbsoluteSurvey();
		try
		{
			Extrapolate_MD_Errors extrapolate_MD_Errors = math.ExtrapolateToMD((double)MD, (double)dogleg, (double)toolface, lastSurvey, ref results, ref lastExtrapolatedSurvey, 5.0);
			lastExtrapolatedSurvey.SetAbsoluteSurveyList(results);
			return extrapolate_MD_Errors switch
			{
				Extrapolate_MD_Errors.DipTooGreat => ExtrapolateToMDResultEnum.DipTooGreat, 
				Extrapolate_MD_Errors.InavlidDogLeg => ExtrapolateToMDResultEnum.InvalidDogleg, 
				Extrapolate_MD_Errors.InvalidMD => ExtrapolateToMDResultEnum.InvalidMD, 
				Extrapolate_MD_Errors.InvalidToolFace => ExtrapolateToMDResultEnum.InvalidToolface, 
				Extrapolate_MD_Errors.NoError => ExtrapolateToMDResultEnum.NoError, 
				Extrapolate_MD_Errors.PathCannotBeAchieved => ExtrapolateToMDResultEnum.PathCannotBeAchieved, 
				Extrapolate_MD_Errors.MaximumDipReachedBeforeDepth => ExtrapolateToMDResultEnum.MaximumDipReachedBeforeDepth, 
				Extrapolate_MD_Errors.DepthTooGreat => ExtrapolateToMDResultEnum.DepthTooGreat, 
				Extrapolate_MD_Errors.InvalidInterval => ExtrapolateToMDResultEnum.UnknownError, 
				_ => ExtrapolateToMDResultEnum.UnknownError, 
			};
		}
		catch
		{
			return ExtrapolateToMDResultEnum.UnknownException;
		}
	}

	public static string ToHumanReadable(ExtrapolateToMDResultEnum value)
	{
		return value switch
		{
			ExtrapolateToMDResultEnum.InvalidDogleg => "The Dogleg entered is invalid. The Dogleg must be greater than 0 and less than or equal to 30", 
			ExtrapolateToMDResultEnum.InvalidToolface => "The Toolface entered is invalid. The Toolface must be greater than or equal to 0 and less than or equal to 360", 
			ExtrapolateToMDResultEnum.InvalidMD => "The MD(Measured Depth) entered is invalid. The MD must be greater than that of the previouse survey", 
			ExtrapolateToMDResultEnum.DipTooGreat => "The Dip generated by the entered data is too great. The Dip of an extrapolated survey must be less than 90 degrees", 
			ExtrapolateToMDResultEnum.PathCannotBeAchieved => "The path generated by the entered data cannot be achieved", 
			ExtrapolateToMDResultEnum.MaximumDipReachedBeforeDepth => "The maximum allowed dip of 89.99 was reached before the entered MD", 
			ExtrapolateToMDResultEnum.DepthTooGreat => "The MD(Measured Depth) entered is greater than the maximum allowed MD of 24,000m", 
			ExtrapolateToMDResultEnum.NoError => "Operation completed successfully", 
			ExtrapolateToMDResultEnum.UnknownError => "The survey could not be extrapolated. A fatal internal error occured during the calculations", 
			ExtrapolateToMDResultEnum.UnknownException => "The survey could not be extrapolated. A unknown error occured during the calculations", 
			_ => "The survey could not be extrapolated. A fatal internal error occured during the calculations", 
		};
	}

	public ExtrapolateToTVDResultEnum ExtrapolateToTVD(decimal TVD, decimal dogleg, decimal toolface, AbsoluteSurvey lastSurvey, out ExtrapolatedAbsoluteSurvey lastExtrapolatedSurvey)
	{
		AbsoluteSurveyList resultList = new AbsoluteSurveyList();
		lastExtrapolatedSurvey = new ExtrapolatedAbsoluteSurvey();
		try
		{
			Extrapolate_TVD_Errors extrapolate_TVD_Errors = math.ExtrapolateToTVD((double)TVD, (double)dogleg, (double)toolface, lastSurvey, ref resultList, ref lastExtrapolatedSurvey, 5.0);
			lastExtrapolatedSurvey.SetAbsoluteSurveyList(resultList);
			return extrapolate_TVD_Errors switch
			{
				Extrapolate_TVD_Errors.DipTooGreat => ExtrapolateToTVDResultEnum.DipTooGreat, 
				Extrapolate_TVD_Errors.InvalidDogLeg => ExtrapolateToTVDResultEnum.InvalidDogleg, 
				Extrapolate_TVD_Errors.InvalidTVD => ExtrapolateToTVDResultEnum.InvalidTVD, 
				Extrapolate_TVD_Errors.InvalidToolFace => ExtrapolateToTVDResultEnum.InvalidToolface, 
				Extrapolate_TVD_Errors.NoError => ExtrapolateToTVDResultEnum.NoError, 
				Extrapolate_TVD_Errors.PathCannotBeAchieved => ExtrapolateToTVDResultEnum.PathCannotBeAchieved, 
				Extrapolate_TVD_Errors.MaximumDipReachedBeforeDepth => ExtrapolateToTVDResultEnum.MaximumDipReachedBeforeDepth, 
				Extrapolate_TVD_Errors.MaxMDReachedBeforeTVD => ExtrapolateToTVDResultEnum.MaxMDReachedBeforeTVD, 
				Extrapolate_TVD_Errors.DepthTooGreat => ExtrapolateToTVDResultEnum.DepthTooGreat, 
				Extrapolate_TVD_Errors.InvalidInterval => ExtrapolateToTVDResultEnum.UnknownError, 
				_ => ExtrapolateToTVDResultEnum.UnknownError, 
			};
		}
		catch
		{
			return ExtrapolateToTVDResultEnum.UnknownException;
		}
	}

	public static string ToHumanReadable(ExtrapolateToTVDResultEnum value)
	{
		return value switch
		{
			ExtrapolateToTVDResultEnum.InvalidDogleg => "The Dogleg entered is invalid. The Dogleg must be greater than 0 and less than or equal to 30", 
			ExtrapolateToTVDResultEnum.InvalidToolface => "The Toolface entered is invalid. The Toolface must be greater than or equal to 0 and less than or equal to 360", 
			ExtrapolateToTVDResultEnum.InvalidTVD => "The TVD entered is invalid. The TVD must be greater than that of the previouse survey", 
			ExtrapolateToTVDResultEnum.DipTooGreat => "The Dip generated by the entered data is too great. The Dip of an extrapolated survey must be less than 90 degrees", 
			ExtrapolateToTVDResultEnum.PathCannotBeAchieved => "The path generated by the entered data cannot be achieved", 
			ExtrapolateToTVDResultEnum.MaximumDipReachedBeforeDepth => "The maximum allowed Dip of 89.99 was reached before the entered TVD (True Vertical Depth)", 
			ExtrapolateToTVDResultEnum.MaxMDReachedBeforeTVD => "The maximum allowed MD (Measured Depth) of 24,000m was reached before the specified TVD (True Vertical Depth)", 
			ExtrapolateToTVDResultEnum.DepthTooGreat => "The TVD entered generates a MD(Measured Depth) that is greater than the maximum allowed MD of 24,000m", 
			ExtrapolateToTVDResultEnum.NoError => "Operation completed successfully", 
			ExtrapolateToTVDResultEnum.UnknownError => "The survey could not be extrapolated. A fatal internal error occured during the calculations", 
			ExtrapolateToTVDResultEnum.UnknownException => "The survey could not be extrapolated. A unknown error occured during the calculations", 
			_ => "The survey could not be extrapolated. A fatal internal error occured during the calculations", 
		};
	}

	public ExtrapolateToDirectionResultEnum ExtrapolateToDirection(decimal inclination, decimal direction, decimal dogleg, AbsoluteSurvey lastSurvey, out ExtrapolatedAbsoluteSurvey lastExtrapolatedSurvey)
	{
		AbsoluteSurveyList Results = new AbsoluteSurveyList();
		lastExtrapolatedSurvey = new ExtrapolatedAbsoluteSurvey();
		try
		{
			Extrapolate_Dir_Errors extrapolate_Dir_Errors = math.ExtrapolateToDirection((double)inclination, (double)direction, (double)dogleg, lastSurvey, ref Results, ref lastExtrapolatedSurvey, 5.0);
			lastExtrapolatedSurvey.SetAbsoluteSurveyList(Results);
			return extrapolate_Dir_Errors switch
			{
				Extrapolate_Dir_Errors.DipTooGreat => ExtrapolateToDirectionResultEnum.DipTooGreat, 
				Extrapolate_Dir_Errors.InvalidAzimuth => ExtrapolateToDirectionResultEnum.InvalidDirection, 
				Extrapolate_Dir_Errors.InvalidDogLeg => ExtrapolateToDirectionResultEnum.InvalidDogleg, 
				Extrapolate_Dir_Errors.InvalidIncliation => ExtrapolateToDirectionResultEnum.InvalidInclination, 
				Extrapolate_Dir_Errors.NoError => ExtrapolateToDirectionResultEnum.NoError, 
				Extrapolate_Dir_Errors.PathCannotBeAchieved => ExtrapolateToDirectionResultEnum.PathCannotBeAchieved, 
				Extrapolate_Dir_Errors.DepthTooGreatPickHigherDogLeg => ExtrapolateToDirectionResultEnum.DepthTooGreatPickHigherDogLeg, 
				Extrapolate_Dir_Errors.AlreadyThere => ExtrapolateToDirectionResultEnum.AlreadyThere, 
				Extrapolate_Dir_Errors.InvalidInterval => ExtrapolateToDirectionResultEnum.UnknownError, 
				_ => ExtrapolateToDirectionResultEnum.UnknownError, 
			};
		}
		catch
		{
			return ExtrapolateToDirectionResultEnum.UnknownException;
		}
	}

	public static string ToHumanReadable(ExtrapolateToDirectionResultEnum value)
	{
		return value switch
		{
			ExtrapolateToDirectionResultEnum.InvalidDogleg => "The Dogleg entered is invalid. The Dogleg must be greater than 0 and less than or equal to 30", 
			ExtrapolateToDirectionResultEnum.InvalidDirection => "The Direction entered is invalid. The Direction must be greater than or equal to 0 and less than or equal to 360", 
			ExtrapolateToDirectionResultEnum.InvalidInclination => "The Dip entered is invalid. The Dip must be greater than or equal to 0 and less than 90", 
			ExtrapolateToDirectionResultEnum.DipTooGreat => "The Dip generated by the entered data is too great. The Dip of an extrapolated survey must be less than 90 degrees", 
			ExtrapolateToDirectionResultEnum.PathCannotBeAchieved => "The path generated by the entered data cannot be achieved", 
			ExtrapolateToDirectionResultEnum.DepthTooGreatPickHigherDogLeg => "The entered Direction could not be achieved before reaching the maximum allowed MD (Measured Depth) of 24,000m. The direction may be achievable with a higher DL (Dogleg)", 
			ExtrapolateToDirectionResultEnum.AlreadyThere => "The entered Dip and Azimuth are the same as the previous survey", 
			ExtrapolateToDirectionResultEnum.NoError => "Operation completed successfully", 
			ExtrapolateToDirectionResultEnum.UnknownError => "The survey could not be extrapolated. A fatal internal error occured during the calculations", 
			ExtrapolateToDirectionResultEnum.UnknownException => "The survey could not be extrapolated. A unknown error occured during the calculations", 
			_ => "The survey could not be extrapolated. A fatal internal error occured during the calculations", 
		};
	}

	public ExtrapolateToInclinationResultEnum ExtrapolateToInclination(decimal inclination, decimal dogleg, decimal toolface, AbsoluteSurvey lastSurvey, out ExtrapolatedAbsoluteSurvey lastExtrapolatedSurvey)
	{
		AbsoluteSurveyList resultList = new AbsoluteSurveyList();
		lastExtrapolatedSurvey = new ExtrapolatedAbsoluteSurvey();
		try
		{
			Extrapolate_Inc_Errors extrapolate_Inc_Errors = math.ExtrapolateToInclination((double)inclination, (double)dogleg, (double)toolface, lastSurvey, ref resultList, ref lastExtrapolatedSurvey, 5.0);
			lastExtrapolatedSurvey.SetAbsoluteSurveyList(resultList);
			return extrapolate_Inc_Errors switch
			{
				Extrapolate_Inc_Errors.DipTooGreat => ExtrapolateToInclinationResultEnum.DipTooGreat, 
				Extrapolate_Inc_Errors.InvalidDogLeg => ExtrapolateToInclinationResultEnum.InvalidDogleg, 
				Extrapolate_Inc_Errors.InvalidInclination => ExtrapolateToInclinationResultEnum.InvalidInclination, 
				Extrapolate_Inc_Errors.InvalidToolFace => ExtrapolateToInclinationResultEnum.InvalidToolface, 
				Extrapolate_Inc_Errors.NoError => ExtrapolateToInclinationResultEnum.NoError, 
				Extrapolate_Inc_Errors.PathCannotBeAchieved => ExtrapolateToInclinationResultEnum.PathCannotBeAchieved, 
				Extrapolate_Inc_Errors.IncSameAsPrevious => ExtrapolateToInclinationResultEnum.IncSameAsPrevious, 
				Extrapolate_Inc_Errors.CannotAchieveIncWithTFS => ExtrapolateToInclinationResultEnum.CannotAchieveIncWithTFS, 
				Extrapolate_Inc_Errors.InvalidInterval => ExtrapolateToInclinationResultEnum.UnknownError, 
				Extrapolate_Inc_Errors.MaximumMDReachedBeforeInclination => ExtrapolateToInclinationResultEnum.MaximumMDReachedBeforeInclination, 
				_ => ExtrapolateToInclinationResultEnum.UnknownError, 
			};
		}
		catch
		{
			return ExtrapolateToInclinationResultEnum.UnknownException;
		}
	}

	public static string ToHumanReadable(ExtrapolateToInclinationResultEnum value)
	{
		return value switch
		{
			ExtrapolateToInclinationResultEnum.InvalidDogleg => "The Dogleg entered is invalid. The Dogleg must be greater than 0 and less than or equal to 30", 
			ExtrapolateToInclinationResultEnum.InvalidToolface => "The Toolface entered is invalid. The Toolface must be greater than or equal to 0 and less than or equal to 360", 
			ExtrapolateToInclinationResultEnum.InvalidInclination => "The Dip entered is invalid. The Dip must be greater than or equal to 0 and less than 90", 
			ExtrapolateToInclinationResultEnum.DipTooGreat => "The Dip generated by the entered data is too great. The dip of an extrapolated survey is restricted to less than 90 degrees", 
			ExtrapolateToInclinationResultEnum.PathCannotBeAchieved => "The path generated by the entered data cannot be achieved", 
			ExtrapolateToInclinationResultEnum.IncSameAsPrevious => "The entered Dip is the same as the Dip of the previous survey.", 
			ExtrapolateToInclinationResultEnum.CannotAchieveIncWithTFS => "The Dip entered cannot be achieved with the Toolface setting entered", 
			ExtrapolateToInclinationResultEnum.MaximumMDReachedBeforeInclination => "The maximum allowed MD of 24,000m was reached before the target Dip could be acheived with the entered DL", 
			ExtrapolateToInclinationResultEnum.NoError => "Operation completed successfully", 
			ExtrapolateToInclinationResultEnum.UnknownError => "The survey could not be extrapolated. A fatal internal error occured during the calculations", 
			ExtrapolateToInclinationResultEnum.UnknownException => "The survey could not be extrapolated. A unknown error occured during the calculations", 
			_ => "The survey could not be extrapolated. A fatal internal error occured during the calculations", 
		};
	}

	public decimal HorizontalDistance(Target target, Collar collar)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target", "Target parameter cannot be null");
		}
		if (collar == null)
		{
			throw new ArgumentNullException("collar", "Collar parameter cannot be null");
		}
		decimal num = collar.Easting - target.Easting;
		decimal num2 = collar.Northing - target.Northing;
		try
		{
			decimal num3 = (decimal)(System.Math.Pow((double)num, 2.0) + System.Math.Pow((double)num2, 2.0));
			return (decimal)System.Math.Sqrt((double)num3);
		}
		catch (OverflowException innerException)
		{
			throw new OverflowException("The calculated horizontal distance exceeded the maximum or minimum value for the decimal type", innerException);
		}
	}
}
