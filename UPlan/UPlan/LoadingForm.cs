namespace UPlan.Frame.View
{
    using UPlan.Common.GlobalResource;
    using UPlan.Frame.Interface;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;

    public class LoadingForm : Form
    {
        private IContainer components = null;
        private GroupBox groupBox1;
        private Label lblLoading;
        private const int LOADING_TAIL_NUMBER = 5;
        private ProgressBar loadProgress;
        private int m_CurLoadNumber;
        private bool m_EventUnload;
        private LoadingFormUIMrg M_LoadingFormUIManager = null;
        private static List<string> m_NotLoadedDlls = new List<string>();
        private Form m_ParentForm;
        private System.Windows.Forms.Timer m_Timer;

        static LoadingForm()
        {
            GetTotalDlls();
        }

        public LoadingForm(Form parent, LoadingFormUIMrg uiM)
        {
            this.InitializeComponent();
            base.Location = new System.Drawing.Point(parent.Location.X + ((parent.Width - base.Width) >> 1), parent.Location.Y + ((parent.Height - base.Height) >> 1));
            this.m_ParentForm = parent;
            this.m_Timer = new System.Windows.Forms.Timer();
            this.m_Timer.Tick += new EventHandler(this.OnTimer);
            this.m_Timer.Interval = 0x3e8;
            this.M_LoadingFormUIManager = uiM;
        }

        private void AssemblyLoadHandler(object sender, AssemblyLoadEventArgs args)
        {
            string name = args.LoadedAssembly.GetName().Name;
            if (IsUserDll(name))
            {
                m_NotLoadedDlls.Remove(name);
                this.m_CurLoadNumber++;
            }
        }

       

        private static void GetTotalDlls()
        {
            string[] files = Directory.GetFiles(Application.StartupPath, "*.dll");
            foreach (string str in files)
            {
                m_NotLoadedDlls.Add(Path.GetFileNameWithoutExtension(str));
            }
        }

        private static void Initialize()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                RemoveNotLoadDLL(assembly);
            }
        }

        private void InitializeComponent()
        {
            this.lblLoading = new System.Windows.Forms.Label();
            this.loadProgress = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // lblLoading
            // 
            this.lblLoading.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoading.ForeColor = System.Drawing.Color.Navy;
            this.lblLoading.Location = new System.Drawing.Point(109, 8);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(119, 19);
            this.lblLoading.TabIndex = 0;
            this.lblLoading.Text = "加载中...";
            // 
            // loadProgress
            // 
            this.loadProgress.BackColor = System.Drawing.Color.WhiteSmoke;
            this.loadProgress.Location = new System.Drawing.Point(30, 30);
            this.loadProgress.Name = "loadProgress";
            this.loadProgress.Size = new System.Drawing.Size(222, 23);
            this.loadProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.loadProgress.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(1, -5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 68);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // LoadingForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(283, 63);
            this.Controls.Add(this.loadProgress);
            this.Controls.Add(this.lblLoading);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadingForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.LoadingForm_Load);
            this.ResumeLayout(false);

        }

        private static bool IsUserDll(string dllName)
        {
            return (m_NotLoadedDlls.IndexOf(dllName) != -1);
        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            this.m_Timer.Enabled = true;
            Initialize();
            AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(this.AssemblyLoadHandler);
            this.loadProgress.Maximum = Math.Max(m_NotLoadedDlls.Count, 5);
            this.m_CurLoadNumber = 1;
            this.loadProgress.Value = this.m_CurLoadNumber;
        }

        private void OnTimer(object sender, EventArgs e)
        {
            try
            {
                if (this.m_CurLoadNumber <= this.loadProgress.Maximum)
                {
                    this.loadProgress.Value = this.m_CurLoadNumber;
                }
                this.UnloadAssemblyHandler();
                if (this.M_LoadingFormUIManager.TimeToClose)
                {
                    this.m_Timer.Enabled = false;
                    this.m_Timer.Dispose();
                    Thread.Sleep(1);
                    base.Dispose();
                }
                else
                {
                    this.m_CurLoadNumber++;
                }
            }
            catch (Exception exception)
            {
                WriteLog.Logger.Error(exception.StackTrace);
            }
        }

        private static void RemoveNotLoadDLL(Assembly assem)
        {
            if (IsUserDll(assem.GetName().Name))
            {
                m_NotLoadedDlls.Remove(assem.GetName().Name);
            }
        }

        private void UnloadAssemblyHandler()
        {
            if (!((this.m_CurLoadNumber != 5) || this.m_EventUnload))
            {
                AppDomain.CurrentDomain.AssemblyLoad -= new AssemblyLoadEventHandler(this.AssemblyLoadHandler);
                this.m_EventUnload = true;
            }
        }
    }
}

