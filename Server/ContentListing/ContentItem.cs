using DotLiquid;
using System;
using System.Collections.Generic;
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
    }
}
