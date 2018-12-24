namespace UPlan.Frame.DockForm
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal static class Win32Helper
    {
        public static Control ControlAtPoint(Point pt)
        {
            return Control.FromChildHandle(UPlan.Frame.DockForm.NativeMethods.WindowFromPoint(pt));
        }

        public static uint MakeLong(int low, int high)
        {
            return (uint) ((high << 0x10) + low);
        }
    }
}

