namespace UPlan.Frame.DockForm
{
    using System;

    public interface IDockContent
    {
        DockContentHandler DockHandler { get; }

        bool WillBeFloat { get; set; }
    }
}

