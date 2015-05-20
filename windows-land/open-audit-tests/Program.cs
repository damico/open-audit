using open_audit_lib.threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace open_audit_tests
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceThreadImpl t = new ServiceThreadImpl();

            double speed;
            t.runDownloadTrafficSensor(out speed);
            Console.WriteLine("Download speed detected: " + speed.ToString("0.#") + "kbps");

            t.runUploadTrafficSensor(out speed);
            Console.WriteLine("Upload speed detected: " + speed.ToString("0.#") + "kbps");

            Console.ReadLine();
        }
    }
}
