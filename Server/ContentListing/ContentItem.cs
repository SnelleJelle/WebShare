﻿using DotLiquid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShare.Server.ContentListing
{    
    class ContentItem : Drop
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }    

        public static ContentItem FromFolder(DirectoryInfo folder)
        {
            int nrOfContents = folder.GetFiles().Length + folder.GetDirectories().Length;
            return new ContentItem() { Icon = getFolderIcon(), Name = "&nbsp;" + folder.Name, Size = nrOfContents + " items" };
        }

        public static ContentItem FromFile(FileInfo file)
        {
            return new ContentItem() { Icon = getBootstrapIcon(file), Name = file.Name, Size = getFileSizeString(file) };
        }

        private static string getFileSizeString(FileInfo file)
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

        private static string getBootstrapIcon(FileInfo file)
        {
            string extension = file.Extension.Replace(".", "").ToLower();
            string icon = bootstrapIcons.TryGetValue(extension, out icon) ? icon : bootstrapIcons["file"];

            return getIconHtmlForClass(icon);
        }

        private static string getIconHtmlForClass(string cssClass)
        {
            return string.Format("<span class=\"{0}\"></span> ", cssClass);
        }

        private static string getFolderIcon()
        {
            return getIconHtmlForClass(bootstrapIcons["folder"]);
        }

        private static IDictionary<string, string> bootstrapIcons = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
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

    }
}
