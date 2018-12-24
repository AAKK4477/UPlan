namespace UPlan.Frame.DockForm
{
    using System;
    using System.Windows.Forms;

    internal class DummyControl : Control
    {
        public DummyControl()
        {
            base.SetStyle(ControlStyles.Selectable, false);
        }
    }
}

