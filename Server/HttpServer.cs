using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Xml.Linq;
using WebShare.Server.ContentListing;
using WebShare.Server.Error;
using System.Windows.Forms;
using WebShare.Server.Settings;
using NetFwTypeLib;
using WebShare.Server.FireWall;

namespace WebShare.Server
{
    class HttpServer
    {
        public int Port { get; private set; }
        public List<SharedFolder> SharedFolders { get; set; } = new List<SharedFolder>();
        public List<Client> Clients { get; set; } = new List<Client>();

        private static string defaultMime = "application/octet-stream";
        private static string mimesPath = @"Server\mimes.xml";

        private IDictionary<string, string> mimeTypes { get; set; }
        private HttpListener listener;
        private Thread serverThread;
        private SettingsManager settings = new SettingsManager();

        public event EventHandler<PermissionEventArgs> OnPermissionPrompt;

        public HttpServer(int port)
        {            
            Port = port;

            SharedFolders = settings.GetSharedFolders();
            Clients = settings.GetClients();
            mimeTypes = loadMimeTypes();

            setFirewallRule();
        }

        public void Start()
        {
            serverThread = new Thread(listen);
            serverThread.Start();
        }

        public void Stop()
        {
            serverThread.Abort();
            listener.Stop();
        }

        private Dictionary<string, string> loadMimeTypes()
        {
            Dictionary< string, string> mimes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            XDocument mimeXml = XDocument.Load(mimesPath);
            foreach (var mime in mimeXml.Element("mimes").Elements())
            {
                string key = mime.Attribute("extension").Value;
                string value = mime.Value;
                mimes.Add(key, value);
            }
            return mimes;
        }

        private void listen()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://*:" + Port + "/");
            // If Exception is thrown here you should run with admin priviledges!
            listener.Start();
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                handleConnection(context);
            }
        }

        private void handleConnection(HttpListenerContext context)
        {
            IPEndPoint client = context.Request.RemoteEndPoint;
            Debug.Write("Incoming connection from: " + client.Address.ToString());
            if (settings.IsClientBlocked(client))
            {
                serveError(401, context);
                Debug.WriteLine(" -> Blocked");
                return;
            }

            if (!settings.IsClientWhiteListed(client))
            {
                promptPermissionFor(client);
                if (settings.IsClientBlocked(client))
                {
                    serveError(401, context);
                    Debug.WriteLine(" -> Blocked");
                    return;
                }
            }
            Debug.WriteLine(" -> Allowed");

            handleRequest(context);
        }

        private void handleRequest(HttpListenerContext context)
        {
            string requestedPath = context.Request.Url.LocalPath;
            Debug.Write("Requested: " + requestedPath + " -> ");
            requestedPath = requestedPath.Substring(1);

            if (string.IsNullOrEmpty(requestedPath))
            {
                Debug.WriteLine("Serving content listing");
                serveContentListing(context);
            }
            else
            {
                DownloadRequest request = new DownloadRequest(requestedPath);

                if (request.IsFileRequest() && fileIsShared(request))
                {
                    Debug.WriteLine("Serving file");
                    serveFile(getFullFilePath(request), context);
                }
                else if (request.IsWebRequest())
                {
                    serveFile("/Web/" + request.FileName, context);
                }
                else
                {
                    Debug.WriteLine("Content not found");
                    serveError(404, context);
                }
            }
        }

        private void promptPermissionFor(IPEndPoint client)
        {
            OnPermissionPrompt(this, new PermissionEventArgs { Client = client});            
        }

        public void BlockClient(IPEndPoint client)
        {
            settings.AddClientToBlockedList(client);
            settings.Save();
            Debug.WriteLine(" blocked");
        }

        public void AllowClient(IPEndPoint client)
        {
            settings.AddClientToWhiteList(client);
            settings.Save();
            Debug.WriteLine(" whitelisted");
        }

        private void serveContentListing(HttpListenerContext context)
        {
            serveStream(new ContentLister(this).getContentStream(), context);            
        }

        private void serveFile(string fullFilePath, HttpListenerContext context)
        {   
            try
            {                
                Stream input = new FileStream(fullFilePath, FileMode.Open);
                    
                string fileExtension = Path.GetExtension(fullFilePath).Replace(".", "");
                string mime;
                context.Response.ContentType = mimeTypes.TryGetValue(fileExtension, out mime) ? mime : defaultMime;
                    
                context.Response.AddHeader("Last-Modified", File.GetLastWriteTime(fullFilePath).ToString("r"));

                serveStream(input, context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.Response.OutputStream.Close();
        }      
        
        private void serveError(int errorCode, HttpListenerContext context)
        {
            Stream error = new ErrorMessage(errorCode).getStream();
            context.Response.StatusCode = errorCode;
            serveStream(error, context);
        }

        private void serveStream(Stream stream, HttpListenerContext context)
        {
            context.Response.ContentLength64 = stream.Length;
            context.Response.AddHeader("Date", DateTime.Now.ToString("r"));

            byte[] buffer = new byte[1024 * 16];
            int nbytes;
            while ((nbytes = stream.Read(buffer, 0, buffer.Length)) > 0)
                context.Response.OutputStream.Write(buffer, 0, nbytes);
            stream.Close();

            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.OutputStream.Flush();
        }

        private bool fileIsShared(DownloadRequest request)
        {
            foreach (SharedFolder folder in SharedFolders)
            {
                if (folder.Alias == request.FolderAlias && folder.Containsfile(request.FileName))
                {
                    return true;
                }
            }
            return false;
        }

        private string getFullFilePath(DownloadRequest request)
        {
            foreach (SharedFolder folder in SharedFolders)
            {
                if (folder.Alias == request.FolderAlias)
                {
                    return Path.Combine(folder.Path, request.FileName);
                }
            }
            return "";
        }

        public void AddSharedFolders(params SharedFolder[] folders)
        {
            SharedFolders.AddRange(folders);
            settings.AddShardFolderRange(folders);
            settings.Save();
        }

        private void setFirewallRule()
        {
            new FirewallManager(this).setFirewallRule();
        }
    }

    public class PermissionEventArgs : EventArgs
    {
        public IPEndPoint Client { get; set; }
    }
}