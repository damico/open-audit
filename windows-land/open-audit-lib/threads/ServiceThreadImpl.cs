using open_audit_lib.dataobjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceProcess;
using System.Text;

namespace open_audit_lib.threads
{
    public class ServiceThreadImpl
    {
        public void runUploadTrafficSensor()
        {
            double s;
            runUploadTrafficSensor(1024, out s);
        }
        /// <summary>
        /// Measures the upload speed
        /// </summary>
        public void runUploadTrafficSensor(long uploadSize, out double speed)
        {
            speed = -1;
            Stopwatch sw = new Stopwatch();
            Utils util = new Utils();
            String content = null;
            byte[] result = null;
            long elapsed = -1L;
            try
            {

                ConfigObj cfg = util.readConfig();
                System.Net.WebClient Client = new System.Net.WebClient();
                Client.Headers.Add("Content-Type", "binary/octet-stream");

                byte[] updata = new byte[uploadSize];
                Random rng = new Random();
                for (int i = 0; i < uploadSize; i++)
                {
                    updata[i] = (byte)rng.Next(0, 255);
                }

                sw.Start();
                result = Client.UploadData(cfg.uploadUrl, "POST", updata);
                sw.Stop();

                elapsed = sw.ElapsedMilliseconds;
                speed = uploadSize / sw.Elapsed.TotalSeconds / 1024.0;

                content = System.Text.Encoding.UTF8.GetString(result, 0, result.Length);
                if (content.Equals("OK"))
                {
                    util.getUrlStatusCode(cfg.remoteServer + "?action=UP&version=" + cfg.version + "&strId=" + cfg.strId + "&elapsed=" + elapsed);
                }

            }
            catch (Exception e)
            {
                util.writeEventLog(e.Message);
                util.writeEventLog(e.StackTrace);
            }
        }


        public bool runDownloadTrafficSensor()
        {
            double s;
            return runDownloadTrafficSensor(out s);
        }

        /// <summary>
        /// Measures the download speed
        /// </summary>
        /// <returns></returns>
        public bool runDownloadTrafficSensor(out double speed)
        {
            speed = -1;

            Boolean hasNewVersion = false;
            Utils util = new Utils();
            Stopwatch sw = new Stopwatch();
            String content = null;
            long elapsed = -1L;
            try
            {
                ConfigObj cfg = util.readConfig();

                sw.Start();
                content = util.getTextFromUrl(cfg.downloadUrl);
                sw.Stop();
                if (content.Length == 1048586)
                {
                    elapsed = sw.ElapsedMilliseconds;
                    long size = Encoding.ASCII.GetByteCount(content);
                    speed = size / sw.Elapsed.TotalSeconds / 1024;

                    util.getUrlStatusCode(cfg.remoteServer + "?action=DW&version=" + cfg.version + "&strId=" + cfg.strId + "&elapsed=" + elapsed);

                    // Check if the current service version is equals to server service version

                    String[] words = content.Split(' ');
                    String serverVersion = null;
                    if (words.Length > 0)
                    {
                        serverVersion = words[0];

                        if (!serverVersion.Equals(cfg.version))
                        {
                            hasNewVersion = true;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                util.writeEventLog(e.Message);
                util.writeEventLog(e.StackTrace);
            }

            return hasNewVersion;

        }

        /// <summary>
        /// Sends a heart-beat to the server, while also checking for updates
        /// </summary>
        public void runHeartBeat()
        {
            Boolean hasNewVersion = false;
            Constants.STATIC_RUN_COUNTER++;
            Utils util = new Utils();
            try
            {
                ConfigObj cfg = util.readConfig();
                if (cfg != null && cfg.remoteTarget != null && cfg.remoteServer != null && cfg.strId != null)
                {
                    String data = util.getUrlStatusCode(cfg.remoteTarget);
                    String link = cfg.remoteServer + "?action=ACK&data=" + data + "&version=" + util.getAssemblyVersion() + "&strId=" + cfg.strId + "&errorcounter=" + Constants.STATIC_ERROR_COUNTER + "&runcounter=" + Constants.STATIC_RUN_COUNTER;
                    link = link.Trim();
                    String code = util.getUrlStatusCode(link);
                    util.writeToLogFile(link);
                    if (code != null)
                    {
                        if (!code.Equals("OK"))
                        {
                            Constants.STATIC_ERROR_COUNTER++;
                        }
                        else
                        {
                            hasNewVersion = runDownloadTrafficSensor();
                            runUploadTrafficSensor();
                            if (hasNewVersion)
                            {
                                updateService();
                            }
                        }
                    }
                    else
                    {
                        Constants.STATIC_ERROR_COUNTER++;
                    }
                }
            }
            catch (Exception e)
            {
                Constants.STATIC_ERROR_COUNTER++;
                util.writeEventLog(e.Message);
                util.writeEventLog(e.StackTrace);
            }
        }

        /// <summary>
        /// Executes the Update service
        /// </summary>
        private void updateService()
        {
            Utils util = new Utils();
            try
            {
                var proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = util.getPfPath();
                proc.FileName = util.getPfPath() + "\\open-audit-update-service.exe";
                proc.Verb = "runas";
                Process.Start(proc);
            }
            catch (Exception e)
            {
                util.writeEventLog(e.Message);
                util.writeEventLog(e.StackTrace);
            }
        }


    }
}
