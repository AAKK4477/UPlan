namespace UPlan.Common.Utility
{
    using UPlan.Common.GlobalResource;
    using System;
    using System.Windows.Forms;

    public class MessageBoxUtil
    {
        public static void ShowError(string info)
        {
            if (!string.IsNullOrEmpty(info))
            {
                MessageBox.Show(info,UPlan.Common.GlobalResource.GlobalResource.PROJECT_APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public static void ShowError(string info, params string[] param)
        {
            MessageBox.Show(string.Format(info, (object[]) param), UPlan.Common.GlobalResource.GlobalResource.PROJECT_APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }

        public static void ShowInfo(string info)
        {
            MessageBox.Show(info, UPlan.Common.GlobalResource.GlobalResource.PROJECT_APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public static void ShowInfo(string info, params string[] param)
        {
            MessageBox.Show(string.Format(info, (object[]) param), UPlan.Common.GlobalResource.GlobalResource.PROJECT_APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public static void ShowWarning(string info)
        {
            MessageBox.Show(info, UPlan.Common.GlobalResource.GlobalResource.PROJECT_APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static void ShowWarning(string info, params string[] param)
        {
            MessageBox.Show(string.Format(info, (object[]) param), UPlan.Common.GlobalResource.GlobalResource.PROJECT_APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static DialogResult ShowYesNo(string info)
        {
            return MessageBox.Show(info, UPlan.Common.GlobalResource.GlobalResource.PROJECT_APPLICATION_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult ShowYesNo(string info, params string[] param)
        {
            return MessageBox.Show(string.Format(info, (object[]) param), UPlan.Common.GlobalResource.GlobalResource.PROJECT_APPLICATION_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static DialogResult ShowYesNoCancel(string info)
        {
            return MessageBox.Show(info, UPlan.Common.GlobalResource.GlobalResource.PROJECT_APPLICATION_NAME, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        public static DialogResult ShowYesNoCancel(string info, params string[] param)
        {
            return MessageBox.Show(string.Format(info, (object[]) param), UPlan.Common.GlobalResource.GlobalResource.PROJECT_APPLICATION_NAME, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }
    }
}

