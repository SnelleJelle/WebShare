using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShare.Server
{
    class DownloadRequest
    {
        public string Command { get; set; }
        public string FolderAlias { get; set; }
        public string FileName { get; set; }

        public static string WebRequest = "web";
        public static string FileRequest = "download";

        public DownloadRequest()
        {

        }

        public DownloadRequest(string request)
        {
            string[] split= request.Split('/');
            if (split.Length == 2 && split[0].ToLower() == WebRequest)
            {
                Command = WebRequest;
                FileName = split[1];
            }
            else if (split.Length == 3 && split[0].ToLower() == FileRequest)
            {
                Command = FileRequest;
                FolderAlias = split[1];
                FileName = split[2];
            }
            else
            {
                throw new ArgumentException("the request doesn't fit the expected format: /download/{folderName}/{fileName} or /web/{favicon.ico | style.css | script.js}");
            }
        }

        public bool IsFileRequest()
        {
            return Command == FileRequest;
        }

        public bool IsWebRequest()
        {
            return Command == WebRequest;
        }
    }
}
