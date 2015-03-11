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
    class OpenAuditUpdater
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        static Utils util = new Utils();

        static void Main(string[] args)
        {
            try
            {

                util.writeToLogFile("Starting Update");
                #region Hide Console Window In Order To Update Quietly
                var handle = GetConsoleWindow();
                ShowWindow(handle, SW_HIDE);
                #endregion

                #region If the computer has internet working
                util.writeToLogFile("Testing Internet Connection");
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() &&
                    new System.Net.NetworkInformation.Ping().Send("www.eigmercados.com.br").Status == IPStatus.Success)
                {
                    util.writeToLogFile("There is Internet Connection");

                    #region Check If Service are Stopped, if not stop them
                    util.writeToLogFile("Stopping services");
                    stopService("open-audit-service");
                    stopService("open-audit-check-service");
                    #endregion


                    #region Download Files
                    try
                    {

                        util.writeToLogFile("Creating temporary directory");
                        if (Directory.Exists(util.getPfPath() + "\\temp"))
                            Directory.Delete(util.getPfPath() + "\\temp", true);
                        Directory.CreateDirectory(util.getPfPath() + "\\temp");


                        util.writeToLogFile("Downloading files...");
                        String mainPath = "https://eigmercados.com.br/open_audit/update/";
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
                        Main(null);
                    }
                    #endregion

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


                        #region Clean Temp Directory

                        util.writeToLogFile("Cleaning Temporary Files");
                        Directory.Delete(util.getPfPath() + "\\temp", true);

                        #endregion

                        #region Start Services
                        util.writeToLogFile("Starting services again");
                        startService("open-audit-service");
                        startService("open-audit-check-service");
                        util.writeToLogFile("DONE!");
                        #endregion

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
                #endregion
            }
            catch (Exception e)
            {
                util.writeToLogFile(e.Message);
                util.writeToLogFile(e.StackTrace);
            }

        }

        private static void stopService(string serviceName)
        {
            try
            {
                util.writeToLogFile("Trying stop " + serviceName);
                ServiceController sc = new ServiceController(serviceName);
                if (sc.Status != ServiceControllerStatus.Stopped)
                    sc.Stop();
                Thread.Sleep(3000);
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
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
                    stopService(serviceName);
            }
            catch (Exception e)
            {
                util.writeToLogFile(e.Message);
                util.writeToLogFile(e.StackTrace);
            }
        }

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

        private static bool replaceFiles()
        {
            bool ret = true;
            try
            {
                File.Copy(util.getPfPath() + "\\temp\\open-audit-service.exe", util.getPfPath() + "\\open-audit-service.exe", true);
                File.Copy(util.getPfPath() + "\\temp\\open-audit-config.exe", util.getPfPath() + "\\open-audit-config.exe", true);
                File.Copy(util.getPfPath() + "\\temp\\open-audit-check-service.exe", util.getPfPath() + "\\open-audit-check-service.exe", true);
                File.Copy(util.getPfPath() + "\\temp\\open-audit.conf", util.getPfPath() + "\\conf\\open-audit.conf", true);
                File.Copy(util.getPfPath() + "\\temp\\open-audit-lib.dll", util.getPfPath() + "\\open-audit-lib.dll", true);
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
