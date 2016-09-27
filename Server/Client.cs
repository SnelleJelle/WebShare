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
        public string IP { get; set; }
        public bool Allowed { get; set; }

        public Client()
        {

        }
        public Client(string ip)
        {
            IP = ip;
        }

        public static Client FromEndpoint(string ip)
        {

            return new Client
            {
                IP = ip
            };
        }

        public override string ToString()
        {
            return IP;
        }
    }

   
}
