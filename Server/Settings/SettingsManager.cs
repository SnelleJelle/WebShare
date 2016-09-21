using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
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
            string host = "";
            try
            {
                host = Dns.GetHostEntry(client.Address).HostName;
            }
            catch (SocketException se) {}

            XElement whiteClient = new XElement("client",
                new XElement("ip", client.Address.ToString()),
                new XElement("name", host)
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

        public List<SharedFolder> GetSHaredFolders()
        {
            List<SharedFolder> sharedFolders = new List<SharedFolder>();
            var xmlSharedFolders = settings.Element("settings").Element("sharedFolders");
            foreach (var xmlSharedFolder in xmlSharedFolders.Elements())
            {
                SharedFolder sharedFolder = new SharedFolder {
                    Path = xmlSharedFolder.Element("path").Value,
                    Alias = xmlSharedFolder.Element("alias").Value
                };
                sharedFolders.Add(sharedFolder);
            }

            return sharedFolders;
        }

        public void AddSharedFolder(SharedFolder folder)
        {
            var xmlSharedFolders = settings.Element("settings").Element("sharedFolders");
            xmlSharedFolders.Add(new XElement("sharedFolder",
                new XElement("path", folder.Path),
                new XElement("alias", folder.Alias)
                ));
        }

        public void AddShardFolderRange(params SharedFolder[] sharedfolders)
        {
            foreach(SharedFolder folder in sharedfolders)
            {
                AddSharedFolder(folder);
            }
        }

        public void Save()
        {
            settings.Save(filePath);
        }
    }
}
