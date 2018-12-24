using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Xml;
namespace UPlan.Frame.Interface
{
	[Serializable]
	public class PrintCfgReader
	{
		private List<PrintSetting> m_PrintSettings = new List<PrintSetting>();
		[NonSerialized]
		private XmlReader m_Reader;
		public List<PrintSetting> GetPrintConfig()
		{
			return this.m_PrintSettings;
		}
		public bool ReaderPrintTemplateFile(string templateFile, ref string info)
		{
			bool result;
			try
			{
				this.m_Reader = XmlReader.Create(templateFile, new XmlReaderSettings
				{
					ProhibitDtd = false,
					ValidationType = ValidationType.DTD
				});
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(this.m_Reader);
				foreach (XmlNode xmlNode in xmlDocument.ChildNodes)
				{
					if (xmlNode.Name.Equals("PrintConfigurations"))
					{
						this.LoadTempalteInfo(xmlNode.ChildNodes);
						break;
					}
				}
			}
			catch (Exception ex)
			{
				info = "Import error:" + ex.Message;
				WriteLog.Logger.Warn("Import PrintConfigurations error:" + ex.Message);
				result = false;
				return result;
			}
			finally
			{
				if (this.m_Reader != null)
				{
					this.m_Reader.Close();
				}
			}
			result = true;
			return result;
		}
		private void LoadTempalteInfo(XmlNodeList nodelist)
		{
			foreach (XmlNode configNode in nodelist)
			{
				PrintSetting printSetting = new PrintSetting();
				this.LoadTempalteInfoCall0(configNode, printSetting);
				this.m_PrintSettings.Add(printSetting);
			}
		}
		private void UpdataDefaultPrinterSetting(PrintSetting tempalte)
		{
			tempalte.PrinterSet = new PrinterSettings();
			this.SetDefaultPrinterDpi(tempalte);
			this.SetDefaultPrinterPageSet(tempalte);
		}
		private void SetDefaultPrinterPageSet(PrintSetting tempalte)
		{
			PageSettings defaultPageSettings = tempalte.PrinterSet.DefaultPageSettings;
			defaultPageSettings.Landscape = tempalte.Landscape;
			defaultPageSettings.PaperSize = this.GetPaperSize(tempalte);
		}
		private PaperSize GetPaperSize(PrintSetting tempalte)
		{
			string generalPageSize = tempalte.GeneralPageSize;
			PaperSize result;
			for (int i = 0; i < tempalte.PrinterSet.PaperSizes.Count; i++)
			{
				PaperSize paperSize = tempalte.PrinterSet.PaperSizes[i];
				if (paperSize.PaperName == generalPageSize)
				{
					result = paperSize;
					return result;
				}
			}
			result = null;
			return result;
		}
		private void SetDefaultPrinterDpi(PrintSetting tempalte)
		{
		}
		private void LoadTempalteInfoCall0(XmlNode configNode, PrintSetting tempalte)
		{
			foreach (XmlNode xmlNode in configNode.ChildNodes)
			{
				string name = xmlNode.Name;
				this.LoadTempalteInfoCall0(tempalte, xmlNode, name);
			}
			this.UpdataDefaultPrinterSetting(tempalte);
		}
		private void LoadTempalteInfoCall0Call0(PrintSetting tempalte, XmlNode childNode, string childName)
		{
			if (childName != null)
			{
				if (!(childName == "Page"))
				{
					if (!(childName == "title"))
					{
						if (!(childName == "Footer"))
						{
							if (childName == "Comments")
							{
								PrintControl commensControl = this.ParsePrintControlNode(childNode);
								tempalte.CommensControl = commensControl;
							}
						}
						else
						{
							PrintControl headerControl = this.ParsePrintControlNode(childNode);
							tempalte.HeaderControl = headerControl;
						}
					}
					else
					{
						PrintControl titleControl = this.ParsePrintControlNode(childNode);
						tempalte.TitleControl = titleControl;
					}
				}
				else
				{
					this.ParsePageNode(tempalte, childNode);
				}
			}
		}
		private void LoadTempalteInfoCall0Call1(PrintSetting tempalte, XmlNode childNode, string childName)
		{
			if (childName != null)
			{
				if (!(childName == "Logo1"))
				{
					if (!(childName == "Logo2"))
					{
						if (!(childName == "Legend"))
						{
							if (childName == "Map")
							{
								this.ParsMapNode(tempalte, childNode);
							}
						}
						else
						{
							PrintControl legendControl = this.ParsePrintControlNode(childNode);
							legendControl.property.bitMapPos = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "legendView.bmp");
							tempalte.LegendControl = legendControl;
						}
					}
					else
					{
						PrintControl logo2Control = this.ParsePrintControlNode(childNode);
						tempalte.Logo2Control = logo2Control;
					}
				}
				else
				{
					PrintControl logo1Control = this.ParsePrintControlNode(childNode);
					tempalte.Logo1Control = logo1Control;
				}
			}
		}
		private void LoadTempalteInfoCall0(PrintSetting tempalte, XmlNode childNode, string childName)
		{
			if ("Page".Equals(childName) || "title".Equals(childName) || "Footer".Equals(childName) || "Comments".Equals(childName))
			{
				this.LoadTempalteInfoCall0Call0(tempalte, childNode, childName);
			}
			else
			{
				this.LoadTempalteInfoCall0Call1(tempalte, childNode, childName);
			}
		}
		private void ParsePageNode(PrintSetting tempalte, XmlNode childNode)
		{
			foreach (XmlNode xmlNode in childNode)
			{
				if (xmlNode.Name == "TemplateName")
				{
					string name = xmlNode.InnerText.Trim();
					tempalte.Name = name;
				}
				else if (xmlNode.Name.Equals("Paper"))
				{
					string value = xmlNode.Attributes.GetNamedItem("size").Value;
					string value2 = xmlNode.Attributes.GetNamedItem("orientation").Value;
					tempalte.GeneralPageSize = value;
					tempalte.Landscape = !value2.Equals("1");
				}
				else if (xmlNode.Name.Equals("Margins"))
				{
					int right = Convert.ToInt32(xmlNode.Attributes.GetNamedItem("right").Value);
					int left = Convert.ToInt32(xmlNode.Attributes.GetNamedItem("left").Value);
					int bottom = Convert.ToInt32(xmlNode.Attributes.GetNamedItem("bottom").Value);
					int top = Convert.ToInt32(xmlNode.Attributes.GetNamedItem("top").Value);
					tempalte.GeneralPageMargin = new Margins
					{
						Right = right,
						Left = left,
						Bottom = bottom,
						Top = top
					};
				}
				else if (xmlNode.Name.Equals("Scale"))
				{
					bool isFitToPage = Convert.ToBoolean(xmlNode.Attributes.GetNamedItem("fitToPage").Value);
					int scale = Convert.ToInt32(xmlNode.InnerText.Trim());
					tempalte.GeneralScaling = new Scaling
					{
						isFitToPage = isFitToPage,
						scale = scale
					};
				}
			}
		}
		private void ParsMapNode(PrintSetting tempalte, XmlNode childNode)
		{
			foreach (XmlNode xmlNode in childNode)
			{
				if (xmlNode.Name.Equals("Rulers"))
				{
					bool isRulers = Convert.ToBoolean(xmlNode.Attributes.GetNamedItem("enable").Value);
					tempalte.IsRulers = isRulers;
				}
			}
		}
		private PrintControl ParsePrintControlNode(XmlNode childNode)
		{
			PrintControl result = default(PrintControl);
			foreach (XmlNode xmlNode in childNode)
			{
				if (xmlNode.Name.Equals("Position"))
				{
					bool isSelected = Convert.ToBoolean(xmlNode.Attributes.GetNamedItem("enable").Value);
					string value = xmlNode.Attributes.GetNamedItem("vPos").Value;
					string value2 = xmlNode.Attributes.GetNamedItem("hPos").Value;
					bool isOnMap = Convert.ToBoolean(xmlNode.Attributes.GetNamedItem("insideMap").Value);
					int width = Convert.ToInt32(xmlNode.Attributes.GetNamedItem("width").Value);
					int height = Convert.ToInt32(xmlNode.Attributes.GetNamedItem("height").Value);
					int top = Convert.ToInt32(xmlNode.Attributes.GetNamedItem("top").Value);
					int left = Convert.ToInt32(xmlNode.Attributes.GetNamedItem("left").Value);
					result.isSelected = isSelected;
					result.posVert = (Vert)Enum.Parse(typeof(Vert), value);
					result.posHori = (Hori)Enum.Parse(typeof(Hori), value2);
					result.isOnMap = isOnMap;
					result.property.top = top;
					result.property.left = left;
					result.property.width = width;
					result.property.height = height;
				}
				else if (xmlNode.Name.Equals("text"))
				{
					string comment = xmlNode.InnerText.Trim();
					result.property.comment = comment;
				}
				else if (xmlNode.Name.Equals("font"))
				{
					float emSize = Convert.ToSingle(xmlNode.Attributes.GetNamedItem("size").Value);
					string value3 = xmlNode.Attributes.GetNamedItem("font").Value;
					string value4 = xmlNode.Attributes.GetNamedItem("fontStyle").Value;
					FontStyle style = (FontStyle)Enum.Parse(typeof(FontStyle), value4);
					Font font = new Font(value3, emSize, style, GraphicsUnit.Point, 134);
					result.property.font = font;
				}
				else if (xmlNode.Name.Equals("bitmap"))
				{
					string bitMapPos = xmlNode.InnerText.Trim();
					result.property.bitMapPos = bitMapPos;
				}
			}
			return result;
		}
	}
}
