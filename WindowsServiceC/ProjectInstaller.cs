using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.ServiceProcess;
using System.Text;

namespace WindowsServiceC
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
         //   this.BeforeUninstall += new InstallEventHandler(ProjectInstaller_BeforeUninstall);
        }


        private void ProjectInstaller_BeforeUninstall(object sender, InstallEventArgs e)
        {
            LogFun("OnBefore Uninstall triggered.....");


            // Your custom code here
            // For example, logging or cleanup tasks
           // System.Diagnostics.EventLog.WriteEntry("MyService", "Service is about to be uninstalled.");
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            try
            {
                // Create an instance of the ServiceController to manage the service
                ServiceController sc = new ServiceController(serviceInstaller1.ServiceName);
                LogFun(serviceInstaller1.ServiceName.ToString());
                // Start the service
                sc.Start();
                LogFun("Starting the Service...");

                // Wait for the service to actually start
                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
                LogFun("Waiting Complete for the 10 seconds");
                LogFun($"Service '{serviceInstaller1.ServiceName}' started successfully.");
            }
            catch (Exception ex)
            {
                LogFun($"An error occurred while starting the service: {ex.Message}");

                throw;
            }

        }




        private void serviceInstaller1_BeforeUninstall(object sender, InstallEventArgs e)
        {
            LogFun("Before Uninstalling try to start...");
            try
            {
                // Create an instance of the ServiceController to manage the service
                using (ServiceController sc = new ServiceController(serviceInstaller1.ServiceName))
                {
                    // Check if the service exists
                    if (sc.Status != ServiceControllerStatus.Stopped)
                    {
                        // Stop the service if it's running
                        LogFun($"Stopping service '{serviceInstaller1.ServiceName}'...");
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                        LogFun($"Service '{serviceInstaller1.ServiceName}' stopped successfully.");
                    }
                }

                // Uninstall the service
                using (var installer = new ServiceInstaller())
                {
                    installer.Context = new InstallContext();
                    installer.ServiceName = serviceInstaller1.ServiceName;

                    LogFun($"Uninstalling service '{serviceInstaller1.ServiceName}'...");
                    installer.Uninstall(null);
                    LogFun($"Service '{serviceInstaller1.ServiceName}' removed successfully.");
                }
            }
            catch (InvalidOperationException ex)
            {
                // Handle the case where the service is not found
                LogFun($"Service '{serviceInstaller1.ServiceName}' is not installed or already removed.");
            }
            catch (Exception ex)
            {
                LogFun($"An error occurred while stopping or uninstalling the service: {ex.Message}");
                throw;
            }
            LogFun("Before Uninstalling try to end...");

        }





        public void LogFun( string data)
        {
            string filePath = @"D:\LOGS.txt"; // Path to your log file
            if (!File.Exists(filePath))
            {
                using (FileStream fs = File.Create(filePath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes("Service installation Log\n");
                    fs.Write(info, 0, info.Length);
                }
            }

            string textToInsert = $"Service Uninstalled at: {DateTime.Now}";
            File.AppendAllText(filePath, data + Environment.NewLine);

        }

        private void serviceInstaller1_AfterUninstall(object sender, InstallEventArgs e)
        {
            LogFun("After Uninstalling try to start...");

            try
            {
                // Create an instance of the ServiceController to manage the service
                using (ServiceController sc = new ServiceController(serviceInstaller1.ServiceName))
                {
                    // Check if the service exists
                    if (sc.Status != ServiceControllerStatus.Stopped)
                    {
                        // Stop the service if it's running
                        LogFun($"Stopping service '{serviceInstaller1.ServiceName}'...");
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                        LogFun($"Service '{serviceInstaller1.ServiceName}' stopped successfully.");
                    }
                }

                // Uninstall the service
                using (var installer = new ServiceInstaller())
                {
                    installer.Context = new InstallContext();
                    installer.ServiceName = serviceInstaller1.ServiceName;

                    LogFun($"Uninstalling service '{serviceInstaller1.ServiceName}'...");
                    installer.Uninstall(null);
                    LogFun($"Service '{serviceInstaller1.ServiceName}' removed successfully.");
                }
            }
            catch (InvalidOperationException ex)
            {
                // Handle the case where the service is not found
                LogFun($"Service '{serviceInstaller1.ServiceName}' is not installed or already removed.");
            }
            catch (Exception ex)
            {
                LogFun($"An error occurred while stopping or uninstalling the service: {ex.Message}");
                throw;
            }
            LogFun("After Uninstalling try to end...");

        }

        private void serviceProcessInstaller1_AfterUninstall(object sender, InstallEventArgs e)
        {
            LogFun("From ProcessInstaller1 After Uninstalling try to end...");
            using (ServiceController sc = new ServiceController(serviceInstaller1.ServiceName))
            {
                // Check if the service exists
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    // Stop the service if it's running
                    LogFun($"Stopping service '{serviceInstaller1.ServiceName}'...");
                    sc.Stop();
                    //sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                    LogFun($"Service '{serviceInstaller1.ServiceName}' stopped successfully.");
                }
            }
        }

        private void serviceProcessInstaller1_BeforeUninstall(object sender, InstallEventArgs e)
        {
            LogFun("From ProcessInstaller1 Before Uninstalling try to end...");
        }
    }
}
