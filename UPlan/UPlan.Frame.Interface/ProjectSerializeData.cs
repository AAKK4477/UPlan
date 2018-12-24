using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace UPlan.Frame.Interface
{
    [Serializable]
    public class ProjectSerializeData
    {
        private PrintSettingCollection m_PSCollection;
        private PrintSetting m_PrintSeeting;
        private Dictionary<ProjectExplorerTree, List<object>> m_TreeTypeToTreeNodeListDic;
        private string m_ProjectIPLFile;
        private NetWorkType m_NetType;
        private string m_Name;
        private List<ISubSystemData> m_AllSubSysData = new List<ISubSystemData>();
        private string m_Version;
        public PrintSettingCollection PSCollection
        {
            get
            {
                return this.m_PSCollection;
            }
            set
            {
                this.m_PSCollection = value;
            }
        }
        public PrintSetting PrintSeeting
        {
            get
            {
                return this.m_PrintSeeting;
            }
            set
            {
                this.m_PrintSeeting = value;
            }
        }
        public Dictionary<ProjectExplorerTree, List<object>> TreeTypeToTreeNodeListDic
        {
            get
            {
                return this.m_TreeTypeToTreeNodeListDic;
            }
        }
        public string ProjectIPLFile
        {
            get
            {
                return this.m_ProjectIPLFile;
            }
            set
            {
                this.m_ProjectIPLFile = value;
            }
        }
        public NetWorkType NetType
        {
            get
            {
                return this.m_NetType;
            }
            set
            {
                this.m_NetType = value;
            }
        }
        public string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                this.m_Name = value;
            }
        }
        public List<ISubSystemData> AllSubSysData
        {
            get
            {
                return this.m_AllSubSysData;
            }
            set
            {
                this.m_AllSubSysData = value;
            }
        }
        public string Version
        {
            get
            {
                return this.m_Version;
            }
            set
            {
                this.m_Version = value;
            }
        }
        public ProjectSerializeData()
        {
            this.m_TreeTypeToTreeNodeListDic = new Dictionary<ProjectExplorerTree, List<object>>();
        }
        private ProjectSerializeData(SerializationInfo info, StreamingContext context)
        {
            this.m_Name = info.GetString("m_Name");
            this.m_NetType = (NetWorkType)info.GetValue("m_NetType", typeof(NetWorkType));
            this.m_PrintSeeting = (info.GetValue("m_PrintSeeting", typeof(PrintSetting)) as PrintSetting);
            this.m_ProjectIPLFile = info.GetString("m_ProjectIPLFile");
            this.m_PSCollection = (info.GetValue("m_PSCollection", typeof(PrintSettingCollection)) as PrintSettingCollection);
            this.m_Version = info.GetString("m_Version");
            this.m_AllSubSysData = (info.GetValue("m_AllSubSysData", typeof(List<ISubSystemData>)) as List<ISubSystemData>);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
