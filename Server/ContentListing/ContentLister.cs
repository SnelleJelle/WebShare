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
            List<ContentFolder> contentFolders = new List<ContentFolder>();
            foreach (SharedFolder folder in Server.SharedFolders)
            {
                ContentFolder contentFolder = new ContentFolder(folder.Alias);
                DirectoryInfo dir = new DirectoryInfo(folder.Path);
                foreach(FileInfo file in dir.GetFiles())
                {
                    contentFolder.Contents.Add(new ContentFile(file));
                    int n = contentFolders.Count;

                }
                contentFolders.Add(contentFolder);
            }
            return renderRazor(contentFolders);
        }       

        public Stream getContentStream()
        {
            string html = getHtmlContentListing();
            return generateStreamFromString(html);
        }        
    }
}
