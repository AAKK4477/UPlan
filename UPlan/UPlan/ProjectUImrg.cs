
namespace UPlan.Frame.View
{
    using UPlan.Common.GlobalResource;
    using UPlan.Common.Utility;
    using UPlan.Controls.UIBase;
    using UPlan.Frame.DockForm;
    using UPlan.Frame.Interface;
    using UPlan.Frame.Model;
    using UPlan.Frame.Print;
    using UPlan.Frame.Util;
    using UPlan.GIS.Legend;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;


    public class ProjectUImrg : IProjectUImrg, IBaseService
    {
        public static string SelectPathbyCheckOpenButton;//zuochen 10.6
        public static string SelectMapNameUseForCheckOpenButton;//zuochen 10.13
        public static string SelectMapPathUseForCheckOpenButton;//zuochen 10.17
        public static bool IsCheckOpenButton=false;//zuochen
        private MainFormUIMrg m_MainFormUIManager = null;
        private LoadingFormUIMrg m_ProcessBarManager = null;

        public ProjectUImrg(MainFormUIMrg mfU)
        {
            this.m_ProcessBarManager = new LoadingFormUIMrg(mfU.MainForm);
            this.m_MainFormUIManager = mfU;
        }

        private void BindMenuStripPanel(GeneralProjectEntity project)
        {
            foreach (IMenuItemOperation operation in project.MenuItems)
            {
                ToolStripMenuItemBase topMenuItem = this.m_MainFormUIManager.MainForm.GetTopMenuItem(operation.MenuItemType);
                if (MenuItemType.File == operation.MenuItemType)
                {
                    topMenuItem.DropDownItems.Insert(topMenuItem.DropDownItems.IndexOf(this.m_MainFormUIManager.MainForm.PropertyMenuItem), operation.ToolMenuItem);
                }
                else
                {
                    topMenuItem.DropDownItems.Add(operation.ToolMenuItem);
                }
            }
        }



        private void BindToolStripPanel(GeneralProjectEntity project)
        {
            foreach (FloatingToolStrip strip in project.ToolStrips)
            {
                strip.TSPanel = this.m_MainFormUIManager.MainForm.DynamicToolPanel;
                this.m_MainFormUIManager.MainForm.DynamicToolPanel.Join(strip);
            }
        }

        public void ChangeProjectName(GeneralProjectEntity GeneralProjectEntity)
        {
            GeneralProjectEntity.MainForm.TabText = GeneralProjectEntity.Name;
        }

        //public bool CloseCurrentProject()
        //{
        //    DockContent doc = null;
        //    foreach (KeyValuePair<DockContent, IProject> pair in this.m_MainFormUIManager.MainForm.MapProject)
        //    {
        //        doc = pair.Key;
        //    }
        //    this.m_MainFormUIManager.MainForm.ContentRemoved(doc);
        //    return true;
        //}

        public bool CloseCurrentProjectUI()
        {
            if (null != ProjectSingleton.CurrentProject)
            {
                this.m_MainFormUIManager.MainForm.ClearToolStripPanel();
                this.m_MainFormUIManager.MainForm.ClearMenuStripPanel();
                this.m_MainFormUIManager.MainForm.AutoTestManager.RemoveAutoTestFunction();
                LegendManagement.RemoveCurLegend();
                List<IDockContent> list = new List<IDockContent>();
                foreach (IDockContent content in this.m_MainFormUIManager.MainForm.DockPanelInstance.Contents)
                {
                    DockContent content2 = content as DockContent;
                    bool flag = false;
                    if (null != content2)
                    {
                        flag = ("ProjectExplorerFrm".Equals(content2.Name) || "EventForm".Equals(content2.Name)) || "ResourceForm".Equals(content2.Name);
                    }
                    if (!((content2 == null) || flag))
                    {
                        list.Add(content);
                    }
                }
                foreach (IDockContent content3 in list)
                {
                    content3.DockHandler.Close();
                }
                this.m_MainFormUIManager.MainForm.ClearPages();
            }
            return true;
        }

        //private TreeNode[] ConvertTreeNodeCollectionToGroup(TreeNodeCollection col)
        //{
        //    List<TreeNode> list = new List<TreeNode>();
        //    for (int i = 0; i < col.Count; i++)
        //    {
        //        list.Add(col[i].Clone() as TreeNode);
        //    }
        //    return list.ToArray();
        //}

        public string CreateNewProjectCanTest(string systemPath, NetWorkType type)
        {
            string str = string.Empty;
            this.m_ProcessBarManager.ShowLoadingForm();
            try
            {
                IAutoTestFunction iAut = null;
                if (null != this.m_MainFormUIManager.MainForm.AutoTestManager.AutoTestSubSys)
                {
                    iAut = this.m_MainFormUIManager.MainForm.AutoTestManager.AutoTestSubSys as IAutoTestFunction;
                }
                ProjectLogicMgr.Instance.CreateProject(systemPath, type, iAut);
                MainFormSingleton.CurrentMainForm.ResourceMrgMenuItem.Visible = false;
                string templateFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PrintConfiguration.xml");
                this.m_MainFormUIManager.MainForm.PrintConfigCollection = new PrintSettingCollection(templateFile);
                //print
                PrintProject.Instance.PrintSeeting = this.m_MainFormUIManager.MainForm.PrintConfigCollection.GetDefaultPrintSetting();
                ProjectSingleton.CurrentProject.ProjectSerializeData.PrintSeeting = PrintProject.Instance.PrintSeeting;
                ProjectSingleton.CurrentProject.ProjectSerializeData.PSCollection = this.m_MainFormUIManager.MainForm.PrintConfigCollection;
            }
            catch (Exception)
            {
                this.m_ProcessBarManager.CloseLoadingForm();
                return FrameworkResource.PROJECT_CREATEPRJ_FAIL;
            }
            finally
            {
                this.m_ProcessBarManager.CloseLoadingForm();
            }
            this.BindProject();
            this.m_MainFormUIManager.MainForm.SetStatusAfterCreateProject();
            //this.StartDsSys();
            return str;
        }

        public bool BindProject()
        {
            try
            {
                GeneralProjectEntity currentProject = ProjectSingleton.CurrentProject as GeneralProjectEntity;
                this.LoadProject(currentProject);
                if (null != this.m_MainFormUIManager.MainForm.AutoTestManager.AutoTestSubSys)
                {
                    IAutoTestFunction autoTestSubSys = this.m_MainFormUIManager.MainForm.AutoTestManager.AutoTestSubSys as IAutoTestFunction;
                }
                this.m_MainFormUIManager.MainForm.MapProject.Add(currentProject.MainForm, currentProject);
                this.m_MainFormUIManager.MainForm.DockContent = currentProject.MainForm;
                currentProject.MainForm.Show(this.m_MainFormUIManager.MainForm.DockPanelInstance, DockState.Document);
                currentProject.DockPanel = this.m_MainFormUIManager.MainForm.DockPanelInstance;
                IAutoTestExcelProcess service = new XlsDataManager();
                currentProject.AppContext.RegisterService(service);
                currentProject.AppContext.RegisterService(ProjectFileManager.Instance());
            }
            catch (Exception exception)
            {
                WriteLog.Logger.Error(exception.StackTrace);
            }
            return true;
        }
        /// <summary>
        /// 序列化工程
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool DeserializeProject(string fileName)
        {
            try
            {
                string str = Path.GetFileName(fileName);
                if (this.ProjectIsOpened(fileName))
                {
                    MessageBoxUtil.ShowWarning(FrameworkResource.PROJECT_HAS_OPENED, new string[] { str });
                    return false;
                }
                string str2 = this.OpenProjectIpLFile(fileName);
                if (!"".Equals(str2))
                {
                    MessageBoxUtil.ShowError(str2);
                    return false;
                }
            }
            catch (Exception exception)
            {
                WriteLog.Logger.Error(exception.StackTrace);
                return false;
            }
            return true;
        }

        public void LoadProject(IProject iprj)
        {

            GeneralProjectEntity project = iprj as GeneralProjectEntity;
            ProjectSingleton.CurrentProject = project;
            if (null != this.m_MainFormUIManager.MainForm.AutoTestManager.AutoTestSubSys)
            {
                this.m_MainFormUIManager.MainForm.AutoTestManager.AutoTestSubSys.InitStage2();
            }
            this.m_MainFormUIManager.InitPrjExpFrmTreeView(ProjectExplorerTree.Edit);
            this.m_MainFormUIManager.InitPrjExpFrmTreeView(ProjectExplorerTree.Geo);
            this.m_MainFormUIManager.InitPrjExpFrmTreeView(ProjectExplorerTree.Home);
            this.m_MainFormUIManager.InitPrjExpFrmTreeView(ProjectExplorerTree.Wizard);
            this.m_MainFormUIManager.MainForm.PrjExpFrm.tabControl.LoadTabPage();
            this.BindToolStripPanel(project);
            this.BindMenuStripPanel(project);

        }
        public static void BindBarButtonItem(bool isEnable)
        {
            //            this.
        }
        public IBaseService Lookup(string serviceName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OpenIPLAutoLoadData(IProject prjEntity)
        {
            ProjectLogicMgr.Instance.AutoLoadData(prjEntity);
            prjEntity.AllSubSysData.Clear();
        }

        private void OpenProject(string filePath)
        {
            try
            {
                string fileName = Path.GetFileName(filePath);
                if (!ProjectFileManager.Instance().CheckProjectFileName(fileName))
                {
                    MessageBoxUtil.ShowWarning(FrameworkResource.ROJECT_FILE_ERROR, new string[] { fileName });
                }
                else if (this.DeserializeProject(filePath))
                {
                    this.m_MainFormUIManager.RecentFileUIManager.AddRecentFileItem(filePath, this.m_MainFormUIManager.MainForm.FileMenuItem);
                    this.m_MainFormUIManager.MainForm.RecentFileServiceLogicMgr.UpdateRecentFilesXML(filePath);
                    this.m_MainFormUIManager.SetRecentlyPrjEnable(false, this.m_MainFormUIManager.MainForm.FileMenuItem);
                }
            }
            catch (Exception exception)
            {
                WriteLog.Logger.Error(exception.StackTrace);
            }
        }

        public string OpenProjectIpLFile(string fileName)
        {
            LockMainForm.Lock();
            string str = string.Empty;
            try
            {
                this.m_ProcessBarManager.ShowLoadingForm();
                GeneralProjectEntity prjEntity = ProjectFileManager.Instance().OpenProjectFile(fileName) as GeneralProjectEntity;
                if (null == prjEntity)
                {
                    this.m_ProcessBarManager.CloseLoadingForm();
                    return ProjectFileManager.Instance().LastErrMsg;
                }
                this.OrderTreeView();
                this.BindProject();
                this.OpenIPLAutoLoadData(prjEntity);
                prjEntity.MainForm.TabText = prjEntity.Name;
                prjEntity.ProjectIPLFile = fileName;
                prjEntity.NeedPromptSave = false;
                this.m_MainFormUIManager.SetRecentlyPrjEnable(false, this.m_MainFormUIManager.MainForm.FileMenuItem);
                this.m_MainFormUIManager.MainForm.SwitchFrameWorkStatus(true);
                this.m_MainFormUIManager.MainForm.PrintConfigCollection = ProjectFileManager.Instance().PrjSerializeData.PSCollection;
                PrintProject.Instance.PrintSeeting = ProjectFileManager.Instance().PrjSerializeData.PrintSeeting;
            }
            catch (Exception exception)
            {
                str = FrameworkResource.PROJECT_LOAD_FAIL;
                WriteLog.Logger.Error("Open Project Failed: " + exception.StackTrace + "\nMessage:" + exception.Message);
                ProjectFileManager.Instance().UnlockProjectFile(fileName);
                return str;
            }
            finally
            {
                this.m_ProcessBarManager.CloseLoadingForm();
                LockMainForm.Unlock();
            }
            return str;
        }

        public void OpenProjects()
        {
            foreach (string str in this.m_MainFormUIManager.MainForm.OpenPrjDlgInstance.FileNames)
            {
                SelectPathbyCheckOpenButton = str;//zuochen
                this.OpenProject(str);
            }
        }

        private void OrderTreeView()
        {
            if (ProjectFileManager.Instance().PrjSerializeData.TreeTypeToTreeNodeListDic != null)
            {
                foreach (KeyValuePair<ProjectExplorerTree, List<object>> pair in ProjectFileManager.Instance().PrjSerializeData.TreeTypeToTreeNodeListDic)
                {
                    List<object> orderList = pair.Value;
                    ProjectExplorerTree key = pair.Key;
                    TreeView tv = ProjectSingleton.CurrentProject.Query(key) as TreeView;
                    this.OrderTreeView(orderList, tv);
                }
            }
        }

        private void OrderTreeView(List<object> orderList, TreeView tv)
        {
            List<TreeNode> sourceList = new List<TreeNode>();
            foreach (TreeNode node in tv.Nodes)
            {
                sourceList.Add(node);
            }
            tv.Nodes.Clear();
            List<TreeNode> list2 = this.SortTreeNodeAfterOpenPrj(orderList, sourceList);
            for (int i = 0; i < list2.Count; i++)
            {
                tv.Nodes.Add(list2[i]);
            }
        }

        private bool ProjectIsOpened(string projectName)
        {
            return ProjectFileManager.Instance().ProjectIsOpened(projectName);
        }

        public bool SaveCurrentProject()
        {
            this.SaveTreeNodeOrder(ProjectSingleton.CurrentProject.ProjectSerializeData.TreeTypeToTreeNodeListDic, this.m_MainFormUIManager.MainForm.PrjExpFrm);
            return ProjectFileManager.Instance().SaveProjectFile();
        }

        public void SaveProjectWhenClose(IProject iprj, FormClosingEventArgs a)
        {
            DialogResult result;
            GeneralProjectEntity entity = iprj as GeneralProjectEntity;
            LockMainForm.Lock();
            if (!this.m_MainFormUIManager.MainForm.IsLicClose)
            {
                result = MessageBoxUtil.ShowYesNoCancel(FrameworkResource.PROJECT_SAVE, new string[] { entity.Name });
                if (DialogResult.Yes == result)
                {
                    result = ProjectFileManager.Instance().SaveProjectFile() ? DialogResult.OK : DialogResult.Cancel;
                }
                if (DialogResult.Cancel == result)
                {
                    a.Cancel = true;
                }
            }
            else
            {
                result = MessageBoxUtil.ShowYesNo(FrameworkResource.PROJECT_SAVE, new string[] { entity.Name });
                if (DialogResult.Yes == result)
                {
                    ProjectFileManager.Instance().SaveProjectFile();
                }
            }
            LockMainForm.Unlock();
        }

        private void SaveTreeNodeOrder(Dictionary<ProjectExplorerTree, List<object>> TreeTypeToTreeNodeListDic, ProjectExplorerNew prjFrm)
        {
            TreeTypeToTreeNodeListDic.Clear();
            foreach (ITabPage page in prjFrm.tabControl.TabPages)
            {
                List<object> list = new List<object>();
                foreach (TreeNode node in page.TreeViewIns.Nodes)
                {
                    list.Add(node.Text);
                }
                TreeTypeToTreeNodeListDic.Add((ProjectExplorerTree)Enum.Parse(typeof(ProjectExplorerTree), page.Name), list);
            }
        }

        private static void SelectTreeNode(List<TreeNode> sourceList, List<TreeNode> result, object order)
        {
            foreach (TreeNode node in sourceList)
            {
                if (node.Text.Equals(order.ToString()))
                {
                    result.Add(node);
                    break;
                }
            }
        }

        private List<TreeNode> SortTreeNodeAfterOpenPrj(List<object> sortList, List<TreeNode> sourceList)
        {
            List<TreeNode> result = new List<TreeNode>();
            foreach (object obj2 in sortList)
            {
                SelectTreeNode(sourceList, result, obj2);
            }
            return result;
        }

        //private void StartDsSys()
        //{
        //}
    }
}

