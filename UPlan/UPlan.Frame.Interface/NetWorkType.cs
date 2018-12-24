using System;
namespace UPlan.Frame.Interface
{
	[Flags]
	public enum NetWorkType
	{
		GSM = 1,
		UMTS = 2,
		TDSCDMA = 4,
		LTE = 8,
		CDMA = 16
	}
}
