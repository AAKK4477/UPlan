using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UPlan.Frame.View
{
    public partial class EventForm : Form
    {
        public EventForm()
        {
            InitializeComponent();
        }
    }
    public ProgressBarXManager PgrBarMgr
    {
        get
        {
            return this.m_PgrBarMgr;
        }
    }
}
