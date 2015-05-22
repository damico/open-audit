using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace open_audit_lib
{
    public static class WebUtil
    {
        public static double DownloadSpeed(bool dumpData = true, string url = "http://fisica.ufpr.br/kurumin/kurumin-7.0.iso", long startSize = 1024)
        {
            try
            {
                WebClient client = new WebClient();

                Stopwatch sw = Stopwatch.StartNew();
                bool hasValue = false;
                bool next = false;
                double speed = 0;
                client.DownloadDataAsync(new Uri(url));

                if (dumpData)
                {
                    Console.WriteLine("Testing download with file size " + (startSize / 1024.0).ToString("0.#") + " kbs");
                }

                bool dumped = false;

                client.DownloadProgressChanged += delegate (object sender, DownloadProgressChangedEventArgs e)
                {
                    if (e.BytesReceived >= startSize)
                    {
                        client.CancelAsync();

                        if (!dumped)
                        {
                            dumped = true;

                            double time = sw.Elapsed.TotalSeconds;
                            if (time < 7)
                            {
                                if (dumpData)
                                {
                                    Console.WriteLine("Finished with time " + time.ToString("0.#") + "s. Too fast");
                                }

                                sw.Stop();
                                next = true;
                                hasValue = true;
                            }
                            else
                            {
                                speed = e.BytesReceived / time / 1024;
                                hasValue = true;

                                if (dumpData)
                                {
                                    Console.WriteLine("Finished with time " + time.ToString("0.#"));
                                    Console.WriteLine("Download speed of " + speed + " kbps");
                                }
                            }
                        }
                    }
                };
                client.DownloadDataCompleted += delegate (object sender, DownloadDataCompletedEventArgs e)
                {
                    if (!e.Cancelled)
                    {
                        hasValue = true;
                        next = false;

                        double time = sw.Elapsed.TotalSeconds;
                        speed = e.Result.Length / time / 1024;
                    }
                };

                while (!hasValue)
                {
                }

                if (next)
                {
                    return DownloadSpeed(dumpData, url, startSize * 2);
                }
                else
                {
                    return speed;
                }
            }
            catch
            {
                return -1;
            }
        }
    }
}
