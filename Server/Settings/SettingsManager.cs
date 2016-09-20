using System;
using System.Diagnostics;
using System.Net;
using System.Xml.Linq;

namespace WebShare.Server.Settings
{
    class SettingsManager
    {
        private string filePath = @"Server\Settings\settings.xml";
        private XDocument settings;

        public SettingsManager()
        {
            settings = XDocument.Load(filePath);
        }

        public void AddClientToWhiteList(IPEndPoint client)
        {            
            XElement whiteClient = new XElement("client",
                new XElement("ip", client.Address.ToString()),
                new XElement("name", Dns.GetHostEntry(client.Address).HostName)
            );

            var whiteList = settings.Element("settings").Element("whitelist");
            whiteList.Add(whiteClient);
        }

        public void AddClientToBlockedList(IPEndPoint client)
        {
            XElement blockedClient = new XElement("client",
                new XElement("ip", client.Address.ToString()),
                new XElement("name", Dns.GetHostEntry(client.Address).HostName)
            );

            var blockedlist = settings.Element("settings").Element("blockedlist");
            blockedlist.Add(blockedClient);
        }

        public bool IsClientWhiteListed(IPEndPoint client)
        {
            var whitelist = settings.Element("settings").Element("whitelist");
            foreach (var whitelisted in whitelist.Elements())
            {
                if (whitelisted.Element("ip").Value == client.Address.ToString() && 
                    whitelisted.Element("name").Value == Dns.GetHostEntry(client.Address).HostName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsClientBlocked(IPEndPoint client)
        {
            var blockedlist = settings.Element("settings").Element("blockedlist");
            foreach (var blockedClient in blockedlist.Elements())
            {
                if (blockedClient.Element("ip").Value == client.Address.ToString() &&
                    blockedClient.Element("name").Value == Dns.GetHostEntry(client.Address).HostName)
                {
                    return true;
                }
            }
            return false;
        }

        public void Save()
        {
            settings.Save(filePath);
        }
    }

    public class PermissionEventArs : EventArgs
    {
        public IPEndPoint Client { get; set; }
    }
}
