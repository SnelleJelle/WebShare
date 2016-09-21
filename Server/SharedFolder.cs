using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace WebShare.Server
{
    class SharedFolder
    {
        public string Path { get; set; }
        public string Alias { get; set; }

        public SharedFolder()
        {

        }

        public SharedFolder(string path)
        {
            Path = path;
            var split =  path.Split(new string[] { "\\" }, StringSplitOptions.None);
            Alias = split[split.Length];
        }

        public bool Containsfile(string fileName)
        {
            DirectoryInfo dir = new DirectoryInfo(Path);
            foreach(FileInfo file in dir.GetFiles())
            {
                if (file.Name == fileName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
