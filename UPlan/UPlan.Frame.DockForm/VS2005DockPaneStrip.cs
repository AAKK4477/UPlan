namespace UPlan.Frame.DockForm
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    internal class VS2005DockPaneStrip : DockPaneStripBase
    {
        private const int _DocumentButtonGapBetween = 0;
        private const int _DocumentButtonGapBottom = 4;
        private const int _DocumentButtonGapRight = 3;
        private const int _DocumentButtonGapTop = 4;
        private const int _DocumentIconGapBottom = 2;
        private const int _DocumentIconGapLeft = 8;
        private const int _DocumentIconGapRight = 0;
        private const int _DocumentIconHeight = 0x10;
        private const int _DocumentIconWidth = 0x10;
        private const int _DocumentStripGapBottom = 1;
        private const int _DocumentStripGapTop = 0;
        private const int _DocumentTabGapLeft = 3;
        private const int _DocumentTabGapRight = 3;
        private const int _DocumentTabGapTop = 3;
        private const int _DocumentTabMaxWidth = 200;
        private const int _DocumentTextGapRight = 3;
        private static Bitmap _imageButtonClose;
        private static Bitmap _imageButtonWindowList;
        private static Bitmap _imageButtonWindowListOverflow;
        private static string _toolTipClose;
        private static string _toolTipSelect;
        private const int _ToolWindowImageGapBottom = 1;
        private const int _ToolWindowImageGapLeft = 2;
        private const int _ToolWindowImageGapRight = 0;
        private const int _ToolWindowImageGapTop = 3;
        private const int _ToolWindowImageHeight = 0x10;
        private const int _ToolWindowImageWidth = 0x10;
        private const int _ToolWindowStripGapBottom = 1;
        private const int _ToolWindowStripGapLeft = 0;
        private const int _ToolWindowStripGapRight = 0;
        private const int _ToolWindowStripGapTop = 0;
        private const int _ToolWindowTabSeperatorGapBottom = 3;
        private const int _ToolWindowTabSeperatorGapTop = 3;
        private const int _ToolWindowTextGapRight = 3;
        private Font m_boldFont;
        private InertButton m_buttonClose;
        private InertButton m_buttonWindowList;
        private IContainer m_components;
        private bool m_documentTabsOverflow;
        private int m_endDisplayingTab;
        private Font m_font;
        private ContextMenuStrip m_selectMenu;
        private int m_startDisplayingTab;
        private ToolTip m_toolTip;

        public VS2005DockPaneStrip(DockPane pane) : base(pane)
        {
            this.m_startDisplayingTab = 0;
            this.m_endDisplayingTab = 0;
            this.m_documentTabsOverflow = false;
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            base.SuspendLayout();
            this.m_components = new Container();
            this.m_toolTip = new ToolTip(this.Components);
            this.m_selectMenu = new ContextMenuStrip(this.Components);
            base.ResumeLayout();
        }

        private bool CalculateDocumentTab(Rectangle rectTabStrip, ref int x, int index)
        {
            bool flag = false;
            TabVS2005 bvs = base.Tabs[index] as TabVS2005;
            bvs.MaxWidth = this.GetMaxTabWidth(index);
            int num = Math.Min(bvs.MaxWidth, DocumentTabMaxWidth);
            if (((x + num) < rectTabStrip.Right) || (index == this.StartDisplayingTab))
            {
                bvs.TabX = x;
                bvs.TabWidth = num;
                this.EndDisplayingTab = index;
            }
            else
            {
                bvs.TabX = 0;
                bvs.TabWidth = 0;
                flag = true;
            }
            x += num;
            return flag;
        }

        private void CalculateTabs()
        {
            if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
            {
                this.CalculateTabs_ToolWindow();
            }
            else
            {
                this.CalculateTabs_Document();
            }
        }

        private void CalculateTabs_Document()
        {
            int num2;
            if (this.m_startDisplayingTab >= base.Tabs.Count)
            {
                this.m_startDisplayingTab = 0;
            }
            Rectangle tabsRectangle = this.TabsRectangle;
            int x = tabsRectangle.X + (tabsRectangle.Height / 2);
            bool flag = false;
            for (num2 = this.StartDisplayingTab; num2 < base.Tabs.Count; num2++)
            {
                flag = this.CalculateDocumentTab(tabsRectangle, ref x, num2);
            }
            for (num2 = 0; num2 < this.StartDisplayingTab; num2++)
            {
                flag = this.CalculateDocumentTab(tabsRectangle, ref x, num2);
            }
            if (!flag)
            {
                this.m_startDisplayingTab = 0;
                x = tabsRectangle.X + (tabsRectangle.Height / 2);
                foreach (TabVS2005 bvs in (IEnumerable<DockPaneStripBase.Tab>) base.Tabs)
                {
                    bvs.TabX = x;
                    x += bvs.TabWidth;
                }
            }
            this.DocumentTabsOverflow = flag;
        }

        private void CalculateTabs_ToolWindow()
        {
            if ((base.Tabs.Count > 1) && !base.DockPane.IsAutoHide)
            {
                Rectangle tabStripRectangle = this.TabStripRectangle;
                int count = base.Tabs.Count;
                foreach (TabVS2005 bvs in (IEnumerable<DockPaneStripBase.Tab>) base.Tabs)
                {
                    bvs.MaxWidth = this.GetMaxTabWidth(base.Tabs.IndexOf(bvs));
                    bvs.Flag = false;
                }
                this.SetTabWidth(tabStripRectangle);
                int num2 = tabStripRectangle.X + ToolWindowStripGapLeft;
                foreach (TabVS2005 bvs in (IEnumerable<DockPaneStripBase.Tab>) base.Tabs)
                {
                    bvs.TabX = num2;
                    num2 += bvs.TabWidth;
                }
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            base.DockPane.CloseActiveContent();
        }

        private void ContextMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if (item != null)
            {
                IDockContent tag = (IDockContent) item.Tag;
                base.DockPane.ActiveContent = tag;
            }
        }

        protected internal override DockPaneStripBase.Tab CreateTab(IDockContent content)
        {
            return new TabVS2005(content);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Components.Dispose();
                if (this.m_boldFont != null)
                {
                    this.m_boldFont.Dispose();
                    this.m_boldFont = null;
                }
            }
            base.Dispose(disposing);
        }

        private void DrawActivedContent(Rectangle rect, DockPaneStripBase.Tab tab, bool full)
        {
            if (((tab.Content == base.DockPane.ActiveContent) || (base.Tabs.IndexOf(tab) == this.StartDisplayingTab)) || full)
            {
                this.ProcessActivedContent(rect);
            }
            else
            {
                this.ProcessNotFullActivedContent(rect);
            }
        }

        private void DrawActivedTabs(Rectangle rect, DockPaneStripBase.Tab tab, bool full)
        {
            if (!((((base.Tabs.IndexOf(tab) == this.EndDisplayingTab) || (base.Tabs.IndexOf(tab) == (base.Tabs.Count - 1))) || (base.Tabs[base.Tabs.IndexOf(tab) + 1].Content != base.DockPane.ActiveContent)) || full))
            {
                this.ProcessIncludeTabs(rect);
            }
            else
            {
                this.ProcessFullTabs(rect);
            }
        }

        private void DrawActivedText(Graphics g, TabVS2005 tab, Rectangle rectText, System.Drawing.Drawing2D.GraphicsPath path)
        {
            if (base.DockPane.ActiveContent == tab.Content)
            {
                g.FillPath(BrushDocumentActiveBackground, path);
                g.DrawPath(PenDocumentTabActiveBorder, path);
                if (base.DockPane.IsActiveDocumentPane)
                {
                    TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, this.BoldFont, rectText, ColorDocumentActiveText, this.DocumentTextFormat);
                }
                else
                {
                    TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, ColorDocumentActiveText, this.DocumentTextFormat);
                }
            }
            else
            {
                g.FillPath(BrushDocumentInactiveBackground, path);
                g.DrawPath(PenDocumentTabInactiveBorder, path);
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectText, ColorDocumentInactiveText, this.DocumentTextFormat);
            }
        }

        private void DrawRightToLeft(Rectangle rect)
        {
            int width = 6;
            if (this.RightToLeft == RightToLeft.Yes)
            {
                GraphicsPath.AddLine((rect.Right - (rect.Height / 2)) - (width / 2), rect.Top, rect.Left + (width / 2), rect.Top);
                GraphicsPath.AddArc(new Rectangle(rect.Left, rect.Top, width, width), 180f, 90f);
            }
            else
            {
                GraphicsPath.AddLine((rect.Left + (rect.Height / 2)) + (width / 2), rect.Top, rect.Right - (width / 2), rect.Top);
                GraphicsPath.AddArc(new Rectangle(rect.Right - width, rect.Top, width, width), -90f, 90f);
            }
        }

        private void DrawTab(Graphics g, TabVS2005 tab, Rectangle rect)
        {
            if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
            {
                this.DrawTab_ToolWindow(g, tab, rect);
            }
            else
            {
                this.DrawTab_Document(g, tab, rect);
            }
        }

        private void DrawTab_Document(Graphics g, TabVS2005 tab, Rectangle rect)
        {
            if (tab.TabWidth != 0)
            {
                Rectangle rectIcon = new Rectangle(rect.X + DocumentIconGapLeft, (((rect.Y + rect.Height) - 1) - DocumentIconGapBottom) - DocumentIconHeight, DocumentIconWidth, DocumentIconHeight);
                Rectangle rectText = rectIcon;
                this.ProcessRectInfo(ref rectIcon, ref rectText, ref rect);
                Rectangle rectangle3 = DrawHelper.RtlTransform(this, rect);
                rectText = DrawHelper.RtlTransform(this, rectText);
                rectIcon = DrawHelper.RtlTransform(this, rectIcon);
                System.Drawing.Drawing2D.GraphicsPath path = this.GetTabOutline(tab, true, false);
                this.DrawActivedText(g, tab, rectText, path);
                if (rectangle3.Contains(rectIcon) && base.DockPane.DockPanel.ShowDocumentIcon)
                {
                    g.DrawIcon(tab.Content.DockHandler.Icon, rectIcon);
                }
            }
        }

        private void DrawTab_ToolWindow(Graphics g, TabVS2005 tab, Rectangle rect)
        {
            Rectangle rectangle = new Rectangle(rect.X + ToolWindowImageGapLeft, (((rect.Y + rect.Height) - 1) - ToolWindowImageGapBottom) - ToolWindowImageHeight, ToolWindowImageWidth, ToolWindowImageHeight);
            Rectangle rectangle2 = rectangle;
            rectangle2.X += rectangle.Width + ToolWindowImageGapRight;
            rectangle2.Width = (((rect.Width - rectangle.Width) - ToolWindowImageGapLeft) - ToolWindowImageGapRight) - ToolWindowTextGapRight;
            Rectangle rectangle3 = DrawHelper.RtlTransform(this, rect);
            rectangle2 = DrawHelper.RtlTransform(this, rectangle2);
            rectangle = DrawHelper.RtlTransform(this, rectangle);
            System.Drawing.Drawing2D.GraphicsPath path = this.GetTabOutline(tab, true, false);
            if (base.DockPane.ActiveContent == tab.Content)
            {
                g.FillPath(BrushToolWindowActiveBackground, path);
                g.DrawPath(PenToolWindowTabBorder, path);
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectangle2, ColorToolWindowActiveText, this.ToolWindowTextFormat);
            }
            else
            {
                if (base.Tabs.IndexOf(base.DockPane.ActiveContent) != (base.Tabs.IndexOf(tab) + 1))
                {
                    Point point = new Point(rect.Right, rect.Top + ToolWindowTabSeperatorGapTop);
                    Point point2 = new Point(rect.Right, rect.Bottom - ToolWindowTabSeperatorGapBottom);
                    g.DrawLine(PenToolWindowTabBorder, DrawHelper.RtlTransform(this, point), DrawHelper.RtlTransform(this, point2));
                }
                TextRenderer.DrawText(g, tab.Content.DockHandler.TabText, TextFont, rectangle2, ColorToolWindowInactiveText, this.ToolWindowTextFormat);
            }
            if (rectangle3.Contains(rectangle))
            {
                g.DrawIcon(tab.Content.DockHandler.Icon, rectangle);
            }
        }

        private void DrawTabStrip(Graphics g)
        {
            if (base.Appearance == DockPane.AppearanceStyle.Document)
            {
                this.DrawTabStrip_Document(g);
            }
            else
            {
                this.DrawTabStrip_ToolWindow(g);
            }
        }

        private void DrawTabStrip_Document(Graphics g)
        {
            int count = base.Tabs.Count;
            if (count != 0)
            {
                Rectangle tabStripRectangle = this.TabStripRectangle;
                Rectangle tabsRectangle = this.TabsRectangle;
                Rectangle empty = Rectangle.Empty;
                TabVS2005 tab = null;
                g.SetClip(DrawHelper.RtlTransform(this, tabsRectangle));
                for (int i = 0; i < count; i++)
                {
                    empty = this.GetTabRectangle(i);
                    if (base.Tabs[i].Content == base.DockPane.ActiveContent)
                    {
                        tab = base.Tabs[i] as TabVS2005;
                    }
                    else if (empty.IntersectsWith(tabsRectangle))
                    {
                        this.DrawTab(g, base.Tabs[i] as TabVS2005, empty);
                    }
                }
                g.SetClip(tabStripRectangle);
                g.DrawLine(PenDocumentTabActiveBorder, tabStripRectangle.Left, tabStripRectangle.Bottom - 1, tabStripRectangle.Right, tabStripRectangle.Bottom - 1);
                g.SetClip(DrawHelper.RtlTransform(this, tabsRectangle));
                if (tab != null)
                {
                    empty = this.GetTabRectangle(base.Tabs.IndexOf(tab));
                    if (empty.IntersectsWith(tabsRectangle))
                    {
                        this.DrawTab(g, tab, empty);
                    }
                }
            }
        }

        private void DrawTabStrip_ToolWindow(Graphics g)
        {
            Rectangle tabStripRectangle = this.TabStripRectangle;
            g.DrawLine(PenToolWindowTabBorder, tabStripRectangle.Left, tabStripRectangle.Top, tabStripRectangle.Right, tabStripRectangle.Top);
            for (int i = 0; i < base.Tabs.Count; i++)
            {
                this.DrawTab(g, base.Tabs[i] as TabVS2005, this.GetTabRectangle(i));
            }
        }

        private bool EnsureDocumentTabVisible(IDockContent content, bool repaint)
        {
            int index = base.Tabs.IndexOf(content);
            TabVS2005 bvs = base.Tabs[index] as TabVS2005;
            if (bvs.TabWidth != 0)
            {
                return false;
            }
            this.StartDisplayingTab = index;
            if (repaint)
            {
                base.Invalidate();
            }
            return true;
        }

        protected internal override void EnsureTabVisible(IDockContent content)
        {
            if ((base.Appearance == DockPane.AppearanceStyle.Document) && base.Tabs.Contains(content))
            {
                this.CalculateTabs();
                this.EnsureDocumentTabVisible(content, true);
            }
        }

        private int GetMaxTabWidth(int index)
        {
            if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
            {
                return this.GetMaxTabWidth_ToolWindow(index);
            }
            return this.GetMaxTabWidth_Document(index);
        }

        private int GetMaxTabWidth_Document(int index)
        {
            IDockContent content = base.Tabs[index].Content;
            int height = this.GetTabRectangle_Document(index).Height;
            Size size = TextRenderer.MeasureText(content.DockHandler.TabText, this.BoldFont, new Size(DocumentTabMaxWidth, height), this.DocumentTextFormat);
            if (base.DockPane.DockPanel.ShowDocumentIcon)
            {
                return ((((size.Width + DocumentIconWidth) + DocumentIconGapLeft) + DocumentIconGapRight) + DocumentTextGapRight);
            }
            return ((size.Width + DocumentIconGapLeft) + DocumentTextGapRight);
        }

        private int GetMaxTabWidth_ToolWindow(int index)
        {
            Size size = TextRenderer.MeasureText(base.Tabs[index].Content.DockHandler.TabText, TextFont);
            return ((((ToolWindowImageWidth + size.Width) + ToolWindowImageGapLeft) + ToolWindowImageGapRight) + ToolWindowTextGapRight);
        }

        protected internal override System.Drawing.Drawing2D.GraphicsPath GetOutline(int index)
        {
            if (base.Appearance == DockPane.AppearanceStyle.Document)
            {
                return this.GetOutline_Document(index);
            }
            return this.GetOutline_ToolWindow(index);
        }

        private System.Drawing.Drawing2D.GraphicsPath GetOutline_Document(int index)
        {
            Rectangle tabRectangle = this.GetTabRectangle(index);
            tabRectangle.X -= tabRectangle.Height / 2;
            tabRectangle.Intersect(this.TabsRectangle);
            tabRectangle = base.RectangleToScreen(DrawHelper.RtlTransform(this, tabRectangle));
            int top = tabRectangle.Top;
            Rectangle rectangle2 = base.DockPane.RectangleToScreen(base.DockPane.ClientRectangle);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.GraphicsPath addingPath = this.GetTabOutline_Document(base.Tabs[index], true, true, true);
            path.AddPath(addingPath, true);
            path.AddLine(tabRectangle.Right, tabRectangle.Bottom, rectangle2.Right, tabRectangle.Bottom);
            path.AddLine(rectangle2.Right, tabRectangle.Bottom, rectangle2.Right, rectangle2.Bottom);
            path.AddLine(rectangle2.Right, rectangle2.Bottom, rectangle2.Left, rectangle2.Bottom);
            path.AddLine(rectangle2.Left, rectangle2.Bottom, rectangle2.Left, tabRectangle.Bottom);
            path.AddLine(rectangle2.Left, tabRectangle.Bottom, tabRectangle.Right, tabRectangle.Bottom);
            return path;
        }

        private System.Drawing.Drawing2D.GraphicsPath GetOutline_ToolWindow(int index)
        {
            Rectangle tabRectangle = this.GetTabRectangle(index);
            tabRectangle.Intersect(this.TabsRectangle);
            tabRectangle = base.RectangleToScreen(DrawHelper.RtlTransform(this, tabRectangle));
            int top = tabRectangle.Top;
            Rectangle rectangle2 = base.DockPane.RectangleToScreen(base.DockPane.ClientRectangle);
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Drawing2D.GraphicsPath addingPath = this.GetTabOutline(base.Tabs[index], true, true);
            path.AddPath(addingPath, true);
            path.AddLine(tabRectangle.Left, tabRectangle.Top, rectangle2.Left, tabRectangle.Top);
            path.AddLine(rectangle2.Left, tabRectangle.Top, rectangle2.Left, rectangle2.Top);
            path.AddLine(rectangle2.Left, rectangle2.Top, rectangle2.Right, rectangle2.Top);
            path.AddLine(rectangle2.Right, rectangle2.Top, rectangle2.Right, tabRectangle.Top);
            path.AddLine(rectangle2.Right, tabRectangle.Top, tabRectangle.Right, tabRectangle.Top);
            return path;
        }

        private System.Drawing.Drawing2D.GraphicsPath GetTabOutline(DockPaneStripBase.Tab tab, bool rtlTransform, bool toScreen)
        {
            if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
            {
                return this.GetTabOutline_ToolWindow(tab, rtlTransform, toScreen);
            }
            return this.GetTabOutline_Document(tab, rtlTransform, toScreen, false);
        }

        private System.Drawing.Drawing2D.GraphicsPath GetTabOutline_Document(DockPaneStripBase.Tab tab, bool rtlTransform, bool toScreen, bool full)
        {
            GraphicsPath.Reset();
            Rectangle tabRectangle = this.GetTabRectangle(base.Tabs.IndexOf(tab));
            if (rtlTransform)
            {
                tabRectangle = DrawHelper.RtlTransform(this, tabRectangle);
            }
            if (toScreen)
            {
                tabRectangle = base.RectangleToScreen(tabRectangle);
            }
            this.DrawActivedContent(tabRectangle, tab, full);
            this.DrawRightToLeft(tabRectangle);
            this.DrawActivedTabs(tabRectangle, tab, full);
            return GraphicsPath;
        }

        private System.Drawing.Drawing2D.GraphicsPath GetTabOutline_ToolWindow(DockPaneStripBase.Tab tab, bool rtlTransform, bool toScreen)
        {
            Rectangle tabRectangle = this.GetTabRectangle(base.Tabs.IndexOf(tab));
            if (rtlTransform)
            {
                tabRectangle = DrawHelper.RtlTransform(this, tabRectangle);
            }
            if (toScreen)
            {
                tabRectangle = base.RectangleToScreen(tabRectangle);
            }
            DrawHelper.GetRoundedCornerTab(GraphicsPath, tabRectangle, false);
            return GraphicsPath;
        }

        private Rectangle GetTabRectangle(int index)
        {
            if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
            {
                return this.GetTabRectangle_ToolWindow(index);
            }
            return this.GetTabRectangle_Document(index);
        }

        private Rectangle GetTabRectangle_Document(int index)
        {
            Rectangle tabStripRectangle = this.TabStripRectangle;
            TabVS2005 bvs = (TabVS2005) base.Tabs[index];
            return new Rectangle(bvs.TabX, tabStripRectangle.Y + DocumentTabGapTop, bvs.TabWidth, tabStripRectangle.Height - DocumentTabGapTop);
        }

        private Rectangle GetTabRectangle_ToolWindow(int index)
        {
            Rectangle tabStripRectangle = this.TabStripRectangle;
            TabVS2005 bvs = (TabVS2005) base.Tabs[index];
            return new Rectangle(bvs.TabX, tabStripRectangle.Y, bvs.TabWidth, tabStripRectangle.Height);
        }

        protected internal override int HitTest(Point ptMouse)
        {
            Rectangle tabsRectangle = this.TabsRectangle;
            if (this.TabsRectangle.Contains(ptMouse))
            {
                foreach (DockPaneStripBase.Tab tab in (IEnumerable<DockPaneStripBase.Tab>) base.Tabs)
                {
                    if (this.GetTabOutline(tab, true, false).IsVisible(ptMouse))
                    {
                        return base.Tabs.IndexOf(tab);
                    }
                }
            }
            return -1;
        }

        protected internal override int MeasureHeight()
        {
            if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
            {
                return this.MeasureHeight_ToolWindow();
            }
            return this.MeasureHeight_Document();
        }

        private int MeasureHeight_Document()
        {
            return ((Math.Max((int) (TextFont.Height + DocumentTabGapTop), (int) ((this.ButtonClose.Height + DocumentButtonGapTop) + DocumentButtonGapBottom)) + DocumentStripGapBottom) + DocumentStripGapTop);
        }

        private int MeasureHeight_ToolWindow()
        {
            if (base.DockPane.IsAutoHide || (base.Tabs.Count <= 1))
            {
                return 0;
            }
            return ((Math.Max(TextFont.Height, (ToolWindowImageHeight + ToolWindowImageGapTop) + ToolWindowImageGapBottom) + ToolWindowStripGapTop) + ToolWindowStripGapBottom);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            if (base.Appearance != DockPane.AppearanceStyle.Document)
            {
                base.OnLayout(levent);
            }
            else
            {
                Rectangle tabStripRectangle = this.TabStripRectangle;
                int width = this.ButtonClose.Image.Width;
                int height = this.ButtonClose.Image.Height;
                int num3 = (tabStripRectangle.Height - DocumentButtonGapTop) - DocumentButtonGapBottom;
                if (height < num3)
                {
                    width *= num3 / height;
                    height = num3;
                }
                Size size = new Size(width, height);
                int x = (((tabStripRectangle.X + tabStripRectangle.Width) - DocumentTabGapLeft) - DocumentButtonGapRight) - width;
                int y = tabStripRectangle.Y + DocumentButtonGapTop;
                Point location = new Point(x, y);
                this.ButtonClose.Bounds = DrawHelper.RtlTransform(this, new Rectangle(location, size));
                location.Offset(-(DocumentButtonGapBetween + width), 0);
                this.ButtonWindowList.Bounds = DrawHelper.RtlTransform(this, new Rectangle(location, size));
                this.OnRefreshChanges();
                base.OnLayout(levent);
            }
        }

        protected override void OnMouseHover(EventArgs e)
        {
            int num = this.HitTest(base.PointToClient(Control.MousePosition));
            string caption = string.Empty;
            base.OnMouseHover(e);
            if (num != -1)
            {
                TabVS2005 bvs = base.Tabs[num] as TabVS2005;
                if (!string.IsNullOrEmpty(bvs.Content.DockHandler.ToolTipText))
                {
                    caption = bvs.Content.DockHandler.ToolTipText;
                }
                else if (bvs.MaxWidth > bvs.TabWidth)
                {
                    caption = bvs.Content.DockHandler.TabText;
                }
            }
            if (this.m_toolTip.GetToolTip(this) != caption)
            {
                this.m_toolTip.Active = false;
                this.m_toolTip.SetToolTip(this, caption);
                this.m_toolTip.Active = true;
            }
            base.ResetMouseEventArgs();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (base.Appearance == DockPane.AppearanceStyle.Document)
            {
                if (this.BackColor != SystemColors.Control)
                {
                    this.BackColor = SystemColors.Control;
                }
            }
            else if (this.BackColor != SystemColors.ControlLight)
            {
                this.BackColor = SystemColors.ControlLight;
            }
            base.OnPaint(e);
            this.CalculateTabs();
            if (((base.Appearance == DockPane.AppearanceStyle.Document) && (base.DockPane.ActiveContent != null)) && this.EnsureDocumentTabVisible(base.DockPane.ActiveContent, false))
            {
                this.CalculateTabs();
            }
            this.DrawTabStrip(e.Graphics);
        }

        protected override void OnRefreshChanges()
        {
            this.SetInertButtons();
            base.Invalidate();
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);
            base.PerformLayout();
        }

        private void ProcessActivedContent(Rectangle rect)
        {
            int num = 6;
            if (this.RightToLeft == RightToLeft.Yes)
            {
                GraphicsPath.AddLine(rect.Right, rect.Bottom, rect.Right + (rect.Height / 2), rect.Bottom);
                GraphicsPath.AddLine(rect.Right + (rect.Height / 2), rect.Bottom, (rect.Right - (rect.Height / 2)) + (num / 2), rect.Top + (num / 2));
            }
            else
            {
                GraphicsPath.AddLine(rect.Left, rect.Bottom, rect.Left - (rect.Height / 2), rect.Bottom);
                GraphicsPath.AddLine(rect.Left - (rect.Height / 2), rect.Bottom, (rect.Left + (rect.Height / 2)) - (num / 2), rect.Top + (num / 2));
            }
        }

        private void ProcessFullTabs(Rectangle rect)
        {
            int num = 6;
            if (this.RightToLeft == RightToLeft.Yes)
            {
                GraphicsPath.AddLine(rect.Left, rect.Top + (num / 2), rect.Left, rect.Bottom);
            }
            else
            {
                GraphicsPath.AddLine(rect.Right, rect.Top + (num / 2), rect.Right, rect.Bottom);
            }
        }

        private void ProcessIncludeTabs(Rectangle rect)
        {
            int num = 6;
            if (this.RightToLeft == RightToLeft.Yes)
            {
                GraphicsPath.AddLine(rect.Left, rect.Top + (num / 2), rect.Left, rect.Top + (rect.Height / 2));
                GraphicsPath.AddLine(rect.Left, rect.Top + (rect.Height / 2), rect.Left + (rect.Height / 2), rect.Bottom);
            }
            else
            {
                GraphicsPath.AddLine(rect.Right, rect.Top + (num / 2), rect.Right, rect.Top + (rect.Height / 2));
                GraphicsPath.AddLine(rect.Right, rect.Top + (rect.Height / 2), rect.Right - (rect.Height / 2), rect.Bottom);
            }
        }

        private void ProcessNotFullActivedContent(Rectangle rect)
        {
            int num = 6;
            if (this.RightToLeft == RightToLeft.Yes)
            {
                GraphicsPath.AddLine(rect.Right, rect.Bottom, rect.Right, rect.Bottom - (rect.Height / 2));
                GraphicsPath.AddLine(rect.Right, rect.Bottom - (rect.Height / 2), (rect.Right - (rect.Height / 2)) + (num / 2), rect.Top + (num / 2));
            }
            else
            {
                GraphicsPath.AddLine(rect.Left, rect.Bottom, rect.Left, rect.Bottom - (rect.Height / 2));
                GraphicsPath.AddLine(rect.Left, rect.Bottom - (rect.Height / 2), (rect.Left + (rect.Height / 2)) - (num / 2), rect.Top + (num / 2));
            }
        }

        private void ProcessRectInfo(ref Rectangle rectIcon, ref Rectangle rectText, ref Rectangle rect)
        {
            if (base.DockPane.DockPanel.ShowDocumentIcon)
            {
                rectText.X += rectIcon.Width + DocumentIconGapRight;
                rectText.Y = rect.Y;
                rectText.Width = (((rect.Width - rectIcon.Width) - DocumentIconGapLeft) - DocumentIconGapRight) - DocumentTextGapRight;
                rectText.Height = rect.Height;
            }
            else
            {
                rectText.Width = (rect.Width - DocumentIconGapLeft) - DocumentTextGapRight;
            }
        }

        private void SetAvgWidth(int remainedTabs, int totalWidth, int totalAllocatedWidth, int averageWidth)
        {
            if (remainedTabs > 0)
            {
                int num = (totalWidth - totalAllocatedWidth) - (averageWidth * remainedTabs);
                foreach (TabVS2005 bvs in (IEnumerable<DockPaneStripBase.Tab>) base.Tabs)
                {
                    if (!bvs.Flag)
                    {
                        bvs.Flag = true;
                        if (num > 0)
                        {
                            bvs.TabWidth = averageWidth + 1;
                            num--;
                        }
                        else
                        {
                            bvs.TabWidth = averageWidth;
                        }
                    }
                }
            }
        }

        private void SetInertButtons()
        {
            if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
            {
                if (this.m_buttonClose != null)
                {
                    this.m_buttonClose.Left = -this.m_buttonClose.Width;
                }
                if (this.m_buttonWindowList != null)
                {
                    this.m_buttonWindowList.Left = -this.m_buttonWindowList.Width;
                }
            }
            else
            {
                bool flag = (base.DockPane.ActiveContent == null) || base.DockPane.ActiveContent.DockHandler.CloseButton;
                this.ButtonClose.Enabled = flag;
                this.ButtonClose.RefreshChanges();
                this.ButtonWindowList.RefreshChanges();
            }
        }

        private void SetTabWidth(Rectangle rectTabStrip)
        {
            int count = base.Tabs.Count;
            bool flag = true;
            int totalWidth = (rectTabStrip.Width - ToolWindowStripGapLeft) - ToolWindowStripGapRight;
            int totalAllocatedWidth = 0;
            int averageWidth = totalWidth / count;
            int remainedTabs = count;
            flag = true;
            while (flag && (remainedTabs > 0))
            {
                flag = false;
                foreach (TabVS2005 bvs in (IEnumerable<DockPaneStripBase.Tab>) base.Tabs)
                {
                    if (!bvs.Flag && (bvs.MaxWidth <= averageWidth))
                    {
                        bvs.Flag = true;
                        bvs.TabWidth = bvs.MaxWidth;
                        totalAllocatedWidth += bvs.TabWidth;
                        flag = true;
                        remainedTabs--;
                    }
                }
                if (remainedTabs != 0)
                {
                    averageWidth = (totalWidth - totalAllocatedWidth) / remainedTabs;
                }
            }
            this.SetAvgWidth(remainedTabs, totalWidth, totalAllocatedWidth, averageWidth);
        }

        private void WindowList_Click(object sender, EventArgs e)
        {
            int x = 0;
            int y = this.ButtonWindowList.Location.Y + this.ButtonWindowList.Height;
            this.SelectMenu.Items.Clear();
            foreach (TabVS2005 bvs in (IEnumerable<DockPaneStripBase.Tab>) base.Tabs)
            {
                IDockContent content = bvs.Content;
                ToolStripItem item = this.SelectMenu.Items.Add(content.DockHandler.TabText, content.DockHandler.Icon.ToBitmap());
                item.Tag = bvs.Content;
                item.Click += new EventHandler(this.ContextMenuItem_Click);
            }
            this.SelectMenu.Show(this.ButtonWindowList, x, y);
        }

        private Font BoldFont
        {
            get
            {
                if (base.IsDisposed)
                {
                    return null;
                }
                if (this.m_boldFont == null)
                {
                    this.m_font = TextFont;
                    this.m_boldFont = new Font(TextFont, FontStyle.Bold);
                }
                else if (this.m_font != TextFont)
                {
                    this.m_boldFont.Dispose();
                    this.m_font = TextFont;
                    this.m_boldFont = new Font(TextFont, FontStyle.Bold);
                }
                return this.m_boldFont;
            }
        }

        private static Brush BrushDocumentActiveBackground
        {
            get
            {
                return SystemBrushes.ControlLightLight;
            }
        }

        private static Brush BrushDocumentInactiveBackground
        {
            get
            {
                return SystemBrushes.ControlLight;
            }
        }

        private static Brush BrushToolWindowActiveBackground
        {
            get
            {
                return SystemBrushes.Control;
            }
        }

        private InertButton ButtonClose
        {
            get
            {
                if (this.m_buttonClose == null)
                {
                    this.m_buttonClose = new InertButton(ImageButtonClose, ImageButtonClose);
                    this.m_toolTip.SetToolTip(this.m_buttonClose, ToolTipClose + " Window");
                    this.m_buttonClose.Click += new EventHandler(this.Close_Click);
                    base.Controls.Add(this.m_buttonClose);
                }
                return this.m_buttonClose;
            }
        }

        private InertButton ButtonWindowList
        {
            get
            {
                if (this.m_buttonWindowList == null)
                {
                    this.m_buttonWindowList = new InertButton(ImageButtonWindowList, ImageButtonWindowListOverflow);
                    this.m_toolTip.SetToolTip(this.m_buttonWindowList, ToolTipSelect);
                    this.m_buttonWindowList.Click += new EventHandler(this.WindowList_Click);
                    base.Controls.Add(this.m_buttonWindowList);
                }
                return this.m_buttonWindowList;
            }
        }

        private static Color ColorDocumentActiveText
        {
            get
            {
                return SystemColors.ControlText;
            }
        }

        private static Color ColorDocumentInactiveText
        {
            get
            {
                return SystemColors.ControlText;
            }
        }

        private static Color ColorToolWindowActiveText
        {
            get
            {
                return SystemColors.ControlText;
            }
        }

        private static Color ColorToolWindowInactiveText
        {
            get
            {
                return SystemColors.ControlDarkDark;
            }
        }

        private IContainer Components
        {
            get
            {
                return this.m_components;
            }
        }

        private static int DocumentButtonGapBetween
        {
            get
            {
                return 0;
            }
        }

        private static int DocumentButtonGapBottom
        {
            get
            {
                return 4;
            }
        }

        private static int DocumentButtonGapRight
        {
            get
            {
                return 3;
            }
        }

        private static int DocumentButtonGapTop
        {
            get
            {
                return 4;
            }
        }

        private static int DocumentIconGapBottom
        {
            get
            {
                return 2;
            }
        }

        private static int DocumentIconGapLeft
        {
            get
            {
                return 8;
            }
        }

        private static int DocumentIconGapRight
        {
            get
            {
                return 0;
            }
        }

        private static int DocumentIconHeight
        {
            get
            {
                return 0x10;
            }
        }

        private static int DocumentIconWidth
        {
            get
            {
                return 0x10;
            }
        }

        private static int DocumentStripGapBottom
        {
            get
            {
                return 1;
            }
        }

        private static int DocumentStripGapTop
        {
            get
            {
                return 0;
            }
        }

        private static int DocumentTabGapLeft
        {
            get
            {
                return 3;
            }
        }

        private static int DocumentTabGapRight
        {
            get
            {
                return 3;
            }
        }

        private static int DocumentTabGapTop
        {
            get
            {
                return 3;
            }
        }

        private static int DocumentTabMaxWidth
        {
            get
            {
                return 200;
            }
        }

        private bool DocumentTabsOverflow
        {
            set
            {
                if (this.m_documentTabsOverflow != value)
                {
                    this.m_documentTabsOverflow = value;
                    if (value)
                    {
                        this.ButtonWindowList.ImageCategory = 1;
                    }
                    else
                    {
                        this.ButtonWindowList.ImageCategory = 0;
                    }
                }
            }
        }

        private TextFormatFlags DocumentTextFormat
        {
            get
            {
                TextFormatFlags flags = TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.PathEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                if (this.RightToLeft == RightToLeft.Yes)
                {
                    return (flags | TextFormatFlags.RightToLeft);
                }
                return flags;
            }
        }

        private static int DocumentTextGapRight
        {
            get
            {
                return 3;
            }
        }

        private int EndDisplayingTab
        {
            get
            {
                return this.m_endDisplayingTab;
            }
            set
            {
                this.m_endDisplayingTab = value;
            }
        }

        private static System.Drawing.Drawing2D.GraphicsPath GraphicsPath
        {
            get
            {
                return VS2005AutoHideStrip.GraphicsPath;
            }
        }

        private static Bitmap ImageButtonClose
        {
            get
            {
                if (_imageButtonClose == null)
                {
                    _imageButtonClose = Resources.DockPane_Close;
                }
                return _imageButtonClose;
            }
        }

        private static Bitmap ImageButtonWindowList
        {
            get
            {
                if (_imageButtonWindowList == null)
                {
                    _imageButtonWindowList = Resources.DockPane_Option;
                }
                return _imageButtonWindowList;
            }
        }

        private static Bitmap ImageButtonWindowListOverflow
        {
            get
            {
                if (_imageButtonWindowListOverflow == null)
                {
                    _imageButtonWindowListOverflow = Resources.DockPane_OptionOverflow;
                }
                return _imageButtonWindowListOverflow;
            }
        }

        private static Pen PenDocumentTabActiveBorder
        {
            get
            {
                return SystemPens.ControlDarkDark;
            }
        }

        private static Pen PenDocumentTabInactiveBorder
        {
            get
            {
                return SystemPens.GrayText;
            }
        }

        private static Pen PenToolWindowTabBorder
        {
            get
            {
                return SystemPens.GrayText;
            }
        }

        private ContextMenuStrip SelectMenu
        {
            get
            {
                return this.m_selectMenu;
            }
        }

        private int StartDisplayingTab
        {
            get
            {
                return this.m_startDisplayingTab;
            }
            set
            {
                this.m_startDisplayingTab = value;
                base.Invalidate();
            }
        }

        private Rectangle TabsRectangle
        {
            get
            {
                if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
                {
                    return this.TabStripRectangle;
                }
                Rectangle tabStripRectangle = this.TabStripRectangle;
                int x = tabStripRectangle.X;
                int y = tabStripRectangle.Y;
                int width = tabStripRectangle.Width;
                int height = tabStripRectangle.Height;
                x += DocumentTabGapLeft;
                return new Rectangle(x, y, width - (((((DocumentTabGapLeft + DocumentTabGapRight) + DocumentButtonGapRight) + this.ButtonClose.Width) + this.ButtonWindowList.Width) + (2 * DocumentButtonGapBetween)), height);
            }
        }

        private Rectangle TabStripRectangle
        {
            get
            {
                if (base.Appearance == DockPane.AppearanceStyle.Document)
                {
                    return this.TabStripRectangle_Document;
                }
                return this.TabStripRectangle_ToolWindow;
            }
        }

        private Rectangle TabStripRectangle_Document
        {
            get
            {
                Rectangle clientRectangle = base.ClientRectangle;
                return new Rectangle(clientRectangle.X, clientRectangle.Top + DocumentStripGapTop, clientRectangle.Width, (clientRectangle.Height - DocumentStripGapTop) - ToolWindowStripGapBottom);
            }
        }

        private Rectangle TabStripRectangle_ToolWindow
        {
            get
            {
                Rectangle clientRectangle = base.ClientRectangle;
                return new Rectangle(clientRectangle.X, clientRectangle.Top + ToolWindowStripGapTop, clientRectangle.Width, (clientRectangle.Height - ToolWindowStripGapTop) - ToolWindowStripGapBottom);
            }
        }

        private static Font TextFont
        {
            get
            {
                return SystemInformation.MenuFont;
            }
        }

        private static string ToolTipClose
        {
            get
            {
                if (_toolTipClose == null)
                {
                    _toolTipClose = Strings.DockPaneStrip_ToolTipClose;
                }
                return _toolTipClose;
            }
        }

        private static string ToolTipSelect
        {
            get
            {
                if (_toolTipSelect == null)
                {
                    _toolTipSelect = Strings.DockPaneStrip_ToolTipWindowList;
                }
                return _toolTipSelect;
            }
        }

        private static int ToolWindowImageGapBottom
        {
            get
            {
                return 1;
            }
        }

        private static int ToolWindowImageGapLeft
        {
            get
            {
                return 2;
            }
        }

        private static int ToolWindowImageGapRight
        {
            get
            {
                return 0;
            }
        }

        private static int ToolWindowImageGapTop
        {
            get
            {
                return 3;
            }
        }

        private static int ToolWindowImageHeight
        {
            get
            {
                return 0x10;
            }
        }

        private static int ToolWindowImageWidth
        {
            get
            {
                return 0x10;
            }
        }

        private static int ToolWindowStripGapBottom
        {
            get
            {
                return 1;
            }
        }

        private static int ToolWindowStripGapLeft
        {
            get
            {
                return 0;
            }
        }

        private static int ToolWindowStripGapRight
        {
            get
            {
                return 0;
            }
        }

        private static int ToolWindowStripGapTop
        {
            get
            {
                return 0;
            }
        }

        private static int ToolWindowTabSeperatorGapBottom
        {
            get
            {
                return 3;
            }
        }

        private static int ToolWindowTabSeperatorGapTop
        {
            get
            {
                return 3;
            }
        }

        private TextFormatFlags ToolWindowTextFormat
        {
            get
            {
                TextFormatFlags flags = TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                if (this.RightToLeft == RightToLeft.Yes)
                {
                    return ((flags | TextFormatFlags.RightToLeft) | TextFormatFlags.Right);
                }
                return flags;
            }
        }

        private static int ToolWindowTextGapRight
        {
            get
            {
                return 3;
            }
        }

        private sealed class InertButton : InertButtonBase
        {
            private Bitmap m_image0;
            private Bitmap m_image1;
            private int m_imageCategory = 0;

            public InertButton(Bitmap image0, Bitmap image1)
            {
                this.m_image0 = image0;
                this.m_image1 = image1;
            }

            protected override void OnRefreshChanges()
            {
                if (VS2005DockPaneStrip.ColorDocumentActiveText != this.ForeColor)
                {
                    this.ForeColor = VS2005DockPaneStrip.ColorDocumentActiveText;
                    base.Invalidate();
                }
            }

            public override Bitmap Image
            {
                get
                {
                    return ((this.ImageCategory == 0) ? this.m_image0 : this.m_image1);
                }
            }

            public int ImageCategory
            {
                get
                {
                    return this.m_imageCategory;
                }
                set
                {
                    if (this.m_imageCategory != value)
                    {
                        this.m_imageCategory = value;
                        base.Invalidate();
                    }
                }
            }
        }

        private class TabVS2005 : DockPaneStripBase.Tab
        {
            private bool m_flag;
            private int m_maxWidth;
            private int m_tabWidth;
            private int m_tabX;

            public TabVS2005(IDockContent content) : base(content)
            {
            }

            protected internal bool Flag
            {
                get
                {
                    return this.m_flag;
                }
                set
                {
                    this.m_flag = value;
                }
            }

            public int MaxWidth
            {
                get
                {
                    return this.m_maxWidth;
                }
                set
                {
                    this.m_maxWidth = value;
                }
            }

            public int TabWidth
            {
                get
                {
                    return this.m_tabWidth;
                }
                set
                {
                    this.m_tabWidth = value;
                }
            }

            public int TabX
            {
                get
                {
                    return this.m_tabX;
                }
                set
                {
                    this.m_tabX = value;
                }
            }
        }
    }
}

