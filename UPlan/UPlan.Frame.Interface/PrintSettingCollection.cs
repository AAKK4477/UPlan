using System;
using System.Collections;
using System.Collections.Generic;
namespace UPlan.Frame.Interface
{
    [Serializable]
    public class PrintSettingCollection : IEnumerable
    {
        private List<PrintSetting> m_SettingList = new List<PrintSetting>();
        private PrintSetting m_DefaultPrintSetting = null;
        private PrintCfgReader m_Reader;
        private short m_MaxID = 0;
        public PrintSetting this[int index]
        {
            get
            {
                return this.m_SettingList[index];
            }
        }
        public PrintSettingCollection(string templateFile)
        {
            string empty = string.Empty;
            this.m_Reader = new PrintCfgReader();
            this.m_Reader.ReaderPrintTemplateFile(templateFile, ref empty);
            foreach (PrintSetting current in this.m_Reader.GetPrintConfig())
            {
                this.AddPrintSetting((PrintSetting)current.Clone());
            }
            this.m_DefaultPrintSetting = this.m_SettingList[0];
        }
        public List<string> GetTemplateNames()
        {
            return this.m_SettingList.ConvertAll<string>(new Converter<PrintSetting, string>(this.ToName));
        }
        private string ToName(PrintSetting setting)
        {
            return setting.Name;
        }
        public PrintSetting GetDefaultPrintSetting()
        {
            return this.m_DefaultPrintSetting;
        }
        public bool AddPrintSetting(PrintSetting printSetting)
        {
            return this.AddPrintSetting(new List<PrintSetting>
            {
                printSetting
            });
        }
        public bool AddPrintSetting(List<PrintSetting> printSetting)
        {
            foreach (PrintSetting current in printSetting)
            {
                current.Id = (this.m_MaxID += 1);
                this.m_SettingList.Add(current);
                this.m_DefaultPrintSetting = current;
            }
            return true;
        }
        public bool ModifyPrintSetting(PrintSetting printSetting)
        {
            short id = printSetting.Id;
            PrintSetting printSetting2 = this.FindPrintSettingById(id);
            printSetting2.CopyFrom(printSetting);
            this.m_DefaultPrintSetting = printSetting2;
            return true;
        }
        public bool DeletePrintSetting(string name)
        {
            PrintSetting printSetting = this.FindPrintSettingByName(name);
            bool result;
            if (printSetting == null)
            {
                result = false;
            }
            else
            {
                this.m_SettingList.Remove(printSetting);
                if (printSetting.Id == this.m_DefaultPrintSetting.Id)
                {
                    this.m_DefaultPrintSetting = null;
                }
                result = true;
            }
            return result;
        }
        public PrintSetting FindPrintSettingById(short id)
        {
            return this.m_SettingList.Find((PrintSetting temp) => temp.Id == id);
        }
        public PrintSetting FindPrintSettingByName(string name)
        {
            return this.m_SettingList.Find((PrintSetting temp) => temp.Name.Equals(name));
        }
        public bool IsExistPrintSetting(string name)
        {
            return this.m_SettingList.Exists((PrintSetting temp) => temp.Name.Equals(name));
        }
        public int Count()
        {
            return this.m_SettingList.Count;
        }
        public string AddDefaultPrintConfig()
        {
            PrintSetting printSetting = (PrintSetting)this.m_Reader.GetPrintConfig()[0].Clone();
            printSetting.Name = this.GetDefaultName();
            printSetting.Id = -1;
            this.AddPrintSetting(printSetting);
            return printSetting.Name;
        }
        private string GetDefaultName()
        {
            string text = string.Empty;
            int num = 0;
            do
            {
                num++;
                text = "Template" + num;
            }
            while (this.IsExistPrintSetting(text));
            return text;
        }
        public IEnumerator GetEnumerator()
        {
            return this.m_SettingList.GetEnumerator();
        }
    }
}

