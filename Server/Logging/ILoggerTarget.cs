using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShare.Server.Logging
{
    interface ILoggerTarget
    {
        void Log(string message);
    }
}
