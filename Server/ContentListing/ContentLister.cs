using System;
using System.Collections.Generic;
using System.IO;

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
            List<ContentItem> contents = new List<ContentItem>();

            foreach (DirectoryInfo subdir in dir.GetDirectories())
            {
                contents.Add(ContentItem.FromFolder(subdir));
            }
            foreach (FileInfo file in dir.GetFiles())
            {
                contents.Add(ContentItem.FromFile(file));
            }
            return renderRazor(contents);
        }       

        public Stream getContentStream()
        {
            string html = getHtmlContentListing();
            return generateStreamFromString(html);
        }        
    }
}
