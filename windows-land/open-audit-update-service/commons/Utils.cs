using open_audit_update_service.dataobjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Xml;

namespace open_audit_update_service
{
    public class Utils
    {


        public String getUrlStatusCode(String url)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            String ret = null;
            HttpStatusCode result = default(HttpStatusCode);
            try
            {
                var request = HttpWebRequest.Create(url);
                request.Method = "HEAD";
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        result = response.StatusCode;
                        response.Close();
                        ret = result.ToString();
                    }
                }
            }
            catch (Exception e)
            {   
                throw e;
            }

            return ret;
        }

        public String getAssemblyVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }

        public String getTextFromUrl(String urlStr)
        {

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
            String ret = null;

            try
            {


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlStr);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    TextReader reader = new StreamReader(response.GetResponseStream());
                    ret = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return ret;
        }

        


        public void writeConfig(ConfigObj config, String configPath)
        {
            StringBuilder sb = null;
            try
            {

                sb = new StringBuilder();

                sb.Append("<?xml version='1.0' encoding='UTF-8' standalone='yes'?>\n");
                sb.Append(" <openaudit>\n");
                sb.Append("     <config strId='"+config.strId+"' remoteServer='"+config.remoteServer+"' remoteTarget='"+config.remoteTarget+"' version='"+config.version+"' uploadUrl='"+config.uploadUrl+"' downloadUrl='"+config.downloadUrl+"' >\n");
                sb.Append("     </config>\n");
                sb.Append(" </openaudit>\n");

                if (File.Exists(configPath)) File.Delete(configPath);
                using (StreamWriter outfile = new StreamWriter(configPath))
                {
                    outfile.Write(sb.ToString());
                    outfile.Close();
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                if (sb != null) sb = null;
            }
        }

        public void writeConfig(String config, String configPath)
        {
            try
            {
                if (File.Exists(configPath)) File.Delete(configPath);
                using (StreamWriter outfile = new StreamWriter(configPath))
                {
                    outfile.Write(config);
                    outfile.Close();
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public ConfigObj parseConfig(String configXml)
        {
            ConfigObj config = new ConfigObj();
            try
            {
                using (XmlReader reader = XmlReader.Create(new StringReader(configXml)))
                {
                    reader.ReadToFollowing("config");
                    config.strId = reader.GetAttribute("strId");
                    config.remoteServer = reader.GetAttribute("remoteServer");
                    config.remoteTarget = reader.GetAttribute("remoteTarget");
                    config.downloadUrl = reader.GetAttribute("downloadUrl");
                    config.uploadUrl = reader.GetAttribute("uploadUrl");
                    config.version = reader.GetAttribute("version");
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return config;
        }

        public void writeEventLog(string sEvent)
        {
            String sSource;
            String sLog;

            sSource = Constants.APP_NAME+"_Event";
            sLog = "Application";

            bool sourceExist = false;
            bool crash = true;

            try
            {
                if (EventLog.SourceExists(sSource))
                {
                    sourceExist = true;
                    crash = false;
                }
            }
            catch (Exception)
            {}

            try
            {
                if (!sourceExist && !crash)
                {
                    EventLog.CreateEventSource(sSource, sLog);
                    EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Error);
                }
            }
            catch (Exception)
            {}
            

            writeToLogFile(sEvent);
        }

        public String getPfPath()
        {
            String pFPath = null;
            String prePath86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\open-audit\";
            String prePath64 = Environment.GetEnvironmentVariable("ProgramFiles") + @"\open-audit\";
            if (Directory.Exists(prePath86)) pFPath = prePath86;
            else if (Directory.Exists(prePath64)) pFPath = prePath64;
            return pFPath;
        }

        public void writeToLogFile(String logLine) {

            try
            {
                DateTime dt = DateTime.Now;
                String timeStamp = String.Format("{0:s}", dt);  // "2008-03-09T16:05:07" 
                File.AppendAllText(getLogPath(), timeStamp+" - "+logLine + "\n");
            }
            catch (Exception)
            {}
            

        }

        public String getLogPath()
        {
            return getPfPath() + "\\" + Constants.LOG_NAME;
        }

        public ConfigObj readConfig()
        {
            ConfigObj config = new ConfigObj();
            try
            {
                StringBuilder output = new StringBuilder();

                String xmlString = getTextFromFile(getConfPath(Constants.CONF_PATH));
                if (xmlString != null)
                {
                    config = parseConfig(xmlString);
                }
    
            }
            catch (Exception e)
            {
                throw e;
            }

            return config;
        }

        public String getConfPath(String targetPath)
        {
            String confPath = null;
            try
            {
                String prePath = getPfPath() + "\\" + targetPath;

                if (File.Exists(prePath)) confPath = prePath;
            }
            catch (Exception e)
            {
                writeToLogFile(e.Message + " - " + e.StackTrace);
            }
            return confPath;
        }

        public String getTextFromFile(String filePath)
        {
            String ret = null;
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    ret = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return ret;
        }

        public void deleteFileBiggerThan(long fileSize, String filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                long realSize = fileInfo.Length;
                if (realSize > fileSize)
                {
                    File.Delete(filePath);
                    writeToLogFile("Log file deleted with " + realSize + " length.");
                }
            }
            catch (Exception)
            { }
            

        }
    }


}
