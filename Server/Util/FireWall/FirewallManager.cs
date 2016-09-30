using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShare.Util.Server;

namespace WebShare.Server.Util.FireWall
{
    class FirewallManager
    {
        private string RuleName;
        private HttpServer server;

        public FirewallManager(HttpServer server)
        {
            this.server = server;
            RuleName = "_WebShare_" + server.Port;
        }

        public void setFirewallRule()
        {
            if (ruleAlreadyExists())
            {
                return;
            }
            string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
            string command = string.Format("/C netsh advfirewall firewall add rule name=\"{0}\" dir=in action=allow program=\"{1}\" enable=yes",
                RuleName, exePath);

            runCmd(command);
            Logger.Log("created firewall rule: " + RuleName);
        }

        private bool ruleAlreadyExists()
        {
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));

            List<INetFwRule> RuleList = new List<INetFwRule>();

            foreach (INetFwRule rule in fwPolicy2.Rules)
            {
                return rule.Name == RuleName;
            }
            return false;
        }

        private void runCmd(string command)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = command;
            process.StartInfo = startInfo;
            process.Start();
        }
    }
}
