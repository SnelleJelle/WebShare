using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShare.Server.Logging;

namespace WebShare.Server
{
    class Logger
    {
        public static List<ILoggerTarget> Targets { get; set; } = new List<ILoggerTarget>();

        internal static void Log(string message)
        {
            foreach(ILoggerTarget target in Targets)
            {
                target.Log(message);
            }
        }
    }
}
