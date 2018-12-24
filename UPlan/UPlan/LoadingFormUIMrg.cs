namespace UPlan.Frame.View
{
    using UPlan.Frame.Interface;
    using System;
    using System.Threading;
    using System.Windows.Forms;

    public class LoadingFormUIMrg
    {
        private LoadingForm m_loadingForm;
        private Thread m_loadingFormThread;
        private Form m_MainForm = null;
        private bool m_TimeToClose;

        public LoadingFormUIMrg(Form mf)
        {
            this.m_MainForm = mf;
        }

        public void CloseLoadingForm()
        {
            try
            {
                this.m_TimeToClose = true;
                if (null != this.m_loadingFormThread)
                {
                    this.m_loadingFormThread.Join();
                }
            }
            catch (Exception exception)
            {
                WriteLog.Logger.Error(exception.StackTrace);
            }
        }

        public void ShowLoadingForm()
        {
            try
            {
                this.m_TimeToClose = false;
                this.m_loadingFormThread = new Thread(new ThreadStart(this.ShowLoadingProject));
                this.m_loadingFormThread.Start();
            }
            catch (Exception exception)
            {
                WriteLog.Logger.Error(exception.StackTrace);
            }
        }

        public void ShowLoadingProject()
        {
            try
            {
                this.m_loadingForm = new LoadingForm(this.m_MainForm, this);
                this.m_loadingForm.ShowDialog();
                Thread.Sleep(10);
            }
            catch (Exception exception)
            {
                WriteLog.Logger.Error(exception.StackTrace);
            }
        }

        public bool TimeToClose
        {
            get
            {
                return this.m_TimeToClose;
            }
        }
    }
}

