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

        public void AddClientToWhiteList(Client client)
        {
            var xmlAllowedClients = settings.Element("settings").Element("whitelist");
            xmlAllowedClients.Add(new XElement("client",
                new XElement("ip", client.IP),
                new XElement("name", client.Hostname))
            );

        }

        public void AddClientToBlockedList(Client client)
        {
            var xmlBlockedClients = settings.Element("settings").Element("blacklist");
            xmlBlockedClients.Add(new XElement("client",
                new XElement("ip", client.IP),
                new XElement("name", client.Hostname))
            );
        }
        //Refactor to one function
        public bool IsClientWhiteListed(Client client)
        {
            return client.Allowed;
        }

        public bool IsClientBlocked(Client client)
        {
            return client.Allowed;
        }

        public List<SharedFolder> GetSharedFolders()
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

        public List<Client> GetClients()
        {
            List<Client> Clients = new List<Client>();
            var xmlClients = settings.Element("settings").Element("whitelist");
            foreach(var xmlClientList in xmlClients.Elements())
            {
                Client client = new Client
                {
                    IP = xmlClientList.Element("ip").Value,
                            Allowed = true,
                            Hostname = xmlClientList.Element("hostname").Value
                            
                        };
                        Clients.Add(client);              
            }
            xmlClients = settings.Element("settings").Element("blacklist");
            foreach (var xmlClientList in xmlClients.Elements())
            {
                Client client = new Client
                {
                    IP = xmlClientList.Element("ip").Value,
                    Allowed = false,
                    Hostname = xmlClientList.Element("hostname").Value
                };
                Clients.Add(client);
            }
            return Clients;
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
        //move to Client
        private string getHostname(IPEndPoint client)
        {
            string host = "";
            try
            {
                host = Dns.GetHostEntry(client.Address).HostName;
            }
            catch (SocketException se) { }
            return host;
        }
    }
}
