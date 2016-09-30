using System.Diagnostics;
using System.IO;

namespace WebShare.Server.Util.Logging
{
    class FileLogger : ILoggerTarget
    {
        private static string LogFilePath { get; set; } = @"Server\Util\Logging\log.txt";

        private static StreamWriter sw;

        public FileLogger()
        {
            FileStream fs = File.Open(LogFilePath, FileMode.Append, FileAccess.Write);
            sw = new StreamWriter(fs) { AutoFlush = true };
        }

        public void Log(string message)
        {
            sw.WriteLine(message);
        }
    }
}
