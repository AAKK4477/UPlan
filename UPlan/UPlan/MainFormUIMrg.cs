namespace UPlan.Frame.View
{
    using UPlan.Controls.UIBase;
    using UPlan.Frame.DockForm;
    using UPlan.Frame.Interface;
    using UPlan.Frame.Model;
    using UPlan.Frame.Print;
    using System;
    using System.IO;
    using System.Windows.Forms;

    public class MainFormUIMrg
    {
        private EventViewUIMrg m_EventViewUIManager;
        private EventViewService m_EVService = null;
        private MainForm m_MainForm = null;
        private ProjectUImrg m_ProjectUIManager = null;
        private RecentFileUIMrg m_RecentFileUIManager;
        private StatusProcessBarUIMrg m_StatusProcessBarUIManager;
        private TreeViewUIMrg m_TreeViewUIManager = null;

        public MainFormUIMrg(MainForm mf)
        {
            this.m_MainForm = mf;
            this.m_ProjectUIManager = new ProjectUImrg(this);
            this.MainForm.FrameAppContext.RegisterService(this.ProjectUIManager);
            this.m_TreeViewUIManager = new TreeViewUIMrg(this);
            this.m_MainForm.EventWindow = new EventForm();
            this.m_EventViewUIManager = new EventViewUIMrg(this.MainForm, this.MainForm.EventWindow);
            this.m_StatusProcessBarUIManager = new StatusProcessBarUIMrg(this);
        }

        public void DisposeToolStripPanel(ToolStripPanel panel)
        {
            foreach (Control control in panel.Controls)
            {
                control.Dispose();
            }
            panel.Dispose();
        }

        public void HideDockWindows()
        {
            if (null != this.m_MainForm.LegendForm)
            {
                this.m_MainForm.LegendForm.Close();
                this.m_MainForm.LegendForm = null;
            }
            this.m_MainForm.EventWindow.Hide();
            this.m_MainForm.ResourceCpuMemForm.Hide();
        }

        public void Init()
        {
            this.m_MainForm.IsMdiContainer = true;
            this.InitEventWindow();
            this.InitEventViewServiceLogic();
            this.m_MainForm.StatusBarServiceInstance = new StatusBarService(this.StatusProcessBarUIManager);
            this.StatusProcessBarUIManager.InitStatusProgressTimer();
            this.m_MainForm.RecentFileServiceLogicMgr = new RecentFileServiceLogicManager();
            this.RecentFileUIManager = new RecentFileUIMrg(this);
            this.InitProgressTimer();
            this.InitProjectExlporer();
            this.InitResourceForm();
            this.InitToolsWindowMap();
            this.m_MainForm.RecentFileServiceLogicMgr.InitRecentFiles();
            this.m_MainForm.DynamicToolPanel.Dock = DockStyle.Top;
            this.m_MainForm.Controls.Add(this.m_MainForm.DynamicToolPanel);
            this.m_MainForm.Controls.SetChildIndex(this.m_MainForm.DynamicToolPanel, this.m_MainForm.Controls.IndexOf(this.m_MainForm.ToolBar));
            this.InitMainFormService();
            this.InitGlobeProjectServices();
            this.RecentFileUIManager.LoadRecentFile(this.m_MainForm.FileMenuItem);
            this.SetRecentlyPrjEnable(true, this.m_MainForm.FileMenuItem);
        }

        private void InitEventViewServiceLogic()
        {
            this.m_EVService = new EventViewService(this.EventViewUIManager, this.MainForm.EventWindow);
            this.m_EVService.EventViewServiceInit();
        }

        private void InitEventWindow()
        {
            this.m_MainForm.EventWindow.Tag = "m_EventWindow";
            this.m_MainForm.EventWindow.DockPanel = this.m_MainForm.DockPanelInstance;
            this.m_MainForm.EventWindow.DockState = DockState.DockBottom;
            this.m_MainForm.EventWindow.DockHandler.NotifyWindowStateChanged = new NotifyWindowStateChangeDelegate(this.NotifyWindowStateChanged);
        }

        private void InitGlobeProjectServices()
        {
            ProjectLogicMgr.Instance.GlobeServices.Add(this.m_EVService);
            ProjectLogicMgr.Instance.GlobeServices.Add(this.m_MainForm.StatusBarServiceInstance);
            ProjectLogicMgr.Instance.GlobeServices.Add(ProjectFileManager.Instance());
            ProjectLogicMgr.Instance.GlobeServices.Add(this.m_MainForm.MainFormService);
            ProjectLogicMgr.Instance.GlobeServices.Add(PrintProject.Instance);
            ProjectLogicMgr.Instance.GlobeServices.Add(PrintProject.Instance);
            ProjectLogicMgr.Instance.GlobeServices.Add(PrintProject.Instance);
            ProjectLogicMgr.Instance.GlobeServices.Add(new ProcessBarUIMrg());
            ProjectLogicMgr.Instance.GlobeServices.Add(this.m_ProjectUIManager);
        }

        private void InitMainFormService()
        {
            this.m_MainForm.MainFormService = new MainFormService(this.m_MainForm);
        }

        public void InitPrjExpFrmTreeView(ProjectExplorerTree treeType)
        {
            TreeView treeView = ProjectSingleton.CurrentProject.Query(treeType) as TreeView;
            this.RegisterDragEvent(treeView);
            this.m_MainForm.PrjExpFrm.SetTreeView(treeType, treeView);
        }

        private void InitProgressTimer()
        {
            this.EventViewUIManager.ProgressTimer.Tick += new EventHandler(this.EventViewUIManager.OnProgressTimer);
            this.EventViewUIManager.ProgressTimer.Interval = 0x3e8;
        }

        private void InitProjectExlporer()
        {
            ProjectExplorerNew new2 = new ProjectExplorerNew
            {
                Name = "ProjectExplorerFrm",
                DockPanel = this.m_MainForm.DockPanelInstance,
                DockState = DockState.DockRight
            };
            new2.DockHandler.NotifyWindowStateChanged = new NotifyWindowStateChangeDelegate(this.NotifyWindowStateChanged);
            this.m_MainForm.PrjExpFrm = new2;
        }

        private void InitResourceForm()
        {
            ResourceForm form = new ResourceForm
            {
                DockPanel = this.m_MainForm.DockPanelInstance,
                DockState = DockState.DockRight
            };
            form.DockHandler.NotifyWindowStateChanged = new NotifyWindowStateChangeDelegate(this.NotifyWindowStateChanged);
            this.m_MainForm.ResourceCpuMemForm = form;
        }

        private void InitToolsWindowMap()
        {
            this.m_MainForm.MapToolWindow2Menu.Add(this.m_MainForm.PrjExpFrm, this.m_MainForm.PrjInfoFrmMenuItem);
            this.m_MainForm.MapToolWindow2Menu.Add(this.m_MainForm.EventWindow, this.m_MainForm.EventViewerMenuItem);
            this.m_MainForm.MapToolWindow2Menu.Add(this.m_MainForm.ResourceCpuMemForm, this.m_MainForm.ResourceMrgMenuItem);
        }

        private void NotifyWindowStateChanged(IDockContent DockContent)
        {
            this.m_MainForm.MapToolWindow2Menu[DockContent as DockContent].CheckState = CheckState.Unchecked;
        }

        private void RegisterDragEvent(TreeView treeView)
        {
            treeView.ItemDrag += new ItemDragEventHandler(this.m_TreeViewUIManager.TreeView_ItemDrag);
            treeView.DragOver += new DragEventHandler(this.m_TreeViewUIManager.TreeView_DragOver);
            treeView.DragDrop += new DragEventHandler(this.m_TreeViewUIManager.TreeView_DragDrop);
            treeView.DragLeave += new EventHandler(this.m_TreeViewUIManager.TreeView_DragLeave);
            treeView.GiveFeedback += new GiveFeedbackEventHandler(this.m_TreeViewUIManager.TreeView_GiveFeedback);
        }

        public void SetRecentlyPrjEnable(bool enable, ToolStripMenuItemBase fileMenuItem)
        {
            ToolStripItem[] array = new ToolStripItem[fileMenuItem.DropDownItems.Count];
            fileMenuItem.DropDownItems.CopyTo(array, 0);
            fileMenuItem.DropDownItems.Clear();
            foreach (ToolStripItem item in array)
            {
                if ((item.Tag != null) && Path.IsPathRooted(item.Tag.ToString()))
                {
                    item.Enabled = enable;
                    if (File.Exists(item.Tag.ToString()))
                    {
                        fileMenuItem.DropDownItems.Add(item);
                    }
                }
                else
                {
                    fileMenuItem.DropDownItems.Add(item);
                }
            }
        }

        public EventViewUIMrg EventViewUIManager
        {
            get
            {
                return this.m_EventViewUIManager;
            }
            set
            {
                this.m_EventViewUIManager = value;
            }
        }

        public EventViewService EVService
        {
            get
            {
                return this.m_EVService;
            }
        }

        public MainForm MainForm
        {
            get
            {
                return this.m_MainForm;
            }
            set
            {
                this.m_MainForm = value;
            }
        }

        public ProjectUImrg ProjectUIManager
        {
            get
            {
                return this.m_ProjectUIManager;
            }
            set
            {
                this.m_ProjectUIManager = value;
            }
        }

        public RecentFileUIMrg RecentFileUIManager
        {
            get
            {
                return this.m_RecentFileUIManager;
            }
            set
            {
                this.m_RecentFileUIManager = value;
            }
        }

        public StatusProcessBarUIMrg StatusProcessBarUIManager
        {
            get
            {
                return this.m_StatusProcessBarUIManager;
            }
            set
            {
                this.m_StatusProcessBarUIManager = value;
            }
        }

        public TreeViewUIMrg TreeViewUIManager
        {
            get
            {
                return this.m_TreeViewUIManager;
            }
        }
    }
}


