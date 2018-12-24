using UPlan.Frame.DockForm;
using UPlan.Frame.Interface;
using UPlan.Frame.RadioDataTree;
using UPlan.Satellite.TaskManage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace UPlan.Frame.Model
{
	public class GeneralProjectEntity : ILogicSubSys, IGeneralSubSys, ISubSysExp, ISubsysConvert, INetworkSetting, IBaseService, IProject, ISubSysSerializable, IDisposable, IVersion
	{
		private DSServiceNodeContext m_ServiceContext = new DSServiceNodeContext();

		private bool m_IsNeedCloseProcessReport = false;

		private bool m_IsDS = false;

		private IApplicationContext m_AppContext;

		private string m_CfgFile;

        public List<IGeneralSubSys> m_AllSubSys;//zuochen  private->public 12.13

		private Dictionary<string, IDataConvert> m_SubSysConvert;

		private Dictionary<ProjectExplorerTree, ITriStateTreeViewContorller> m_TreeTypeToTreeView;

		private List<IMenuItemOperation> m_MenuItems;

		private List<FloatingToolStrip> m_ToolStrips;

		private ToolStripPanel m_ToolStripPanel;

		private DockContent m_GisForm = null;

		private BitArray m_BitVec = new BitArray(10, false);

		private ProjectSerializeData m_ProjectSerializeData;

		private bool m_UndoBtnEnabled;

		private bool m_RedoBtnEnabled;

		private ToolStripCollectivity m_ToolStripConfigXML = null;

		private List<FloatingToolStrip> m_DefaultToolStripList = new List<FloatingToolStrip>();

		private Dictionary<string, List<FloatingToolStrip>> m_SubSysType2ToolStripListDic = new Dictionary<string, List<FloatingToolStrip>>();

		private bool m_NeedPromptSave;

		[NonSerialized]
		private DockPanel m_DockPanel;

		public IDSServiceNodeContext DSNodeContext
		{
			get
			{
				return this.m_ServiceContext;
			}
		}

		public bool IsNeedCloseProcessReport
		{
			get
			{
				return this.m_IsNeedCloseProcessReport;
			}
			set
			{
				this.m_IsNeedCloseProcessReport = value;
			}
		}

		public bool IsDS
		{
			get
			{
				return this.m_IsDS;
			}
		}

		public Dictionary<ProjectExplorerTree, ITriStateTreeViewContorller> TreeTypeToTreeView
		{
			get
			{
				return this.m_TreeTypeToTreeView;
			}
			set
			{
				this.m_TreeTypeToTreeView = value;
			}
		}

		public ProjectSerializeData ProjectSerializeData
		{
			get
			{
				return this.m_ProjectSerializeData;
			}
			set
			{
				this.m_ProjectSerializeData = value;
			}
		}

		public List<IGeneralSubSys> AllSubSys
		{
			get
			{
				return this.m_AllSubSys;
			}
		}

		public List<ISubSystemData> AllSubSysData
		{
			get
			{
				return this.ProjectSerializeData.AllSubSysData;
			}
			set
			{
				this.ProjectSerializeData.AllSubSysData = value;
			}
		}

		public Dictionary<string, IDataConvert> SubSysConvert
		{
			get
			{
				return this.m_SubSysConvert;
			}
			set
			{
				this.m_SubSysConvert = value;
			}
		}

		public BitArray GisOptState
		{
			get
			{
				return this.m_BitVec;
			}
			set
			{
				this.m_BitVec = value;
			}
		}

		public List<FloatingToolStrip> ToolStrips
		{
			get
			{
				return this.m_ToolStrips;
			}
		}

		public List<IMenuItemOperation> MenuItems
		{
			get
			{
				return this.m_MenuItems;
			}
			private set
			{
				this.m_MenuItems = value;
			}
		}

		public DockContent MainForm
		{
			get
			{
				if (null == this.m_GisForm)
				{
					this.m_GisForm = new DockContent();
				}
				return this.m_GisForm;
			}
		}

		public IApplicationContext AppContext
		{
			get
			{
				return this.m_AppContext;
			}
		}

		public bool UndoBtnEnabled
		{
			get
			{
				return this.m_UndoBtnEnabled;
			}
			set
			{
				this.m_UndoBtnEnabled = value;
			}
		}

		public bool RedoBtnEnabled
		{
			get
			{
				return this.m_RedoBtnEnabled;
			}
			set
			{
				this.m_RedoBtnEnabled = value;
			}
		}

		public NetWorkType NetType
		{
			get
			{
				return this.ProjectSerializeData.NetType;
			}
		}

		public string Name
		{
			get
			{
				return this.ProjectSerializeData.Name;
			}
			set
			{
				this.ProjectSerializeData.Name = value;
			}
		}

		public string ProjectIPLFile
		{
			get
			{
				return this.ProjectSerializeData.ProjectIPLFile;
			}
			set
			{
				this.ProjectSerializeData.ProjectIPLFile = value;
			}
		}

		public object RfPlan
		{
			get
			{
				return null;
			}
		}

		public bool NeedPromptSave
		{
			get
			{
				return this.m_NeedPromptSave;
			}
			set
			{
				this.m_NeedPromptSave = value;
			}
		}

		public DockPanel DockPanel
		{
			get
			{
				return this.m_DockPanel;
			}
			set
			{
				this.m_DockPanel = value;
			}
		}

		public GeneralProjectEntity(NetWorkType netType, string cfgFile)
		{
			this.m_ProjectSerializeData = new ProjectSerializeData();
			this.ProjectSerializeData.NetType = netType;
			this.m_CfgFile = cfgFile;
			this.m_AppContext = new ApplicationContext();
			this.m_AllSubSys = new List<IGeneralSubSys>();
			this.m_SubSysConvert = new Dictionary<string, IDataConvert>();
			this.ProjectIPLFile = string.Empty;
			this.NeedPromptSave = false;
		}

		public void InitialControlCollection()
		{
			TriStateTreeView triStateTreeView = new TriStateTreeView();
			triStateTreeView.Name = "geoTree";
			TriStateTreeView triStateTreeView2 = new TriStateTreeView();
			triStateTreeView2.Name = "homeTree";
			TriStateTreeView triStateTreeView3 = new TriStateTreeView();
			triStateTreeView3.Name = "editTree";
			TriStateTreeView triStateTreeView4 = new TriStateTreeView();
			triStateTreeView4.Name = "wizardTree";
			this.m_ToolStripPanel = new ToolStripPanel();
			this.m_ToolStrips = new List<FloatingToolStrip>();
			this.MenuItems = new List<IMenuItemOperation>();
			this.m_TreeTypeToTreeView = new Dictionary<ProjectExplorerTree, ITriStateTreeViewContorller>();
			this.m_TreeTypeToTreeView.Add(ProjectExplorerTree.Edit, triStateTreeView3);
			this.m_TreeTypeToTreeView.Add(ProjectExplorerTree.Geo, triStateTreeView);
			this.m_TreeTypeToTreeView.Add(ProjectExplorerTree.Home, triStateTreeView2);
			this.m_TreeTypeToTreeView.Add(ProjectExplorerTree.Wizard, triStateTreeView4);
		}

		public ITriStateTreeView Query(ProjectExplorerTree treeType)
		{
			return this.m_TreeTypeToTreeView[treeType] as TriStateTreeView;
		}

		private void CreateSubSys()
		{
			SubSysCfgReader subSysCfgReader = new SubSysCfgReader(this.m_CfgFile);
			Dictionary<string, string> subSysTypeName = subSysCfgReader.GetSubSysTypeName();
			Dictionary<string, Dictionary<string, string>> subSysConvertName = subSysCfgReader.GetSubSysConvertName();
			foreach (string current in subSysTypeName.Keys)
			{
				string text = subSysTypeName[current];
				IGeneralSubSys geSys = null;
				try
				{
					object objectFromAssembly = AssemblyAssist.GetObjectFromAssembly(current, text);
					geSys = (objectFromAssembly as IGeneralSubSys);
				}
				catch (Exception ex)
				{
					WriteLog.Logger.Error(string.Concat(new string[]
					{
						"Load ",
						text,
						"Failed :",
						ex.Message,
						" ",
						ex.StackTrace
					}));
					throw ex;
				}
				this.CreateSubSysCall0(subSysConvertName, current, text, geSys);
			}
		}

		private void CreateSubSysCall0(Dictionary<string, Dictionary<string, string>> subsystemConver, string sysname, string dllName, IGeneralSubSys geSys)
		{
			if (geSys != null)
			{
				this.m_AllSubSys.Add(geSys);
				WriteLog.Logger.Info("CreateSubSystem Successfully: " + sysname + "  " + dllName);
				if (subsystemConver.ContainsKey(sysname))
				{
					this.m_SubSysConvert.Add(sysname, this.LoadDataConvert(subsystemConver[sysname], dllName));
				}
			}
		}

		private IDataConvert LoadDataConvert(Dictionary<string, string> converInfo, string dllName)
		{
			string[] array = new string[converInfo.Keys.Count];
			converInfo.Keys.CopyTo(array, 0);
			List<string> versions = new List<string>(array);
			return this.GetConvertChain(versions, converInfo, dllName);
		}

		public IDataConvert GetConvertChain(List<string> versions, Dictionary<string, string> convertInfo, string dllName)
		{
			IDataConvert result;
			if (versions.Count != 0)
			{
				string text = versions[0];
				object objectFromAssembly = AssemblyAssist.GetObjectFromAssembly(convertInfo[text], dllName);
				IDataConvert dataConvert = objectFromAssembly as IDataConvert;
				dataConvert.SetVersion(text);
				versions.Remove(text);
				convertInfo.Remove(text);
				if (dataConvert.GetNext() == null)
				{
					IDataConvert convertChain = this.GetConvertChain(versions, convertInfo, dllName);
					dataConvert.SetNext(convertChain);
					result = dataConvert;
				}
				else
				{
					result = dataConvert;
				}
			}
			else
			{
				result = null;
			}
			return result;
		}

		private void AddToolStrips(IUISubSys wsf)
		{
			if (wsf.SubToolStrips != null)
			{
				this.m_SubSysType2ToolStripListDic.Add(wsf.GetType().FullName, wsf.SubToolStrips);
			}
		}

		public void InitialUserControls()
		{
			this.InitialControlCollection();
			foreach (IGeneralSubSys current in this.m_AllSubSys)
			{
				try
				{
					IUISubSys iUISubSys = current as IUISubSys;
					if (iUISubSys != null)
					{
						this.AddTreeNodes(iUISubSys);
						this.AddToolStrips(iUISubSys);
						this.AddMenuItems(iUISubSys);
					}
					IGisSubSysWsf gisSubSysWsf = current as IGisSubSysWsf;
					if (gisSubSysWsf != null)
					{
						this.m_GisForm = gisSubSysWsf.GisForm;
					}
				}
				catch (Exception ex)
				{
					WriteLog.Logger.Error("add UI Failed : " + ex.StackTrace);
					throw ex;
				}
			}
			this.LoadToolStripByXml();
			this.CreateToolStripPanel();
		}

		private FloatingToolStrip ToolStripDefaultDeal()
		{
			FloatingToolStrip floatingToolStrip = new FloatingToolStrip();
			foreach (List<FloatingToolStrip> current in this.m_SubSysType2ToolStripListDic.Values)
			{
				foreach (FloatingToolStrip current2 in current)
				{
					if (current2.Tag == null || "".Equals(current2.Tag.ToString()))
					{
						ToolStripItem[] array = new ToolStripItem[current2.Items.Count];
						current2.Items.CopyTo(array, 0);
						floatingToolStrip.Items.AddRange(array);
					}
				}
			}
			return floatingToolStrip;
		}

		private void LoadToolStripByXml()
		{
			ToolStripCollectivity toolStripCollectivity = null;
			try
			{
				string path = Path.Combine(MainFormSingleton.AppBaseDirectory, "ToolBarConfig.xml");
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(ToolStripCollectivity));
				FileStream fileStream = new FileStream(path, FileMode.Open);
				toolStripCollectivity = (xmlSerializer.Deserialize(fileStream) as ToolStripCollectivity);
				fileStream.Close();
			}
			catch (Exception ex)
			{
				WriteLog.Logger.Error(ex.Message + ex.StackTrace);
				toolStripCollectivity = null;
			}
			this.m_ToolStripConfigXML = toolStripCollectivity;
			if (null != toolStripCollectivity)
			{
				foreach (UserToolStripGroup userToolStripGroup in toolStripCollectivity.UserToolStripGroupCollection)
				{
					FloatingToolStrip floatingToolStrip = new FloatingToolStrip();
					floatingToolStrip.Tag = userToolStripGroup.Tag;
					floatingToolStrip.Name = userToolStripGroup.Title;
					this.LoadToolStripByXmlCall0(userToolStripGroup, floatingToolStrip);
					if (0 != floatingToolStrip.Items.Count)
					{
						this.m_ToolStrips.Add(floatingToolStrip);
					}
				}
				FloatingToolStrip floatingToolStrip2 = this.ToolStripDefaultDeal();
				if (0 != floatingToolStrip2.Items.Count)
				{
					this.m_ToolStrips.Add(floatingToolStrip2);
				}
			}
			else
			{
				foreach (List<FloatingToolStrip> current in this.m_SubSysType2ToolStripListDic.Values)
				{
					this.m_ToolStrips.AddRange(current);
				}
			}
		}

		private void LoadToolStripByXmlCall0(UserToolStripGroup Group, FloatingToolStrip userGroup)
		{
			foreach (SubSysOrder subSysOrder in Group.SubSysOrderCollection)
			{
				List<FloatingToolStrip> list = new List<FloatingToolStrip>();
				this.m_SubSysType2ToolStripListDic.TryGetValue(subSysOrder.Type, out list);
				if (null != list)
				{
					foreach (FloatingToolStrip current in list)
					{
						if (current.Tag != null && current.Tag.ToString().Equals(userGroup.Tag))
						{
							ToolStripItem[] array = new ToolStripItem[current.Items.Count];
							current.Items.CopyTo(array, 0);
							userGroup.Items.AddRange(array);
						}
					}
				}
				else
				{
					WriteLog.Logger.Info(subSysOrder.Type + "May not Loaded");
				}
			}
		}

		private void CreateToolStripPanel()
		{
			this.m_ToolStripPanel.Controls.Clear();
			foreach (FloatingToolStrip current in this.m_ToolStrips)
			{
				try
				{
					this.m_ToolStripPanel.Join(current);
				}
				catch (Exception ex)
				{
					WriteLog.Logger.Error("Join ToolStripPanel Failed : " + ex.StackTrace);
					throw ex;
				}
			}
		}

		private void AddTreeNodes(IUISubSys wsf)
		{
			if (wsf.SubTreeNodes != null)
			{
				foreach (ITreeNodeOperation current in wsf.SubTreeNodes)
				{
					this.SetTreeNode(current, this.m_TreeTypeToTreeView[current.TreeType]);
				}
			}
		}

		private void SetTreeNode(ITreeNodeOperation nodeopt, ITriStateTreeViewContorller dtv)
		{
			if (null != nodeopt.SubSysNode)
			{
				dtv.AddNode(nodeopt.SubSysNode, new TreeNodeMouseClickEventHandler(nodeopt.HandleNodeClick), new KeyEventHandler(nodeopt.HandleKeyDown), new NodeLabelEditEventHandler(nodeopt.HandleLabelEdit));
				dtv.AddNodeCheckEvent(nodeopt.SubSysNode, new EventHandler<TriEventArgs>(nodeopt.HandleNodeCheck));
				dtv.AddNodeDragdropEvent(nodeopt.SubSysNode, new EventHandler(nodeopt.HandleNodeDragdrop));
				dtv.AddImageList(nodeopt.IconList);
			}
		}

		private void SetTreeNodes(ITriStateTreeViewContorller dtv)
		{
		}

		private void AddMenuItems(IUISubSys wsf)
		{
			if (wsf.SubMenuItems != null)
			{
				foreach (IMenuItemOperation current in wsf.SubMenuItems)
				{
					this.MenuItems.Add(current);
				}
			}
		}

		public void InitStage1(IApplicationContext appContext)
		{
			this.CreateSubSys();
			this.m_AppContext = appContext;
			this.m_AppContext.RegisterService(this);
			this.SetNetworkType(this.ProjectSerializeData.NetType);
			foreach (IGeneralSubSys current in this.m_AllSubSys)
			{
				try
				{
					current.InitStage1(this.m_AppContext);
					WriteLog.Logger.Info(current.ToString() + " InitStage1 Successfully");
				}
				catch (Exception ex)
				{
					WriteLog.Logger.Error(string.Concat(new string[]
					{
						current.ToString(),
						" InitStage1 Failed  ",
						ex.Message,
						"  ",
						ex.StackTrace
					}));
					throw ex;
				}
			}
		}

		public void InitStage2()
		{
			foreach (IGeneralSubSys current in this.m_AllSubSys)
			{
				try
				{
					current.InitStage2();
					WriteLog.Logger.Info(current.ToString() + " InitStage2 Successfully");
				}
				catch (Exception ex)
				{
                    WriteLog.Logger.Error(string.Concat(new string[]
                    {
                        current.ToString(),
                        " InitStage2 Failed  ",
                        ex.Message,
                        "  ",
                        ex.StackTrace
                    }));
                    throw new ArgumentException(ex.Source);
				    //continue;
				}
			}
			this.InitialUserControls();
		}

		public void SetNetworkType(NetWorkType netType)
		{
			this.ProjectSerializeData.NetType = netType;
			foreach (IGeneralSubSys current in this.m_AllSubSys)
			{
				if (current is INetworkSetting)
				{
					((INetworkSetting)current).SetNetworkType(netType);
				}
			}
		}

		private void ClearTreeNode()
		{
			foreach (KeyValuePair<ProjectExplorerTree, ITriStateTreeViewContorller> current in this.m_TreeTypeToTreeView)
			{
				(current.Value as TreeView).Nodes.Clear();
				(current.Value as TreeView).Dispose();
			}
		}

		public bool CanChangUPlanworkType(NetWorkType netType)
		{
			bool result;
			foreach (IGeneralSubSys current in this.m_AllSubSys)
			{
				if (current is INetworkSetting)
				{
					if (!((INetworkSetting)current).CanChangUPlanworkType(netType))
					{
						result = false;
						return result;
					}
				}
			}
			result = true;
			return result;
		}

		public void Dispose()
		{
			foreach (IGeneralSubSys current in this.m_AllSubSys)
			{
				IUISubSys iUISubSys = current as IUISubSys;
				if (iUISubSys != null && iUISubSys.SubToolStrips != null)
				{
					foreach (FloatingToolStrip current2 in iUISubSys.SubToolStrips)
					{
						current2.Dispose();
					}
				}
				current.Dispose();
			}
			this.ClearTreeNode();
		}

		public IBaseService Lookup(string serviceName)
		{
			return this.m_AppContext.Lookup(serviceName);
		}

		public void AutoLoadData(ISubSystemData systemData)
		{
		}

		public ISubSystemData Serialize()
		{
			return null;
		}

		public void SerializeStart()
		{
		}

		public void SerializeEnd()
		{
		}

		public string GetVersion()
		{
			return this.m_ProjectSerializeData.Version;
		}

		public void SetVersion(string version)
		{
			this.m_ProjectSerializeData.Version = version;
		}

		public XlsTable[] Export()
		{
			return null;
		}

		public void ExportStart(IApplicationContext context)
		{
		}

		public void ExportEnd()
		{
		}

		public ISubSystemData ConvertXls(XlsTable[] xlsTables)
		{
			return null;
		}

		public void Import(ISubSystemData subsystem, bool isUpdate)
		{
		}

		public XlsTable[] ExportLCS()
		{
			return null;
		}
	}
}
