using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace open_audit_lib
{
    public class Constants
    {
        public static int STATIC_ERROR_COUNTER = 0;
        public static int STATIC_RUN_COUNTER = 0;
        public const String CONF_PATH = @"conf\open-audit.conf";
        public const String UP_PATH = @"conf\up.dat";
        public const String APP_NAME = "Open Audit";

        public static int LOG_FILE_MAX_SIZE = 5120;
        public const String LOG_NAME = "open-audit.log";

        public static readonly string[] SERVICES = new string[]
        {
            "open-audit-service",
            "open-audit-check-service"
        };

        public static void KillServices(Utils util = null)
        {
            for (int i = 0; i < SERVICES.Length; i++)
            {
                string s = SERVICES[i];
                ServiceUtil.StopService(s, util);
            }
        }

        public static void StartServices(Utils util = null)
        {
            for (int i = 0; i < SERVICES.Length; i++)
            {
                string s = SERVICES[i];
                ServiceUtil.StartService(s, util);
            }
        }
    }
}
