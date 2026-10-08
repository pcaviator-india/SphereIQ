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
public class AbsoluteSurveyList : IEnumerable<AbsoluteSurvey>, IEnumerable
{
	[Serializable]
	public class Data
	{
		public SortableBindingList<AbsoluteSurvey> items;

		public Data()
		{
			string[] names = new string[7] { "MD", "Dip", "Azimuth", "TVD", "Northing", "Easting", "DL" };
			items = new SortableBindingList<AbsoluteSurvey>(names);
		}
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
	private sealed class _003CGetEnumerator_003Ed__0 : IEnumerator<AbsoluteSurvey>, IEnumerator, IDisposable
	{
		private AbsoluteSurvey _003C_003E2__current;

		private int _003C_003E1__state;

		public AbsoluteSurveyList _003C_003E4__this;

		public AbsoluteSurvey _003Celement_003E5__1;

		public IEnumerator<AbsoluteSurvey> _003C_003E7__wrap2;

		AbsoluteSurvey IEnumerator<AbsoluteSurvey>.Current
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

	public SortableBindingList<AbsoluteSurvey> List
	{
		get
		{
			return m_list.items;
		}
		set
		{
			m_list.items = value;
		}
	}

	public AbsoluteSurveyList()
	{
		m_list = new Data();
	}

	public AbsoluteSurveyList(AbsoluteSurveyList original)
		: this()
	{
		Copy(original);
	}

	public AbsoluteSurvey RecordAt(int index)
	{
		if (m_list.items.Count > 0)
		{
			int num = m_list.items.Count - 1;
			if ((index >= 0) & (index <= num))
			{
				return m_list.items[index];
			}
			throw new IndexOutOfRangeException();
		}
		return null;
	}

	public AbsoluteSurvey LastRecord()
	{
		int count = m_list.items.Count;
		if (count > 0)
		{
			return m_list.items[count - 1];
		}
		return null;
	}

	public bool Add(AbsoluteSurvey element)
	{
		AbsoluteSurvey item = new AbsoluteSurvey(element);
		m_list.items.Add(item);
		return m_list.items.Contains(item);
	}

	public bool Remove(int index)
	{
		try
		{
			m_list.items.RemoveAt(index);
		}
		catch (ArgumentOutOfRangeException)
		{
			return false;
		}
		return true;
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

	public IEnumerator<AbsoluteSurvey> GetEnumerator()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		_003CGetEnumerator_003Ed__0 obj = new _003CGetEnumerator_003Ed__0(0);
		obj._003C_003E4__this = this;
		return obj;
	}

	public void Copy(AbsoluteSurveyList original)
	{
		Clear();
		if (original == null)
		{
			return;
		}
		foreach (AbsoluteSurvey item in original)
		{
			Add(item);
		}
	}

	public void ExportCSV(string filePath)
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
}
