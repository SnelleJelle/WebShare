using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShare.Util.Server;

namespace WebShare.Server.Util.Zip
{
    class RemoveZip
    {
        public static void RemoveFile(string path)
        {
            Logger.Log("Removing file: " + path);
            File.Delete(path);
        }
    }
}
