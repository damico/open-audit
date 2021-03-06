﻿using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace open_audit_check_service
{
    static class OpenAuditCheckService
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (System.Environment.UserInteractive)
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
                new OpenAuditCheckServiceImpl() 
            };
                ServiceBase.Run(ServicesToRun);
            }
        }


        private static void serviceStart()
        {
            ServiceController[] scServices;
            scServices = ServiceController.GetServices();

            foreach (ServiceController scTemp in scServices)
            {

                if (scTemp.ServiceName == "open-audit-check-service")
                {

                    ServiceController sc = new ServiceController("open-audit-check-service");
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
