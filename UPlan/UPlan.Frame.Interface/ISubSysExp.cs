using System;
namespace UPlan.Frame.Interface
{
	public interface ISubSysExp
	{
		XlsTable[] Export();
		void ExportStart(IApplicationContext context);
		void ExportEnd();
		XlsTable[] ExportLCS();
	}
}
