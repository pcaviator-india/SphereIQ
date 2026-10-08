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
public class ReportList : IEnumerable<Report>, IEnumerable
{
	[Serializable]
	public class Data
	{
		public List<Report> items;

		public Data()
		{
			items = new List<Report>();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetEnumerator_003Ed__0 : IEnumerator<Report>, IEnumerator, IDisposable
	{
		private Report _003C_003E2__current;

		private int _003C_003E1__state;

		public ReportList _003C_003E4__this;

		public Report _003Celement_003E5__1;

		public List<Report>.Enumerator _003C_003E7__wrap2;

		Report IEnumerator<Report>.Current
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
					_003C_003E7__wrap2 = _003C_003E4__this.list.items.GetEnumerator();
					_003C_003E1__state = 1;
					goto IL_0075;
				case 2:
					{
						_003C_003E1__state = 1;
						goto IL_0075;
					}
					IL_0075:
					if (_003C_003E7__wrap2.MoveNext())
					{
						_003Celement_003E5__1 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = _003Celement_003E5__1;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E1__state = -1;
					((IDisposable)_003C_003E7__wrap2/*cast due to constrained. prefix*/).Dispose();
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
					((IDisposable)_003C_003E7__wrap2/*cast due to constrained. prefix*/).Dispose();
				}
			}
		}

		[DebuggerHidden]
		public _003CGetEnumerator_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}
	}

	private Data list;

	public ReportList()
	{
		list = new Data();
	}

	public ReportList(ReportList original)
	{
		Copy(original);
	}

	public Report RecordAt(int index)
	{
		int num = 0;
		foreach (Report item in list.items)
		{
			if (num == index)
			{
				return item;
			}
			num++;
		}
		return null;
	}

	public bool Add(Report element)
	{
		Report item = new Report(element);
		list.items.Add(item);
		return list.items.Contains(item);
	}

	public bool Remove(int index)
	{
		try
		{
			list.items.RemoveAt(index);
		}
		catch (ArgumentOutOfRangeException)
		{
			return false;
		}
		return true;
	}

	public bool Clear()
	{
		list.items.Clear();
		return list.items.Count == 0;
	}

	public int Count()
	{
		return list.items.Count;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public IEnumerator<Report> GetEnumerator()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		_003CGetEnumerator_003Ed__0 obj = new _003CGetEnumerator_003Ed__0(0);
		obj._003C_003E4__this = this;
		return obj;
	}

	public bool SaveToXML(string filePath)
	{
		StreamWriter stream = null;
		Utility_FileOperations.CreateStreamWriter_ErrorsEnum createStreamWriter_ErrorsEnum = Utility_FileOperations.CreateStreamWriter(filePath, ref stream);
		if (createStreamWriter_ErrorsEnum == Utility_FileOperations.CreateStreamWriter_ErrorsEnum.NoError)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(Data));
			xmlSerializer.Serialize(stream, list);
			stream.Close();
			return true;
		}
		return false;
	}

	public bool LoadFromXML(string filePath)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Data));
		FileStream stream = null;
		Utility_FileOperations.CreateFileStream_ErrorsEnum createFileStream_ErrorsEnum = Utility_FileOperations.CreateFileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, out stream);
		if (createFileStream_ErrorsEnum == Utility_FileOperations.CreateFileStream_ErrorsEnum.NoError)
		{
			XmlReader xmlReader = new XmlTextReader(stream);
			if (xmlSerializer.CanDeserialize(xmlReader))
			{
				stream.Position = 0L;
				list = (Data)xmlSerializer.Deserialize(stream);
				stream.Close();
				return true;
			}
			stream.Close();
			return false;
		}
		return false;
	}

	public bool Copy(ReportList original)
	{
		if (original != null)
		{
			foreach (Report item in original)
			{
				if (!Add(item))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}
}
