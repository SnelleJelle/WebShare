using DotLiquid;
using System;
using System.Collections.Generic;
using System.IO;
using WebShare.Server.ContentListing;

namespace WebShare.Server
{
    public abstract class TemplateGenerator
    {
        internal string templateFile { get; set; }

        internal Stream generateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        internal string renderTemplate(object contents)
        {
            string templateHtml = File.ReadAllText(templateFile);
            Template template = Template.Parse(templateHtml);           
            string result = template.Render(Hash.FromAnonymousObject(new { model = contents }));
            return result;
        }
    }
}
