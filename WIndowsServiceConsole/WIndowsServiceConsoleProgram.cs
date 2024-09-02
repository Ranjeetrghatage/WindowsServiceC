using System;
using System.Reflection;
using System.Diagnostics;
using System.Security.Principal;
using System.IO;

namespace WIndowsServiceConsole
{
    public static class WIndowsServiceConsoleProgram 
    {


        static bool IsRunAsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public static void Main(string[] args)
        {

            Install();

        }

        public static void Uninstall()
        {

        }

        public static void Install()
        {
            if (!IsRunAsAdmin())
            {
                ProcessStartInfo proc = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = Assembly.GetExecutingAssembly().Location,
                    Verb = "runas",
                };

                try
                {
                    Process.Start(proc);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to restart as admin: {ex.Message}");
                }
            }
            else
            {
                string systemRoot = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
                string installUtilPath = Path.Combine(systemRoot, "Microsoft.NET", "Framework64", "v4.0.30319", "InstallUtil.exe");

                string serviceExePath = @"C:\\Users\\LENOVO\\source\\repos\\WindowsServiceC\\WindowsServiceC\\bin\\Release\\WindowsServiceC.exe";


                Console.WriteLine("serviceExePath :" + serviceExePath);
                Console.ReadLine();

                // Create a Process to run the InstallUtil.exe
                Process process = new Process();
                process.StartInfo.FileName = installUtilPath;
                process.StartInfo.Arguments = $"\"{serviceExePath}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                try
                {
                    // Start the process
                    process.Start();

                    // Read the output and error (if any)
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    process.WaitForExit();

                    Console.WriteLine("Output: " + output);
                    if (!string.IsNullOrEmpty(error))
                    {
                        Console.WriteLine("Error: " + error);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    Console.WriteLine("Console Catch");
                }



            }
        }








    }
}
