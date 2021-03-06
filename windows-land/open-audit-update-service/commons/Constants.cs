﻿using System;
using System.Collections.Generic;
using System.Text;

namespace open_audit_update_service
{
    public class Constants
    {
        public static int STATIC_ERROR_COUNTER = 0;
        public static int STATIC_RUN_COUNTER = 0;
        public const String CONF_PATH = @"conf\open-audit.conf";
        public const String UP_PATH = @"conf\up.dat";
        public const String APP_NAME = "Open Audit";

        public static int LOG_FILE_MAX_SIZE = 5120;
        public const String LOG_NAME = "open-audit-update.log";
    }
}
