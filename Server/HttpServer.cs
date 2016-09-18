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

namespace WebShare.Server
{
    class HttpServer
    {
        private static string DEFAULT_MIME = "application/octet-stream";
        private static string DEFAULT_404 = "file not found";

        public IDictionary<string, string> mimeTypes { get; private set; }
        public string RootDirectory { get; private set; }
        public int Port { get; private set; }
        private HttpListener listener;
        private Thread serverThread;

        public HttpServer(string rootDirectory, int port)
        {
            RootDirectory = rootDirectory;
            Port = port;

            mimeTypes = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            XDocument mimeXml = XDocument.Load(@"Server\mimes.xml");
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
            string requestedFileName = context.Request.Url.AbsolutePath;
            Debug.Write("Requested: " + requestedFileName + " -> ");
            requestedFileName = requestedFileName.Substring(1);
            
            if (string.IsNullOrEmpty(requestedFileName))
            {
                serveContentListing(context);
                Debug.WriteLine("Serving content listing");
            }
            else
            {
                serveFile(requestedFileName, context);
                Debug.WriteLine("Serving file");
            }            
        }        

        private void serveContentListing(HttpListenerContext context)
        {
            Stream input = new ContentLister(this).getContentStream();          

            serveStream(input, context);            
        }

        private void serveFile(string requestedFileName, HttpListenerContext context)
        {
            string fullFilePath = Path.Combine(RootDirectory, requestedFileName);

            if (File.Exists(fullFilePath))
            {
                try
                {
                    Stream input = new FileStream(fullFilePath, FileMode.Open);
                    
                    string fileExtension = Path.GetExtension(requestedFileName).Replace(".", "");
                    string mime;
                    context.Response.ContentType = mimeTypes.TryGetValue(fileExtension, out mime) ? mime : DEFAULT_MIME;
                    
                    context.Response.AddHeader("Last-Modified", File.GetLastWriteTime(requestedFileName).ToString("r"));

                    serveStream(input, context);
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }

            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            context.Response.OutputStream.Close();
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