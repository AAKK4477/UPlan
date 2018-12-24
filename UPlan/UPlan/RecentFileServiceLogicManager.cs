namespace UPlan.Frame.View
{
    using UPlan.Frame.Interface;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    public class RecentFileServiceLogicManager
    {
        private void CreateRecentFilesXML(string filePath)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                XmlNode newChild = document.CreateXmlDeclaration("1.0", "utf-8", null);
                document.AppendChild(newChild);
                XmlNode node2 = document.CreateNode(XmlNodeType.Element, "Recnet_Files", null);
                document.AppendChild(node2);
                XmlNode node3 = document.CreateNode(XmlNodeType.Element, "Project_Files", null);
                node2.AppendChild(node3);
                document.Save(filePath);
            }
            catch (Exception exception)
            {
                WriteLog.Logger.Warn("Create History XML Fail :\n" + exception.Message + "\n" + exception.StackTrace);
            }
        }

        public string FormatFileName(string filePath)
        {
            if (filePath.Length <= 0x2d)
            {
                return filePath;
            }
            string str = "...";
            string str2 = @"\";
            string str3 = "";
            string[] strArray = filePath.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length == 2)
            {
                return filePath;
            }
            string str4 = strArray[0] + str2;
            string str5 = strArray[strArray.Length - 1];
            for (int i = strArray.Length - 2; i > 0; i--)
            {
                if ((str4 + str + str2 + strArray[i] + str2 + str3 + str5).Length > 0x2d)
                {
                    break;
                }
                str3 = strArray[i] + str2 + str3;
            }
            return (str4 + str + str2 + str3 + str5);
        }

        public void InitRecentFiles()
        {
            try
            {
                if (!this.ValidateRecentFilesXML(FrameConst.OPEN_FILE_HISTORY_FILENAME))
                {
                    this.CreateRecentFilesXML(FrameConst.OPEN_FILE_HISTORY_FILENAME);
                }
                XmlDocument document = new XmlDocument();
                document.Load(FrameConst.OPEN_FILE_HISTORY_FILENAME);
                string xpath = "/Recnet_Files/Project_Files/FilePath";
                XmlNodeList list = document.SelectNodes(xpath);
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    XmlNode node = list[i];
                }
            }
            catch (Exception exception)
            {
                WriteLog.Logger.Warn("Init Open File History Fail :\n" + exception.Message + "\n" + exception.StackTrace);
            }
        }

        public void UpdateRecentFilesXML(string projectFilePath)
        {
            if (!this.ValidateRecentFilesXML(FrameConst.OPEN_FILE_HISTORY_FILENAME))
            {
                this.CreateRecentFilesXML(FrameConst.OPEN_FILE_HISTORY_FILENAME);
            }
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(FrameConst.OPEN_FILE_HISTORY_FILENAME);
                string xpath = "/Recnet_Files/Project_Files";
                XmlNode node = document.SelectSingleNode(xpath);
                List<XmlNode> list = new List<XmlNode>();
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    if (!node2.InnerText.Trim().ToLower().Equals(projectFilePath.Trim().ToLower()))
                    {
                        list.Add(node2);
                    }
                }
                node.RemoveAll();
                int num = Math.Min(list.Count, 4);
                for (int i = 0; i < num; i++)
                {
                    node.AppendChild(list[i]);
                }
                XmlNode newChild = document.CreateNode(XmlNodeType.Element, "FilePath", null);
                XmlNode node4 = document.CreateNode(XmlNodeType.CDATA, null, null);
                node4.Value = projectFilePath.Trim();
                newChild.AppendChild(node4);
                node.PrependChild(newChild);
                document.Save(FrameConst.OPEN_FILE_HISTORY_FILENAME);
            }
            catch (Exception exception)
            {
                WriteLog.Logger.Warn("Update History XML Fail :\n" + exception.Message + "\n" + exception.StackTrace);
            }
        }

        private bool ValidateRecentFilesXML(string filePath)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(filePath);
                string xpath = "/Recnet_Files/Project_Files";
                return (document.SelectSingleNode(xpath) != null);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}


