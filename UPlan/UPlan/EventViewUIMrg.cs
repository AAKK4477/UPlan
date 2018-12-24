namespace UPlan.Frame.View
{
    //using 命名空间名;
    using UPlan.Frame.Interface;
    using System;
    using System.Collections.Generic;
    //灰色代表该using指令是不必要的。
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;

    public class EventViewUIMrg
    {
        private EventForm m_EventViewForm = null;
        //字段可以用readonly修饰符声明。其作用类似于将字段声明为const，一旦值被设定就不能改变。
        /*
         * readonly修饰符
         * readonly和const的区别
         * 1、const字段只能在字段的声明语句中初始化，而readonly字段可以在下列任意位置设置它的值。
         *    字段声明语句初始化：类似const。
         *    类的任何构造函数：如果是static字段，初始化必须在静态构造函数中完成。
         * 2、const字段的值必须在编译时决定，而readonly字段的值可以在运行时决定。这种增加的自由性
         * 允许你在不同的环境或不同的构造函数中设置不同的值！
         * 3、和const不同，const的行为（方法）总是静态的，而对于readonly字段有以下两点：
         *     它可以是实例字段，也可以是静态字段。
         *     它在内存中有存储位置。
         */
        private static readonly object m_LockObj = new object();
        private MainForm m_MainForm = null;
        private static Dictionary<string, UPlan.Frame.View.ValuePair> m_NewProgressBar = new Dictionary<string, UPlan.Frame.View.ValuePair>();
        private Timer m_ProgressTimer = new Timer();
        private static List<string> m_RemoveProgressBar = new List<string>();

        public EventViewUIMrg(MainForm mf, EventForm ef)
        {
            this.m_MainForm = mf;
            this.m_EventViewForm = ef;
        }

        private void AddNewProgressBar(string title)
        {
            if (null == this.m_MainForm.EventWindow.PgrBarMgr.GetProgressBarX(title))
            {
                this.m_MainForm.EventWindow.PgrBarMgr.AddProgressBarX(title);
                this.m_MainForm.EventWindow.PgrBarMgr.GetProgressBarX(title).Value = m_NewProgressBar[title].Minimum;
                this.m_MainForm.EventWindow.PgrBarMgr.GetProgressBarX(title).Minimum = m_NewProgressBar[title].Minimum;
                this.m_MainForm.EventWindow.PgrBarMgr.GetProgressBarX(title).Maximum = m_NewProgressBar[title].Maximum;
            }
        }

        private void AddNewProgressBars()
        {
            foreach (string str in m_NewProgressBar.Keys)
            {
                this.AddNewProgressBar(str);
            }
        }

        private void AddProgress(string title, int minimum, int maximum)
        {
            WriteLog.Logger.Info("AddProgress start:" + title);
            lock (m_LockObj)
            {
                WriteLog.Logger.Info("AddProgress enter:" + title);
                m_NewProgressBar.Add(title, new ENet.Frame.View.ValuePair(string.Empty, minimum, maximum, minimum));
            }
            WriteLog.Logger.Info("AddProgress end:" + title);
        }

        private void ChangeNewProgressStatus(string title, int value, string text)
        {
            if (m_NewProgressBar.ContainsKey(title))
            {
                m_NewProgressBar[title].Value = value;
                m_NewProgressBar[title].Text = text;
            }
            else
            {
                WriteLog.Logger.Warn(title + " has been removed!");
            }
        }

        public void InitEventWindowProgress(string title, int minimum, int maximum)
        {
            try
            {
                //Invoke方法是为了确保不发生线程冲突，Invoke方法需要创建一个委托。
                this.m_MainForm.Invoke(new InitProgressScopeDelegate(this.InitProgressScope), new object[] { title, minimum, maximum });
            }
            catch (Exception exception)
            {
                WriteLog.Logger.Error(exception.Message, exception);
            }
        }

        private void InitProgressScope(string title, int minimum, int maximum)
        {
            this.m_MainForm.EventWindow.EnableProgress();
            this.AddProgress(title, minimum, maximum);
            this.m_MainForm.MapToolWindow2Menu[this.m_MainForm.EventWindow].CheckState = CheckState.Checked;
            this.m_MainForm.EventWindow.Show();
            if (!this.m_ProgressTimer.Enabled)
            {
                this.m_ProgressTimer.Start();
            }
        }

        public void OnProgressTimer(object sender, EventArgs e)
        {
            this.m_MainForm.EventWindow.BeginInvoke(new UpdateAddProgressDelegate(this.UpdateAddProgress), new object[0]);
            this.m_MainForm.EventWindow.BeginInvoke(new UpdateRemoveProgressDelegate(this.UpdateRemoveProgress), new object[0]);
            this.m_MainForm.EventWindow.BeginInvoke(new UpdateReportProgressDelegate(this.UpdateReportProgress), new object[0]);
            this.m_MainForm.EventWindow.BeginInvoke(new UpdateProgressTimerDelegate(this.UpdateProgressTimer), new object[0]);
        }

        private void RemoveProgressBars()
        {
            foreach (string str in m_RemoveProgressBar)
            {
                this.m_MainForm.EventWindow.PgrBarMgr.RemoveProgress(str);
            }
        }

        private void ReportAllProgress()
        {
            foreach (string str in m_NewProgressBar.Keys)
            {
                this.m_MainForm.EventWindow.PgrBarMgr.ReportProgress(str, m_NewProgressBar[str].Value, m_NewProgressBar[str].Text);
            }
        }

        public void ReportProgress(string title, int value, string text)
        {
            WriteLog.Logger.Info("ReportProgress start:" + title);
            lock (m_LockObj)
            {
                WriteLog.Logger.Info("ReportProgress enter:" + title);
                this.ChangeNewProgressStatus(title, value, text);
            }
            WriteLog.Logger.Info("ReportProgress end:" + title);
        }

        public void ResetProgress(string title)
        {
            WriteLog.Logger.Info("ResetProgress start:" + title);
            lock (m_LockObj)
            {
                WriteLog.Logger.Info("ResetProgress enter:" + title);
                m_RemoveProgressBar.Add(title);
                m_NewProgressBar.Remove(title);
            }
            WriteLog.Logger.Info("ResetProgress end:" + title);
        }

        private void UpdateAddProgress()
        {
            lock (m_LockObj)
            {
                this.AddNewProgressBars();
            }
        }

        private void UpdateProgressTimer()
        {
            if ((m_NewProgressBar.Keys.Count == 0) && this.m_ProgressTimer.Enabled)
            {
                this.m_ProgressTimer.Stop();
            }
        }

        private void UpdateRemoveProgress()
        {
            lock (m_LockObj)
            {
                this.RemoveProgressBars();
                m_RemoveProgressBar.Clear();
            }
        }

        private void UpdateReportProgress()
        {
            /*lock关键字可以用来确保代码块完成运行，而不会被其他线程中断。
              这是通过在代码块运行期间为给定对象获取互斥锁来实现的。
              lock(obj){
                 //其他代码         
              }
              若有线程A和线程B，如果线程A先执行，线程B稍微慢一点。线程A执行到lock语句，
              判断obj是否已申请了互斥锁,这时假设线程B启动了，而线程A还未执行完lock里面
              的代码，线程B执行到lock语句，检查到obj已经申请了互斥锁，于是等待，直到线
              程A执行完毕，释放互斥锁，线程B 才能申请新的互斥锁并执行lock里面的代码。
            */
            lock (m_LockObj)
            {
                this.ReportAllProgress();
            }
        }

        public Timer ProgressTimer
        {
            get
            {
                return this.m_ProgressTimer;
            }
            set
            {
                this.m_ProgressTimer = value;
            }
        }
        /*
         * 委托是一个类，可以将方法作为参数来进行传递，这种将方法动态地赋给参数的做法，可以避免在程序中
         * 大量的使用if-else(swith)语句，使得程序具有更好的可扩展性。
         * 委托是引用类型，因此有引用和对象。
         * 用关键字delegat来声明委托类型，委托声明和方法声明相似，只是没有实现块，
         * 且不需要在类内部声明因为它是类型声明。。
         * 使用该委托类型声明一个委托变量。
        */
        private delegate void InitProgressScopeDelegate(string title, int minimum, int maximum);

        private delegate void UpdateAddProgressDelegate();

        private delegate void UpdateProgressTimerDelegate();

        private delegate void UpdateRemoveProgressDelegate();

        private delegate void UpdateReportProgressDelegate();
    }
}


