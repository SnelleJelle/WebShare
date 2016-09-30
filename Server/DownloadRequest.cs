using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShare.Server
{
    class DownloadRequest
    {
        private string exception_favicon = "favicon.ico";

        public string Command { get; set; }
        public string FolderAlias { get; set; }
        public string FileName { get; set; }
        public string FullPath { get; private set; } = "";

        public static string WebRequest = "web";
        public static string FileRequest = "download";
        public static string ZipRequest = "zip";

        public DownloadRequest(string request)
        {
            if (request == exception_favicon)
            {
                return;
            }
            string[] split= request.Split('/');
            if (split.Length == 2 && split[0].ToLower() == WebRequest)
            {
                Command = WebRequest;
                FileName = split[1];
            }
            else if (split.Length == 2 && split[0].ToLower() == ZipRequest)
            {
                Command = ZipRequest;            
                FileName = split[1];
                FolderAlias = FileName;
            }
            else if (split.Length == 3 && split[0].ToLower() == FileRequest)
            {
                Command = FileRequest;
                FolderAlias = split[1];
                FileName = split[2];
            }
            else
            {
                throw new ArgumentException("the request doesn't fit the expected format: /download/{folderName}/{fileName} or /web/{favicon.ico | style.css | script.js} or /zip/{folderName}");
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

        public bool IsZipRequest()
        {
            return Command == ZipRequest;
        }

        public DownloadRequest WithSharedFolders(List<SharedFolder> sharedFolders)
        {
            foreach (SharedFolder folder in sharedFolders)
            {
                if (folder.Alias == this.FolderAlias)
                {
                    if (Command == WebRequest)
                    {
                        FullPath = Path.Combine(WebRequest, FileName);
                    }
                    if (Command == ZipRequest)
                    {
                        FullPath = folder.Path;
                    }
                    if (Command == FileRequest)
                    {
                        FullPath = Path.Combine(folder.Path, this.FileName);
                    }
                }
            }
            return this;
        }
    }
}
