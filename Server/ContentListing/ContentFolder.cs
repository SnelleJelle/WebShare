using DotLiquid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShare.Server.ContentListing
{
    class ContentFolder : Drop
    {
        public List<ContentFile> Contents { get; set; } = new List<ContentFile>();
        public string Name { get; set; }

        public ContentFolder(string name)
        {
            Name = name;
        }
    }
}
