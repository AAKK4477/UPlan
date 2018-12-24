namespace UPlan.Frame.DockForm
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    internal interface IDockDragSource : IDragSource
    {
        Rectangle BeginDrag(Point ptMouse);
        bool CanDockTo(DockPane pane);
        void DockTo(DockPanel panel, DockStyle dockStyle);
        void DockTo(DockPane pane, DockStyle dockStyle, int contentIndex);
        void FloatAt(Rectangle floatWindowBounds);
        bool IsDockStateValid(DockState dockState);
    }
}

