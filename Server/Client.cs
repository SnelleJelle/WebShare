using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebShare.Server
{
    class Client
    {
        public IPEndPoint IP { get; set; }
        public bool Allowed { get; set; }

        public Client()
        {

        }
        public Client(IPEndPoint ip)
        {
            IP = ip;
        }

        public override string ToString()
        {
            return IP.ToString();
        }
    }

   
}
