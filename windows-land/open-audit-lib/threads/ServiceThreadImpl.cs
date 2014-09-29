using open_audit_lib.dataobjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace open_audit_lib.threads
{
    public class ServiceThreadImpl
    {
        public void runUploadTrafficSensor()
        {
            Stopwatch sw = new Stopwatch();
            Utils util = new Utils();
            String content = null;
            byte[] result = null;
            long elapsed = -1L;
            try
            {
                
                ConfigObj cfg = util.readConfig();
                sw.Start();
                System.Net.WebClient Client = new System.Net.WebClient();
                Client.Headers.Add("Content-Type", "binary/octet-stream");
                String upPath = util.getConfPath(Constants.UP_PATH);
                result = Client.UploadFile(cfg.uploadUrl, "POST", upPath);
                content = System.Text.Encoding.UTF8.GetString(result, 0, result.Length); 
                sw.Stop();
                if (content.Equals("OK"))
                {
                    elapsed = sw.ElapsedMilliseconds;
                    util.getUrlStatusCode(cfg.remoteServer + "?action=UP&version=" + cfg.version + "&strId=" + cfg.strId + "&elapsed=" + elapsed);
                }

            }
            catch (Exception e)
            {
                util.writeEventLog(e.Message);
                util.writeEventLog(e.StackTrace);
            }

            
        }

        public void runDownloadTrafficSensor()
        {
            Utils util = new Utils();
            Stopwatch sw = new Stopwatch();
            String content = null;
            long elapsed = -1L;
            try
            {
                sw.Start();
                
                ConfigObj cfg = util.readConfig();
                content = util.getTextFromUrl(cfg.downloadUrl);
                sw.Stop();
                if (content.Length == 1048576)
                {
                    elapsed = sw.ElapsedMilliseconds;
                    util.getUrlStatusCode(cfg.remoteServer + "?action=DW&version=" + cfg.version + "&strId=" + cfg.strId + "&elapsed=" + elapsed);
                }
                
            }
            catch (Exception e)
            { 
                util.writeEventLog(e.Message);
                util.writeEventLog(e.StackTrace);
            }

           

        }

        public void runHeartBeat()
        {
            Constants.STATIC_RUN_COUNTER++;
            try
            {
                Utils util = new Utils();
                ConfigObj cfg = util.readConfig();
                if (cfg != null && cfg.remoteTarget != null && cfg.remoteServer != null && cfg.strId != null)
                {
                    String data = util.getUrlStatusCode(cfg.remoteTarget);
                    String code = util.getUrlStatusCode(cfg.remoteServer + "?action=ACK&data=" + data + "&version = "+cfg.version+"&strId=" + cfg.strId + "&errorcounter=" + Constants.STATIC_ERROR_COUNTER + "&runcounter=" + Constants.STATIC_RUN_COUNTER);
                    if (code != null)
                    {
                        if (!code.Equals("OK"))
                        {
                            Constants.STATIC_ERROR_COUNTER++;
                        }
                        else
                        {
                            runDownloadTrafficSensor();
                            runUploadTrafficSensor();
                        }
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
