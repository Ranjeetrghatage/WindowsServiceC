using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WIndowsServiceConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string serviceExePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "WindowsServiceC.exe");
            string serviceName = "YCService";
            try
            {
                if (args.Length > 0 && args[0].Equals("-install", StringComparison.OrdinalIgnoreCase))
                {
                    InstallService(serviceExePath);
                    StartService(serviceName);
                }
                else if (args.Length > 0 && args[0].Equals("-uninstall", StringComparison.OrdinalIgnoreCase))
                {
                    StopService(serviceName);
                    UninstallService(serviceExePath);
                }
                else
                {
                    Console.WriteLine("Invalid arguments");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }



        static void InstallService(string serviceExePath)
        {
            ManagedInstallerClass.InstallHelper(new string[] { serviceExePath });
        }

        static void UninstallService(string serviceExePath)
        {
            ManagedInstallerClass.InstallHelper(new string[] { "/u", serviceExePath });
        }

        static void StartService(string serviceName)
        {
            using (ServiceController sc = new ServiceController(serviceName))
            {
                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    sc.Start();
                }
            }
        }

        static void StopService(string serviceName)
        {
            using (ServiceController sc = new ServiceController(serviceName))
            {
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    sc.Stop();
                }
            }
        }




    }
}
