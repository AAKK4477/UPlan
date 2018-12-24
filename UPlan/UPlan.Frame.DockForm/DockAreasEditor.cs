namespace UPlan.Frame.DockForm
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using System.Windows.Forms.Design;

    internal class DockAreasEditor : UITypeEditor
    {
        private DockAreasEditorControl m_ui = null;

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
        {
            if (this.m_ui == null)
            {
                this.m_ui = new DockAreasEditorControl();
            }
            this.m_ui.SetStates((DockAreas) value);
            ((IWindowsFormsEditorService) sp.GetService(typeof(IWindowsFormsEditorService))).DropDownControl(this.m_ui);
            return this.m_ui.DockAreas;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        private class DockAreasEditorControl : UserControl
        {
            private CheckBox checkBoxDockBottom = new CheckBox();
            private CheckBox checkBoxDockFill = new CheckBox();
            private CheckBox checkBoxDockLeft = new CheckBox();
            private CheckBox checkBoxDockRight = new CheckBox();
            private CheckBox checkBoxDockTop = new CheckBox();
            private CheckBox checkBoxFloat = new CheckBox();
            private UPlan.Frame.DockForm.DockAreas m_oldDockAreas;

            public DockAreasEditorControl()
            {
                base.SuspendLayout();
                this.checkBoxFloat.Appearance = Appearance.Button;
                this.checkBoxFloat.Dock = DockStyle.Top;
                this.checkBoxFloat.Height = 0x18;
                this.checkBoxFloat.Text = Strings.DockAreaEditor_FloatCheckBoxText;
                this.checkBoxFloat.TextAlign = ContentAlignment.MiddleCenter;
                this.checkBoxFloat.FlatStyle = FlatStyle.System;
                this.checkBoxDockLeft.Appearance = Appearance.Button;
                this.checkBoxDockLeft.Dock = DockStyle.Left;
                this.checkBoxDockLeft.Width = 0x18;
                this.checkBoxDockLeft.FlatStyle = FlatStyle.System;
                this.checkBoxDockRight.Appearance = Appearance.Button;
                this.checkBoxDockRight.Dock = DockStyle.Right;
                this.checkBoxDockRight.Width = 0x18;
                this.checkBoxDockRight.FlatStyle = FlatStyle.System;
                this.checkBoxDockTop.Appearance = Appearance.Button;
                this.checkBoxDockTop.Dock = DockStyle.Top;
                this.checkBoxDockTop.Height = 0x18;
                this.checkBoxDockTop.FlatStyle = FlatStyle.System;
                this.checkBoxDockBottom.Appearance = Appearance.Button;
                this.checkBoxDockBottom.Dock = DockStyle.Bottom;
                this.checkBoxDockBottom.Height = 0x18;
                this.checkBoxDockBottom.FlatStyle = FlatStyle.System;
                this.checkBoxDockFill.Appearance = Appearance.Button;
                this.checkBoxDockFill.Dock = DockStyle.Fill;
                this.checkBoxDockFill.FlatStyle = FlatStyle.System;
                base.Controls.AddRange(new Control[] { this.checkBoxDockFill, this.checkBoxDockBottom, this.checkBoxDockTop, this.checkBoxDockRight, this.checkBoxDockLeft, this.checkBoxFloat });
                base.Size = new Size(160, 0x90);
                this.BackColor = SystemColors.Control;
                base.ResumeLayout();
            }

            public void SetStates(UPlan.Frame.DockForm.DockAreas dockAreas)
            {
                this.m_oldDockAreas = dockAreas;
                if ((dockAreas & UPlan.Frame.DockForm.DockAreas.DockLeft) != 0)
                {
                    this.checkBoxDockLeft.Checked = true;
                }
                if ((dockAreas & UPlan.Frame.DockForm.DockAreas.DockRight) != 0)
                {
                    this.checkBoxDockRight.Checked = true;
                }
                if ((dockAreas & UPlan.Frame.DockForm.DockAreas.DockTop) != 0)
                {
                    this.checkBoxDockTop.Checked = true;
                }
                if ((dockAreas & UPlan.Frame.DockForm.DockAreas.DockTop) != 0)
                {
                    this.checkBoxDockTop.Checked = true;
                }
                if ((dockAreas & UPlan.Frame.DockForm.DockAreas.DockBottom) != 0)
                {
                    this.checkBoxDockBottom.Checked = true;
                }
                if ((dockAreas & UPlan.Frame.DockForm.DockAreas.Document) != 0)
                {
                    this.checkBoxDockFill.Checked = true;
                }
                if ((dockAreas & UPlan.Frame.DockForm.DockAreas.Float) != 0)
                {
                    this.checkBoxFloat.Checked = true;
                }
            }

            public UPlan.Frame.DockForm.DockAreas DockAreas
            {
                get
                {
                    UPlan.Frame.DockForm.DockAreas areas = 0;
                    if (this.checkBoxFloat.Checked)
                    {
                        areas |= UPlan.Frame.DockForm.DockAreas.Float;
                    }
                    if (this.checkBoxDockLeft.Checked)
                    {
                        areas |= UPlan.Frame.DockForm.DockAreas.DockLeft;
                    }
                    if (this.checkBoxDockRight.Checked)
                    {
                        areas |= UPlan.Frame.DockForm.DockAreas.DockRight;
                    }
                    if (this.checkBoxDockTop.Checked)
                    {
                        areas |= UPlan.Frame.DockForm.DockAreas.DockTop;
                    }
                    if (this.checkBoxDockBottom.Checked)
                    {
                        areas |= UPlan.Frame.DockForm.DockAreas.DockBottom;
                    }
                    if (this.checkBoxDockFill.Checked)
                    {
                        areas |= UPlan.Frame.DockForm.DockAreas.Document;
                    }
                    if (areas == 0)
                    {
                        return this.m_oldDockAreas;
                    }
                    return areas;
                }
            }
        }
    }
}

