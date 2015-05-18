using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace open_audit_service
{
    static class OpenAuditService
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                string parameter = string.Concat(args);
                switch (parameter)
                {
                    case "--install":
                        ManagedInstallerClass.InstallHelper(new string[] { Assembly.GetExecutingAssembly().Location });
                        break;
                    case "--uninstall":
                        ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                        break;
                    case "--start":
                        serviceStart();
                        break;
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new OpenAuditServiceImpl()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }

        /// <summary>
        /// Starts the OpenAudit service
        /// </summary>
        private static void serviceStart()
        {
            ServiceController[] scServices;
            scServices = ServiceController.GetServices();

            foreach (ServiceController scTemp in scServices)
            {
                // checks if the service is installed on the users machine
                if (scTemp.ServiceName == "open-audit-service")
                {
                    ServiceController sc = new ServiceController("open-audit-service");
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        int tries = 0;
                        sc.Start();
                        while (sc.Status == ServiceControllerStatus.Stopped && tries < 5)
                        {
                            Thread.Sleep(1000);
                            sc.Refresh();
                            tries++;
                        }
                    }

                }
            }
        }
    }
}
