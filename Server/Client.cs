using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebShare.Server
{
    public class Client
    {
        public string IP { get; set; }
        public bool Allowed { get; set; }
        public string Hostname { get; set; }

        public Client()
        {

        }
        public Client(string ip)
        {
            IP = ip;
        }
        //fishy, public constructor in public class? Double usage?
        public static Client FromEndpoint(string ip, string hostname)
        {

            return new Client
            {
                IP = ip,
                Hostname = hostname,
                Allowed = false
            };
        }

        public override string ToString()
        {
            return IP;
        }
    }

   
}
