using open_audit_lib;
using open_audit_lib.threads;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;

namespace open_audit_service
{
    public partial class OpenAuditServiceImpl : ServiceBase
    {
        private Utils util = new Utils();
        private static System.Timers.Timer aTimer;
        public OpenAuditServiceImpl()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                aTimer = new System.Timers.Timer();
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

                int interval = 3600000;
                aTimer.Interval = interval;
                aTimer.Enabled = true;

                if (Constants.STATIC_RUN_COUNTER == 0)
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
            ServiceThreadImpl serviceTimpl = new ServiceThreadImpl();
            Thread serviceThread = new Thread(new ThreadStart(serviceTimpl.runHeartBeat));
            serviceThread.Start();

            if (Constants.STATIC_RUN_COUNTER > 0)
            {
                CheckServiceThreadImpl checkServiceTimpl = new CheckServiceThreadImpl("open-audit-check-service");
                Thread checkServiceThread = new Thread(new ThreadStart(checkServiceTimpl.run));
                checkServiceThread.Start(); 
            }
        }
    }
}
