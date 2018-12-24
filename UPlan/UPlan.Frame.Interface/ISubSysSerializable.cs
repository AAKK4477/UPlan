using System;
namespace UPlan.Frame.Interface
{
    public interface ISubSysSerializable
    {
        void AutoLoadData(ISubSystemData systemData);
        ISubSystemData Serialize();
        void SerializeStart();
        void SerializeEnd();
    }
}
