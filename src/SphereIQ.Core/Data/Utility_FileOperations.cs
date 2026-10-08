using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace EzyPlanData;

public static class Utility_FileOperations
{
	public enum CreateFileStream_ErrorsEnum
	{
		ModeInvalid,
		DirectoryNotFound,
		FileNotFound,
		PathIsNull,
		PathIsInvalid,
		IOException,
		PathTooLong,
		AccessDenied,
		NoPermission,
		NoError
	}

	public enum CreateStreamWriter_ErrorsEnum
	{
		DirectoryNotFound,
		PathIsNull,
		PathIsEmptyString,
		InvalidPath,
		PathTooLong,
		AccessDenied,
		NoPermission,
		NoError
	}

	public enum CreateStreamReader_ErrorsEnum
	{
		DirectoryNotFound,
		PathIsNull,
		IOException,
		FileNotFound,
		NoError
	}

	public enum ListFilesInDirectory_ErrorsEnum
	{
		DirectoryNotFound,
		PathOrFilterIsNull,
		PathOrFilterIsInvalidString,
		PathTooLong,
		AccessDenied,
		NoError
	}

	public enum DeleteFile_ErrorsEnum
	{
		FileNotFound,
		DirectoryNotFound,
		PathTooLong,
		FileInUse,
		PathIsNull,
		PathIsInvalid,
		AccessDenied,
		NoError
	}

	public enum RenameFile_ErrorsEnum
	{
		FileNotFound,
		DirectoryNotFound,
		PathTooLong,
		FileAlreadyExists,
		PathIsNull,
		AccessDenied,
		PathIsInvalid,
		NoError
	}

	public enum CreateFile_ErrorsEnum
	{
		DirectoryNotFound,
		PathIsNull,
		PathIsInvalid,
		IOError,
		PathTooLong,
		AccessDenied,
		NoPermission,
		NoError
	}

	public enum OpenCSV_ErrorsEnum
	{
		NoError,
		NullReader,
		CannotReadFromReader
	}

	public static CreateFileStream_ErrorsEnum CreateFileStream(string path, FileMode mode, FileAccess access, FileShare share, out FileStream stream)
	{
		try
		{
			stream = new FileStream(path, mode, access, share);
			return CreateFileStream_ErrorsEnum.NoError;
		}
		catch (FileNotFoundException)
		{
			stream = null;
			return CreateFileStream_ErrorsEnum.FileNotFound;
		}
		catch (NotSupportedException)
		{
			stream = null;
			return CreateFileStream_ErrorsEnum.PathIsInvalid;
		}
		catch (ArgumentOutOfRangeException)
		{
			stream = null;
			return CreateFileStream_ErrorsEnum.ModeInvalid;
		}
		catch (DirectoryNotFoundException)
		{
			stream = null;
			return CreateFileStream_ErrorsEnum.DirectoryNotFound;
		}
		catch (ArgumentNullException)
		{
			stream = null;
			return CreateFileStream_ErrorsEnum.PathIsNull;
		}
		catch (ArgumentException)
		{
			stream = null;
			return CreateFileStream_ErrorsEnum.PathIsInvalid;
		}
		catch (SecurityException)
		{
			stream = null;
			return CreateFileStream_ErrorsEnum.NoPermission;
		}
		catch (UnauthorizedAccessException)
		{
			stream = null;
			return CreateFileStream_ErrorsEnum.AccessDenied;
		}
		catch (PathTooLongException)
		{
			stream = null;
			return CreateFileStream_ErrorsEnum.PathTooLong;
		}
		catch (IOException)
		{
			stream = null;
			return CreateFileStream_ErrorsEnum.IOException;
		}
	}

	public static bool IsFileStreamOpen(FileStream stream)
	{
		if (stream.CanRead | stream.CanWrite)
		{
			return true;
		}
		return false;
	}

	public static CreateStreamWriter_ErrorsEnum CreateStreamWriter(string FilePath, ref StreamWriter stream)
	{
		try
		{
			stream = new StreamWriter(FilePath);
			return CreateStreamWriter_ErrorsEnum.NoError;
		}
		catch (DirectoryNotFoundException)
		{
			return CreateStreamWriter_ErrorsEnum.DirectoryNotFound;
		}
		catch (ArgumentNullException)
		{
			return CreateStreamWriter_ErrorsEnum.PathIsNull;
		}
		catch (ArgumentException)
		{
			return CreateStreamWriter_ErrorsEnum.PathIsEmptyString;
		}
		catch (SecurityException)
		{
			return CreateStreamWriter_ErrorsEnum.NoPermission;
		}
		catch (UnauthorizedAccessException)
		{
			return CreateStreamWriter_ErrorsEnum.AccessDenied;
		}
		catch (PathTooLongException)
		{
			return CreateStreamWriter_ErrorsEnum.PathTooLong;
		}
		catch (IOException)
		{
			return CreateStreamWriter_ErrorsEnum.InvalidPath;
		}
	}

	public static CreateStreamReader_ErrorsEnum CreateStreamReader(string FilePath, ref StreamReader stream)
	{
		try
		{
			stream = new StreamReader(FilePath);
			return CreateStreamReader_ErrorsEnum.NoError;
		}
		catch (DirectoryNotFoundException)
		{
			return CreateStreamReader_ErrorsEnum.DirectoryNotFound;
		}
		catch (FileNotFoundException)
		{
			return CreateStreamReader_ErrorsEnum.FileNotFound;
		}
		catch (ArgumentNullException)
		{
			return CreateStreamReader_ErrorsEnum.PathIsNull;
		}
		catch (ArgumentException)
		{
			return CreateStreamReader_ErrorsEnum.IOException;
		}
		catch (IOException)
		{
			return CreateStreamReader_ErrorsEnum.IOException;
		}
	}

	public static string ExplainError(CreateStreamReader_ErrorsEnum value)
	{
		return value switch
		{
			CreateStreamReader_ErrorsEnum.DirectoryNotFound => "A directory in the path could not be found", 
			CreateStreamReader_ErrorsEnum.PathIsNull => "The file path is empty", 
			CreateStreamReader_ErrorsEnum.IOException => "The file is already open or a file system error has occured", 
			CreateStreamReader_ErrorsEnum.FileNotFound => "The file could not be found", 
			CreateStreamReader_ErrorsEnum.NoError => "No error", 
			_ => "Unexpected error", 
		};
	}

	public static DeleteFile_ErrorsEnum DeleteFile(string FileName)
	{
		if (FileExists(FileName))
		{
			try
			{
				File.Delete(FileName);
				return DeleteFile_ErrorsEnum.NoError;
			}
			catch (DirectoryNotFoundException)
			{
				return DeleteFile_ErrorsEnum.DirectoryNotFound;
			}
			catch (PathTooLongException)
			{
				return DeleteFile_ErrorsEnum.PathTooLong;
			}
			catch (IOException)
			{
				return DeleteFile_ErrorsEnum.FileInUse;
			}
			catch (ArgumentNullException)
			{
				return DeleteFile_ErrorsEnum.PathIsNull;
			}
			catch (NotSupportedException)
			{
				return DeleteFile_ErrorsEnum.PathIsInvalid;
			}
			catch (UnauthorizedAccessException)
			{
				return DeleteFile_ErrorsEnum.AccessDenied;
			}
			catch (ArgumentException)
			{
				return DeleteFile_ErrorsEnum.PathIsInvalid;
			}
		}
		return DeleteFile_ErrorsEnum.FileNotFound;
	}

	public static RenameFile_ErrorsEnum RenameFile(string oldFileName, string newFileName)
	{
		try
		{
			File.Move(oldFileName, newFileName);
			return RenameFile_ErrorsEnum.NoError;
		}
		catch (FileNotFoundException)
		{
			return RenameFile_ErrorsEnum.FileNotFound;
		}
		catch (DirectoryNotFoundException)
		{
			return RenameFile_ErrorsEnum.DirectoryNotFound;
		}
		catch (PathTooLongException)
		{
			return RenameFile_ErrorsEnum.PathTooLong;
		}
		catch (IOException)
		{
			return RenameFile_ErrorsEnum.FileAlreadyExists;
		}
		catch (ArgumentNullException)
		{
			return RenameFile_ErrorsEnum.PathIsNull;
		}
		catch (NotSupportedException)
		{
			return RenameFile_ErrorsEnum.PathIsInvalid;
		}
		catch (UnauthorizedAccessException)
		{
			return RenameFile_ErrorsEnum.AccessDenied;
		}
		catch (ArgumentException)
		{
			return RenameFile_ErrorsEnum.PathIsInvalid;
		}
	}

	public static CreateFile_ErrorsEnum CreateFile(string newFileName)
	{
		try
		{
			FileStream fileStream = File.Create(newFileName);
			fileStream.Close();
			return CreateFile_ErrorsEnum.NoError;
		}
		catch (DirectoryNotFoundException)
		{
			return CreateFile_ErrorsEnum.DirectoryNotFound;
		}
		catch (PathTooLongException)
		{
			return CreateFile_ErrorsEnum.PathTooLong;
		}
		catch (ArgumentNullException)
		{
			return CreateFile_ErrorsEnum.PathIsNull;
		}
		catch (NotSupportedException)
		{
			return CreateFile_ErrorsEnum.PathIsInvalid;
		}
		catch (UnauthorizedAccessException)
		{
			return CreateFile_ErrorsEnum.AccessDenied;
		}
		catch (ArgumentException)
		{
			return CreateFile_ErrorsEnum.PathIsInvalid;
		}
		catch (IOException)
		{
			return CreateFile_ErrorsEnum.IOError;
		}
	}

	public static bool FileExists(string filePath)
	{
		return File.Exists(filePath);
	}

	public static string ExplainError(OpenCSV_ErrorsEnum error)
	{
		return error switch
		{
			OpenCSV_ErrorsEnum.NoError => "Operation completed succesfully", 
			OpenCSV_ErrorsEnum.NullReader => "Program has encountered an unexpected error", 
			OpenCSV_ErrorsEnum.CannotReadFromReader => "File is incorectly formated or corrupt", 
			_ => "Unknown error code", 
		};
	}

	public static OpenCSV_ErrorsEnum OpenCSVFile(StreamReader stream, out ICsvReader csv)
	{
		csv = null;
		try
		{
			if (stream == null)
			{
				return OpenCSV_ErrorsEnum.NullReader;
			}
			csv = new SimpleCsvReader(stream, hasHeaders: false);
			return OpenCSV_ErrorsEnum.NoError;
		}
		catch (ArgumentNullException)
		{
			csv = null;
			return OpenCSV_ErrorsEnum.NullReader;
		}
		catch (ArgumentException)
		{
			csv = null;
			return OpenCSV_ErrorsEnum.CannotReadFromReader;
		}
		catch (IOException)
		{
			csv = null;
			return OpenCSV_ErrorsEnum.CannotReadFromReader;
		}
	}
}
