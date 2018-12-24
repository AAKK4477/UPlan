using System;
namespace UPlan.Frame.Interface
{
    public class LockMainForm
    {
        public static void Lock()
        {
            MouseKeyMessageFilter.Instance.Enable = true;
        }
        public static void Unlock()
        {
            MouseKeyMessageFilter.Instance.Enable = false;
        }
    }
}


