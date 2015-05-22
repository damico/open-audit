using System;
using System.Collections.Generic;
using System.Text;

namespace open_audit_lib
{
    public struct HeartbeatData
    {
        public string version;
        public double downloadSpeed;
        public double uploadSpeed;
        public string data;
        public string assVersion;
        public string strId;
        public int errorCounter;
        public int runCounter;
    }
}
