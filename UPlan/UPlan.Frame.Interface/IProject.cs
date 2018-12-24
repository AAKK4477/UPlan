using UPlan.Frame.DockForm;
using System;
using System.Collections.Generic;
namespace UPlan.Frame.Interface
{
    public interface IProject : ISubSysSerializable, IDisposable, IVersion
    {
        ProjectSerializeData ProjectSerializeData
        {
            get;
        }
        NetWorkType NetType
        {
            get;
        }
        string Name
        {
            get;
            set;
        }
        string ProjectIPLFile
        {
            get;
            set;
        }
        bool NeedPromptSave
        {
            get;
            set;
        }
        bool IsNeedCloseProcessReport
        {
            get;
            set;
        }
        DockPanel DockPanel
        {
            get;
            set;
        }
        DockContent MainForm
        {
            get;
        }
        IApplicationContext AppContext
        {
            get;
        }
        List<ISubSystemData> AllSubSysData
        {
            get;
        }
        bool IsDS
        {
            get;
        }
        object RfPlan
        {
            get;
        }
        IDSServiceNodeContext DSNodeContext
        {
            get;
        }
        ITriStateTreeView Query(ProjectExplorerTree treeType);
    }
}
