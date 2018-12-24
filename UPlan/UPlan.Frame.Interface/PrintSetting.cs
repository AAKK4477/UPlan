using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
namespace UPlan.Frame.Interface
{
    [Serializable]
    public class PrintSetting : ICloneable
    {
        private short m_Id;
        private string m_Name;
        private string m_GeneralPageSize;
        private string m_GeneralPageSource;
        private Margins m_GeneralPageMargin;
        private Scaling m_GeneralScaling;
        private bool m_Landscape;
        private bool m_IsRulers;
        private PrintControl m_LegendControl;
        private PrintControl m_CommensControl;
        private PrintControl m_TitleControl;
        private PrintControl m_Logo1Control;
        private PrintControl m_Logo2Control;
        private PrintControl m_HeaderControl;
        private PrinterSettings m_PrinterSet;
        public short Id
        {
            get
            {
                return this.m_Id;
            }
            set
            {
                this.m_Id = value;
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
        public string GeneralPageSize
        {
            get
            {
                return this.m_GeneralPageSize;
            }
            set
            {
                this.m_GeneralPageSize = value;
            }
        }
        public string GeneralPageSource
        {
            get
            {
                return this.m_GeneralPageSource;
            }
            set
            {
                this.m_GeneralPageSource = value;
            }
        }
        public Margins GeneralPageMargin
        {
            get
            {
                return this.m_GeneralPageMargin;
            }
            set
            {
                this.m_GeneralPageMargin = value;
            }
        }
        public Scaling GeneralScaling
        {
            get
            {
                return this.m_GeneralScaling;
            }
            set
            {
                this.m_GeneralScaling = value;
            }
        }
        public bool Landscape
        {
            get
            {
                return this.m_Landscape;
            }
            set
            {
                this.m_Landscape = value;
            }
        }
        public bool IsRulers
        {
            get
            {
                return this.m_IsRulers;
            }
            set
            {
                this.m_IsRulers = value;
            }
        }
        public PrintControl LegendControl
        {
            get
            {
                return this.m_LegendControl;
            }
            set
            {
                this.m_LegendControl = value;
                this.m_LegendControl.name = "Legend";
            }
        }
        public PrintControl CommensControl
        {
            get
            {
                return this.m_CommensControl;
            }
            set
            {
                this.m_CommensControl = value;
                this.m_CommensControl.name = "Comment";
            }
        }
        public PrintControl TitleControl
        {
            get
            {
                return this.m_TitleControl;
            }
            set
            {
                this.m_TitleControl = value;
                this.m_TitleControl.name = "Title";
            }
        }
        public PrintControl Logo1Control
        {
            get
            {
                return this.m_Logo1Control;
            }
            set
            {
                this.m_Logo1Control = value;
                this.m_Logo1Control.name = "Logo 1";
            }
        }
        public PrintControl Logo2Control
        {
            get
            {
                return this.m_Logo2Control;
            }
            set
            {
                this.m_Logo2Control = value;
                this.m_Logo2Control.name = "Logo 2";
            }
        }
        public PrintControl HeaderControl
        {
            get
            {
                return this.m_HeaderControl;
            }
            set
            {
                this.m_HeaderControl = value;
                this.m_HeaderControl.name = "Header and Footer";
            }
        }
        public PrinterSettings PrinterSet
        {
            get
            {
                return this.m_PrinterSet;
            }
            set
            {
                this.m_PrinterSet = value;
            }
        }
        public PrintSetting()
        {
            this.SetDefaultValue();
        }
        private void SetDefaultValue()
        {
            this.Id = -1;
            this.GeneralPageMargin = new Margins(0, 0, 0, 0);
            Property property = default(Property);
            property.font = new Font("Arial", 9f);
            this.TitleControl = new PrintControl
            {
                property = property
            };
            //default(Property).font = new Font("Arial", 9f);
            this.HeaderControl = new PrintControl
            {
                property = property
            };
            //default(Property).font = new Font("Arial", 9f);
            this.CommensControl = new PrintControl
            {
                property = property
            };
        }
        public void CopyFrom(PrintSetting printSetting)
        {
            this.m_GeneralPageMargin = printSetting.GeneralPageMargin;
            this.m_GeneralPageSize = printSetting.GeneralPageSize;
            this.m_GeneralPageSource = printSetting.GeneralPageSource;
            this.m_GeneralScaling = printSetting.GeneralScaling;
            this.m_Landscape = printSetting.Landscape;
            this.Name = printSetting.Name;
            this.LegendControl = printSetting.LegendControl;
            this.CommensControl = printSetting.CommensControl;
            this.TitleControl = printSetting.TitleControl;
            this.Logo1Control = printSetting.Logo1Control;
            this.Logo2Control = printSetting.Logo2Control;
            this.HeaderControl = printSetting.HeaderControl;
            this.IsRulers = printSetting.IsRulers;
            this.PrinterSet = printSetting.PrinterSet;
        }
        public List<PrintControl> GetControls()
        {
            return new List<PrintControl>
            {
                this.HeaderControl,
                this.LegendControl,
                this.CommensControl,
                this.TitleControl,
                this.Logo1Control,
                this.Logo2Control
            };
        }
        public List<PrintControl> FilterOnMap()
        {
            List<PrintControl> controls = this.GetControls();
            return controls.FindAll((PrintControl c) => c.isOnMap && c.isSelected && c.IsValidated());
        }
        public List<PrintControl> FilterOffMap(bool skipHeader)
        {
            List<PrintControl> controls = this.GetControls();
            return controls.FindAll((PrintControl c) => (!object.ReferenceEquals(c, this.HeaderControl) || skipHeader) && (!c.isOnMap && c.isSelected) && c.IsValidated());
        }
        public object Clone()
        {
            return base.MemberwiseClone();
        }
    }
}

