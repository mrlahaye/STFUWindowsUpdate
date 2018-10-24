using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Diagnostics;
using System.Threading;
using System.Timers;


namespace STFUWindowsUpdate
{
    class Program
    {
        static System.Timers.Timer timer1 = new System.Timers.Timer();
        static void Main(string[] args)
        {
            int i = 0;
            while (true)
            {
                Console.Clear();
                string status = ServiceStatus("wuauserv");
                if (status == "Running")
                {
                    StopService("wuauserv");
                    i++;
                    Console.WriteLine("A new instance of the service has been closed at : " + DateTime.Now);
                }
                Console.WriteLine("Service was closed " + i + " time(s).");
                Console.WriteLine("Last checked on : " + DateTime.Now + "-> Next check in 1 minute");
                System.Threading.Thread.Sleep(60000);
            }
        }
        static string ServiceStatus(string serviceName)
        {
            var sc = new ServiceController(serviceName);
            string status = sc.Status.ToString();
            return status;

        }
        static void StopService(string serviceName)
        {
            string svcStatusWas = "";
            var sc = new ServiceController(serviceName);
            try
            {
                Console.WriteLine("Stopping Service : " + serviceName);
                sc.Stop();
                while (sc.Status.ToString() != "Stopped")
                {
                    svcStatusWas = sc.Status.ToString();
                    sc.Refresh();
                    Console.WriteLine("Stopping Service in progress: " + serviceName);
                    System.Threading.Thread.Sleep(5000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot interact with service, please relaunch as admin. Press any key to kill the process");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
