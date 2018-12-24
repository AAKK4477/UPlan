using System;
namespace UPlan.Frame.Interface
{
	public interface IBaseService
	{
		IBaseService Lookup(string serviceName);
	}
}
