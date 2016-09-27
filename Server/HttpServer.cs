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
using System.IO.Compression;

namespace WebShare.Server
{
    class HttpServer
    {
        public int Port { get; private set; }
        public List<SharedFolder> SharedFolders { get; set; } = new List<SharedFolder>();

        private static string defaultMime = "application/octet-stream";
        private static string mimesPath = @"Server\mimes.xml";
        private static string zipCachePath = @"Cache\zip";

        private IDictionary<string, string> mimeTypes { get; set; }
        private HttpListener listener;
        private Thread serverThread;
        private SettingsManager settings = new SettingsManager();

        public event EventHandler<PermissionEventArgs> OnPermissionPrompt;

        public HttpServer(int port)
        {            
            Port = port;

            SharedFolders = settings.GetSHaredFolders();
            mimeTypes = loadMimeTypes();

            setFirewallRule();
        }

        public void Start()
        {
            serverThread = new Thread(listen);
            serverThread.Start();
            Logger.Log("Server starting on port: " + Port);
            Logger.Log("Browse locally on: http://localhost:" + Port + "/");
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
            if (settings.IsClientBlocked(client))
            {
                serveError(401, context);
                Logger.Log("Incoming connection from: " + client.Address.ToString() + " -> Blocked");
                return;
            }

            if (!settings.IsClientWhiteListed(client))
            {
                promptPermissionFor(client);
                if (settings.IsClientBlocked(client))
                {
                    serveError(401, context);
                    Logger.Log("Incoming connection from: " + client.Address.ToString() + " -> Blocked");
                    return;
                }
            }
            Logger.Log("Incoming connection from: " + client.Address.ToString() + " -> Allowed");

            handleRequest(context);
        }

        private void handleRequest(HttpListenerContext context)
        {
            string requestedPath = context.Request.Url.LocalPath;
            requestedPath = requestedPath.Substring(1);

            if (string.IsNullOrEmpty(requestedPath))
            {
                Logger.Log("Requested: " + requestedPath + " -> Serving content listing");
                serveContentListing(context);
            }
            else
            {
                DownloadRequest request = new DownloadRequest(requestedPath).WithSharedFolders(SharedFolders);

                if (request.IsFileRequest() && fileIsShared(request))
                {
                    Logger.Log("Requested: " + requestedPath + " -> Serving file");
                    serveFile(request.FullPath, context);
                }
                else if (request.IsWebRequest())
                {
                    Logger.Log("Requested: " + requestedPath + " -> Serving web file");
                    serveFile("/Web/" + request.FileName, context);
                }
                else if (request.IsZipRequest())
                {
                    Logger.Log("Requested: " + requestedPath + " -> Serving zip");
                    string fullDirPath = request.FullPath;
                    serveFile(Zip(fullDirPath), context);
                }
                else
                {
                    Logger.Log("Requested: " + requestedPath + " -> Content not found");
                    serveError(404, context);
                }
            }
        }

        public string Zip(string directoryPath)
        {
            string zipFileName = directoryPath.Split('\\').Last();
            zipFileName += DateTime.Now.ToString("_MM_dd_yy_HH_mm_ss") + ".zip";
            string zipCacheFilePath = Path.Combine(zipCachePath, zipFileName);
            Logger.Log("Zipping directory" + directoryPath + " -> to file: " + zipCacheFilePath);
            ZipFile.CreateFromDirectory(directoryPath, zipCacheFilePath, CompressionLevel.Optimal, true);
            return zipCacheFilePath;
        }

        private void promptPermissionFor(IPEndPoint client)
        {
            OnPermissionPrompt(this, new PermissionEventArgs { Client = client});            
        }

        public void BlockClient(IPEndPoint client)
        {
            settings.AddClientToBlockedList(client);
            settings.Save();
            Logger.Log("Blocked: " + client.Address.ToString());
        }

        public void AllowClient(IPEndPoint client)
        {
            settings.AddClientToWhiteList(client);
            settings.Save();
            Logger.Log("Whitelisted: " + client.Address.ToString());
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