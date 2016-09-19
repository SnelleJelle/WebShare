using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShare.Server
{
    public abstract class TemplateGenerator
    {
        internal static string TEMPLATE_DELIMITER = "<!--body-->";
        internal string templateFile { get; set; }

        internal Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        internal string encapsulateInTemplate(string body)
        {
            string TemplateHtml = File.ReadAllText(templateFile);
            string[] template = TemplateHtml.Split(new string[] { TEMPLATE_DELIMITER }, StringSplitOptions.None);
            return template[0] + body + template[1];
        }
    }
}
