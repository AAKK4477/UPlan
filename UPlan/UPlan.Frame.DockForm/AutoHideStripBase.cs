namespace UPlan.Frame.DockForm
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public abstract class AutoHideStripBase : Control
    {
        private GraphicsPath m_displayingArea = null;
        private UPlan.Frame.DockForm.DockPanel m_dockPanel;
        private Dictionary<DockState, TabStripRectangleKind> m_MapState = new Dictionary<DockState, TabStripRectangleKind>();
        private Dictionary<DockState, PaneCollection> m_MapStatePanec = new Dictionary<DockState, PaneCollection>();
        private PaneCollection m_panesBottom;
        private PaneCollection m_panesLeft;
        private PaneCollection m_panesRight;
        private PaneCollection m_panesTop;

        protected AutoHideStripBase(UPlan.Frame.DockForm.DockPanel panel)
        {
            this.m_dockPanel = panel;
            this.m_panesTop = new PaneCollection(panel, DockState.DockTopAutoHide);
            this.m_panesBottom = new PaneCollection(panel, DockState.DockBottomAutoHide);
            this.m_panesLeft = new PaneCollection(panel, DockState.DockLeftAutoHide);
            this.m_panesRight = new PaneCollection(panel, DockState.DockRightAutoHide);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.Selectable, false);
            this.InitMapState();
            this.InitMapStatePanec();
        }

        protected virtual Pane CreatePane(DockPane dockPane)
        {
            return new Pane(dockPane);
        }

        protected virtual Tab CreateTab(IDockContent content)
        {
            return new Tab(content);
        }

        internal int GetNumberOfPanes(DockState dockState)
        {
            return this.GetPanes(dockState).Count;
        }

        protected PaneCollection GetPanes(DockState dockState)
        {
            if (!this.m_MapStatePanec.ContainsKey(dockState))
            {
                throw new ArgumentOutOfRangeException("dockState");
            }
            return this.m_MapStatePanec[dockState];
        }

        protected internal Rectangle GetTabStripRectangle(DockState dockState)
        {
            int height = this.MeasureHeight();
            return this.GetTabStripRectangle(dockState, height);
        }

        private Rectangle GetTabStripRectangle(DockState dockState, int height)
        {
            if (this.m_MapState.ContainsKey(dockState))
            {
                return ((this.m_MapState[dockState].m_Panes.Count > 0) ? this.m_MapState[dockState].m_GetTabStripRectangle(height) : Rectangle.Empty);
            }
            return Rectangle.Empty;
        }

        private Rectangle GetTabStripRectangleBottom(int height)
        {
            return new Rectangle(this.RectangleBottomLeft.Width, base.Height - height, (base.Width - this.RectangleBottomLeft.Width) - this.RectangleBottomRight.Width, height);
        }

        private Rectangle GetTabStripRectangleLeft(int height)
        {
            return new Rectangle(0, this.RectangleTopLeft.Width, height, (base.Height - this.RectangleTopLeft.Height) - this.RectangleBottomLeft.Height);
        }

        private Rectangle GetTabStripRectangleRight(int height)
        {
            return new Rectangle(base.Width - height, this.RectangleTopRight.Width, height, (base.Height - this.RectangleTopRight.Height) - this.RectangleBottomRight.Height);
        }

        private Rectangle GetTabStripRectangleTop(int height)
        {
            return new Rectangle(this.RectangleTopLeft.Width, 0, (base.Width - this.RectangleTopLeft.Width) - this.RectangleTopRight.Width, height);
        }

        private IDockContent HitTest()
        {
            Point point = base.PointToClient(Control.MousePosition);
            return this.HitTest(point);
        }

        protected abstract IDockContent HitTest(Point point);
        private void InitMapState()
        {
            TabStripRectangleKind kind = new TabStripRectangleKind {
                m_Panes = this.PanesTop,
                m_GetTabStripRectangle = new CreateTabStripRectangleDelegate(this.GetTabStripRectangleTop)
            };
            TabStripRectangleKind kind2 = new TabStripRectangleKind {
                m_Panes = this.PanesBottom,
                m_GetTabStripRectangle = new CreateTabStripRectangleDelegate(this.GetTabStripRectangleBottom)
            };
            TabStripRectangleKind kind3 = new TabStripRectangleKind {
                m_Panes = this.PanesLeft,
                m_GetTabStripRectangle = new CreateTabStripRectangleDelegate(this.GetTabStripRectangleLeft)
            };
            TabStripRectangleKind kind4 = new TabStripRectangleKind {
                m_Panes = this.PanesRight,
                m_GetTabStripRectangle = new CreateTabStripRectangleDelegate(this.GetTabStripRectangleRight)
            };
            this.m_MapState.Add(DockState.DockTopAutoHide, kind);
            this.m_MapState.Add(DockState.DockBottomAutoHide, kind2);
            this.m_MapState.Add(DockState.DockLeftAutoHide, kind3);
            this.m_MapState.Add(DockState.DockRightAutoHide, kind4);
        }

        private void InitMapStatePanec()
        {
            this.m_MapStatePanec.Add(DockState.DockTopAutoHide, this.PanesTop);
            this.m_MapStatePanec.Add(DockState.DockBottomAutoHide, this.PanesBottom);
            this.m_MapStatePanec.Add(DockState.DockLeftAutoHide, this.PanesLeft);
            this.m_MapStatePanec.Add(DockState.DockRightAutoHide, this.PanesRight);
        }

        protected internal abstract int MeasureHeight();
        protected override void OnLayout(LayoutEventArgs levent)
        {
            this.RefreshChanges();
            base.OnLayout(levent);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                IDockContent content = this.HitTest();
                if (content != null)
                {
                    content.DockHandler.Activate();
                }
            }
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            IDockContent content = this.HitTest();
            if ((content != null) && (this.DockPanel.ActiveAutoHideContent != content))
            {
                this.DockPanel.ActiveAutoHideContent = content;
            }
            base.ResetMouseEventArgs();
        }

        protected virtual void OnRefreshChanges()
        {
        }

        internal void RefreshChanges()
        {
            if (!base.IsDisposed)
            {
                this.SetRegion();
                this.OnRefreshChanges();
            }
        }

        private void SetRegion()
        {
            this.DisplayingArea.Reset();
            this.DisplayingArea.AddRectangle(this.RectangleTopLeft);
            this.DisplayingArea.AddRectangle(this.RectangleTopRight);
            this.DisplayingArea.AddRectangle(this.RectangleBottomLeft);
            this.DisplayingArea.AddRectangle(this.RectangleBottomRight);
            this.DisplayingArea.AddRectangle(this.GetTabStripRectangle(DockState.DockTopAutoHide));
            this.DisplayingArea.AddRectangle(this.GetTabStripRectangle(DockState.DockBottomAutoHide));
            this.DisplayingArea.AddRectangle(this.GetTabStripRectangle(DockState.DockLeftAutoHide));
            this.DisplayingArea.AddRectangle(this.GetTabStripRectangle(DockState.DockRightAutoHide));
            base.Region = new Region(this.DisplayingArea);
        }

        private GraphicsPath DisplayingArea
        {
            get
            {
                if (this.m_displayingArea == null)
                {
                    this.m_displayingArea = new GraphicsPath();
                }
                return this.m_displayingArea;
            }
        }

        protected UPlan.Frame.DockForm.DockPanel DockPanel
        {
            get
            {
                return this.m_dockPanel;
            }
        }

        protected PaneCollection PanesBottom
        {
            get
            {
                return this.m_panesBottom;
            }
        }

        protected PaneCollection PanesLeft
        {
            get
            {
                return this.m_panesLeft;
            }
        }

        protected PaneCollection PanesRight
        {
            get
            {
                return this.m_panesRight;
            }
        }

        protected PaneCollection PanesTop
        {
            get
            {
                return this.m_panesTop;
            }
        }

        protected Rectangle RectangleBottomLeft
        {
            get
            {
                int width = this.MeasureHeight();
                return (((this.PanesBottom.Count > 0) && (this.PanesLeft.Count > 0)) ? new Rectangle(0, base.Height - width, width, width) : Rectangle.Empty);
            }
        }

        protected Rectangle RectangleBottomRight
        {
            get
            {
                int width = this.MeasureHeight();
                return (((this.PanesBottom.Count > 0) && (this.PanesRight.Count > 0)) ? new Rectangle(base.Width - width, base.Height - width, width, width) : Rectangle.Empty);
            }
        }

        protected Rectangle RectangleTopLeft
        {
            get
            {
                int width = this.MeasureHeight();
                return (((this.PanesTop.Count > 0) && (this.PanesLeft.Count > 0)) ? new Rectangle(0, 0, width, width) : Rectangle.Empty);
            }
        }

        protected Rectangle RectangleTopRight
        {
            get
            {
                int width = this.MeasureHeight();
                return (((this.PanesTop.Count > 0) && (this.PanesRight.Count > 0)) ? new Rectangle(base.Width - width, 0, width, width) : Rectangle.Empty);
            }
        }

        private delegate Rectangle CreateTabStripRectangleDelegate(int height);

        protected class Pane : IDisposable
        {
            private UPlan.Frame.DockForm.DockPane m_dockPane;

            protected internal Pane(UPlan.Frame.DockForm.DockPane dockPane)
            {
                this.m_dockPane = dockPane;
            }

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
            }

            ~Pane()
            {
                this.Dispose(false);
            }

            public AutoHideStripBase.TabCollection AutoHideTabs
            {
                get
                {
                    if (this.DockPane.AutoHideTabs == null)
                    {
                        this.DockPane.AutoHideTabs = new AutoHideStripBase.TabCollection(this.DockPane);
                    }
                    return (this.DockPane.AutoHideTabs as AutoHideStripBase.TabCollection);
                }
            }

            public UPlan.Frame.DockForm.DockPane DockPane
            {
                get
                {
                    return this.m_dockPane;
                }
            }
        }

        protected sealed class PaneCollection : IEnumerable<AutoHideStripBase.Pane>, IEnumerable
        {
            private UPlan.Frame.DockForm.DockPanel m_dockPanel;
            private AutoHideStateCollection m_states;

            internal PaneCollection(UPlan.Frame.DockForm.DockPanel panel, DockState dockState)
            {
                this.m_dockPanel = panel;
                this.m_states = new AutoHideStateCollection();
                this.States[DockState.DockTopAutoHide].Selected = dockState == DockState.DockTopAutoHide;
                this.States[DockState.DockBottomAutoHide].Selected = dockState == DockState.DockBottomAutoHide;
                this.States[DockState.DockLeftAutoHide].Selected = dockState == DockState.DockLeftAutoHide;
                this.States[DockState.DockRightAutoHide].Selected = dockState == DockState.DockRightAutoHide;
            }

            public bool Contains(AutoHideStripBase.Pane pane)
            {
                return (this.IndexOf(pane) != -1);
            }

            public int IndexOf(AutoHideStripBase.Pane pane)
            {
                if (pane != null)
                {
                    int num = 0;
                    foreach (DockPane pane2 in this.DockPanel.Panes)
                    {
                        if (this.States.ContainsPane(pane.DockPane))
                        {
                            if (pane == pane2.AutoHidePane)
                            {
                                return num;
                            }
                            num++;
                        }
                    }
                }
                return -1;
            }

            IEnumerator<AutoHideStripBase.Pane> IEnumerable<AutoHideStripBase.Pane>.GetEnumerator()
            {
                for (int i = 0; i < this.Count; i++)
                {
                    yield return this[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                for (int i = 0; i < this.Count; i++)
                {
                    yield return this[i];
                }
            }

            public int Count
            {
                get
                {
                    int num = 0;
                    foreach (DockPane pane in this.DockPanel.Panes)
                    {
                        if (this.States.ContainsPane(pane))
                        {
                            num++;
                        }
                    }
                    return num;
                }
            }

            public UPlan.Frame.DockForm.DockPanel DockPanel
            {
                get
                {
                    return this.m_dockPanel;
                }
            }

            public AutoHideStripBase.Pane this[int index]
            {
                get
                {
                    int num = 0;
                    foreach (DockPane pane in this.DockPanel.Panes)
                    {
                        if (this.States.ContainsPane(pane))
                        {
                            if (num == index)
                            {
                                if (pane.AutoHidePane == null)
                                {
                                    pane.AutoHidePane = this.DockPanel.AutoHideStripControl.CreatePane(pane);
                                }
                                return (pane.AutoHidePane as AutoHideStripBase.Pane);
                            }
                            num++;
                        }
                    }
                    throw new ArgumentOutOfRangeException("index");
                }
            }

            private AutoHideStateCollection States
            {
                get
                {
                    return this.m_states;
                }
            }

            private class AutoHideState
            {
                public UPlan.Frame.DockForm.DockState m_dockState;
                public bool m_selected = false;

                public AutoHideState(UPlan.Frame.DockForm.DockState dockState)
                {
                    this.m_dockState = dockState;
                }

                public UPlan.Frame.DockForm.DockState DockState
                {
                    get
                    {
                        return this.m_dockState;
                    }
                }

                public bool Selected
                {
                    get
                    {
                        return this.m_selected;
                    }
                    set
                    {
                        this.m_selected = value;
                    }
                }
            }

            private class AutoHideStateCollection
            {
                private AutoHideStripBase.PaneCollection.AutoHideState[] m_states = new AutoHideStripBase.PaneCollection.AutoHideState[] { new AutoHideStripBase.PaneCollection.AutoHideState(DockState.DockTopAutoHide), new AutoHideStripBase.PaneCollection.AutoHideState(DockState.DockBottomAutoHide), new AutoHideStripBase.PaneCollection.AutoHideState(DockState.DockLeftAutoHide), new AutoHideStripBase.PaneCollection.AutoHideState(DockState.DockRightAutoHide) };

                public bool ContainsPane(DockPane pane)
                {
                    if (!pane.IsHidden)
                    {
                        for (int i = 0; i < this.m_states.Length; i++)
                        {
                            if ((this.m_states[i].DockState == pane.DockState) && this.m_states[i].Selected)
                            {
                                return true;
                            }
                        }
                    }
                    return false;
                }

                public AutoHideStripBase.PaneCollection.AutoHideState this[DockState dockState]
                {
                    get
                    {
                        for (int i = 0; i < this.m_states.Length; i++)
                        {
                            if (this.m_states[i].DockState == dockState)
                            {
                                return this.m_states[i];
                            }
                        }
                        throw new ArgumentOutOfRangeException("dockState");
                    }
                }
            }


        }

        protected class Tab : IDisposable
        {
            private IDockContent m_content;

            protected internal Tab(IDockContent content)
            {
                this.m_content = content;
            }

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
            }

            ~Tab()
            {
                this.Dispose(false);
            }

            public IDockContent Content
            {
                get
                {
                    return this.m_content;
                }
            }
        }

        protected sealed class TabCollection : IEnumerable<AutoHideStripBase.Tab>, IEnumerable
        {
            private UPlan.Frame.DockForm.DockPane m_dockPane = null;

            internal TabCollection(UPlan.Frame.DockForm.DockPane pane)
            {
                this.m_dockPane = pane;
            }

            public bool Contains(AutoHideStripBase.Tab tab)
            {
                return (this.IndexOf(tab) != -1);
            }

            public bool Contains(IDockContent content)
            {
                return (this.IndexOf(content) != -1);
            }

            public int IndexOf(AutoHideStripBase.Tab tab)
            {
                if (tab == null)
                {
                    return -1;
                }
                return this.IndexOf(tab.Content);
            }

            public int IndexOf(IDockContent content)
            {
                return this.DockPane.DisplayingContents.IndexOf(content);
            }

            IEnumerator<AutoHideStripBase.Tab> IEnumerable<AutoHideStripBase.Tab>.GetEnumerator()
            {
                for (int i = 0; i < this.Count; i++)
                {
                    yield return this[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                for (int i = 0; i < this.Count; i++)
                {
                    yield return this[i];
                }
            }

            public int Count
            {
                get
                {
                    return this.DockPane.DisplayingContents.Count;
                }
            }

            public UPlan.Frame.DockForm.DockPane DockPane
            {
                get
                {
                    return this.m_dockPane;
                }
            }

            public UPlan.Frame.DockForm.DockPanel DockPanel
            {
                get
                {
                    return this.DockPane.DockPanel;
                }
            }

            public AutoHideStripBase.Tab this[int index]
            {
                get
                {
                    IDockContent content = this.DockPane.DisplayingContents[index];
                    if (content == null)
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }
                    if (content.DockHandler.AutoHideTab == null)
                    {
                        content.DockHandler.AutoHideTab = this.DockPanel.AutoHideStripControl.CreateTab(content);
                    }
                    return (content.DockHandler.AutoHideTab as AutoHideStripBase.Tab);
                }
            }


        }

        private class TabStripRectangleKind
        {
            public AutoHideStripBase.CreateTabStripRectangleDelegate m_GetTabStripRectangle;
            public AutoHideStripBase.PaneCollection m_Panes;
        }
    }
}

