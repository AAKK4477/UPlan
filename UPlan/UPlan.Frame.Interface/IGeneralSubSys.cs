using System;
namespace UPlan.Frame.Interface
{
	public interface IGeneralSubSys : ISubSysSerializable, IDisposable, ISubSysExp, ISubsysConvert
	{
		void InitStage1(IApplicationContext appContext);
		void InitStage2();
	}
}
