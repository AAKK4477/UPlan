using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UPlan.Frame.Interface;
using UPlan.Frame.Model;

namespace UPlan.Frame.View
{
    public partial class ProjectCreate
    {
        private string m_CfgFile = null;
        private NetWorkType m_NetType;
        private GeneralProjectEntity m_Project = null;

        public ProjectCreate()
        {
            this.SetDefaultCfgFilePath();
            this.NetType = NetWorkType.LTE;
        }

        private void SetDefaultCfgFilePath()
        {
            this.m_CfgFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UPlanConfig.xml");
        }


        //属性
        public string CfgFile
        {
            get
            {
                return this.m_CfgFile;
            }
        }


        public NetWorkType NetType
        {
            get
            {
                return this.m_NetType;
            }
            private set
            {
                this.m_NetType = value;
            }
        }

        public GeneralProjectEntity NewProject
        {
            get
            {
                return this.m_Project;
            }
        }
    }
}
