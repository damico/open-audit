using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using open_audit_update_service;
using open_audit_update_service.dataobjects;
using System.Diagnostics;

namespace open_audit_update_service
{
    /// <summary>
    /// Updates the Open Audit software
    /// </summary>
    public class OpenAuditUpdater
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static Utils util = new Utils();

        public static readonly string EIGURL = "http://www.eigmercados.com.br/";

        static void Main(string[] args)
        {
            try
            {
                retry:

                util.writeToLogFile("Starting Update");
                
                // Hide console window ( update silently )
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);

                // Check for an internet connection
                util.writeToLogFile("Testing Internet Connection");

                if (NetworkInterface.GetIsNetworkAvailable() &&
                    new Ping().Send(EIGURL).Status == IPStatus.Success)
                {
                    util.writeToLogFile("There is Internet Connection");

                    // Check if services are stopped, if not stop them
                    util.writeToLogFile("Stopping services");
                    stopService("open-audit-service");
                    stopService("open-audit-check-service");

                    // Download Files
                    try
                    {
                        util.writeToLogFile("Creating temporary directory");
                        if (Directory.Exists(util.getPfPath() + "\\temp"))
                        {
                            Directory.Delete(util.getPfPath() + "\\temp", true);
                        }
                        Directory.CreateDirectory(util.getPfPath() + "\\temp");

                        util.writeToLogFile("Downloading files...");
                        String mainPath = EIGURL + "open_audit/update/";
                        WebClient webClient = new WebClient();
                        webClient.DownloadFile(mainPath + "open-audit-service.exe", util.getPfPath() + "\\temp\\open-audit-service.exe");
                        webClient.DownloadFile(mainPath + "open-audit-config.exe", util.getPfPath() + "\\temp\\open-audit-config.exe");
                        webClient.DownloadFile(mainPath + "open-audit-check-service.exe", util.getPfPath() + "\\temp\\open-audit-check-service.exe");
                        webClient.DownloadFile(mainPath + "open-audit.conf", util.getPfPath() + "\\temp\\open-audit.conf");
                        webClient.DownloadFile(mainPath + "open-audit-lib.dll", util.getPfPath() + "\\temp\\open-audit-lib.dll");
                    }
                    catch (Exception e)
                    {
                        util.writeToLogFile(e.Message);
                        util.writeToLogFile(e.StackTrace);
                        Thread.Sleep(30000);
                        goto retry;
                    }

                    util.writeToLogFile("Updating");

                    //BKP Obj
                    ConfigObj config = util.readConfig();
                    
                    int error_count = 0;
                    while (!replaceFiles() && error_count < 5)
                    {
                        error_count++;
                        Thread.Sleep(3000);
                    }

                    if (error_count < 5)
                    {
                        ConfigObj newConfig = util.readConfig();
                        newConfig.strId = config.strId;
                        util.writeConfig(newConfig, util.getConfPath(Constants.CONF_PATH));

                        // Clean temp directory
                        util.writeToLogFile("Cleaning Temporary Files");
                        Directory.Delete(util.getPfPath() + "\\temp", true);

                        // Start Services
                        util.writeToLogFile("Starting services again");
                        startService("open-audit-service");
                        startService("open-audit-check-service");
                        util.writeToLogFile("DONE!");
                    }
                    else
                    {
                        util.writeToLogFile("Não foi possível atualizar os arquivos baixados, tentando novamente...");
                        Thread.Sleep(5000);
                        Main(null);
                    }
                }
                else
                {
                    util.writeToLogFile("There isn't internet connection, trying again in 30s");
                    Thread.Sleep(30000);
                    Main(null);
                }
            }
            catch (Exception e)
            {
                util.writeToLogFile(e.Message);
                util.writeToLogFile(e.StackTrace);
            }

        }

        /// <summary>
        /// Stops the service with the designated name
        /// </summary>
        /// <param name="serviceName">The name of the service to stop</param>
        private static void stopService(string serviceName)
        {
            try
            {
                util.writeToLogFile("Trying to stop " + serviceName);
                ServiceController sc = new ServiceController(serviceName);
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    sc.Stop();
                }
                Thread.Sleep(3000); // wait 3 seconds
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    // The service is still open, just kill it
                    try
                    {
                        foreach (Process proc in Process.GetProcessesByName(serviceName + ".exe"))
                        {
                            proc.Kill();
                        }
                    }
                    catch (Exception e)
                    {
                        util.writeToLogFile(e.Message);
                        util.writeToLogFile(e.StackTrace);
                        stopService(serviceName);
                    }
                }
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    stopService(serviceName);
                }
            }
            catch (Exception e)
            {
                util.writeToLogFile(e.Message);
                util.writeToLogFile(e.StackTrace);
            }
        }

        /// <summary>
        /// Starts a service
        /// </summary>
        /// <param name="serviceName">The name of the service to start</param>
        private static void startService(string serviceName)
        {
            try
            {
                ServiceController sc = new ServiceController(serviceName);
                if (sc.Status != ServiceControllerStatus.StartPending)
                    sc.Start();
                if (sc.Status != ServiceControllerStatus.StartPending)
                    startService(serviceName);
            }
            catch (Exception e)
            {
                util.writeToLogFile(e.Message);
                util.writeToLogFile(e.StackTrace);
            }
        }
            
        /// <summary>
        /// Copies the files from the temp folder to the application folder
        /// </summary>
        /// <returns></returns>
        private static bool replaceFiles()
        {
            bool ret = false;
            try
            {
                File.Copy(util.getPfPath() + @"\temp\open-audit-service.exe", util.getPfPath() + @"\open-audit-service.exe", true);
                File.Copy(util.getPfPath() + @"\temp\open-audit-config.exe", util.getPfPath() + @"\open-audit-config.exe", true);
                File.Copy(util.getPfPath() + @"\temp\open-audit-check-service.exe", util.getPfPath() + @"\open-audit-check-service.exe", true);
                File.Copy(util.getPfPath() + @"\temp\open-audit.conf", util.getPfPath() + @"\conf\open-audit.conf", true);
                File.Copy(util.getPfPath() + @"\temp\open-audit-lib.dll", util.getPfPath() + @"\open-audit-lib.dll", true);
                ret = true;
            }
            catch (Exception e)
            {
                util.writeToLogFile(e.Message);
                util.writeToLogFile(e.StackTrace);
                ret = false;
            }

            return ret;
        }
    }
}
