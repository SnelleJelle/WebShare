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

namespace WebShare.Server
{
    class HttpServer
    {
        public string RootDirectory { get; private set; }
        public int Port { get; private set; }
        public bool AllowSharingSubfolders { get; set; }

        private static string defaultMime = "application/octet-stream";
        private static string mimesPath = @"Server\mimes.xml";
        private IDictionary<string, string> mimeTypes { get; set; }
        private HttpListener listener;
        private Thread serverThread;
        private SettingsManager settings = new SettingsManager();

        public HttpServer(string rootDirectory, int port)
        {
            RootDirectory = rootDirectory;
            Port = port;

            mimeTypes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            XDocument mimeXml = XDocument.Load(mimesPath);
            foreach (var mime in mimeXml.Element("mimes").Elements())
            {
                string key = mime.Attribute("extension").Value;
                string value = mime.Value;
                mimeTypes.Add(key, value);
            }
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

        private void listen()
        {
            listener = new HttpListener();
            listener.Prefixes.Add("http://*:" + Port + "/");
            // If Exception is thrown here you should run with admin priviledges!
            listener.Start();
            while (true)
            {
                HttpListenerContext context = listener.GetContext();
                handleRequest(context);
            }
        }

        private void handleRequest(HttpListenerContext context)
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

            string requestedFileName = context.Request.Url.LocalPath;
            Debug.Write("Requested: " + requestedFileName + " -> ");
            requestedFileName = requestedFileName.Substring(1);
            
            if (string.IsNullOrEmpty(requestedFileName))
            {
                Debug.WriteLine("Serving content listing");
                serveContentListing(context);
            }
            else if (fileExistsInWebRoot(requestedFileName))
            {
                Debug.WriteLine("Serving file");
                serveFile(requestedFileName, context);
            }
            else
            {
                Debug.WriteLine("Content not found");
                serveError(404, context);
            }        
        }

        private void promptPermissionFor(IPEndPoint client)
        {
            DialogResult incoming = MessageBox.Show("Allow client?\n" + client.Address + "\n" + Dns.GetHostEntry(client.Address).HostName,
                "Incoming connection request", MessageBoxButtons.YesNo);
            if (incoming == DialogResult.Yes)
            {
                allowClient(client);
            }
            else if (incoming == DialogResult.No)
            {
                blockClient(client);
            }
        }

        private void blockClient(IPEndPoint client)
        {
            settings.AddClientToBlockedList(client);
            settings.Save();
        }

        private void allowClient(IPEndPoint client)
        {
            settings.AddClientToWhiteList(client);
            settings.Save();
        }

        private void serveContentListing(HttpListenerContext context)
        {
            serveStream(new ContentLister(this).getContentStream(), context);            
        }

        private void serveFile(string requestedFileName, HttpListenerContext context)
        {   
            try
            {
                string fullFilePath = Path.Combine(RootDirectory, requestedFileName);
                Stream input = new FileStream(fullFilePath, FileMode.Open);
                    
                string fileExtension = Path.GetExtension(requestedFileName).Replace(".", "");
                string mime;
                context.Response.ContentType = mimeTypes.TryGetValue(fileExtension, out mime) ? mime : defaultMime;
                    
                context.Response.AddHeader("Last-Modified", File.GetLastWriteTime(requestedFileName).ToString("r"));

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

        private bool fileExistsInWebRoot(string fileName)
        {
            return File.Exists(Path.Combine(RootDirectory, fileName));
        }
    }
}