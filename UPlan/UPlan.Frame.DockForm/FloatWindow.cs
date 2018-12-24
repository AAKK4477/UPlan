namespace UPlan.Frame.DockForm
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Security.Permissions;
    using System.Windows.Forms;
    

    public class FloatWindow : Form, INestedPanesContainer, IDockDragSource, IDragSource
    {
        private bool m_allowEndUserDocking;
        private UPlan.Frame.DockForm.DockPanel m_dockPanel;
        private NestedPaneCollection m_nestedPanes;
        private Dictionary<int, HandleMsg> WinMsgHandleMap;
        internal const int WM_CHECKDISPOSE = 0x401;

        protected internal FloatWindow(UPlan.Frame.DockForm.DockPanel dockPanel, DockPane pane)
        {
            this.m_allowEndUserDocking = true;
            this.WinMsgHandleMap = new Dictionary<int, HandleMsg>();
            this.InitHandleWmsgMap();
            this.InternalConstruct(dockPanel, pane, false, Rectangle.Empty);
        }

        protected internal FloatWindow(UPlan.Frame.DockForm.DockPanel dockPanel, DockPane pane, Rectangle bounds)
        {
            this.m_allowEndUserDocking = true;
            this.WinMsgHandleMap = new Dictionary<int, HandleMsg>();
            this.InitHandleWmsgMap();
            this.InternalConstruct(dockPanel, pane, true, bounds);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.DockPanel != null)
                {
                    this.DockPanel.RemoveFloatWindow(this);
                }
                this.m_dockPanel = null;
            }
            base.Dispose(disposing);
        }

        public void DockTo(UPlan.Frame.DockForm.DockPanel panel, DockStyle dockStyle)
        {
            if (panel != this.DockPanel)
            {
                throw new ArgumentException(Strings.IDockDragSource_DockTo_InvalidPanel, "panel");
            }
            NestedPaneCollection nestedPanesTo = null;
            if (dockStyle == DockStyle.Top)
            {
                nestedPanesTo = this.DockPanel.DockWindows[UPlan.Frame.DockForm.DockState.DockTop].NestedPanes;
            }
            else if (dockStyle == DockStyle.Bottom)
            {
                nestedPanesTo = this.DockPanel.DockWindows[UPlan.Frame.DockForm.DockState.DockBottom].NestedPanes;
            }
            else if (dockStyle == DockStyle.Left)
            {
                nestedPanesTo = this.DockPanel.DockWindows[UPlan.Frame.DockForm.DockState.DockLeft].NestedPanes;
            }
            else if (dockStyle == DockStyle.Right)
            {
                nestedPanesTo = this.DockPanel.DockWindows[UPlan.Frame.DockForm.DockState.DockRight].NestedPanes;
            }
            else if (dockStyle == DockStyle.Fill)
            {
                nestedPanesTo = this.DockPanel.DockWindows[UPlan.Frame.DockForm.DockState.Document].NestedPanes;
            }
            DockPane prevPane = null;
            for (int i = nestedPanesTo.Count - 1; i >= 0; i--)
            {
                if (nestedPanesTo[i] != this.VisibleNestedPanes[0])
                {
                    prevPane = nestedPanesTo[i];
                }
            }
            MergeNestedPanes(this.VisibleNestedPanes, nestedPanesTo, prevPane, DockAlignment.Left, 0.5);
        }

        public void DockTo(DockPane pane, DockStyle dockStyle, int contentIndex)
        {
            if (dockStyle == DockStyle.Fill)
            {
                for (int i = this.NestedPanes.Count - 1; i >= 0; i--)
                {
                    DockPane pane2 = this.NestedPanes[i];
                    for (int j = pane2.Contents.Count - 1; j >= 0; j--)
                    {
                        IDockContent content = pane2.Contents[j];
                        content.DockHandler.Pane = pane;
                        if (contentIndex != -1)
                        {
                            pane.SetContentIndex(content, contentIndex);
                        }
                    }
                }
            }
            else
            {
                DockAlignment left = DockAlignment.Left;
                if (dockStyle == DockStyle.Left)
                {
                    left = DockAlignment.Left;
                }
                else if (dockStyle == DockStyle.Right)
                {
                    left = DockAlignment.Right;
                }
                else if (dockStyle == DockStyle.Top)
                {
                    left = DockAlignment.Top;
                }
                else if (dockStyle == DockStyle.Bottom)
                {
                    left = DockAlignment.Bottom;
                }
                MergeNestedPanes(this.VisibleNestedPanes, pane.NestedPanesContainer.NestedPanes, pane, left, 0.5);
            }
        }

        public void FloatAt(Rectangle floatWindowBounds)
        {
            base.Bounds = floatWindowBounds;
        }

        Rectangle IDockDragSource.BeginDrag(Point ptMouse)
        {
            return base.Bounds;
        }

        bool IDockDragSource.CanDockTo(DockPane pane)
        {
            if (!this.IsDockStateValid(pane.DockState))
            {
                return false;
            }
            if (pane.FloatWindow == this)
            {
                return false;
            }
            return true;
        }

        bool IDockDragSource.IsDockStateValid(UPlan.Frame.DockForm.DockState dockState)
        {
            return this.IsDockStateValid(dockState);
        }

        private void InitHandleWmsgMap()
        {
            this.WinMsgHandleMap.Add(0xa1, new HandleMsg(this.LeftButtonDown));
            this.WinMsgHandleMap.Add(0xa4, new HandleMsg(this.RightButtonDown));
            this.WinMsgHandleMap.Add(0x10, new HandleMsg(this.WmClose));
            this.WinMsgHandleMap.Add(0xa3, new HandleMsg(this.LeftDoubleClick));
            this.WinMsgHandleMap.Add(0x401, new HandleMsg(this.WmCheckDispose));
        }

        private void InternalConstruct(UPlan.Frame.DockForm.DockPanel dockPanel, DockPane pane, bool boundsSpecified, Rectangle bounds)
        {
            if (dockPanel == null)
            {
                throw new ArgumentNullException(Strings.FloatWindow_Constructor_NullDockPanel);
            }
            this.m_nestedPanes = new NestedPaneCollection(this);
            base.FormBorderStyle = FormBorderStyle.Sizable;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            if (dockPanel.RightToLeft != this.RightToLeft)
            {
                this.RightToLeft = dockPanel.RightToLeft;
            }
            if (this.RightToLeftLayout != dockPanel.RightToLeftLayout)
            {
                this.RightToLeftLayout = dockPanel.RightToLeftLayout;
            }
            base.SuspendLayout();
            if (boundsSpecified)
            {
                base.Bounds = bounds;
                base.StartPosition = FormStartPosition.Manual;
            }
            else
            {
                base.StartPosition = FormStartPosition.WindowsDefaultLocation;
                base.Size = dockPanel.DefaultFloatWindowSize;
            }
            this.m_dockPanel = dockPanel;
            base.Owner = this.DockPanel.FindForm();
            this.DockPanel.AddFloatWindow(this);
            if (pane != null)
            {
                pane.FloatWindow = this;
            }
            this.MinimumSize = new Size(100, 100);
            base.ResumeLayout();
        }

        internal bool IsDockStateValid(UPlan.Frame.DockForm.DockState dockState)
        {
            foreach (DockPane pane in this.NestedPanes)
            {
                foreach (IDockContent content in pane.Contents)
                {
                    if (!DockHelper.IsDockStateValid(dockState, content.DockHandler.DockAreas))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void LeftButtonDown(ref Message m)
        {
            if (!base.IsDisposed)
            {
                if (((UPlan.Frame.DockForm.NativeMethods.SendMessage(base.Handle, 0x84, 0, (uint) ((int) m.LParam)) == 2) && this.DockPanel.AllowEndUserDocking) && this.AllowEndUserDocking)
                {
                    base.Activate();
                    this.m_dockPanel.BeginDrag(this);
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
        }

        private void LeftDoubleClick(ref Message m)
        {
            if (UPlan.Frame.DockForm.NativeMethods.SendMessage(base.Handle, 0x84, 0, (uint) ((int) m.LParam)) != 2)
            {
                base.WndProc(ref m);
            }
            else
            {
                this.DockPanel.SuspendLayout(true);
                foreach (DockPane pane in this.NestedPanes)
                {
                    if (pane.DockState == UPlan.Frame.DockForm.DockState.Float)
                    {
                        pane.RestoreToPanel();
                    }
                }
                this.DockPanel.ContentActivating = null;
                this.DockPanel.ResumeLayout(true, true);
            }
        }

        private static void MergeNestedPanes(VisibleNestedPaneCollection nestedPanesFrom, NestedPaneCollection nestedPanesTo, DockPane prevPane, DockAlignment alignment, double proportion)
        {
            if (nestedPanesFrom.Count != 0)
            {
                int num2;
                int count = nestedPanesFrom.Count;
                DockPane[] paneArray = new DockPane[count];
                DockPane[] paneArray2 = new DockPane[count];
                DockAlignment[] alignmentArray = new DockAlignment[count];
                double[] numArray = new double[count];
                for (num2 = 0; num2 < count; num2++)
                {
                    paneArray[num2] = nestedPanesFrom[num2];
                    paneArray2[num2] = nestedPanesFrom[num2].NestedDockingStatus.PreviousPane;
                    alignmentArray[num2] = nestedPanesFrom[num2].NestedDockingStatus.Alignment;
                    numArray[num2] = nestedPanesFrom[num2].NestedDockingStatus.Proportion;
                }
                DockPane pane = paneArray[0].DockTo(nestedPanesTo.Container, prevPane, alignment, proportion);
                paneArray[0].DockState = nestedPanesTo.DockState;
                for (num2 = 1; num2 < count; num2++)
                {
                    for (int i = num2; i < count; i++)
                    {
                        if (paneArray2[i] == paneArray[num2 - 1])
                        {
                            paneArray2[i] = pane;
                        }
                    }
                    pane = paneArray[num2].DockTo(nestedPanesTo.Container, paneArray2[num2], alignmentArray[num2], numArray[num2]);
                    paneArray[num2].DockState = nestedPanesTo.DockState;
                }
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            this.DockPanel.FloatWindows.BringWindowToFront(this);
            base.OnActivated(e);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            this.VisibleNestedPanes.Refresh();
            this.RefreshChanges();
            base.Visible = this.VisibleNestedPanes.Count > 0;
            this.SetText();
            base.OnLayout(levent);
        }

        internal void RefreshChanges()
        {
            if (!base.IsDisposed)
            {
                if (this.VisibleNestedPanes.Count == 0)
                {
                    base.ControlBox = true;
                }
                else
                {
                    for (int i = this.VisibleNestedPanes.Count - 1; i >= 0; i--)
                    {
                        DockContentCollection contents = this.VisibleNestedPanes[i].Contents;
                        for (int j = contents.Count - 1; j >= 0; j--)
                        {
                            IDockContent content = contents[j];
                            if ((content.DockHandler.DockState == UPlan.Frame.DockForm.DockState.Float) && content.DockHandler.CloseButton)
                            {
                                base.ControlBox = true;
                                return;
                            }
                        }
                    }
                    base.ControlBox = false;
                }
            }
        }

        private void RightButtonDown(ref Message m)
        {
            if (UPlan.Frame.DockForm.NativeMethods.SendMessage(base.Handle, 0x84, 0, (uint) ((int) m.LParam)) == 2)
            {
                DockPane pane = (this.VisibleNestedPanes.Count == 1) ? this.VisibleNestedPanes[0] : null;
                if ((pane != null) && (pane.ActiveContent != null))
                {
                    pane.ShowTabPageContextMenu(this, base.PointToClient(Control.MousePosition));
                    return;
                }
            }
            base.WndProc(ref m);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            Rectangle virtualScreen = SystemInformation.VirtualScreen;
            if ((y + height) > virtualScreen.Bottom)
            {
                y -= (y + height) - virtualScreen.Bottom;
            }
            if (y < virtualScreen.Top)
            {
                y += virtualScreen.Top - y;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        internal void SetText()
        {
            DockPane pane = (this.VisibleNestedPanes.Count == 1) ? this.VisibleNestedPanes[0] : null;
            if (pane == null)
            {
                this.Text = " ";
            }
            else if (pane.ActiveContent == null)
            {
                this.Text = " ";
            }
            else
            {
                this.Text = pane.ActiveContent.DockHandler.TabText;
            }
        }

        internal void TestDrop(IDockDragSource dragSource, DockOutlineBase dockOutline)
        {
            if (this.VisibleNestedPanes.Count == 1)
            {
                DockPane pane = this.VisibleNestedPanes[0];
                if (dragSource.CanDockTo(pane))
                {
                    Point mousePosition = Control.MousePosition;
                    uint lParam = Win32Helper.MakeLong(mousePosition.X, mousePosition.Y);
                    if (UPlan.Frame.DockForm.NativeMethods.SendMessage(base.Handle, 0x84, 0, lParam) == 2)
                    {
                        dockOutline.Show(this.VisibleNestedPanes[0], -1);
                    }
                }
            }
        }

        private void WmCheckDispose(ref Message m)
        {
            if (this.NestedPanes.Count == 0)
            {
                base.Dispose();
            }
        }

        private void WmClose(ref Message m)
        {
            if (this.NestedPanes.Count == 0)
            {
                base.WndProc(ref m);
            }
            else
            {
                for (int i = this.NestedPanes.Count - 1; i >= 0; i--)
                {
                    DockContentCollection contents = this.NestedPanes[i].Contents;
                    for (int j = contents.Count - 1; j >= 0; j--)
                    {
                        IDockContent content = contents[j];
                        if ((content.DockHandler.DockState == UPlan.Frame.DockForm.DockState.Float) && content.DockHandler.CloseButton)
                        {
                            if (content.DockHandler.HideOnClose)
                            {
                                content.DockHandler.Hide();
                            }
                            else
                            {
                                content.DockHandler.Close();
                            }
                        }
                    }
                }
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (this.WinMsgHandleMap.ContainsKey(m.Msg))
            {
                this.WinMsgHandleMap[m.Msg](ref m);
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        public bool AllowEndUserDocking
        {
            get
            {
                return this.m_allowEndUserDocking;
            }
            set
            {
                this.m_allowEndUserDocking = value;
            }
        }

        public virtual Rectangle DisplayingRectangle
        {
            get
            {
                return base.ClientRectangle;
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
                return UPlan.Frame.DockForm.DockState.Float;
            }
        }

        Control IDragSource.DragControl
        {
            get
            {
                return this;
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

        private delegate void HandleMsg(ref Message m);

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FloatWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Name = "FloatWindow";
            this.ResumeLayout(false);

        }

       
    }
}

