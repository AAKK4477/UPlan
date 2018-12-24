using log4net;
using System;
using System.Reflection;
namespace UPlan.Frame.Interface
{
	public static class WriteLog
	{
		private static readonly ILog m_Log4net = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public static ILog Logger
		{
			get
			{
				return WriteLog.m_Log4net;
			}
		}
		public static string GetExceptionStack(Exception ex)
		{
			string text = string.Empty;
			text += ex.Message;
			Exception ex2 = ex.InnerException;
			while (null != ex2)
			{
				text = text + Environment.NewLine + ex2.Message;
				Exception innerException = ex2.InnerException;
				ex2 = innerException;
			}
			return text;
		}
	}
}
