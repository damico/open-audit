using open_audit_lib;
using open_audit_lib.threads;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Timers;

namespace open_audit_check_service
{
    public partial class OpenAuditCheckServiceImpl : ServiceBase
    {
        private bool started = false;
        private Utils util = new Utils();
        private static System.Timers.Timer aTimer;
        public OpenAuditCheckServiceImpl()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                aTimer = new System.Timers.Timer(10000);
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

                int interval = 300000;
                aTimer.Interval = interval;
                aTimer.Enabled = true;
                if (!started)
                {
                    runThread();
                }
            }
            catch (Exception ex)
            {
                util.writeEventLog(ex.Message);
                util.writeEventLog(ex.StackTrace);
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (aTimer != null)
                {
                    aTimer.Enabled = false;
                    aTimer.Dispose();
                    aTimer.Close();
                }
            }
            catch (Exception ex)
            {
                util.writeEventLog(ex.Message);
                util.writeEventLog(ex.StackTrace);
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                runThread();
            }
            catch (Exception ex)
            {
                util.writeEventLog(ex.Message);
                util.writeEventLog(ex.StackTrace);
            }

        }

        private void runThread()
        {
            CheckServiceThreadImpl serviceTimpl = new CheckServiceThreadImpl("open-audit-service");
            Thread serviceThread = new Thread(new ThreadStart(serviceTimpl.runServiceCheck));
            serviceThread.Start();
            started = true;
        }
    }
}
