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
            Constants.STATIC_RUN_COUNTER++;
            try
            {
                Utils util = new Utils();
                ConfigObj cfg = util.readConfig();
                if (cfg != null && cfg.remoteTarget != null && cfg.remoteServer != null && cfg.strId != null)
                {
                    String data = util.getUrlStatusCode(cfg.remoteTarget);
                    String code = util.getUrlStatusCode(cfg.remoteServer + "?action=ACK&data=" + data + "&strId=" + cfg.strId + "&errorcounter=" + Constants.STATIC_ERROR_COUNTER + "&runcounter=" + Constants.STATIC_RUN_COUNTER);
                    if (code != null)
                    {
                        if (!code.Equals("OK")) Constants.STATIC_ERROR_COUNTER++;
                    }
                    else Constants.STATIC_ERROR_COUNTER++;
                }
            }
            catch (Exception e)
            {
                Constants.STATIC_ERROR_COUNTER++;
                throw e;
            }
            
        }
    }
}
