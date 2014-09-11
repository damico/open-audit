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
        
        private static System.Timers.Timer aTimer;
        public OpenAuditServiceImpl()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                aTimer = new System.Timers.Timer(10000);
                aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

                int interval = 3600000;
                aTimer.Interval = interval;
                aTimer.Enabled = true;
            }
            catch (Exception)
            { }
            
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
            catch (Exception)
            {}
            
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                ServiceThreadImpl serviceTimpl = new ServiceThreadImpl();
                Thread serviceThread = new Thread(new ThreadStart(serviceTimpl.run));
                serviceThread.Start();
            }
            catch (Exception)
            { }
            
        }
    }
}
