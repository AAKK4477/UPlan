namespace UPlan.Frame.DockForm
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    [ToolboxItem(false)]
    public class DockWindow : Panel, INestedPanesContainer, ISplitterDragSource, IDragSource
    {
        private UPlan.Frame.DockForm.DockPanel m_dockPanel;
        private UPlan.Frame.DockForm.DockState m_dockState;
        private NestedPaneCollection m_nestedPanes;
        private SplitterControl m_splitter;

        internal DockWindow(UPlan.Frame.DockForm.DockPanel dockPanel, UPlan.Frame.DockForm.DockState dockState)
        {
            this.m_nestedPanes = new NestedPaneCollection(this);
            this.m_dockPanel = dockPanel;
            this.m_dockState = dockState;
            base.Visible = false;
            base.SuspendLayout();
            if ((((this.DockState == UPlan.Frame.DockForm.DockState.DockLeft) || (this.DockState == UPlan.Frame.DockForm.DockState.DockRight)) || (this.DockState == UPlan.Frame.DockForm.DockState.DockTop)) || (this.DockState == UPlan.Frame.DockForm.DockState.DockBottom))
            {
                this.m_splitter = new SplitterControl();
                base.Controls.Add(this.m_splitter);
            }
            if (this.DockState == UPlan.Frame.DockForm.DockState.DockLeft)
            {
                this.Dock = DockStyle.Left;
                this.m_splitter.Dock = DockStyle.Right;
            }
            else if (this.DockState == UPlan.Frame.DockForm.DockState.DockRight)
            {
                this.Dock = DockStyle.Right;
                this.m_splitter.Dock = DockStyle.Left;
            }
            else if (this.DockState == UPlan.Frame.DockForm.DockState.DockTop)
            {
                this.Dock = DockStyle.Top;
                this.m_splitter.Dock = DockStyle.Bottom;
            }
            else if (this.DockState == UPlan.Frame.DockForm.DockState.DockBottom)
            {
                this.Dock = DockStyle.Bottom;
                this.m_splitter.Dock = DockStyle.Top;
            }
            else if (this.DockState == UPlan.Frame.DockForm.DockState.Document)
            {
                this.Dock = DockStyle.Fill;
            }
            base.ResumeLayout();
        }

        private Rectangle GetBottomPortion(int offset, Rectangle rectDockArea)
        {
            if (this.DockPanel.DockBottomPortion > 1.0)
            {
                this.DockPanel.DockBottomPortion = base.Height - offset;
                return rectDockArea;
            }
            UPlan.Frame.DockForm.DockPanel dockPanel = this.DockPanel;
            dockPanel.DockBottomPortion -= ((double) offset) / ((double) rectDockArea.Height);
            return rectDockArea;
        }

        private Rectangle GetLeftPortion(int offset, Rectangle rectDockArea)
        {
            if (this.DockPanel.DockLeftPortion > 1.0)
            {
                this.DockPanel.DockLeftPortion = base.Width + offset;
                return rectDockArea;
            }
            UPlan.Frame.DockForm.DockPanel dockPanel = this.DockPanel;
            dockPanel.DockLeftPortion += ((double) offset) / ((double) rectDockArea.Width);
            return rectDockArea;
        }

        private Rectangle GetRightPortion(int offset, Rectangle rectDockArea)
        {
            if (this.DockPanel.DockRightPortion > 1.0)
            {
                this.DockPanel.DockRightPortion = base.Width - offset;
                return rectDockArea;
            }
            UPlan.Frame.DockForm.DockPanel dockPanel = this.DockPanel;
            dockPanel.DockRightPortion -= ((double) offset) / ((double) rectDockArea.Width);
            return rectDockArea;
        }

        private Rectangle GetTopPortion(int offset, Rectangle rectDockArea)
        {
            if (this.DockPanel.DockTopPortion > 1.0)
            {
                this.DockPanel.DockTopPortion = base.Height + offset;
                return rectDockArea;
            }
            UPlan.Frame.DockForm.DockPanel dockPanel = this.DockPanel;
            dockPanel.DockTopPortion += ((double) offset) / ((double) rectDockArea.Height);
            return rectDockArea;
        }

        private Rectangle HandleDockPortion(int offset, Rectangle rectDockArea)
        {
            if ((this.DockState == UPlan.Frame.DockForm.DockState.DockLeft) && (rectDockArea.Width > 0))
            {
                rectDockArea = this.GetLeftPortion(offset, rectDockArea);
                return rectDockArea;
            }
            if ((this.DockState == UPlan.Frame.DockForm.DockState.DockRight) && (rectDockArea.Width > 0))
            {
                rectDockArea = this.GetRightPortion(offset, rectDockArea);
                return rectDockArea;
            }
            if ((this.DockState == UPlan.Frame.DockForm.DockState.DockBottom) && (rectDockArea.Height > 0))
            {
                rectDockArea = this.GetBottomPortion(offset, rectDockArea);
                return rectDockArea;
            }
            if ((this.DockState == UPlan.Frame.DockForm.DockState.DockTop) && (rectDockArea.Height > 0))
            {
                rectDockArea = this.GetTopPortion(offset, rectDockArea);
            }
            return rectDockArea;
        }

        void ISplitterDragSource.BeginDrag(Rectangle rectSplitter)
        {
        }

        void ISplitterDragSource.EndDrag()
        {
        }

        void ISplitterDragSource.MoveSplitter(int offset)
        {
            if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
            {
                base.SendToBack();
            }
            Rectangle dockArea = this.DockPanel.DockArea;
            dockArea = this.HandleDockPortion(offset, dockArea);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            this.VisibleNestedPanes.Refresh();
            if (this.VisibleNestedPanes.Count == 0)
            {
                if (base.Visible)
                {
                    base.Visible = false;
                }
            }
            else if (!base.Visible)
            {
                base.Visible = true;
                this.VisibleNestedPanes.Refresh();
            }
            base.OnLayout(levent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DockState == UPlan.Frame.DockForm.DockState.Document)
            {
                e.Graphics.DrawRectangle(SystemPens.ControlDark, base.ClientRectangle.X, base.ClientRectangle.Y, base.ClientRectangle.Width - 1, base.ClientRectangle.Height - 1);
            }
            base.OnPaint(e);
        }

        internal DockPane DefaultPane
        {
            get
            {
                return ((this.VisibleNestedPanes.Count == 0) ? null : this.VisibleNestedPanes[0]);
            }
        }

        public virtual Rectangle DisplayingRectangle
        {
            get
            {
                Rectangle clientRectangle = base.ClientRectangle;
                if (this.DockState == UPlan.Frame.DockForm.DockState.Document)
                {
                    clientRectangle.X++;
                    clientRectangle.Y++;
                    clientRectangle.Width -= 2;
                    clientRectangle.Height -= 2;
                    return clientRectangle;
                }
                if (this.DockState == UPlan.Frame.DockForm.DockState.DockLeft)
                {
                    clientRectangle.Width -= 4;
                    return clientRectangle;
                }
                if (this.DockState == UPlan.Frame.DockForm.DockState.DockRight)
                {
                    clientRectangle.X += 4;
                    clientRectangle.Width -= 4;
                    return clientRectangle;
                }
                if (this.DockState == UPlan.Frame.DockForm.DockState.DockTop)
                {
                    clientRectangle.Height -= 4;
                    return clientRectangle;
                }
                if (this.DockState == UPlan.Frame.DockForm.DockState.DockBottom)
                {
                    clientRectangle.Y += 4;
                    clientRectangle.Height -= 4;
                }
                return clientRectangle;
            }
        }

        public UPlan.Frame.DockForm.DockPanel DockPanel
        {
            get
            {
                return this.m_dockPanel;
            }
        }

        public UPlan.Frame.DockForm.DockState DockState
        {
            get
            {
                return this.m_dockState;
            }
        }

        Control IDragSource.DragControl
        {
            get
            {
                return this;
            }
        }

        Rectangle ISplitterDragSource.DragLimitBounds
        {
            get
            {
                Point location;
                Rectangle dockArea = this.DockPanel.DockArea;
                if ((Control.ModifierKeys & Keys.Shift) == Keys.None)
                {
                    location = base.Location;
                }
                else
                {
                    location = this.DockPanel.DockArea.Location;
                }
                if (((ISplitterDragSource) this).IsVertical)
                {
                    dockArea.X += 0x18;
                    dockArea.Width -= 0x30;
                    dockArea.Y = location.Y;
                    if ((Control.ModifierKeys & Keys.Shift) == Keys.None)
                    {
                        dockArea.Height = base.Height;
                    }
                }
                else
                {
                    dockArea.Y += 0x18;
                    dockArea.Height -= 0x30;
                    dockArea.X = location.X;
                    if ((Control.ModifierKeys & Keys.Shift) == Keys.None)
                    {
                        dockArea.Width = base.Width;
                    }
                }
                return this.DockPanel.RectangleToScreen(dockArea);
            }
        }

        bool ISplitterDragSource.IsVertical
        {
            get
            {
                return ((this.DockState == UPlan.Frame.DockForm.DockState.DockLeft) || (this.DockState == UPlan.Frame.DockForm.DockState.DockRight));
            }
        }

        public bool IsFloat
        {
            get
            {
                return (this.DockState == UPlan.Frame.DockForm.DockState.Float);
            }
        }

        public NestedPaneCollection NestedPanes
        {
            get
            {
                return this.m_nestedPanes;
            }
        }

        public VisibleNestedPaneCollection VisibleNestedPanes
        {
            get
            {
                return this.NestedPanes.VisibleNestedPanes;
            }
        }

        private class SplitterControl : SplitterBase
        {
            protected override void StartDrag()
            {
                DockWindow parent = base.Parent as DockWindow;
                if (parent != null)
                {
                    parent.DockPanel.BeginDrag(parent, parent.RectangleToScreen(base.Bounds));
                }
            }

            protected override int SplitterSize
            {
                get
                {
                    return 4;
                }
            }
        }
    }
}

