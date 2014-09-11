using open_audit_lib.dataobjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace open_audit_lib.threads
{
    public class ServiceThreadImpl
    {
        public void run()
        {
            try
            {
                Utils util = new Utils();
                ConfigObj cfg = util.readConfig();
                if (cfg != null && cfg.remoteTarget != null && cfg.remoteServer != null && cfg.strId != null)
                {
                    String data = util.getUrlStatusCode(cfg.remoteTarget);
                    util.getUrlStatusCode(cfg.remoteServer + "?action=ACK&data=" + data + "&strId=" + cfg.strId);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
    }
}
