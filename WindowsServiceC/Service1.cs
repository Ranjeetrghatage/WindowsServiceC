using System;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace WindowsServiceC
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private readonly string sourcePath = @"D:\Source";
        private readonly string destinationPath = @"D:\Destination";

        protected override async void OnStart(string[] args)
        {
            while (true)
            {
                MoveFiles();
                await Task.Delay(5000);
            }
        }

        private void MoveFiles()
        {
            try
            {
                if (!Directory.Exists(destinationPath))
                {
                    Directory.CreateDirectory(destinationPath);
                }

                string[] files = Directory.GetFiles(sourcePath);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine(destinationPath, fileName);

                    File.Move(file, destFile);

                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string content = $"Moved file {fileName} from {sourcePath} to {destinationPath} at {timestamp}{Environment.NewLine}";
                    File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"), content);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string errorContent = $"Error occurred at {timestamp}: {ex.Message}{Environment.NewLine}";
                File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"), errorContent);
            }
        }

        protected override void OnStop()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string content = $"Service has been stopped by Ranjeet Ghatage at {timestamp}{Environment.NewLine}";

            try
            {
                File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"), content);
            }
            catch (Exception ex)
            {
                File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs"), ex.Message);
            }
        }
    }
}
