using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace open_audit_lib
{
    public static class ServiceUtil
    {
        /// <summary>
        /// Stops the service with the designated name
        /// </summary>
        /// <param name="serviceName">The name of the service to stop</param>
        public static void StopService(string serviceName, Utils util = null)
        {
            try
            {
                if (util != null)
                {
                    util.writeToLogFile("Trying to stop " + serviceName);
                }
                ServiceController sc = new ServiceController(serviceName);
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    sc.Stop();
                }
                Thread.Sleep(3000); // wait 3 seconds
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    // The service is still open, just kill it
                    try
                    {
                        foreach (Process proc in Process.GetProcessesByName(serviceName + ".exe"))
                        {
                            proc.Kill();
                        }
                    }
                    catch (Exception e)
                    {
                        if (util != null)
                        {
                            util.writeToLogFile(e.Message);
                            util.writeToLogFile(e.StackTrace);
                        }
                        ServiceUtil.StopService(serviceName, util);
                    }
                }
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    ServiceUtil.StopService(serviceName, util);
                }
            }
            catch (Exception e)
            {
                if (util != null)
                {
                    util.writeToLogFile(e.Message);
                    util.writeToLogFile(e.StackTrace);
                }
            }
        }

        /// <summary>
        /// Starts a service
        /// </summary>
        /// <param name="serviceName">The name of the service to start</param>
        public static void StartService(string serviceName, Utils util = null)
        {
            try
            {
                ServiceController sc = new ServiceController(serviceName);
                if (sc.Status != ServiceControllerStatus.StartPending)
                    sc.Start();
                if (sc.Status != ServiceControllerStatus.StartPending)
                    StartService(serviceName);
            }
            catch (Exception e)
            {
                if (util != null)
                {
                    util.writeToLogFile(e.Message);
                    util.writeToLogFile(e.StackTrace);
                }
            }
        }
    }
}
