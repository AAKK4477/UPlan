using System;
namespace UPlan.Frame.Interface
{
	public interface IProjectUImrg : IBaseService
	{
		string CreateNewProjectCanTest(string systemPath, NetWorkType type);
		string OpenProjectIpLFile(string fileName);
	}
}
