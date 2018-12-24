using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UPlan.Frame.Interface;
using UPlan.Common.Utility;
using UPlan.Controls.UIBase;

namespace UPlan.Frame.View
{
    public partial class MainForm : Form
    {
        private int childFormNumber = 0;
        private MainFormUIMrg m_MainFormUIManager;
        private EventForm m_EventWindow;
        private ToolStrip TempStrip;
        private OpenFileDialog openPrjDlg;
        private ToolStripMenuItemBase m_fileMenuItem;
        private RecentFileServiceLogicManager m_RecentFileServiceLogicMgr;
        public MainForm()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "窗口 " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void NewMenuItem_Click(object sender, EventArgs e)
        {
          this.CreateNewProject();
        }
        private void CreateNewProject()
        {
            //ProjectCreate类两个方法：CfgFile：设置默认的配置文件路径和NetType：网络类型（LTE）
            ProjectCreate frm = new ProjectCreate();
            LockMainForm.Lock();
            try
            {
                string str = this.m_MainFormUIManager.ProjectUIManager.CreateNewProjectCanTest(frm.CfgFile, frm.NetType);
                if (!"".Equals(str))
                {
                    MessageBoxUtil.ShowError(str);
                }
            }
            catch (Exception exception)
            {
                WriteLog.Logger.Error(exception.Message + exception.StackTrace);
            }
            finally
            {
                LockMainForm.Unlock();
                this.TempStrip.Visible = false;
            }

        }
        public EventForm EventWindow
        {
            get
            {
                return this.m_EventWindow;
            }
            set
            {
                this.m_EventWindow = value;
            }
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.OK == this.openPrjDlg.ShowDialog())
                {
                    this.m_MainFormUIManager.ProjectUIManager.OpenProjects();
                    this.TempStrip.Visible = false;
                }
            }
            catch (Exception exception)
            {
                WriteLog.Logger.Error(exception.StackTrace);
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {

            if (this.m_MainFormUIManager.ProjectUIManager.SaveCurrentProject())
            {
                string projectIPLFile = ProjectSingleton.CurrentProject.ProjectIPLFile;
                this.m_MainFormUIManager.RecentFileUIManager.AddRecentFileItem(projectIPLFile, this.m_fileMenuItem);
                this.m_RecentFileServiceLogicMgr.UpdateRecentFilesXML(projectIPLFile);
                this.m_MainFormUIManager.SetRecentlyPrjEnable(false, this.FileMenuItem);
            }
        }
        public ToolStripMenuItemBase FileMenuItem
        {
            get
            {
                return this.m_fileMenuItem;
            }
    set
            {
                this.m_fileMenuItem = value;
            }
}

        private void SaveAs_Click(object sender, EventArgs e)
        {
            if (ProjectFileManager.Instance().SaveAsProjectFile())
            {
                string projectIPLFile = ProjectSingleton.CurrentProject.ProjectIPLFile;
                this.m_MainFormUIManager.RecentFileUIManager.AddRecentFileItem(projectIPLFile, this.m_fileMenuItem);
                this.m_RecentFileServiceLogicMgr.UpdateRecentFilesXML(projectIPLFile);
            }
            this.m_MainFormUIManager.SetRecentlyPrjEnable(false, this.FileMenuItem);
        }

        private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void indexToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
