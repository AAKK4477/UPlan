namespace UPlan.Frame.DockForm
{
    using System;
    using System.Drawing;

    internal interface ISplitterDragSource : IDragSource
    {
        void BeginDrag(Rectangle rectSplitter);
        void EndDrag();
        void MoveSplitter(int offset);

        Rectangle DragLimitBounds { get; }

        bool IsVertical { get; }
    }
}

