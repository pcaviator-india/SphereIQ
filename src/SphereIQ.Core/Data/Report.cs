using System;

namespace EzyPlanData;

[Serializable]
public class Report
{
	private string m_createdBy;

	private DateTime m_createdDate;

	private string m_notes;

	private string m_name;

	public string CreatedBy
	{
		get
		{
			return m_createdBy;
		}
		set
		{
			if (value != m_createdBy)
			{
				m_createdBy = value;
			}
		}
	}

	public DateTime CreatedDate
	{
		get
		{
			return m_createdDate;
		}
		set
		{
			if (value != m_createdDate)
			{
				m_createdDate = value;
			}
		}
	}

	public string Notes
	{
		get
		{
			return m_notes;
		}
		set
		{
			if (value != m_notes)
			{
				m_notes = value;
			}
		}
	}

	public string Name
	{
		get
		{
			return m_name;
		}
		set
		{
			if (value != m_name)
			{
				m_name = value;
			}
		}
	}

	public Report()
	{
	}

	public Report(string setName, string setCreatedBy, DateTime setCreatedDate, string setNotes)
		: this()
	{
		Set(setName, setCreatedBy, setCreatedDate, setNotes);
	}

	public Report(Report original)
		: this()
	{
		Copy(original);
	}

	public void Set(string setName, string setCreatedBy, DateTime setCreatedDate, string setNotes)
	{
		Name = setName;
		CreatedBy = setCreatedBy;
		CreatedDate = setCreatedDate;
		Notes = setNotes;
	}

	public void Copy(Report original)
	{
		Name = original.Name;
		CreatedBy = original.CreatedBy;
		CreatedDate = original.CreatedDate;
		Notes = original.Notes;
	}

	public override string ToString()
	{
		return Name + ", " + CreatedBy + ", " + CreatedDate.ToString() + ", " + Notes;
	}
}
