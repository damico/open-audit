using open_audit_lib.dataobjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace open_audit_lib
{
    public class Utils
    {
        public String getUrlStatusCode(String url)
        {
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
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return config;
        }

        public ConfigObj readConfig()
        {
            ConfigObj config = new ConfigObj();
            try
            {
                StringBuilder output = new StringBuilder();

                String xmlString = getTextFromFile(getConfPath());
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

        public String getConfPath()
        {
            String confPath = null;
            try
            {
                String prePath86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\open-audit\" + Constants.CONF_PATH;
                String prePath64 = Environment.GetEnvironmentVariable("ProgramFiles") + @"\open-audit\" + Constants.CONF_PATH;

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
