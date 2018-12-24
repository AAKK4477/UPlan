using UPlan.Frame.DockForm;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace UPlan.Frame.Interface
{
    public class ProjectSingleton
    {
        public static ProjectSingleton Instance = new ProjectSingleton();
        public bool OpenV3R5ProjectFile;
        private static IProject m_CurrentProject = null;
        public static IProject CurrentProject
        {
            get
            {
                return ProjectSingleton.m_CurrentProject;
            }
            set
            {
                if (ProjectSingleton.m_CurrentProject != null && null == value)
                {
                    ProjectSingleton.m_CurrentProject.Dispose();
                }
                ProjectSingleton.m_CurrentProject = value;
            }
        }
        public static Form FindProjectForm(string tableName)
        {
            string name = ProjectSingleton.CurrentProject.Name;
            Form result;
            foreach (IDockContent current in ProjectSingleton.CurrentProject.DockPanel.Contents)
            {
                Form form = current as Form;
                if (form.Name == tableName)
                {
                    DockContent dockContent = (DockContent)form;
                    if (dockContent.TabText.Contains(name))
                    {
                        result = form;
                        return result;
                    }
                }
            }
            result = null;
            return result;
        }
        public static List<NetWorkType> ParseNetworkType(NetWorkType mulNetType)
        {
            List<NetWorkType> list = new List<NetWorkType>();
            foreach (NetWorkType netWorkType in Enum.GetValues(typeof(NetWorkType)))
            {
                if ((netWorkType & mulNetType) == netWorkType)
                {
                    list.Add(netWorkType);
                }
            }
            return list;
        }
    }
}
