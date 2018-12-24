using System;
namespace UPlan.Frame.Interface
{
	public interface IApplicationContext : IBaseService
	{
		void RegisterService(IBaseService service);
	}
}
