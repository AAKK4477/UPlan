namespace UPlan.Frame.DockForm
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class DockContent :Form, IDockContent
    {
        private static readonly object DockStateChangedEvent = new object();
        private DockContentHandler m_dockHandler = null;
        private int m_LDClickTimes = 0;
        private bool m_WillBeFloat = false;

        [LocalizedCategory("Category_PropertyChanged"), LocalizedDescription("Pane_DockStateChanged_Description")]
        public event EventHandler DockStateChanged;
       

        public DockContent()
        {
            this.m_dockHandler = new DockContentHandler(this, new GetPersistStringCallback(this.GetPersistString));
            this.m_dockHandler.DockStateChanged += new EventHandler(this.DockHandler_DockStateChanged);
        }

        public void Activate()
        {
            this.DockHandler.Activate();
        }

        private void DockHandler_DockStateChanged(object sender, EventArgs e)
        {
            this.OnDockStateChanged(e);
        }

        public void DockTo(UPlan.Frame.DockForm.DockPanel panel, DockStyle dockStyle)
        {
            this.DockHandler.DockTo(panel, dockStyle);
        }

        public void DockTo(DockPane paneTo, DockStyle dockStyle, int contentIndex)
        {
            this.DockHandler.DockTo(paneTo, dockStyle, contentIndex);
        }

        public void FloatAt(Rectangle floatWindowBounds)
        {
            this.DockHandler.FloatAt(floatWindowBounds);
        }

        protected virtual string GetPersistString()
        {
            return base.GetType().ToString();
        }

        public void Hide()
        {
            this.DockHandler.Hide();
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.ClientSize = new Size(0x124, 0x111);
            this.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
            base.Name = "DockContent";
            base.ResumeLayout(false);
        }

        public bool IsDockStateValid(UPlan.Frame.DockForm.DockState dockState)
        {
            return this.DockHandler.IsDockStateValid(dockState);
        }

        protected virtual void OnDockStateChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler) base.Events[DockStateChangedEvent];
            if (handler != null)
            {
                handler(this, e);
            }
            if (this.m_WillBeFloat && (0 == this.m_LDClickTimes))
            {
                this.m_LDClickTimes++;
                Point point = new Point();
                Size size = new Size();
                int x = base.ParentForm.Owner.Location.X;
                int y = base.ParentForm.Owner.Location.Y;
                int num3 = this.DockHandler.DockPanel.Location.Y;
                int width = 0;
                foreach (DockContent content in this.DockHandler.DockPanel.ContentFocusManager.ListContent)
                {
                    Form form = content;
                    if (form.Name.Equals("ProjectExplorerFrm"))
                    {
                        DockContent content2 = content;
                        width = content.Size.Width;
                        break;
                    }
                }
                int num5 = ((this.DockHandler.DockPanel.Size.Width - width) - base.Size.Width) / 2;
                int num6 = (this.DockHandler.DockPanel.Size.Height - base.Size.Height) / 2;
                point = new Point((x + width) + num5, (y + num3) + num6);
                size = new Size(450, 0x116);
                base.ParentForm.StartPosition = FormStartPosition.Manual;
                base.ParentForm.Location = point;
                base.ParentForm.ClientSize = size;
            }
            this.m_WillBeFloat = false;
        }

        private bool ShouldSerializeTabText()
        {
            return (this.DockHandler.TabText != null);
        }

        public void Show()
        {
            this.DockHandler.Show();
        }

        public void Show(UPlan.Frame.DockForm.DockPanel dockPanel)
        {
            this.DockHandler.Show(dockPanel);
        }

        public void Show(DockPane pane, IDockContent beforeContent)
        {
            this.DockHandler.Show(pane, beforeContent);
        }

        public void Show(UPlan.Frame.DockForm.DockPanel dockPanel, UPlan.Frame.DockForm.DockState dockState)
        {
            this.DockHandler.Show(dockPanel, dockState);
        }

        public void Show(UPlan.Frame.DockForm.DockPanel dockPanel, Rectangle floatWindowBounds)
        {
            this.DockHandler.Show(dockPanel, floatWindowBounds);
        }

        public void Show(DockPane previousPane, DockAlignment alignment, double proportion)
        {
            this.DockHandler.Show(previousPane, alignment, proportion);
        }

        [LocalizedCategory("Category_Docking"), LocalizedDescription("DockContent_AllowEndUserDocking_Description"), DefaultValue(true)]
        public bool AllowEndUserDocking
        {
            get
            {
                return this.DockHandler.AllowEndUserDocking;
            }
            set
            {
                this.DockHandler.AllowEndUserDocking = value;
            }
        }

        [LocalizedCategory("Category_Docking"), LocalizedDescription("DockContent_AutoHidePortion_Description"), DefaultValue((double) 0.25)]
        public double AutoHidePortion
        {
            get
            {
                return this.DockHandler.AutoHidePortion;
            }
            set
            {
                this.DockHandler.AutoHidePortion = value;
            }
        }

        [LocalizedCategory("Category_Docking"), DefaultValue(true), LocalizedDescription("DockContent_CloseButton_Description")]
        public bool CloseButton
        {
            get
            {
                return this.DockHandler.CloseButton;
            }
            set
            {
                this.DockHandler.CloseButton = value;
            }
        }

        [DefaultValue(0x3f), LocalizedDescription("DockContent_DockAreas_Description"), LocalizedCategory("Category_Docking")]
        public UPlan.Frame.DockForm.DockAreas DockAreas
        {
            get
            {
                return this.DockHandler.DockAreas;
            }
            set
            {
                this.DockHandler.DockAreas = value;
            }
        }

        [Browsable(false)]
        public DockContentHandler DockHandler
        {
            get
            {
                return this.m_dockHandler;
            }
        }

        [Browsable(false)]
        public UPlan.Frame.DockForm.DockPanel DockPanel
        {
            get
            {
                return this.DockHandler.DockPanel;
            }
            set
            {
                this.DockHandler.DockPanel = value;
            }
        }

        [Browsable(false)]
        public UPlan.Frame.DockForm.DockState DockState
        {
            get
            {
                return this.DockHandler.DockState;
            }
            set
            {
                this.DockHandler.DockState = value;
            }
        }

        [Browsable(false)]
        public DockPane FloatPane
        {
            get
            {
                return this.DockHandler.FloatPane;
            }
            set
            {
                this.DockHandler.FloatPane = value;
            }
        }

        [DefaultValue(false), LocalizedCategory("Category_Docking"), LocalizedDescription("DockContent_HideOnClose_Description")]
        public bool HideOnClose
        {
            get
            {
                return this.DockHandler.HideOnClose;
            }
            set
            {
                this.DockHandler.HideOnClose = value;
            }
        }

        [Browsable(false)]
        public bool IsActivated
        {
            get
            {
                return this.DockHandler.IsActivated;
            }
        }

        [Browsable(false)]
        public bool IsFloat
        {
            get
            {
                return this.DockHandler.IsFloat;
            }
            set
            {
                this.DockHandler.IsFloat = value;
            }
        }

        [Browsable(false)]
        public bool IsHidden
        {
            get
            {
                return this.DockHandler.IsHidden;
            }
            set
            {
                this.DockHandler.IsHidden = value;
            }
        }

        public int LDClickTimes
        {
            get
            {
                return this.m_LDClickTimes;
            }
            set
            {
                this.m_LDClickTimes = value;
            }
        }

        [Browsable(false)]
        public DockPane Pane
        {
            get
            {
                return this.DockHandler.Pane;
            }
            set
            {
                this.DockHandler.Pane = value;
            }
        }

        [Browsable(false)]
        public DockPane PanelPane
        {
            get
            {
                return this.DockHandler.PanelPane;
            }
            set
            {
                this.DockHandler.PanelPane = value;
            }
        }

        [LocalizedCategory("Category_Docking"), LocalizedDescription("DockContent_ShowHint_Description"), DefaultValue(0)]
        public UPlan.Frame.DockForm.DockState ShowHint
        {
            get
            {
                return this.DockHandler.ShowHint;
            }
            set
            {
                this.DockHandler.ShowHint = value;
            }
        }

        [LocalizedCategory("Category_Docking"), DefaultValue((string) null), LocalizedDescription("DockContent_TabPageContextMenu_Description")]
        public ContextMenu TabPageContextMenu
        {
            get
            {
                return this.DockHandler.TabPageContextMenu;
            }
            set
            {
                this.DockHandler.TabPageContextMenu = value;
            }
        }

        [LocalizedCategory("Category_Docking"), DefaultValue((string) null), LocalizedDescription("DockContent_TabPageContextMenuStrip_Description")]
        public ContextMenuStrip TabPageContextMenuStrip
        {
            get
            {
                return this.DockHandler.TabPageContextMenuStrip;
            }
            set
            {
                this.DockHandler.TabPageContextMenuStrip = value;
            }
        }

        [LocalizedDescription("DockContent_TabText_Description"), LocalizedCategory("Category_Docking"), DefaultValue((string) null), Localizable(true)]
        public string TabText
        {
            get
            {
                return this.DockHandler.TabText;
            }
            set
            {
                this.DockHandler.TabText = value;
            }
        }

        [Category("Appearance"), Localizable(true), LocalizedDescription("DockContent_ToolTipText_Description"), DefaultValue((string) null)]
        public string ToolTipText
        {
            get
            {
                return this.DockHandler.ToolTipText;
            }
            set
            {
                this.DockHandler.ToolTipText = value;
            }
        }

        [Browsable(false)]
        public UPlan.Frame.DockForm.DockState VisibleState
        {
            get
            {
                return this.DockHandler.VisibleState;
            }
            set
            {
                this.DockHandler.VisibleState = value;
            }
        }

        public bool WillBeFloat
        {
            get
            {
                return this.m_WillBeFloat;
            }
            set
            {
                this.m_WillBeFloat = value;
            }
        }
    }
}

