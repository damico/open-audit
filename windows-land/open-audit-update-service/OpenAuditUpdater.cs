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

        static void Main(string[] args)
        {
            #region Hide Console Window In Order To Update Quietly
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
            #endregion

            #region If the computer has internet working
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() &&
                new System.Net.NetworkInformation.Ping().Send("www.eigmercados.com.br").Status == IPStatus.Success)
            {

                #region Download Files
                try
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadFile("URL", @"path");
                }
                catch (Exception)
                {

                    throw;
                }
                #endregion


                #region Check If Service are Stopped, if not stop them
                stopService("open-audit-service");
                stopService("open-audit-check-service");
                #endregion


                #region Copy New Files
                File.Copy(getPfPath() + "\\temp\\open-audit-service.exe", getPfPath() + "\\open-audit-service.exe",true);
                File.Copy(getPfPath() + "\\temp\\open-audit-config.exe", getPfPath() + "\\open-audit-config.exe", true);
                File.Copy(getPfPath() + "\\temp\\open-audit-check-service.exe", getPfPath() + "\\open-audit-check-service.exe", true);
                File.Copy(getPfPath() + "\\temp\\open-audit.conf", getPfPath() + "\\conf\\open-audit.conf", true);
                #endregion

            }
            else
            {
                Thread.Sleep(3000);
                Main(null);
            }
            #endregion
        }

        private static void stopService(string serviceName)
        {
            try
            {
                ServiceController sc = new ServiceController(serviceName);
                if (sc.Status != ServiceControllerStatus.Stopped)
                    sc.Stop();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static String getPfPath()
        {
            String pFPath = null;
            String prePath86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + @"\open-audit\";
            String prePath64 = Environment.GetEnvironmentVariable("ProgramFiles") + @"\open-audit\";
            if (Directory.Exists(prePath86)) pFPath = prePath86;
            else if (Directory.Exists(prePath64)) pFPath = prePath64;
            return pFPath;
        }
    }
}
