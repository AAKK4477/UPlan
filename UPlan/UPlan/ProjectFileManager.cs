namespace UPlan.Frame.View
{
    using UPlan.Common.GlobalResource;
    using UPlan.Common.Utility;
    //using UPlan.Common.Validate;
    using UPlan.Frame.Interface;
    using UPlan.Frame.Model;
    //using UPlan.Frame.ProjectCompatible;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public class ProjectFileManager : IProjectManager, IBaseService
    {
        private string m_CfgPath;
        private IBaseService m_ContextService;
        private static string m_DefaultFolder = (Application.StartupPath + @"\Temp\U-Net" + Process.GetCurrentProcess().Id.ToString() + ".losses");
        private static ProjectFileManager m_Instance = null;
        private string m_LastErrMsg = string.Empty;
        private string m_OldLossPath;
        private ProjectSerializeData m_PrjSerializeData = null;
        private static readonly Dictionary<string, ProjectInfo> m_ProjectInfo = new Dictionary<string, ProjectInfo>();
        private bool m_Result;
        private SaveFileDialog m_SaveDlg = new SaveFileDialog();
        private AutoCloseLoadingForm m_SavingForm;
        private IProject project;

        static ProjectFileManager()
        {
            CreateDirectory(m_DefaultFolder);
        }

        protected ProjectFileManager()
        {
            this.m_SaveDlg.Filter = "U-Net file(*.ipl)|*.ipl";
            this.m_SaveDlg.Title = CommonResource.CONTROLS_SAVE_FILE;
            this.m_SaveDlg.InitialDirectory = ".";
            this.m_SaveDlg.DefaultExt = "ipl";
            this.m_SaveDlg.SupportMultiDottedExtensions = true;
            this.CfgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UPlanConfig.xml");
        }

        public bool CheckProjectFileName(string name)
        {
            if ((name.Length < "*.ipl".Length) || (name.Substring(name.Length - ".ipl".Length, ".ipl".Length) != ".ipl"))
            {
                this.LastErrMsg = "Invalid U-Net file name:" + name;
                return false;
            }
            return true;
        }

        public void ClearDefaultProjectLossPath()
        {
            DirectoryInfo dir = new DirectoryInfo(m_DefaultFolder);
            if (dir.Exists)
            {
                this.DeleteDirectory(dir);
            }
            this.ClearTempPath();
        }

        private void ClearTempPath()
        {
            Process[] processesByName = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (1 == processesByName.Length)
            {
                this.DeleteDirectory(new DirectoryInfo(Application.StartupPath + @"\Temp"));
            }
        }

        private void CopyFiles(DirectoryInfo dir, string destDirName)
        {
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo info in files)
            {
                string destFileName = Path.Combine(destDirName, info.Name);
                info.CopyTo(destFileName, true);
            }
        }

        private void CopySubDirectory(DirectoryInfo[] dirs, string destDirName)
        {
            foreach (DirectoryInfo info in dirs)
            {
                string str = Path.Combine(destDirName, info.Name);
                this.DirectoryCopy(info.FullName, str);
            }
        }

        private static void CreateDirectory(string destDirName)
        {
            if (!Directory.Exists(destDirName))
            {
                DirectoryInfo info = Directory.CreateDirectory(destDirName);
            }
        }

        private void CreateLossPath(string lossPath)
        {
            Directory.CreateDirectory(lossPath);
        }

        private void DeleteDirectory(DirectoryInfo dir)
        {
            dir.Create();
            this.DeleteDirFiles(dir);
            this.DeleteSubDirs(dir);
        }

        private void DeleteDirFiles(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo info in files)
            {
                info.Delete();
            }
        }

        private void DeleteLossPath(string srcLossPath, string destLossPath)
        {
            if (srcLossPath != destLossPath)
            {
                this.DeleteDirectory(new DirectoryInfo(destLossPath));
            }
        }

        private void DeleteSubDirs(DirectoryInfo dir)
        {
            DirectoryInfo[] directories = dir.GetDirectories();
            foreach (DirectoryInfo info in directories)
            {
                info.Delete(true);
            }
        }

        private void DirectoryCopy(string sourceDirName, string destDirName)
        {
            if (sourceDirName != destDirName)
            {
                DirectoryInfo dir = new DirectoryInfo(sourceDirName);
                dir.Create();
                DirectoryInfo[] directories = dir.GetDirectories();
                CreateDirectory(destDirName);
                this.CopyFiles(dir, destDirName);
                this.CopySubDirectory(directories, destDirName);
            }
        }

        private string GetFileOpenDlgFileName()
        {
            this.SetSaveDlgFileName();
            if (DialogResult.OK != this.m_SaveDlg.ShowDialog())
            {
                return string.Empty;
            }
            string fileName = Path.GetFileName(this.m_SaveDlg.FileName);
            return (this.CheckProjectFileName(fileName) ? this.m_SaveDlg.FileName : string.Empty);
        }

        private string GetLossPath(string projectFile)
        {
            StringBuilder builder = new StringBuilder(Path.GetDirectoryName(projectFile));
            builder.Append(@"\");
            builder.Append(Path.GetFileNameWithoutExtension(projectFile));
            builder.Append(".losses");
            return builder.ToString();
        }

        private string GetProjectLossPath()
        {
            string currentProjectLossPath = this.CurrentProjectLossPath;
            if (string.Empty == currentProjectLossPath)
            {
                currentProjectLossPath = this.DefaultProjectLossPath;
            }
            return currentProjectLossPath;
        }

        private long GetProjectSaveSize(string newLossPath)
        {
            if (this.m_OldLossPath == newLossPath)
            {
                return 0xa00000L;
            }
            return this.m_SavingForm.ProjectSizes;
        }

        public static ProjectFileManager Instance()
        {
            if (m_Instance == null)
            {
                m_Instance = new ProjectFileManager();
            }
            return m_Instance;
        }

        private void LockProjectFile(ProjectInfo projectInfo)
        {
            if (!m_ProjectInfo.ContainsKey(projectInfo.projectDocument))
            {
                m_ProjectInfo.Add(projectInfo.projectDocument, projectInfo);
                projectInfo.projectLock = new FileStream(projectInfo.projectDocument, FileMode.Open);
            }
        }

        public IBaseService Lookup(string serviceName)
        {
            return this.ContextService.Lookup(serviceName);
        }

        public IProject OpenProjectFile(string projectFile)
        {
            ProjectSerializeData data = null;
            IProject currentProject = null;
            this.LastErrMsg = string.Empty;
            string fileName = Path.GetFileName(projectFile);
            try
            {
                try
                {
                    using (FileStream stream = new FileStream(projectFile, FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        data = formatter.Deserialize(stream) as ProjectSerializeData;
                    }
                }
                catch (SerializationException)
                {
                    ProjectSingleton.Instance.OpenV3R5ProjectFile = true;
                    data = new ProjectConversion(projectFile).SerializeDataConvert();
                }
                this.m_PrjSerializeData = data;
                ProjectLogicMgr.Instance.CreateProject(this.CfgPath, data.NetType, null);
                currentProject = ProjectSingleton.CurrentProject;
                ProjectSingleton.CurrentProject.ProjectSerializeData.PrintSeeting = this.m_PrjSerializeData.PrintSeeting;
                ProjectSingleton.CurrentProject.ProjectSerializeData.PSCollection = this.m_PrjSerializeData.PSCollection;
                MainFormSingleton.CurrentMainForm.ResourceMrgMenuItem.Visible = false;
                ProjectInfo projectInfo = new ProjectInfo
                {
                    projectName = fileName,
                    projectDocument = projectFile,
                    projectLossPath = this.GetLossPath(projectFile)
                };
                currentProject.Name = fileName;
                currentProject.ProjectIPLFile = projectFile;
                (currentProject as GeneralProjectEntity).AllSubSysData = data.AllSubSysData;
                currentProject = ProjectDataConvert.Convert(currentProject, data.Version);
                this.CreateLossPath(projectInfo.projectLossPath);
                this.LockProjectFile(projectInfo);
            }
            catch (IOException exception)
            {
                this.LastErrMsg = "The \"" + fileName + "\" had been opened by another application!";
                WriteLog.Logger.Error("OpenProjectFile failed!:" + exception.Message);
                return null;
            }
            catch (Exception exception2)
            {
                this.LastErrMsg = "Open \"" + fileName + "\" failed!";
                WriteLog.Logger.Error("OpenProjectFile failed!:" + exception2.Message);
                return null;
            }
            return currentProject;
        }

        public bool ProjectIsOpened(string projectIPLName)
        {
            return m_ProjectInfo.ContainsKey(projectIPLName);
        }

        public DialogResult PromptSaveProject()
        {
            if (m_ProjectInfo.ContainsKey(ProjectSingleton.CurrentProject.ProjectIPLFile))
            {
                return DialogResult.OK;
            }
            if (DialogResult.Yes == MessageBoxUtil.ShowYesNo(FrameworkResource.PROJECT_SAVE_PROMPT))
            {
                if (!this.SaveProjectFile())
                {
                    return DialogResult.Abort;
                }
                return DialogResult.OK;
            }
            return DialogResult.Cancel;
        }

        public bool SaveAsProjectFile()
        {
            this.LastErrMsg = string.Empty;
            string fileOpenDlgFileName = this.GetFileOpenDlgFileName();
            if (string.Empty == fileOpenDlgFileName)
            {
                this.ShowLastError();
                return false;
            }
            if (!this.SaveCurrentProject(fileOpenDlgFileName))
            {
                this.ShowLastError();
                return false;
            }
            this.project.MainForm.TabText = this.project.Name;
            return true;
        }

        public bool SaveCurrentProject(string projectIPLName)
        {
            this.project = ProjectSingleton.CurrentProject;
            if ((this.project.ProjectIPLFile != projectIPLName) && this.ProjectIsOpened(projectIPLName))
            {
                this.LastErrMsg = "Project file \"" + projectIPLName + "\" has been opened!";
                return false;
            }
            ProjectLogicMgr.Instance.GatherSerializeData(this.project as GeneralProjectEntity);
            GC.Collect();
            this.m_OldLossPath = this.GetProjectLossPath();
            ProjectInfo projectInfo = new ProjectInfo
            {
                projectName = Path.GetFileName(projectIPLName),
                projectDocument = projectIPLName,
                projectLossPath = this.GetLossPath(projectIPLName)
            };
            this.m_SavingForm = new AutoCloseLoadingForm(this.m_OldLossPath, projectInfo.projectLossPath);
            if (!this.SaveToFile(projectInfo))
            {
                return false;
            }
            this.project.Name = projectInfo.projectName;
            this.project.ProjectIPLFile = projectInfo.projectDocument;
            this.LockProjectFile(projectInfo);
            ProjectSingleton.CurrentProject.NeedPromptSave = false;
            return true;
        }

        private void SaveProject(object obj)
        {
            ProjectInfo info2;
            IProject currentProject = ProjectSingleton.CurrentProject;
            ProjectInfo info = obj as ProjectInfo;
            m_ProjectInfo.TryGetValue(currentProject.ProjectIPLFile, out info2);
            this.UnlockProjectFile(currentProject.ProjectIPLFile);
            try
            {
                using (FileStream stream = new FileStream(info.projectDocument, FileMode.Create))
                {
                    new BinaryFormatter().Serialize(stream, (currentProject as GeneralProjectEntity).ProjectSerializeData);
                }
                GC.Collect();
                WriteLog.Logger.Debug("SaveProject::Serialize end");
                this.m_SavingForm.SaveIplFinished = true;
                this.DeleteLossPath(this.m_OldLossPath, info.projectLossPath);
                this.DirectoryCopy(this.m_OldLossPath, info.projectLossPath);
            }
            catch (IOException exception)
            {
                this.LastErrMsg = "Save \"" + info.projectLossPath + "\" failed,\nit had been locked by another application!";
                WriteLog.Logger.Error("SaveProject failed!:" + exception.Message);
                this.m_Result = false;
            }
            catch (Exception exception2)
            {
                this.LastErrMsg = "Save \"" + info.projectLossPath + "\" failed!";
                WriteLog.Logger.Error("SaveProject failed!:" + exception2.Message);
                this.m_Result = false;
            }
            if (!this.m_Result && (null != info2))
            {
                this.LockProjectFile(info2);
            }
            this.m_SavingForm.TimeToClose = true;
        }

        public bool SaveProjectFile()
        {
            IProject currentProject = ProjectSingleton.CurrentProject;
            string projectIPLFile = currentProject.ProjectIPLFile;
            if (string.Empty == projectIPLFile)
            {
                return this.SaveAsProjectFile();
            }
            this.LastErrMsg = string.Empty;
            if (!this.SaveCurrentProject(projectIPLFile))
            {
                this.ShowLastError();
                return false;
            }
            currentProject.MainForm.TabText = currentProject.Name;
            return true;
        }

        private bool SaveToFile(ProjectInfo projectInfo)
        {
            long projectSaveSize = this.GetProjectSaveSize(projectInfo.projectLossPath);
            if (!DataValidate.IsDiskSpaceEnough(projectInfo.projectLossPath, projectSaveSize))
            {
                this.LastErrMsg = "Save project failed, there is not enough space of disk!";
                return false;
            }
            this.m_Result = true;
            new Thread(new ParameterizedThreadStart(this.SaveProject)).Start(projectInfo);
            this.m_SavingForm.ShowDialog();
            return this.m_Result;
        }

        private void SetSaveDlgFileName()
        {
            string str = ProjectSingleton.CurrentProject.Name.Replace(".ipl", string.Empty);
            this.m_SaveDlg.FileName = str + ".ipl";
            int index = this.m_SaveDlg.FileName.IndexOf(' ');
            if (-1 != index)
            {
                this.m_SaveDlg.FileName = this.m_SaveDlg.FileName.Remove(index, 1);
            }
        }

        private void ShowLastError()
        {
            if (string.Empty != this.LastErrMsg)
            {
                MessageBoxUtil.ShowError(this.LastErrMsg);
            }
        }

        public void UnlockProjectFile(string projectIPLFile)
        {
            if (m_ProjectInfo.ContainsKey(projectIPLFile))
            {
                m_ProjectInfo[projectIPLFile].projectLock.Close();
                m_ProjectInfo.Remove(projectIPLFile);
            }
        }

        public string CfgPath
        {
            get
            {
                return this.m_CfgPath;
            }
            set
            {
                this.m_CfgPath = value;
            }
        }

        public IBaseService ContextService
        {
            get
            {
                return this.m_ContextService;
            }
            set
            {
                this.m_ContextService = value;
            }
        }

        public string CurrentProjectLossPath
        {
            get
            {
                if (m_ProjectInfo.ContainsKey(ProjectSingleton.CurrentProject.ProjectIPLFile))
                {
                    return m_ProjectInfo[ProjectSingleton.CurrentProject.ProjectIPLFile].projectLossPath;
                }
                return this.DefaultProjectLossPath;
            }
        }

        public string DefaultProjectLossPath
        {
            get
            {
                string destDirName = m_DefaultFolder + @"\" + ProjectSingleton.CurrentProject.Name + ".losses";
                CreateDirectory(destDirName);
                return destDirName;
            }
        }

        public string LastErrMsg
        {
            get
            {
                return this.m_LastErrMsg;
            }
            set
            {
                this.m_LastErrMsg = value;
            }
        }

        public ProjectSerializeData PrjSerializeData
        {
            get
            {
                return this.m_PrjSerializeData;
            }
        }

        private class ProjectInfo
        {
            public string projectDocument;
            public FileStream projectLock;
            public string projectLossPath;
            public string projectName;
        }
    }
}


