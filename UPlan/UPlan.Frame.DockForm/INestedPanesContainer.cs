namespace UPlan.Frame.DockForm
{
    using System;
    using System.Drawing;

    public interface INestedPanesContainer
    {
        Rectangle DisplayingRectangle { get; }

        UPlan.Frame.DockForm.DockState DockState { get; }

        bool IsFloat { get; }

        NestedPaneCollection NestedPanes { get; }

        VisibleNestedPaneCollection VisibleNestedPanes { get; }
    }
}

