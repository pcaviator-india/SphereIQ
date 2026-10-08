using System;
using System.IO;
using EzyNavMath;
using EzyPlanData;

namespace EzyPlanStorage;

public class DrillHoleFile
{
	public enum FileStreamStatus
	{
		Disconected,
		Connected
	}

	public enum DrillHoleTypeEnum
	{
		motherHole,
		daughterHole,
		daughterHoleNoSurveyAtSidetrack
	}

	public enum OpenFile_ErrorsEnum
	{
		Null,
		FileAlreadyOpen,
		DeSerialisationError,
		NoError,
		DirectoryNotFound,
		FileNotFound,
		PathIsNull,
		IOException,
		AccessNotPermited,
		PathIsTooLong,
		NotSupported,
		CodeBug,
		UnknownFileError
	}

	public enum SaveAs_ErrorsEnum
	{
		SerialisationError,
		NoError,
		DirectoryNotFound,
		FileNotFound,
		PathIsNull,
		PathIsInvalid,
		IOException,
		AccessNotPermited,
		PathIsTooLong,
		NotSupported,
		UnknownFileError
	}

	public enum Save_ErrorsEnum
	{
		SerialisationError,
		NoFileOpen,
		IOError,
		NoError
	}

	private SurveyList m_surveyList;

	private Target m_target;

	private Collar m_collar;

	private ReportList m_reportList;

	private DateTime m_createdDate;

	private decimal m_sideTrackMD;

	private string m_holeId;

	private string m_rigId;

	private string m_location;

	private string m_area;

	private string m_drillingContractor;

	private string m_surveyingContractor;

	private string m_client;

	private FileStream m_fileStream;

	private FileStreamStatus m_fileStatus;

	public FileStreamStatus Status => m_fileStatus;

	public string HoleID
	{
		get
		{
			return m_holeId;
		}
		set
		{
			m_holeId = value;
		}
	}

	public string RigID
	{
		get
		{
			return m_rigId;
		}
		set
		{
			m_rigId = value;
		}
	}

	public string Location
	{
		get
		{
			return m_location;
		}
		set
		{
			m_location = value;
		}
	}

	public string Area
	{
		get
		{
			return m_area;
		}
		set
		{
			m_area = value;
		}
	}

	public string DrillingContractor
	{
		get
		{
			return m_drillingContractor;
		}
		set
		{
			m_drillingContractor = value;
		}
	}

	public string SurveyingContractor
	{
		get
		{
			return m_surveyingContractor;
		}
		set
		{
			m_surveyingContractor = value;
		}
	}

	public string Client
	{
		get
		{
			return m_client;
		}
		set
		{
			m_client = value;
		}
	}

	public DateTime CreatedDate => m_createdDate;

	public void SetCreatedDate(DateTime value)
	{
		m_createdDate = value;
	}

	public Target Target
	{
		get
		{
			return m_target;
		}
		set
		{
			if (value != null)
			{
				if (m_collar != null)
				{
					NavMath navMath = new NavMath();
					try
					{
						if (navMath.HorizontalDistance(value, m_collar) > 24000m)
						{
							throw new ArgumentException("The horizontal distance between the collar and target cannot exceed 24,000 meters");
						}
						m_target = new Target(value);
						return;
					}
					catch (OverflowException)
					{
						throw new ArgumentException("The horizontal distance between the collar and target cannot exceed 24,000 meters");
					}
				}
				m_target = new Target(value);
			}
			else
			{
				m_target = null;
			}
		}
	}

	public Collar Collar
	{
		get
		{
			return m_collar;
		}
		set
		{
			if (value != null)
			{
				if (m_target != null)
				{
					NavMath navMath = new NavMath();
					try
					{
						if (navMath.HorizontalDistance(m_target, value) > 24000m)
						{
							throw new ArgumentException("The horizontal distance between the collar and target cannot exceed 24,000 meters");
						}
						m_collar = new Collar(value);
						return;
					}
					catch (OverflowException)
					{
						throw new ArgumentException("The horizontal distance between the collar and target cannot exceed 24,000 meters");
					}
				}
				m_collar = new Collar(value);
			}
			else
			{
				m_collar = null;
			}
		}
	}

	public ReportList ReportList
	{
		get
		{
			return m_reportList;
		}
		set
		{
			if (m_reportList != null)
			{
				m_reportList = new ReportList(value);
			}
		}
	}

	public SurveyList SurveyList
	{
		get
		{
			return m_surveyList;
		}
		set
		{
			if (value != null)
			{
				m_surveyList = new SurveyList(value);
			}
			else
			{
				m_surveyList = new SurveyList();
			}
		}
	}

	public decimal SideTrackMD
	{
		get
		{
			return m_sideTrackMD;
		}
		set
		{
			m_sideTrackMD = value;
		}
	}

	public DrillHoleTypeEnum DrillHoleType
	{
		get
		{
			if (SideTrackMD == -1m)
			{
				return DrillHoleTypeEnum.motherHole;
			}
			if (SurveyList.Count() > 0)
			{
				if (SurveyList.RecordAt(SurveyList.Count() - 1).MD < SideTrackMD)
				{
					return DrillHoleTypeEnum.daughterHoleNoSurveyAtSidetrack;
				}
				return DrillHoleTypeEnum.daughterHole;
			}
			return DrillHoleTypeEnum.daughterHoleNoSurveyAtSidetrack;
		}
	}

	public DrillHoleFile()
	{
		m_createdDate = DateTime.Now;
		m_fileStatus = FileStreamStatus.Disconected;
		m_surveyList = new SurveyList();
		m_sideTrackMD = -1m;
	}

	public DrillHoleFile(DrillHoleFile original, bool copyFileStream)
		: this()
	{
		Copy(original, copyFileStream);
	}

	public DrillHoleFile(string filePath)
		: this()
	{
		Open(filePath);
	}

	public OpenFile_ErrorsEnum Open(string filePath)
	{
		FileStream stream;
		switch (Utility_FileOperations.CreateFileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, out stream))
		{
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.NoError:
		{
			HoleFileFormat.HoleFileDto dto;
			try
			{
				dto = HoleFileFormat.Read(stream);
			}
			catch (LegacyHoleFileException)
			{
				stream.Close();
				return OpenFile_ErrorsEnum.DeSerialisationError;
			}
			catch (InvalidDataException)
			{
				stream.Close();
				return OpenFile_ErrorsEnum.DeSerialisationError;
			}
			catch (ArgumentException)
			{
				stream.Close();
				return OpenFile_ErrorsEnum.DeSerialisationError;
			}
			if (Status == FileStreamStatus.Connected)
			{
				Close();
			}
			HoleFileFormat.ApplyTo(dto, this);
			m_fileStream = stream;
			m_fileStatus = FileStreamStatus.Connected;
			return OpenFile_ErrorsEnum.NoError;
		}
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.AccessDenied:
			return OpenFile_ErrorsEnum.AccessNotPermited;
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.DirectoryNotFound:
			return OpenFile_ErrorsEnum.DirectoryNotFound;
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.FileNotFound:
			return OpenFile_ErrorsEnum.FileNotFound;
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.IOException:
			return OpenFile_ErrorsEnum.IOException;
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.PathIsNull:
			return OpenFile_ErrorsEnum.PathIsNull;
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.PathTooLong:
			return OpenFile_ErrorsEnum.PathIsTooLong;
		default:
			return OpenFile_ErrorsEnum.UnknownFileError;
		}
	}

	public static string ExplainError(OpenFile_ErrorsEnum error)
	{
		return error switch
		{
			OpenFile_ErrorsEnum.Null => "Open file operation not attempted", 
			OpenFile_ErrorsEnum.FileAlreadyOpen => "A file is already open and must be closed first", 
			OpenFile_ErrorsEnum.DeSerialisationError => "The file is not of the correct format or is corrupt", 
			OpenFile_ErrorsEnum.NoError => "File opened successfully", 
			OpenFile_ErrorsEnum.DirectoryNotFound => "The directory could not be found", 
			OpenFile_ErrorsEnum.FileNotFound => "The file could not be found", 
			OpenFile_ErrorsEnum.PathIsNull => "No file path was provided", 
			OpenFile_ErrorsEnum.IOException => "The file is already open or a file system error has occured", 
			OpenFile_ErrorsEnum.AccessNotPermited => "You do not have the required access permissions to open the file", 
			OpenFile_ErrorsEnum.PathIsTooLong => "The path is too long", 
			OpenFile_ErrorsEnum.NotSupported => "The file dose not support the required operations", 
			OpenFile_ErrorsEnum.CodeBug => "Unexpected system error", 
			OpenFile_ErrorsEnum.UnknownFileError => "Unexpected system error", 
			_ => "Unexpected system error", 
		};
	}

	public SaveAs_ErrorsEnum SaveAs(string filePath)
	{
		Utility_FileOperations.CreateFileStream_ErrorsEnum createFileStream_ErrorsEnum = Utility_FileOperations.CreateFileStream(filePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, out var stream);
		if (createFileStream_ErrorsEnum == Utility_FileOperations.CreateFileStream_ErrorsEnum.NoError)
		{
			try
			{
				HoleFileFormat.Write(stream, this);
				if (Status == FileStreamStatus.Connected)
				{
					Close();
				}
				m_fileStatus = FileStreamStatus.Connected;
				m_fileStream = stream;
				return SaveAs_ErrorsEnum.NoError;
			}
			catch (InvalidDataException)
			{
				stream.Close();
				return SaveAs_ErrorsEnum.SerialisationError;
			}
			catch (IOException)
			{
				stream.Close();
				return SaveAs_ErrorsEnum.SerialisationError;
			}
		}
		stream?.Close();
		return createFileStream_ErrorsEnum switch
		{
			Utility_FileOperations.CreateFileStream_ErrorsEnum.ModeInvalid => SaveAs_ErrorsEnum.NotSupported, 
			Utility_FileOperations.CreateFileStream_ErrorsEnum.DirectoryNotFound => SaveAs_ErrorsEnum.DirectoryNotFound, 
			Utility_FileOperations.CreateFileStream_ErrorsEnum.FileNotFound => SaveAs_ErrorsEnum.FileNotFound, 
			Utility_FileOperations.CreateFileStream_ErrorsEnum.PathIsNull => SaveAs_ErrorsEnum.PathIsNull, 
			Utility_FileOperations.CreateFileStream_ErrorsEnum.PathIsInvalid => SaveAs_ErrorsEnum.PathIsInvalid, 
			Utility_FileOperations.CreateFileStream_ErrorsEnum.IOException => SaveAs_ErrorsEnum.IOException, 
			Utility_FileOperations.CreateFileStream_ErrorsEnum.PathTooLong => SaveAs_ErrorsEnum.PathIsTooLong, 
			Utility_FileOperations.CreateFileStream_ErrorsEnum.AccessDenied => SaveAs_ErrorsEnum.AccessNotPermited, 
			Utility_FileOperations.CreateFileStream_ErrorsEnum.NoPermission => SaveAs_ErrorsEnum.AccessNotPermited, 
			_ => SaveAs_ErrorsEnum.UnknownFileError, 
		};
	}

	public static string ExplainError(SaveAs_ErrorsEnum error)
	{
		return error switch
		{
			SaveAs_ErrorsEnum.SerialisationError => "The data could not be writen to the new file", 
			SaveAs_ErrorsEnum.NoError => "Operation completed successfully", 
			SaveAs_ErrorsEnum.DirectoryNotFound => "A directory within the path could not be found", 
			SaveAs_ErrorsEnum.FileNotFound => "The specified file could not be found", 
			SaveAs_ErrorsEnum.PathIsNull => "No path has been provided", 
			SaveAs_ErrorsEnum.PathIsInvalid => "The file name is invalid. It may contain reserved system characters such as: \n < > : \" / \\ | ? * ", 
			SaveAs_ErrorsEnum.IOException => "The file already exists or a file system error has occurred", 
			SaveAs_ErrorsEnum.AccessNotPermited => "You do not have the required permissions to access the file", 
			SaveAs_ErrorsEnum.PathIsTooLong => "The specified path is too long", 
			SaveAs_ErrorsEnum.NotSupported => "The file does not support the requested mode of operation", 
			SaveAs_ErrorsEnum.UnknownFileError => "An unexpected file error occured", 
			_ => "Unknown error code", 
		};
	}

	public Save_ErrorsEnum Save()
	{
		if (m_fileStatus == FileStreamStatus.Connected)
		{
			m_fileStream.Position = 0L;
			m_fileStream.SetLength(0L);
			try
			{
				HoleFileFormat.Write(m_fileStream, this);
			}
			catch (InvalidDataException)
			{
				return Save_ErrorsEnum.SerialisationError;
			}
			catch (IOException)
			{
				return Save_ErrorsEnum.IOError;
			}
			return Save_ErrorsEnum.NoError;
		}
		return Save_ErrorsEnum.NoFileOpen;
	}

	public static string ExplainError(Save_ErrorsEnum error)
	{
		return error switch
		{
			Save_ErrorsEnum.NoError => "Operation completed succesfully", 
			Save_ErrorsEnum.SerialisationError => "The currently open file could not be updated", 
			Save_ErrorsEnum.NoFileOpen => "There is no file open to save the data to", 
			Save_ErrorsEnum.IOError => "The EzyNav file could not be accessed", 
			_ => "Unknown error code", 
		};
	}

	public void Close()
	{
		if (m_fileStream != null)
		{
			try
			{
				m_fileStream.Close();
				m_fileStatus = FileStreamStatus.Disconected;
			}
			catch (IOException)
			{
				m_fileStatus = FileStreamStatus.Disconected;
				m_fileStream = null;
			}
		}
	}

	public bool TimeOut()
	{
		if (m_fileStream != null)
		{
			return m_fileStream.CanTimeout;
		}
		return false;
	}

	public void Copy(DrillHoleFile original, bool copyStream)
	{
		if (original != null)
		{
			m_createdDate = original.CreatedDate;
			HoleID = original.HoleID;
			RigID = original.RigID;
			Location = original.Location;
			Area = original.Area;
			DrillingContractor = original.DrillingContractor;
			SurveyingContractor = original.SurveyingContractor;
			Client = original.Client;
			SideTrackMD = original.SideTrackMD;
			SurveyList = original.SurveyList;
			ReportList = original.ReportList;
			m_target = original.Target;
			m_collar = original.Collar;
			if (copyStream)
			{
				m_fileStream = original.m_fileStream;
				m_fileStatus = original.m_fileStatus;
			}
		}
	}

	public override bool Equals(object obj)
	{
		try
		{
			DrillHoleFile drillHoleFile = (DrillHoleFile)obj;
			if (HoleID != drillHoleFile.HoleID)
			{
				return false;
			}
			if (RigID != drillHoleFile.RigID)
			{
				return false;
			}
			if (Location != drillHoleFile.Location)
			{
				return false;
			}
			if (Area != drillHoleFile.Area)
			{
				return false;
			}
			if (DrillingContractor != drillHoleFile.DrillingContractor)
			{
				return false;
			}
			if (SurveyingContractor != drillHoleFile.SurveyingContractor)
			{
				return false;
			}
			if (Client != drillHoleFile.Client)
			{
				return false;
			}
			if (CreatedDate != drillHoleFile.CreatedDate)
			{
				return false;
			}
			if (Collar != drillHoleFile.Collar)
			{
				return false;
			}
			if (Target != drillHoleFile.Target)
			{
				return false;
			}
			return true;
		}
		catch (InvalidCastException)
		{
			return false;
		}
	}
}
