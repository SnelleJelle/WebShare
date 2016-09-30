using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShare.Util.Server;

namespace WebShare.Server.Util.Zip
{
    class Zip
    {
        public string SourceDirectory { get; set; }
        private static string zipCachePath = @"Cache\zip";
        public string ZipFileDir { get; private set; }

        public Zip(string sourceDir)
        {
            SourceDirectory = sourceDir;
        }

        public string Create()
        {
            string zipFileName = SourceDirectory.Split('\\').Last();
            zipFileName += DateTime.Now.ToString("_MM_dd_yy_HH_mm_ss") + ".zip";
            string zipCacheFilePath = Path.Combine(zipCachePath, zipFileName);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (ZipArchive zip = ZipFile.Open(zipCacheFilePath, ZipArchiveMode.Create))
            {
                Logger.Log("Zipping directory" + SourceDirectory + " -> to file: " + zipCacheFilePath);
                foreach (string filePath in Directory.GetFiles(SourceDirectory))
                {
                    string fileName = filePath.Split('\\').Last();
                    zip.CreateEntryFromFile(filePath, fileName);
                }
            }
            watch.Stop();
            Logger.Log("Zip created in " + watch.ElapsedMilliseconds + " milliseconds");

            ZipFileDir = zipCacheFilePath;
            return zipCacheFilePath;
        }
    }
}
