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

        private static IDictionary<string, string> BOOTSTRAP_ICONS = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            #region bootstrap icons
            {"file", "glyphicon glyphicon-file"},
            {"folder", "glyphicon glyphicon-folder-open"},
            {"mp3", "glyphicon glyphicon-music"},
            {"flac", "glyphicon glyphicon-music"},
            {"mp4", "glyphicon glyphicon-film"},
            {"flv", "glyphicon glyphicon-film"},
            {"webm", "glyphicon glyphicon-film"},
            {"jpeg", "glyphicon glyphicon-picture"},
            {"jpg", "glyphicon glyphicon-picture"},
            {"png", "glyphicon glyphicon-picture"},
            {"gif", "glyphicon glyphicon-picture"},
            {"bmp", "glyphicon glyphicon-picture"},
            {"exe", "fa fa-windows"},
            {"pdf", "fa fa-file-pdf-o"},
            {"doc", "fa fa-file-word-o"},
            {"docx", "fa fa-file-word-o"},
            {"xls", "fa fa-file-excel-o"},
            {"xlsx", "fa fa-file-excel-o"},
            {"ppt", "fa fa-file-powerpoint-o"},
            {"pptx", "fa fa-file-powerpoint-o"},
            {"zip", "fa fa-file-archive-o"},
            {"txt", "fa fa-file-text-o"},
            {"iso", "fa fa-dot-circle-o"}
            #endregion
        };

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
            return string.Format("<tr><td><a href=\"/{1}\" ><div>{0} {1}</div></a></td> <td>{2}</td></tr>", getBootstrapIcon(file), file.Name, getFileSizeString(file));
        }

        private string encapsulateFolder(DirectoryInfo dir)
        {
            int nrOfContents = dir.GetFiles().Length + dir.GetDirectories().Length;
            return string.Format("<tr><td><div>{0} <b>{1}</b></div></td> <td>{2}</td></tr>", getFolderIcon(), dir.Name, nrOfContents + " items");
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
        
        private string getBootstrapIcon(FileInfo file)
        {
            string extension = file.Extension.Replace(".","").ToLower();
            string icon = BOOTSTRAP_ICONS.TryGetValue(extension, out icon) ? icon : BOOTSTRAP_ICONS["file"];
            
            return getIconHtmlForClass(icon);
        }

        private string getIconHtmlForClass(string cssClass)
        {
            return string.Format("<span class=\"{0}\"></span>", cssClass);
        }

        private string getFolderIcon()
        {
            return getIconHtmlForClass(BOOTSTRAP_ICONS["folder"]);
        }

        public Stream getContentStream()
        {
            string html = getHtmlContentListing();
            return GenerateStreamFromString(html);
        }        
    }
}
