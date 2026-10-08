using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;

namespace EzyPlanData;

[Serializable]
public class SurveyList : IEnumerable<Survey>, IEnumerable
{
	[Serializable]
	public class Data
	{
		public SortableBindingList<Survey> items;

		public Data()
		{
			string[] names = new string[3] { "MD", "Dip", "Azimuth" };
			items = new SortableBindingList<Survey>(names);
		}
	}

	public enum EditMDAtEnum
	{
		EditSuccessful,
		IndexInvalid,
		InvalidMD,
		MdOutOfLowerBound,
		MdOutOfUpperBound,
		FirstSurveyMustHaveZeroMD
	}

	public enum EditDipAtEnum
	{
		EditSuccesful,
		InvalidIndex,
		InvalidDip
	}

	public enum UpdateSurveyAtEnum
	{
		UpdateSuccessful,
		MdOutOfUpperBound,
		MdOutOfLowerBound,
		FirstSurveyMustHaveZeroMD,
		IndexInvalid,
		SurveyNull
	}

	public enum EditAzimuthAtEnum
	{
		EditSuccessful,
		InvalidIndex,
		InvalidAzimuth
	}

	public enum AppendErrorsEnum
	{
		MdNotGreaterThanPrevious,
		FirstSurveyMustHaveZeroMD,
		NoError
	}

	public enum InsertErrorsEnum
	{
		MdOutOfLowerBound,
		MdOutOfUpperBound,
		InvalidIndex,
		EmptyList,
		NoInsertAboveFirst,
		NoError
	}

	public enum RemoveEnum
	{
		NoRemoveFirst,
		ListEmpty,
		RemoveSuccessfull,
		NotAllMDFound
	}

	public enum ImportCSVFilename_ErrorsEnum
	{
		InvalidAzimuthField,
		InvalidDipField,
		InvalidMDField,
		MdNotGreaterThanPrevious,
		FirstSurveyMustHaveZeroMD,
		InvalidIndexValue,
		MissingField,
		CSVReaderIsNull,
		DirectoryNotFound,
		PathIsNull,
		IOError,
		FileNotFound,
		NullReader,
		CannotReadFromReader,
		UnknownError,
		NoError
	}

	public enum ImportCSV_ErrorsEnum
	{
		InvalidAzimuthField,
		InvalidDipField,
		InvalidMDField,
		MdNotGreaterThanPrevious,
		FirstSurveyMustHaveZeroMD,
		InvalidIndexValue,
		MissingField,
		MissingColumn,
		CSVReaderIsNull,
		NoError,
		UnknownAppendError
	}

	public enum SaveToXML_ErrorsEnum
	{
		NoError,
		CouldNotSerialiseData,
		AccessDenied,
		DirectoryNotFound,
		InvalidPath,
		NoPermission,
		PathIsEmptyString,
		PathIsNull,
		PathTooLong,
		UnknownFileError
	}

	public enum LoadFromXML_ErrorsEnum
	{
		NoError,
		StreamIsNull,
		IncompatibleFile,
		ModeInvalid,
		DirectoryNotFound,
		FileNotFound,
		PathIsNull,
		PathIsInvalid,
		IOException,
		PathTooLong,
		AccessDenied,
		NoPermission,
		UnknownFileError
	}

	[CompilerGenerated]
	private sealed class _003CGetEnumerator_003Ed__0 : IEnumerator<Survey>, IEnumerator, IDisposable
	{
		private Survey _003C_003E2__current;

		private int _003C_003E1__state;

		public SurveyList _003C_003E4__this;

		public Survey _003Celement_003E5__1;

		public IEnumerator<Survey> _003C_003E7__wrap2;

		Survey IEnumerator<Survey>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap2 = _003C_003E4__this.m_list.items.GetEnumerator();
					_003C_003E1__state = 1;
					goto IL_0078;
				case 2:
					{
						_003C_003E1__state = 1;
						goto IL_0078;
					}
					IL_0078:
					if (_003C_003E7__wrap2.MoveNext())
					{
						_003Celement_003E5__1 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = _003Celement_003E5__1;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E1__state = -1;
					if (_003C_003E7__wrap2 != null)
					{
						_003C_003E7__wrap2.Dispose();
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case 1:
			case 2:
				try
				{
					break;
				}
				finally
				{
					_003C_003E1__state = -1;
					if (_003C_003E7__wrap2 != null)
					{
						_003C_003E7__wrap2.Dispose();
					}
				}
			}
		}

		[DebuggerHidden]
		public _003CGetEnumerator_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}
	}

	private Data m_list;

	public SortableBindingList<Survey> List => m_list.items;

	public SurveyList()
	{
		m_list = new Data();
	}

	public SurveyList(SurveyList original)
		: this()
	{
		Copy(original);
	}

	public SurveyList(string filePath)
		: this()
	{
		LoadFromXML(filePath);
	}

	public Survey RecordAt(int index)
	{
		if ((index >= 0) & (index <= m_list.items.Count - 1))
		{
			return m_list.items[index];
		}
		return null;
	}

	public Survey RecordAtMD(decimal md)
	{
		foreach (Survey item in m_list.items)
		{
			if (item.MD == md)
			{
				return item;
			}
		}
		return null;
	}

	public EditMDAtEnum EditMDAt(int index, decimal newMD)
	{
		Survey survey = RecordAt(index);
		if (survey != null)
		{
			if (newMD >= 0m)
			{
				if (index > 0)
				{
					bool flag = false;
					bool flag2 = false;
					flag = index >= m_list.items.Count - 1 || ((newMD < m_list.items[index + 1].MD) ? true : false);
					if (!(newMD > m_list.items[index - 1].MD) || 1 == 0)
					{
						return EditMDAtEnum.MdOutOfLowerBound;
					}
					if (!flag)
					{
						return EditMDAtEnum.MdOutOfUpperBound;
					}
					survey.SetMD(newMD);
					return EditMDAtEnum.EditSuccessful;
				}
				return EditMDAtEnum.FirstSurveyMustHaveZeroMD;
			}
			return EditMDAtEnum.InvalidMD;
		}
		return EditMDAtEnum.IndexInvalid;
	}

	public static string ToHumanReadable(EditMDAtEnum value)
	{
		return value switch
		{
			EditMDAtEnum.EditSuccessful => "Operation completed successfully", 
			EditMDAtEnum.IndexInvalid => "The specified index is invalid", 
			EditMDAtEnum.InvalidMD => "MD value is invalid - MD must be a positive value", 
			EditMDAtEnum.MdOutOfLowerBound => "The MD value is invalid - MD must be greater than the MD of the previous survey", 
			EditMDAtEnum.MdOutOfUpperBound => "The MD value is invalid - MD must be less than the MD of the next survey", 
			EditMDAtEnum.FirstSurveyMustHaveZeroMD => "The first survey in the list must have a MD of 0", 
			_ => "Unexpected Error", 
		};
	}

	public EditDipAtEnum EditDipAt(int index, decimal newDip)
	{
		Survey survey = RecordAt(index);
		if (survey != null)
		{
			if (survey.SetDip(newDip) == Survey.SetDipEnum.ValueSet)
			{
				return EditDipAtEnum.EditSuccesful;
			}
			return EditDipAtEnum.InvalidDip;
		}
		return EditDipAtEnum.InvalidIndex;
	}

	public UpdateSurveyAtEnum UpdateSurveyAt(int index, Survey updated)
	{
		if (updated != null)
		{
			Survey survey = RecordAt(index);
			if (survey != null)
			{
				if (index > 0)
				{
					bool flag = false;
					bool flag2 = false;
					flag = index >= m_list.items.Count - 1 || ((updated.MD < m_list.items[index + 1].MD) ? true : false);
					if (!(updated.MD > m_list.items[index - 1].MD) || 1 == 0)
					{
						return UpdateSurveyAtEnum.MdOutOfLowerBound;
					}
					if (!flag)
					{
						return UpdateSurveyAtEnum.MdOutOfUpperBound;
					}
					survey.Copy(updated);
					return UpdateSurveyAtEnum.UpdateSuccessful;
				}
				if (updated.MD != 0m)
				{
					return UpdateSurveyAtEnum.FirstSurveyMustHaveZeroMD;
				}
				survey.Copy(updated);
				return UpdateSurveyAtEnum.UpdateSuccessful;
			}
			return UpdateSurveyAtEnum.IndexInvalid;
		}
		return UpdateSurveyAtEnum.SurveyNull;
	}

	public static string ToHumanReadable(UpdateSurveyAtEnum value)
	{
		return value switch
		{
			UpdateSurveyAtEnum.UpdateSuccessful => "Opertation completed successfully", 
			UpdateSurveyAtEnum.MdOutOfUpperBound => "The MD value is invalid - MD must be less than the MD of the next survey", 
			UpdateSurveyAtEnum.MdOutOfLowerBound => "The MD value is invalid - MD must be greater than the MD of the previous survey", 
			UpdateSurveyAtEnum.FirstSurveyMustHaveZeroMD => "The first survey in the list must have a MD of 0", 
			UpdateSurveyAtEnum.IndexInvalid => "The specified index is invalid", 
			UpdateSurveyAtEnum.SurveyNull => "The specified survey is null", 
			_ => "Unexpected Error", 
		};
	}

	public static string ToHumanReadable(EditDipAtEnum value)
	{
		return value switch
		{
			EditDipAtEnum.EditSuccesful => "Opertation completed successfully", 
			EditDipAtEnum.InvalidIndex => "Specified index is invalid", 
			EditDipAtEnum.InvalidDip => "Dip value is invalid - Dip must be in the range 0 to 90 inclusive or 0 to -90 inclusive", 
			_ => "Unexpected error", 
		};
	}

	public EditAzimuthAtEnum EditAzimuthAt(int index, decimal newAzi)
	{
		Survey survey = RecordAt(index);
		if (survey != null)
		{
			if (survey.SetAzimuth(newAzi) == Survey.SetAzimuthEnum.ValueSet)
			{
				return EditAzimuthAtEnum.EditSuccessful;
			}
			return EditAzimuthAtEnum.InvalidAzimuth;
		}
		return EditAzimuthAtEnum.InvalidIndex;
	}

	public static string ToHumanReadable(EditAzimuthAtEnum value)
	{
		return value switch
		{
			EditAzimuthAtEnum.EditSuccessful => "Operation completed successfully", 
			EditAzimuthAtEnum.InvalidIndex => "Specified index is invalid", 
			EditAzimuthAtEnum.InvalidAzimuth => "Azimuth value is invalid - Azimuth mus be in the range 0 to 360", 
			_ => "Unexpected error", 
		};
	}

	public AppendErrorsEnum Append(Survey element)
	{
		Survey survey = new Survey(element);
		int count = m_list.items.Count;
		if (count > 0)
		{
			int index = count - 1;
			if (element.MD > m_list.items[index].MD)
			{
				m_list.items.Add(survey);
				return AppendErrorsEnum.NoError;
			}
			return AppendErrorsEnum.MdNotGreaterThanPrevious;
		}
		if (survey.MD == 0m)
		{
			m_list.items.Add(survey);
			return AppendErrorsEnum.NoError;
		}
		return AppendErrorsEnum.FirstSurveyMustHaveZeroMD;
	}

	public static string ToHumanReadable(AppendErrorsEnum value)
	{
		return value switch
		{
			AppendErrorsEnum.MdNotGreaterThanPrevious => "The MD field value must be greater than the previous survey's MD", 
			AppendErrorsEnum.FirstSurveyMustHaveZeroMD => "The first survey must have a MD value of 0", 
			AppendErrorsEnum.NoError => "Operation completed successfully", 
			_ => "Unexpected error", 
		};
	}

	public InsertErrorsEnum Insert(int index, Survey element)
	{
		Survey survey = new Survey(element);
		if (m_list.items.Count > 0)
		{
			if ((index <= m_list.items.Count - 1) & (index >= 0))
			{
				if (index > 0)
				{
					bool flag = false;
					bool flag2 = false;
					flag = index >= m_list.items.Count - 1 || ((survey.MD < m_list.items[index].MD) ? true : false);
					if (!(survey.MD > m_list.items[index - 1].MD) || 1 == 0)
					{
						return InsertErrorsEnum.MdOutOfLowerBound;
					}
					if (!flag)
					{
						return InsertErrorsEnum.MdOutOfUpperBound;
					}
					m_list.items.Insert(index, survey);
					return InsertErrorsEnum.NoError;
				}
				return InsertErrorsEnum.NoInsertAboveFirst;
			}
			return InsertErrorsEnum.InvalidIndex;
		}
		return InsertErrorsEnum.EmptyList;
	}

	public static string ToHumanReadable(InsertErrorsEnum value)
	{
		return value switch
		{
			InsertErrorsEnum.MdOutOfLowerBound => "The MD is less than or equal to the previous survey's MD", 
			InsertErrorsEnum.MdOutOfUpperBound => "The MD is greater than or equal to the following survey", 
			InsertErrorsEnum.InvalidIndex => "The specified index is invalid", 
			InsertErrorsEnum.EmptyList => "It is not possible to insert into an empty list", 
			InsertErrorsEnum.NoInsertAboveFirst => "A survey cannot be inserted to the position of first survey", 
			InsertErrorsEnum.NoError => "Operation completed successfully", 
			_ => "Unexpected error", 
		};
	}

	public RemoveEnum Remove(List<decimal> mdToDelete)
	{
		bool flag = true;
		if (m_list.items.Count == 0)
		{
			return RemoveEnum.ListEmpty;
		}
		foreach (decimal item in mdToDelete)
		{
			if (item == 0m && Count() != 1 && mdToDelete.Count != Count())
			{
				return RemoveEnum.NoRemoveFirst;
			}
		}
		foreach (decimal item2 in mdToDelete)
		{
			Survey survey = RecordAtMD(item2);
			if (survey != null)
			{
				m_list.items.Remove(survey);
			}
			else
			{
				flag = false;
			}
		}
		if (flag)
		{
			return RemoveEnum.RemoveSuccessfull;
		}
		return RemoveEnum.NotAllMDFound;
	}

	public static string ToHumanReadable(RemoveEnum value)
	{
		return value switch
		{
			RemoveEnum.NoRemoveFirst => "The first survey can't be deleted if there are surveys remaining below it", 
			RemoveEnum.ListEmpty => "The list is already empty", 
			RemoveEnum.NotAllMDFound => "Not all the selected surveys were succesfully deleted", 
			RemoveEnum.RemoveSuccessfull => "Opperation completed successfully", 
			_ => "Unexpected error", 
		};
	}

	public bool Clear()
	{
		m_list.items.Clear();
		return m_list.items.Count == 0;
	}

	public int Count()
	{
		return m_list.items.Count;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public IEnumerator<Survey> GetEnumerator()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		_003CGetEnumerator_003Ed__0 obj = new _003CGetEnumerator_003Ed__0(0);
		obj._003C_003E4__this = this;
		return obj;
	}

	public ImportCSV_ErrorsEnum ImportCSV(ICsvReader csv, ImportSurveyMapping mapping)
	{
		return ImportCSV(csv, mapping.MD, mapping.Dip, mapping.Azimuth, mapping.Header);
	}

	public ImportCSVFilename_ErrorsEnum ImportCSV(string filePath, ImportSurveyMapping mapping)
	{
		StreamReader stream = null;
		ICsvReader csv = null;
		return Utility_FileOperations.CreateStreamReader(filePath, ref stream) switch
		{
			Utility_FileOperations.CreateStreamReader_ErrorsEnum.NoError => Utility_FileOperations.OpenCSVFile(stream, out csv) switch
			{
				Utility_FileOperations.OpenCSV_ErrorsEnum.NoError => ImportCSV(csv, mapping.MD, mapping.Dip, mapping.Azimuth, mapping.Header) switch
				{
					ImportCSV_ErrorsEnum.NoError => ImportCSVFilename_ErrorsEnum.NoError, 
					ImportCSV_ErrorsEnum.InvalidAzimuthField => ImportCSVFilename_ErrorsEnum.InvalidAzimuthField, 
					ImportCSV_ErrorsEnum.InvalidDipField => ImportCSVFilename_ErrorsEnum.InvalidDipField, 
					ImportCSV_ErrorsEnum.InvalidMDField => ImportCSVFilename_ErrorsEnum.InvalidMDField, 
					ImportCSV_ErrorsEnum.MdNotGreaterThanPrevious => ImportCSVFilename_ErrorsEnum.MdNotGreaterThanPrevious, 
					ImportCSV_ErrorsEnum.FirstSurveyMustHaveZeroMD => ImportCSVFilename_ErrorsEnum.FirstSurveyMustHaveZeroMD, 
					ImportCSV_ErrorsEnum.InvalidIndexValue => ImportCSVFilename_ErrorsEnum.InvalidIndexValue, 
					ImportCSV_ErrorsEnum.MissingField => ImportCSVFilename_ErrorsEnum.MissingField, 
					ImportCSV_ErrorsEnum.CSVReaderIsNull => ImportCSVFilename_ErrorsEnum.CSVReaderIsNull, 
					_ => ImportCSVFilename_ErrorsEnum.UnknownError, 
				}, 
				Utility_FileOperations.OpenCSV_ErrorsEnum.NullReader => ImportCSVFilename_ErrorsEnum.NullReader, 
				Utility_FileOperations.OpenCSV_ErrorsEnum.CannotReadFromReader => ImportCSVFilename_ErrorsEnum.CannotReadFromReader, 
				_ => ImportCSVFilename_ErrorsEnum.UnknownError, 
			}, 
			Utility_FileOperations.CreateStreamReader_ErrorsEnum.DirectoryNotFound => ImportCSVFilename_ErrorsEnum.DirectoryNotFound, 
			Utility_FileOperations.CreateStreamReader_ErrorsEnum.PathIsNull => ImportCSVFilename_ErrorsEnum.PathIsNull, 
			Utility_FileOperations.CreateStreamReader_ErrorsEnum.IOException => ImportCSVFilename_ErrorsEnum.IOError, 
			Utility_FileOperations.CreateStreamReader_ErrorsEnum.FileNotFound => ImportCSVFilename_ErrorsEnum.FileNotFound, 
			_ => ImportCSVFilename_ErrorsEnum.UnknownError, 
		};
	}

	public ImportCSV_ErrorsEnum ImportCSV(ICsvReader csv, int MD_Index, int Dip_Index, int Azi_Index, bool header)
	{
		SurveyList surveyList = new SurveyList();
		try
		{
			csv.MoveToStart();
			if (header)
			{
				csv.ReadNextRecord();
			}
			Survey survey = new Survey();
			foreach (string[] item in csv)
			{
				decimal md = 0m;
				decimal number = 0m;
				decimal number2 = 0m;
				survey.Azimuth = 0m;
				survey.Dip = 0m;
				survey.MD = 0m;
				AppendErrorsEnum appendErrorsEnum = AppendErrorsEnum.NoError;
				if (Utility_Conversions.StringToMD(item[MD_Index], out md) != Utility_Conversions.StringToMDResult.ConversionSuccessful)
				{
					return ImportCSV_ErrorsEnum.InvalidMDField;
				}
				if (survey.SetMD(md) != Survey.SetMDEnum.ValueSet)
				{
					return ImportCSV_ErrorsEnum.InvalidMDField;
				}
				if (Utility_Conversions.StringToDecimal(item[Dip_Index], out number) == Utility_Conversions.StringToDecimalResult.NoError)
				{
					if (survey.SetDip(number) != Survey.SetDipEnum.ValueSet)
					{
						return ImportCSV_ErrorsEnum.InvalidDipField;
					}
					if (Utility_Conversions.StringToDecimal(item[Azi_Index], out number2) == Utility_Conversions.StringToDecimalResult.NoError)
					{
						if (survey.SetAzimuth(number2) != Survey.SetAzimuthEnum.ValueSet)
						{
							return ImportCSV_ErrorsEnum.InvalidAzimuthField;
						}
						new Survey(survey);
						switch (surveyList.Append(survey))
						{
						case AppendErrorsEnum.MdNotGreaterThanPrevious:
							return ImportCSV_ErrorsEnum.MdNotGreaterThanPrevious;
						case AppendErrorsEnum.FirstSurveyMustHaveZeroMD:
							return ImportCSV_ErrorsEnum.FirstSurveyMustHaveZeroMD;
						default:
							return ImportCSV_ErrorsEnum.UnknownAppendError;
						case AppendErrorsEnum.NoError:
							break;
						}
						continue;
					}
					return ImportCSV_ErrorsEnum.InvalidAzimuthField;
				}
				return ImportCSV_ErrorsEnum.InvalidDipField;
			}
		}
		catch (InvalidDataException)
		{
			return ImportCSV_ErrorsEnum.MissingField;
		}
		catch (NullReferenceException)
		{
			return ImportCSV_ErrorsEnum.CSVReaderIsNull;
		}
		catch (IndexOutOfRangeException)
		{
			return ImportCSV_ErrorsEnum.MissingColumn;
		}
		Copy(surveyList);
		return ImportCSV_ErrorsEnum.NoError;
	}

	public static string ExplainError(ImportCSV_ErrorsEnum error)
	{
		return error switch
		{
			ImportCSV_ErrorsEnum.InvalidAzimuthField => "One of the Aimuth field values in the CSV are invalid", 
			ImportCSV_ErrorsEnum.InvalidDipField => "One of the Dip field values in the CSV are invalid", 
			ImportCSV_ErrorsEnum.InvalidMDField => "One of the MD field values in the CSV are invalid", 
			ImportCSV_ErrorsEnum.MdNotGreaterThanPrevious => "A MD field value in the CSV is less than or equal to the previous survey's MD", 
			ImportCSV_ErrorsEnum.FirstSurveyMustHaveZeroMD => "The first survey must have a MD value of 0", 
			ImportCSV_ErrorsEnum.InvalidIndexValue => "One or more of the specified mapping index values are out of range", 
			ImportCSV_ErrorsEnum.MissingField => "A record in the CSV is missing a field", 
			ImportCSV_ErrorsEnum.MissingColumn => "A column specified by the import map is not present in the CSV", 
			ImportCSV_ErrorsEnum.CSVReaderIsNull => "Internal Error: The CSV reader is null", 
			ImportCSV_ErrorsEnum.NoError => "Operation completed successfully", 
			_ => "Unknown error occured", 
		};
	}

	public void ExportCSV(string fileName)
	{
		throw new NotImplementedException();
	}

	public SaveToXML_ErrorsEnum SaveToXML(string filePath)
	{
		StreamWriter stream = null;
		switch (Utility_FileOperations.CreateStreamWriter(filePath, ref stream))
		{
		case Utility_FileOperations.CreateStreamWriter_ErrorsEnum.NoError:
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(Data));
				xmlSerializer.Serialize(stream, m_list);
				stream.Close();
				return SaveToXML_ErrorsEnum.NoError;
			}
			catch (InvalidOperationException)
			{
				return SaveToXML_ErrorsEnum.CouldNotSerialiseData;
			}
		case Utility_FileOperations.CreateStreamWriter_ErrorsEnum.AccessDenied:
			return SaveToXML_ErrorsEnum.AccessDenied;
		case Utility_FileOperations.CreateStreamWriter_ErrorsEnum.DirectoryNotFound:
			return SaveToXML_ErrorsEnum.DirectoryNotFound;
		case Utility_FileOperations.CreateStreamWriter_ErrorsEnum.InvalidPath:
			return SaveToXML_ErrorsEnum.InvalidPath;
		case Utility_FileOperations.CreateStreamWriter_ErrorsEnum.NoPermission:
			return SaveToXML_ErrorsEnum.NoPermission;
		case Utility_FileOperations.CreateStreamWriter_ErrorsEnum.PathIsEmptyString:
			return SaveToXML_ErrorsEnum.PathIsEmptyString;
		case Utility_FileOperations.CreateStreamWriter_ErrorsEnum.PathIsNull:
			return SaveToXML_ErrorsEnum.PathIsNull;
		case Utility_FileOperations.CreateStreamWriter_ErrorsEnum.PathTooLong:
			return SaveToXML_ErrorsEnum.PathTooLong;
		default:
			return SaveToXML_ErrorsEnum.UnknownFileError;
		}
	}

	public LoadFromXML_ErrorsEnum LoadFromXML(string filePath)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Data));
		FileStream stream = null;
		switch (Utility_FileOperations.CreateFileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, out stream))
		{
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.NoError:
		{
			XmlReader xmlReader = new XmlTextReader(stream);
			if (xmlSerializer.CanDeserialize(xmlReader))
			{
				stream.Position = 0L;
				m_list = (Data)xmlSerializer.Deserialize(stream);
				stream.Close();
				return LoadFromXML_ErrorsEnum.NoError;
			}
			stream.Close();
			return LoadFromXML_ErrorsEnum.IncompatibleFile;
		}
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.ModeInvalid:
			return LoadFromXML_ErrorsEnum.ModeInvalid;
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.DirectoryNotFound:
			return LoadFromXML_ErrorsEnum.DirectoryNotFound;
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.FileNotFound:
			return LoadFromXML_ErrorsEnum.FileNotFound;
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.PathIsNull:
			return LoadFromXML_ErrorsEnum.PathIsNull;
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.PathIsInvalid:
			return LoadFromXML_ErrorsEnum.PathIsInvalid;
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.IOException:
			return LoadFromXML_ErrorsEnum.IOException;
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.PathTooLong:
			return LoadFromXML_ErrorsEnum.PathTooLong;
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.AccessDenied:
			return LoadFromXML_ErrorsEnum.AccessDenied;
		case Utility_FileOperations.CreateFileStream_ErrorsEnum.NoPermission:
			return LoadFromXML_ErrorsEnum.NoPermission;
		default:
			return LoadFromXML_ErrorsEnum.UnknownFileError;
		}
	}

	public bool Copy(SurveyList original)
	{
		if (original != null)
		{
			foreach (Survey item in original)
			{
				Append(item);
			}
			return true;
		}
		return false;
	}
}
