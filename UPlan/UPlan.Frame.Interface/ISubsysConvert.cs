using System;
namespace UPlan.Frame.Interface
{
	public interface ISubsysConvert
	{
		ISubSystemData ConvertXls(XlsTable[] xlsTables);
		void Import(ISubSystemData subsystem, bool isUpdate);
	}
}
