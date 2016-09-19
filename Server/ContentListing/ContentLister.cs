using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShare.Server.ContentListing
{
    class ContentLister : TemplateGenerator
    {
        public HttpServer Server { get; set; }

        public ContentLister(HttpServer server)
        {
            this.Server = server;
            templateFile = @"Server\ContentListing\ContentListing.html";
        }

        private string getHtmlContentListing()
        {            
            DirectoryInfo dir = new DirectoryInfo(Server.RootDirectory);
            StringBuilder body = new StringBuilder();

            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                body.Append(encapsulateFolder(subdir));
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                body.Append(encapsulateFile(file));
            }
            return encapsulateInTemplate(body.ToString());
        }

        private string encapsulateFile(FileInfo file)
        {
            return "<tr><td><a href=\"/" + file.Name + "\" >" + file.Name + "</a></td><td>" + getFileSizeString(file) + "</td></tr>";
        }

        private string encapsulateFolder(DirectoryInfo dir)
        {
            return "<tr><td><b>" + dir.Name + "</b></td></tr>";
        }

        private string getFileSizeString(FileInfo file)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = file.Length;
            int order = 0;
            while (len >= 1024 && ++order < sizes.Length)
            {
                len = len / 1024;
            }
            string result = String.Format("{0:0.##} {1}", len, sizes[order]);
            return result;
        }        

        public Stream getContentStream()
        {
            return GenerateStreamFromString(getHtmlContentListing());
        }        
    }
}
