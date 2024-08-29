using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceC
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            string logFilePath = @"C:\Users\LENOVO\source\repos\WindowsServiceC\EntryFile.txt";
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
            File.AppendAllText(logFilePath, $"Service started at {DateTime.Now}\n");
        }

        protected override void OnStop()
        {
            string logFilePath = @"C:\Users\LENOVO\source\repos\WindowsServiceC\EntryFile.txt";
            File.AppendAllText(logFilePath, $"Service stopped at {DateTime.Now}\n");
        }
    }
}
