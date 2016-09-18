using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShare.Server
{
    class ContentLister
    {
        private static string header = "<!DOCTYPE html><html><head><style> table{ font-family:arial,sans-serif; border-collapse:collapse; width:100%; } td,th{ border:1pxsolid#dddddd; text-align:left; padding:8px; } tr:nth-child(even){ background-color:#dddddd; } </style><title>WebShare Content Listing</title></head><body><h1>Hello, World</h1>";
        private static string footer = "</body></html>";
        public HttpServer Server { get; set; }

        public ContentLister(HttpServer server)
        {
            this.Server = server;
        }

        private string getHtmlContentListing()
        {            
            DirectoryInfo dir = new DirectoryInfo(Server.RootDirectory);
            StringBuilder body = new StringBuilder("<table><tr><th><u>Content</u></th></tr>");

            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                body.Append("<tr><td><b>" + subdir.Name + "</b></td></tr>");
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                body.Append("<tr><td><a href=\"/" + file.Name + "\" >" + file.Name + "</a></td></tr>");
            }

            body.Append("</table>");
            return header + body.ToString() + footer;
        }

        private Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public Stream getContentStream()
        {
            return GenerateStreamFromString(getHtmlContentListing());
        }
    }
}
