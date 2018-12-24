using System;
using System.ComponentModel;
using System.Windows.Forms;
namespace UPlan.Controls.UIBase
{
	public class ToolStripMenuItemBase : ToolStripMenuItem
	{
		private IContainer components = null;
		public ToolStripMenuItemBase()
		{
			this.InitializeComponent();
		}
		public ToolStripMenuItemBase(string text)
		{
			this.Text = text;
			this.InitializeComponent();
		}
		public ToolStripMenuItemBase(IContainer container)
		{
			container.Add(this);
			this.InitializeComponent();
		}
		protected override bool ProcessCmdKey(ref Message m, Keys keyData)
		{
			if (keyData == Keys.RWin || keyData == Keys.LWin)
			{
				this.CloseMenuStrip(this.GetRootMenuItem(this));
			}
			return base.ProcessCmdKey(ref m, keyData);
		}
		private void CloseMenuStrip(ToolStripMenuItemBase menuItem)
		{
			ToolStripDropDown dropDown = menuItem.DropDown;
			if (menuItem.Owner is MenuStrip)
			{
				menuItem.HideDropDown();
			}
			else
			{
				((ToolStripDropDownMenu)menuItem.Owner).Close();
			}
		}
		private ToolStripMenuItemBase GetRootMenuItem(ToolStripMenuItemBase menuItem)
		{
			ToolStripMenuItemBase toolStripMenuItemBase = menuItem;
			while (null != toolStripMenuItemBase.OwnerItem)
			{
				toolStripMenuItemBase = (ToolStripMenuItemBase)toolStripMenuItemBase.OwnerItem;
			}
			return toolStripMenuItemBase;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.components = new Container();
		}
	}
}
