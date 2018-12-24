using System;
using System.Collections.Generic;
using System.Data;
namespace UPlan.Frame.Interface
{
	public class XlsTable
	{
		private string m_SheetName;
		private int m_TableNo;
		private string m_TableName;
		private string m_SubSystemName;
		private Dictionary<string, string> m_moduleXlsConvertProper;
		private DataTable m_Data;
		private string m_Version;
		public string SheetName
		{
			get
			{
				return this.m_SheetName;
			}
			set
			{
				this.m_SheetName = value;
			}
		}
		public int TableNo
		{
			get
			{
				return this.m_TableNo;
			}
			set
			{
				this.m_TableNo = value;
			}
		}
		public string TableName
		{
			get
			{
				return this.m_TableName;
			}
			set
			{
				this.m_TableName = value;
			}
		}
		public string SubSystemName
		{
			get
			{
				return this.m_SubSystemName;
			}
			set
			{
				this.m_SubSystemName = value;
			}
		}
		public Dictionary<string, string> ModuleXlsConvertProper
		{
			get
			{
				return this.m_moduleXlsConvertProper;
			}
			set
			{
				this.m_moduleXlsConvertProper = value;
			}
		}
		public DataTable Data
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}
		public string Version
		{
			get
			{
				return this.m_Version;
			}
			set
			{
				this.m_Version = value;
			}
		}
	}
}
