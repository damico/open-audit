using open_audit_lib.dataobjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Xml;

namespace open_audit_lib
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

            sSource = Constants.APP_NAME;
            sLog = "Application";

            if (!EventLog.SourceExists(sSource)) EventLog.CreateEventSource(sSource, sLog);

            EventLog.WriteEntry(sSource, sEvent, EventLogEntryType.Warning, 234);
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
                String prePath86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\open-audit\" + targetPath;
                String prePath64 = Environment.GetEnvironmentVariable("ProgramFiles") + @"\open-audit\" + targetPath;

                if (File.Exists(prePath86)) confPath = prePath86;
                else if (File.Exists(prePath64)) confPath = prePath64;
            }
            catch (Exception)
            { }
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
    }


}
