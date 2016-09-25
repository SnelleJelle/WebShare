using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShare.Server.Logging
{
    class FileLogger : ILoggerTarget
    {
        public string LogFilePath { get; set; } = @"Server\Logging\log.txt";

        public void Log(string message)
        {
            using (StreamWriter sw = File.AppendText(LogFilePath))
            {
                sw.WriteLine("message");
            }
        }
    }
}
