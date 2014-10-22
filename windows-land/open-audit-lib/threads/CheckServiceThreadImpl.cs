using open_audit_lib.dataobjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.ServiceProcess;
using System.Text;

namespace open_audit_lib.threads
{
    public class CheckServiceThreadImpl
    {
        private String serviceName = null;
        public CheckServiceThreadImpl(String serviceName)
        {
            this.serviceName = serviceName;
        }


        public void run()
        {
            runServiceCheck();
            runLogCheck();
        }

        public void runLogCheck()
        {
            Utils util = new Utils();
            try
            {
                
                util.deleteFileBiggerThan(Constants.LOG_FILE_MAX_SIZE, util.getLogPath());
            }
            catch (Exception e)
            {
                util.writeEventLog(e.Message);
                util.writeEventLog(e.StackTrace);
            }
            
        }

        public void runServiceCheck()
        {
            
            Utils util = new Utils();
            ServiceController sc = null;
            try
            {
                sc = new ServiceController(serviceName);

                switch (sc.Status)
                {
                    case ServiceControllerStatus.Running:
                        break;
                    case ServiceControllerStatus.Stopped:
                        sc.Start();
                        util.writeToLogFile("service checker starting "+serviceName);
                        break;
                    case ServiceControllerStatus.Paused:
                        sc.Stop();
                        sc.Start();
                        util.writeToLogFile("service checker starting " + serviceName);
                        break;
                    case ServiceControllerStatus.StopPending:
                        sc.Stop();
                        sc.Start();
                        util.writeToLogFile("service checker starting " + serviceName);
                        break;
                    case ServiceControllerStatus.StartPending:
                        sc.Stop();
                        sc.Start();
                        util.writeToLogFile("service checker starting " + serviceName);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                util.writeEventLog(e.Message);
                util.writeEventLog(e.StackTrace);
            }
            finally
            {
                try
                {
                    if (sc != null) sc.Close();

                } catch (Exception) {}
            }

            
        }


    }
}
