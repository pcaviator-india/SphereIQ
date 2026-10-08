using System;

namespace EzyPlanData;

public class ExtrapolatedSurveyList
{
	public enum AppendEnum
	{
		Appended,
		NotGreaterThanPreviouse
	}

	private AbsoluteSurvey m_lastRealSurvey;

	private SortableBindingList<ExtrapolatedAbsoluteSurvey> m_surveyList;

	private decimal m_mdDelete;

	public AbsoluteSurvey LastRealSurvey => new AbsoluteSurvey(m_lastRealSurvey);

	public SortableBindingList<ExtrapolatedAbsoluteSurvey> List => m_surveyList;

	public SortableBindingList<AbsoluteSurvey> AbsoluteSurveyList
	{
		get
		{
			string[] names = new string[7] { "MD", "Dip", "Azimuth", "TVD", "Northing", "Easting", "DL" };
			SortableBindingList<AbsoluteSurvey> sortableBindingList = new SortableBindingList<AbsoluteSurvey>(names);
			if (LastRealSurvey != null)
			{
				sortableBindingList.Add(LastRealSurvey);
			}
			foreach (ExtrapolatedAbsoluteSurvey item in List)
			{
				sortableBindingList.Add(item);
			}
			return sortableBindingList;
		}
	}

	public ExtrapolatedSurveyList(AbsoluteSurvey lastRealSurvey)
	{
		if (lastRealSurvey != null)
		{
			m_lastRealSurvey = lastRealSurvey;
			string[] names = new string[8] { "MD", "Dip", "Azimuth", "TVD", "Northing", "Easting", "DL", "Toolface" };
			m_surveyList = new SortableBindingList<ExtrapolatedAbsoluteSurvey>(names);
			return;
		}
		throw new ArgumentNullException("The lastRealSurvey (AbsoluteSurvey) cannot be null");
	}

	public AppendEnum Append(ExtrapolatedAbsoluteSurvey survey)
	{
		ExtrapolatedAbsoluteSurvey lastSurvey = GetLastSurvey();
		if (survey.MD > lastSurvey.MD)
		{
			m_surveyList.Add(survey);
			return AppendEnum.Appended;
		}
		return AppendEnum.NotGreaterThanPreviouse;
	}

	public string ToHumanReadable(AppendEnum value)
	{
		return value switch
		{
			AppendEnum.Appended => "Operation Successfull", 
			AppendEnum.NotGreaterThanPreviouse => "The MD is not greater than the MD of the previouse survey.", 
			_ => "Unknown Error", 
		};
	}

	public void Remove()
	{
		if (m_surveyList.Count > 0)
		{
			m_surveyList.RemoveAt(m_surveyList.Count - 1);
		}
	}

	public ExtrapolatedAbsoluteSurvey GetLastSurvey()
	{
		int count = m_surveyList.Count;
		if (count > 0)
		{
			return m_surveyList[count - 1];
		}
		return new ExtrapolatedAbsoluteSurvey(LastRealSurvey);
	}

	public AbsoluteSurveyList GetFullExtrapolatedPath()
	{
		AbsoluteSurveyList absoluteSurveyList = new AbsoluteSurveyList();
		absoluteSurveyList.Add(LastRealSurvey);
		foreach (ExtrapolatedAbsoluteSurvey item in List)
		{
			foreach (AbsoluteSurvey absoluteSurvey in item.GetAbsoluteSurveyList())
			{
				absoluteSurveyList.Add(absoluteSurvey);
			}
		}
		return absoluteSurveyList;
	}

	public ExtrapolatedAbsoluteSurvey GetSurveyFromMD(decimal md)
	{
		m_mdDelete = md;
		foreach (ExtrapolatedAbsoluteSurvey survey in m_surveyList)
		{
			if (survey.MD == md)
			{
				return survey;
			}
		}
		return null;
	}

	private bool MatchMDPredicate(AbsoluteSurvey survey)
	{
		if (survey.MD == m_mdDelete)
		{
			return true;
		}
		return false;
	}
}
