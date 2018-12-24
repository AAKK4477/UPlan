using System;
namespace UPlan.Frame.Interface
{
	public interface INetworkSetting
	{
		bool CanChangeNetworkType(NetWorkType netType);
		void SetNetworkType(NetWorkType netType);
	}
}
