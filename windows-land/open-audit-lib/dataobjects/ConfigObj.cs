using System;
using System.Collections.Generic;
using System.Text;

namespace open_audit_lib.dataobjects
{
    public class ConfigObj
    {
        public String remoteServer { get; set; }
        public String strId { get; set; }
        public String remoteTarget { get; set; }
        public String uploadUrl { get; set; }
        public String downloadUrl { get; set; }
        public String version { get; set; }
    }
}
